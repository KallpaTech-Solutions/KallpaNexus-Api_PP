# Build (contexto = raíz del repo: .sln + carpeta KallpaNexus_API/)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["KallpaNexus_API/KallpaNexus_API.csproj", "KallpaNexus_API/"]
RUN dotnet restore "KallpaNexus_API/KallpaNexus_API.csproj"

COPY . .
RUN dotnet publish "KallpaNexus_API/KallpaNexus_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render define PORT en runtime; Kestrel debe escuchar en ese puerto
CMD ["/bin/sh", "-c", "export ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}; exec dotnet KallpaNexus_API.dll"]
