# GestionVentasServicios

API REST para gestión de ventas con funcionalidad de consultas mediante IA.

## Configuración de Desarrollo

### User Secrets (Requerido)

⚠️ **NUNCA subas tu API key a Git**. Este proyecto usa **User Secrets**.

Para configurar tu API key de OpenAI:

```bash
cd GestionVentasServicios
dotnet user-secrets set "OpenAI:ApiKey" "sk-tu-api-key-real"
```

La configuración se guarda localmente en `~/.microsoft/usersecrets/` y **nunca se sube a Git**.

### Variables de Entorno (Producción)

Para producción, configura las siguientes variables de entorno:

- `OpenAI__ApiKey`: Tu API key de OpenAI
- `OpenAI__Model`: Modelo a usar (default: gpt-4o-mini)
- `OpenAI__BaseUrl`: URL base de OpenAI API (default: https://api.openai.com/v1)

## Ejecutar la Aplicación

```bash
dotnet run
```

## Características

- ✅ Gestión de Clientes
- ✅ Gestión de Usuarios
- ✅ Planes de Venta
- ✅ Consultas en lenguaje natural mediante IA
