FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BleepGuard.Api/BleepGuard.Api.csproj", "src/BleepGuard.Api/"]
RUN dotnet restore "src/BleepGuard.Api/BleepGuard.Api.csproj"
COPY . .
WORKDIR "/src/src/BleepGuard.Api"
RUN dotnet build "BleepGuard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BleepGuard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BleepGuard.Api.dll"]
