FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-bookworm-slim-amd64 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore Flo.sln
WORKDIR /src/flo.API

RUN dotnet build "PhysicsSimulationWebApi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhysicsSimulationWebApi.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhysicsSimulationWebApi.API.dll"]