FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BleepGuard.CensorshipWorker/BleepGuard.CensorshipWorker.csproj", "src/BleepGuard.CensorshipWorker/"]
RUN dotnet restore "src/BleepGuard.CensorshipWorker/BleepGuard.CensorshipWorker.csproj"
COPY . .
WORKDIR "/src/src/BleepGuard.CensorshipWorker"
RUN dotnet build "BleepGuard.CensorshipWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BleepGuard.CensorshipWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BleepGuard.CensorshipWorker.dll"]
