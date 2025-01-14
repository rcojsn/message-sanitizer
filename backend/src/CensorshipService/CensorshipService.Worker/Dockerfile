FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CensorshipService/CensorshipService.Worker/CensorshipService.Worker.csproj", "src/CensorshipService/CensorshipService.Worker/"]
COPY ["src/CensorshipService/CensorshipService.Application/CensorshipService.Application.csproj", "src/CensorshipService/CensorshipService.Application/"]
COPY ["src/AdminService/AdminService.Domain/AdminService.Domain.csproj", "src/AdminService/AdminService.Domain/"]
COPY ["src/BleepGuard.Contracts/BleepGuard.Contracts.csproj", "src/BleepGuard.Contracts/"]
COPY ["src/CensorshipService/CensorshipService.Infrastructure/CensorshipService.Infrastructure.csproj", "src/CensorshipService/CensorshipService.Infrastructure/"]
COPY ["src/BleepGuard.Application/BleepGuard.Application.csproj", "src/BleepGuard.Application/"]
COPY ["src/BleepGuard.Infrastructure/BleepGuard.Infrastructure.csproj", "src/BleepGuard.Infrastructure/"]
RUN dotnet restore "src/CensorshipService/CensorshipService.Worker/CensorshipService.Worker.csproj"
COPY . .
WORKDIR "/src/src/CensorshipService/CensorshipService.Worker"
RUN dotnet build "CensorshipService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CensorshipService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CensorshipService.Worker.dll"]
