# =========================
# ETAPA 1: Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos todo el código al contenedor
COPY . .

# Restauramos dependencias
RUN dotnet restore InvestmentOrders.Api/InvestmentOrders.Api.csproj

# Publicamos la app
RUN dotnet publish InvestmentOrders.Api/InvestmentOrders.Api.csproj -c Release -o /app/publish

# =========================
# ETAPA 2: Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiamos solo el resultado del build
COPY --from=build /app/publish .

# Puerto interno del contenedor
EXPOSE 8080

# Comando de inicio
ENTRYPOINT ["dotnet", "InvestmentOrders.Api.dll"]
