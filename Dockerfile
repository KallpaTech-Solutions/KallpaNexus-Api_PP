# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos del proyecto y restaura dependencias
# Asegúrate de que el nombre del .csproj sea el correcto
COPY ["KallpaNexus_API.csproj", "./"]
RUN dotnet restore "KallpaNexus_API.csproj"

# Copia el resto del código y compila
COPY . .
RUN dotnet publish "KallpaNexus_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final: Imagen de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto que configuramos en Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "KallpaNexus_API.dll"]