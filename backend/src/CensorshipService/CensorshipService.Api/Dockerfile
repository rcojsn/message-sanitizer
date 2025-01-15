FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./backend/src/CensorshipService/CensorshipService.Api/CensorshipService.Api.csproj", "/CensorshipService/"]
COPY ["./backend/src/CensorshipService/CensorshipService.Application/CensorshipService.Application.csproj", "/CensorshipService/"]
COPY ["./backend/src/CensorshipService/CensorshipService.Contracts/CensorshipService.Contracts.csproj", "/CensorshipService/"]
COPY ["./backend/src/CensorshipService/CensorshipService.Infrastructure/CensorshipService.Infrastructure.csproj", "/CensorshipService/"]
COPY ["./backend/src/AdminService/AdminService.Domain/AdminService.Domain.csproj", "/CensorshipService/"]
COPY ["./backend/src/BleepGuard.Contracts\BleepGuard.Contracts.csproj", "/CensorshipService/"]
COPY ["./backend/src/BleepGuard.Application\BleepGuard.Application.csproj", "/CensorshipService/"]
COPY ["./backend/src/BleepGuard.Infrastructure\BleepGuard.Infrastructure.csproj", "/CensorshipService/"]

RUN dotnet restore "CensorshipService"

RUN dotnet build "CensorshipService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CensorshipService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CensorshipService.Api.dll"]
