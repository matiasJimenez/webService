using GestionVentasServicios.Data;
using Microsoft.EntityFrameworkCore;
using GestionVentasServicios.Services;
using GestionVentasServicios.Services.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IAiQueryService, AiQueryService>();
builder.Services.AddHttpClient<IAiSqlPlanner, OpenAiSqlPlanner>();
builder.Services.AddHttpClient<IAiAnswerFormatter, OpenAiAnswerFormatter>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<OpenAiOptions>(builder.Configuration.GetSection("OpenAI"));
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 
app.MapControllers();
app.Run();

