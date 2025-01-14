FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./backend/src/CensorshipService/CensorshipService.Worker/CensorshipService.Worker.csproj", "/CensorshipService/"]
COPY ["./backend/src/CensorshipService/CensorshipService.Application/CensorshipService.Application.csproj", "/CensorshipService/"]
COPY ["./backend/src/CensorshipService/CensorshipService.Infrastructure/CensorshipService.Infrastructure.csproj", "/CensorshipService/"]

RUN dotnet restore "CensorshipService"

RUN dotnet build "CensorshipService.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CensorshipService.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CensorshipService.Worker.Api.dll"]
