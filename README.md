# KallpaNexus-Api_PP

API **ASP.NET Core 8** para el pretotipo KallpaNexus: leads, analíticas, recomendaciones anónimas y encuestas (PostgreSQL + Entity Framework Core).

## Desarrollo local (por ahora)

1. PostgreSQL en marcha (en este equipo el puerto suele ser **5433**; ajústalo en la cadena si el tuyo es otro).
2. Copia `KallpaNexus_API/appsettings.example.json` a `KallpaNexus_API/appsettings.json` y pon **usuario, contraseña y nombre de base reales**. Ese archivo está en `.gitignore` y no se sube al remoto.
3. Crea o actualiza el esquema cuando cambien las migraciones:
   ```bash
   cd KallpaNexus_API
   dotnet ef database update
   ```
4. Arranca la API (perfil **http** / **https** en Visual Studio usa `ASPNETCORE_ENVIRONMENT=Development`). En **Development** no se ejecuta `Migrate()` al iniciar: evitas depender del arranque automático mientras trabajas solo en local.

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
