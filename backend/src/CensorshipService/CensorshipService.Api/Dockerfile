FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CensorshipService/CensorshipService.Api/CensorshipService.Api.csproj", "src/CensorshipService/CensorshipService.Api/"]
RUN dotnet restore "src/CensorshipService/CensorshipService.Api/CensorshipService.Api.csproj"
COPY . .
WORKDIR "/src/src/CensorshipService/CensorshipService.Api"
RUN dotnet build "CensorshipService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CensorshipService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CensorshipService.Api.dll"]
