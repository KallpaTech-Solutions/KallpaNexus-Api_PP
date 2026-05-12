# KallpaNexus-Api_PP

API **ASP.NET Core 8** para el pretotipo KallpaNexus: leads, analíticas, recomendaciones anónimas y encuestas (PostgreSQL + Entity Framework Core).

## Desarrollo local (por ahora)

1. **Cadena de conexión** solo en `KallpaNexus_API/appsettings.json` (está en `.gitignore`, no se sube a Git). `appsettings.Development.json` **no** define `ConnectionStrings`, para que no pise lo que pongas en `appsettings.json`.
2. Puedes apuntar a **Postgres local** (ej. `Host=localhost;Port=5433;…`) o a la **misma base de Render** que en producción. Desde tu PC suele hacer falta la cadena **External** del panel de Postgres en Render (la **Internal** solo sirve entre servicios dentro de Render). Incluye `SSL Mode=Require;Trust Server Certificate=true` si Render lo exige.
3. Crea o actualiza el esquema cuando cambien las migraciones:
   ```bash
   cd KallpaNexus_API
   dotnet ef database update
   ```
4. Arranca la API (perfil **http** / **https** en Visual Studio usa `ASPNETCORE_ENVIRONMENT=Development`). En **Development** no se ejecuta `Migrate()` al iniciar: evitas depender del arranque automático mientras trabajas solo en local.

### CORS desde Vite (`http://localhost:5173`)

Si la consola del navegador dice que no hay cabecera `Access-Control-Allow-Origin` hacia `http://localhost:5062`, comprueba que el API esté en marcha y reinicia el API tras cambios en `Program.cs`. En **Development** no se usa `UseHttpsRedirection`, para que peticiones HTTP al API no reciban un redirect que rompe el preflight CORS frente a Vite.

## Requisitos

- .NET 8 SDK
- PostgreSQL (cadena en `appsettings.json` / secretos de usuario)

## Comandos útiles

```bash
cd KallpaNexus_API
dotnet restore
dotnet ef database update
dotnet run
```

Swagger suele estar en `/swagger` en desarrollo.

## Producción (Render)

Con `ASPNETCORE_ENVIRONMENT=Production`, al iniciar se ejecuta `Database.Migrate()` si la cadena `ConnectionStrings__DefaultConnection` apunta a tu Postgres (p. ej. URL interna de Render + SSL si aplica).

## Repositorio remoto

`https://github.com/KallpaTech-Solutions/KallpaNexus-Api_PP.git`
