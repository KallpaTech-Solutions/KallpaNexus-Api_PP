# KallpaNexus-Api_PP

API **ASP.NET Core 8** para el pretotipo KallpaNexus: leads, analíticas, recomendaciones anónimas y encuestas (PostgreSQL + Entity Framework Core).

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

## Configuración

1. Copia `KallpaNexus_API/appsettings.example.json` a `KallpaNexus_API/appsettings.json`.
2. Ajusta host, puerto, base de datos y credenciales de PostgreSQL (no subas `appsettings.json` con contraseñas reales).

## Repositorio remoto

`https://github.com/KallpaTech-Solutions/KallpaNexus-Api_PP.git`
