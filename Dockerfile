# ==============================================================================
# Stage 1 - Restore (cacheia as dependências separadamente)
# ==============================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

COPY src/RevitaParceiros.API/RevitaParceiros.API.csproj src/RevitaParceiros.API/
COPY src/RevitaParceiros.Application/RevitaParceiros.Application.csproj src/RevitaParceiros.Application/
COPY src/RevitaParceiros.Domain/RevitaParceiros.Domain.csproj src/RevitaParceiros.Domain/
COPY src/RevitaParceiros.Infra/RevitaParceiros.Infra.csproj src/RevitaParceiros.Infra/

RUN dotnet restore src/RevitaParceiros.API/RevitaParceiros.API.csproj

# ==============================================================================
# Stage 2 - Build & Publish
# ==============================================================================
FROM restore AS publish
COPY . .
RUN dotnet publish src/RevitaParceiros.API/RevitaParceiros.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ==============================================================================
# Stage 3 - Runtime (imagem final leve)
# ==============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "RevitaParceiros.API.dll"]
