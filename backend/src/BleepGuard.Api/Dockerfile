FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BleepGuard.Api/BleepGuard.Api.csproj", "BleepGuard.Api/"]
RUN dotnet restore "BleepGuard.Api/BleepGuard.Api.csproj"
COPY . .
WORKDIR "/src/BleepGuard.Api"
RUN dotnet build "BleepGuard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BleepGuard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BleepGuard.Api.dll"]
