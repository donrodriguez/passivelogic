#FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-bookworm-slim-amd64 AS base
FROM mcr.microsoft.com/dotnet/aspnet:8.0.8-bookworm-slim-arm64v8 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore PhysicsSimulationWebApi.sln
WORKDIR /src/API

RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]