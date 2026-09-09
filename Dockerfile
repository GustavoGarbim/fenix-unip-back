# Dockerfile do backend Fênix UNIP (ASP.NET Core 8) — usado para deploy no Render.
# Build context esperado: raiz do repositório (onde este arquivo está).

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY fenix-unip-back/fenix-unip-back.csproj fenix-unip-back/
RUN dotnet restore fenix-unip-back/fenix-unip-back.csproj

COPY fenix-unip-back/ fenix-unip-back/
RUN dotnet publish fenix-unip-back/fenix-unip-back.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# O Render define a porta de escuta na variável de ambiente PORT em runtime;
# o valor abaixo (10000) é só um fallback para rodar o container localmente.
ENV PORT=10000
EXPOSE 10000

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT} dotnet fenix-unip-back.dll"]
