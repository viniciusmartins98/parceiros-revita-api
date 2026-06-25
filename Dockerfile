# ==============================================================================
# Stage 1 - Restore (cacheia as dependências separadamente)
# ==============================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

COPY RevitaParceiros.API/RevitaParceiros.API.csproj RevitaParceiros.API/
COPY RevitaParceiros.Application/RevitaParceiros.Application.csproj RevitaParceiros.Application/
COPY RevitaParceiros.Domain/RevitaParceiros.Domain.csproj RevitaParceiros.Domain/
COPY RevitaParceiros.Infra/RevitaParceiros.Infra.csproj RevitaParceiros.Infra/

RUN dotnet restore RevitaParceiros.API/RevitaParceiros.API.csproj

# ==============================================================================
# Stage 2 - Build & Publish
# ==============================================================================
FROM restore AS publish
COPY . .
RUN dotnet publish RevitaParceiros.API/RevitaParceiros.API.csproj \
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
