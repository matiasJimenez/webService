using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GestionVentasServicios.Data;
using GestionVentasServicios.Models;
using GestionVentasServicios.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GestionVentasServicios.Services
{
    public interface IAiQueryService
    {
        Task<AiQueryResponse> ResolveAsync(string naturalLanguageQuery, CancellationToken cancellationToken = default);
    }

    public class AiQueryService : IAiQueryService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IAiSqlPlanner _planner;
        private readonly IAiAnswerFormatter _answerFormatter;

        public AiQueryService(ApplicationDBContext dbContext, IAiSqlPlanner planner, IAiAnswerFormatter answerFormatter)
        {
            _dbContext = dbContext;
            _planner = planner;
            _answerFormatter = answerFormatter;
        }

        public async Task<AiQueryResponse> ResolveAsync(string naturalLanguageQuery, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(naturalLanguageQuery))
            {
                return new AiQueryResponse { Error = "La consulta en lenguaje natural es requerida." };
            }

            AiQueryPlan? plan;
            try
            {
                plan = await _planner.BuildPlanAsync(naturalLanguageQuery, cancellationToken);
            }
            catch (Exception ex)
            {
                return new AiQueryResponse { Error = $"No se pudo generar el plan de consulta: {ex.Message}" };
            }

            if (plan == null || string.IsNullOrWhiteSpace(plan.Entity))
            {
                return new AiQueryResponse { Error = "La IA no devolvió un plan de consulta utilizable." };
            }

            try
            {
                var data = await ExecutePlanAsync(plan, cancellationToken);
                var natural = await BuildNaturalResponseAsync(naturalLanguageQuery, data, cancellationToken);
                return new AiQueryResponse
                {
                    Plan = plan,
                    Data = null, // no exponemos los datos crudos en la respuesta pública
                    NaturalResponse = natural
                };
            }
            catch (Exception ex)
            {
                return new AiQueryResponse { Plan = plan, Error = $"No se pudo ejecutar la consulta: {ex.Message}" };
            }
        }

        private async Task<object> ExecutePlanAsync(AiQueryPlan plan, CancellationToken cancellationToken)
        {
            var limit = plan.Limit.GetValueOrDefault(50);
            limit = Math.Clamp(limit, 1, 200);

            switch (plan.Entity.Trim().ToLowerInvariant())
            {
                case "cliente":
                case "clientes":
                    var clientesQuery = ApplyFilters(_dbContext.Clientes.AsQueryable(), plan.Filters);
                    var clientes = await clientesQuery.Take(limit).ToListAsync(cancellationToken);
                    return ShapeResults(clientes, plan.Select);

                case "usuario":
                case "usuarios":
                    var usuariosQuery = ApplyFilters(_dbContext.Usuarios.AsQueryable(), plan.Filters);
                    var usuarios = await usuariosQuery.Take(limit).ToListAsync(cancellationToken);
                    return ShapeResults(usuarios, plan.Select);

                case "pago":
                case "pagos":
                    var pagos = await ExecutePagoPlanAsync(plan, limit, cancellationToken);
                    return pagos;

                case "planventa":
                case "planventas":
                case "plan_venta":
                case "plan_ventas":
                    var planesQuery = ApplyFilters(_dbContext.PlanVentas.AsQueryable(), plan.Filters);
                    var planes = await planesQuery.Take(limit).ToListAsync(cancellationToken);
                    return ShapeResults(planes, plan.Select);

                default:
                    throw new InvalidOperationException($"Entidad '{plan.Entity}' no soportada.");
            }
        }

        private static IEnumerable<object> ShapeResults<T>(IEnumerable<T> items, IEnumerable<string> select)
        {
            var selectedFields = select?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList() ?? new List<string>();
            if (selectedFields.Count == 0)
            {
                return items.Cast<object>();
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return items.Select(item =>
            {
                var shaped = new Dictionary<string, object?>();
                foreach (var field in selectedFields)
                {
                    var prop = properties.FirstOrDefault(p => string.Equals(p.Name, field, StringComparison.OrdinalIgnoreCase));
                    if (prop == null) continue;
                    shaped[prop.Name] = prop.GetValue(item);
                }
                return (object)shaped;
            });
        }

        private sealed class JoinedPago
        {
            public Pago Pago { get; set; } = null!;
            public Cliente? Cliente { get; set; }
            public Usuario? Usuario { get; set; }
            public PlanVenta? PlanVenta { get; set; }
        }

        private async Task<IEnumerable<object>> ExecutePagoPlanAsync(AiQueryPlan plan, int limit, CancellationToken cancellationToken)
        {
            // Traemos pagos con left joins a las entidades relacionadas conocidas
            var query =
                from p in _dbContext.Pagos
                join c in _dbContext.Clientes on p.ClientId equals c.Id into cj
                from c in cj.DefaultIfEmpty()
                join u in _dbContext.Usuarios on p.UsuarioId equals u.Id into uj
                from u in uj.DefaultIfEmpty()
                join pv in _dbContext.PlanVentas on p.PlanVentaId equals pv.Id into pvj
                from pv in pvj.DefaultIfEmpty()
                select new JoinedPago { Pago = p, Cliente = c, Usuario = u, PlanVenta = pv };

            query = ApplyPagoFilters(query, plan.Filters);

            var pagos = await query.Take(limit).ToListAsync(cancellationToken);
            return ShapePagoResults(pagos, plan.Select);
        }

        private static IQueryable<JoinedPago> ApplyPagoFilters(IQueryable<JoinedPago> source, IEnumerable<AiQueryFilter> filters)
        {
            if (filters == null)
            {
                return source;
            }

            foreach (var filter in filters)
            {
                if (string.IsNullOrWhiteSpace(filter.Field) && string.IsNullOrWhiteSpace(filter.Entity))
                {
                    continue;
                }

                var (entityName, fieldName) = ParseEntityAndField(filter);

                var parameter = Expression.Parameter(typeof(JoinedPago), "x");
                if (!TryGetPagoPropertyExpression(parameter, entityName, fieldName, out var propertyAccess, out var propertyType, out var nullGuard))
                {
                    continue;
                }

                if (!TryParseValue(propertyType, filter.Value, out var typedValue))
                {
                    continue;
                }

                var comparison = BuildComparisonExpression(propertyAccess, filter.Operator, typedValue!, propertyType);
                if (comparison == null)
                {
                    continue;
                }

                var body = comparison;

                // Si hay null-guard (para joins), se aplica para evitar NullReference
                if (nullGuard != null)
                {
                    body = Expression.AndAlso(nullGuard, body);
                }

                var lambda = Expression.Lambda<Func<JoinedPago, bool>>(body, parameter);
                source = source.Where(lambda);
            }

            return source;
        }

        private static (string entity, string field) ParseEntityAndField(AiQueryFilter filter)
        {
            var entity = filter.Entity?.Trim() ?? string.Empty;
            var field = filter.Field.Trim();

            if (field.Contains("."))
            {
                var parts = field.Split('.', 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    entity = string.IsNullOrWhiteSpace(entity) ? parts[0] : entity;
                    field = parts[1];
                }
            }

            if (string.IsNullOrWhiteSpace(entity))
            {
                entity = "pagos"; // por defecto pagos
            }

            return (entity.ToLowerInvariant(), field);
        }

        private static bool TryGetPagoPropertyExpression(ParameterExpression parameter, string entityName, string fieldName, out MemberExpression propertyAccess, out Type propertyType, out Expression? nullGuard)
        {
            propertyType = typeof(object);
            nullGuard = null;

            Expression target;
            switch (entityName)
            {
                case "pago":
                case "pagos":
                    target = Expression.Property(parameter, nameof(JoinedPago.Pago));
                    break;
                case "cliente":
                case "clientes":
                    target = Expression.Property(parameter, nameof(JoinedPago.Cliente));
                    nullGuard = Expression.NotEqual(target, Expression.Constant(null, typeof(Cliente)));
                    break;
                case "usuario":
                case "usuarios":
                    target = Expression.Property(parameter, nameof(JoinedPago.Usuario));
                    nullGuard = Expression.NotEqual(target, Expression.Constant(null, typeof(Usuario)));
                    break;
                case "planventa":
                case "planventas":
                case "plan_venta":
                case "plan_ventas":
                    target = Expression.Property(parameter, nameof(JoinedPago.PlanVenta));
                    nullGuard = Expression.NotEqual(target, Expression.Constant(null, typeof(PlanVenta)));
                    break;
                default:
                    propertyAccess = null!;
                    return false;
            }

            var propInfo = target.Type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propInfo == null)
            {
                propertyAccess = null!;
                return false;
            }

            propertyAccess = Expression.Property(target, propInfo);
            propertyType = propInfo.PropertyType;
            return true;
        }

        private static IEnumerable<object> ShapePagoResults(IEnumerable<JoinedPago> items, IEnumerable<string> select)
        {
            var selectedFields = select?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList() ?? new List<string>();
            if (selectedFields.Count == 0)
            {
                return items.Select(x => (object)x.Pago);
            }

            return items.Select(item =>
            {
                var shaped = new Dictionary<string, object?>();
                foreach (var field in selectedFields)
                {
                    var (entity, fieldName) = ParseEntityAndField(new AiQueryFilter { Field = field });
                    switch (entity)
                    {
                        case "pago":
                        case "pagos":
                            TryAddValue(shaped, field, item.Pago, fieldName);
                            break;
                        case "cliente":
                        case "clientes":
                            TryAddValue(shaped, field, item.Cliente, fieldName);
                            break;
                        case "usuario":
                        case "usuarios":
                            TryAddValue(shaped, field, item.Usuario, fieldName);
                            break;
                        case "planventa":
                        case "planventas":
                        case "plan_venta":
                        case "plan_ventas":
                            TryAddValue(shaped, field, item.PlanVenta, fieldName);
                            break;
                    }
                }
                return (object)shaped;
            });
        }

        private static void TryAddValue(IDictionary<string, object?> target, string key, object? source, string fieldName)
        {
            if (source == null)
            {
                return;
            }

            var prop = source.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null)
            {
                return;
            }

            target[key] = prop.GetValue(source);
        }

        private static IQueryable<T> ApplyFilters<T>(IQueryable<T> source, IEnumerable<AiQueryFilter> filters)
        {
            if (filters == null)
            {
                return source;
            }

            foreach (var filter in filters)
            {
                if (string.IsNullOrWhiteSpace(filter.Field) || string.IsNullOrWhiteSpace(filter.Operator))
                {
                    continue;
                }

                var property = typeof(T).GetProperty(filter.Field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null)
                {
                    continue; // Ignore unrecognized fields to keep the query safe
                }

                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyAccess = Expression.Property(parameter, property);

                if (!TryParseValue(property.PropertyType, filter.Value, out var typedValue))
                {
                    continue;
                }

                Expression? comparison = BuildComparisonExpression(propertyAccess, filter.Operator, typedValue!, property.PropertyType);
                if (comparison == null)
                {
                    continue;
                }

                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                source = source.Where(lambda);
            }

            return source;
        }

        private static Expression? BuildComparisonExpression(Expression propertyAccess, string @operator, object typedValue, Type propertyType)
        {
            var op = @operator.Trim().ToLowerInvariant();
            if (op is "equals" or "eq")
            {
                return Expression.Equal(propertyAccess, Expression.Constant(typedValue, propertyAccess.Type));
            }

            if (op is "contains" && propertyType == typeof(string))
            {
                return Expression.Call(propertyAccess, nameof(string.Contains), Type.EmptyTypes, Expression.Constant(typedValue, propertyAccess.Type));
            }

            if (op is "gt" or "greaterthan" or "greater_than")
            {
                return Expression.GreaterThan(propertyAccess, Expression.Constant(typedValue, propertyAccess.Type));
            }

            if (op is "lt" or "lessthan" or "less_than")
            {
                return Expression.LessThan(propertyAccess, Expression.Constant(typedValue, propertyAccess.Type));
            }

            return null;
        }

        private static bool TryParseValue(Type targetType, string? value, out object? typedValue)
        {
            typedValue = null;
            if (value == null)
            {
                return false;
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                if (underlyingType == typeof(string))
                {
                    typedValue = value;
                    return true;
                }

                if (underlyingType == typeof(DateTime))
                {
                    if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date))
                    {
                        typedValue = date;
                        return true;
                    }

                    return false;
                }

                if (underlyingType.IsEnum)
                {
                    typedValue = Enum.Parse(underlyingType, value, true);
                    return true;
                }

                typedValue = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string?> BuildNaturalResponseAsync(string userQuery, object data, CancellationToken cancellationToken)
        {
            try
            {
                // Limitamos el payload al formatter serializando a JSON
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = false });
                return await _answerFormatter.BuildNaturalResponseAsync(userQuery, json, cancellationToken);
            }
            catch
            {
                return null;
            }
        }
    }
}
