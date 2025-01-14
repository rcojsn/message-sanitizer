FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["./backend/src/AdminService/AdminService.Api/AdminService.Api.csproj", "/AdminService/"]
COPY ["./backend/src/AdminService/AdminService.Application/AdminService.Application.csproj", "/AdminService/"]
COPY ["./backend/src/AdminService/AdminService.Domain/AdminService.Domain.csproj", "/AdminService/"]
COPY ["./backend/src/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj", "/AdminService/"]
COPY ["./backend/src/AdminService/AdminService.Contracts/AdminService.Contracts.csproj", "/AdminService/"]
COPY ["./backend/src/BleepGuard.Application/BleepGuard.Application.csproj", "/AdminService/"]
COPY ["./backend/src/BleepGuard.Contracts/BleepGuard.Contracts.csproj", "/AdminService/"]
COPY ["./backend/src/BleepGuard.Infrastructure/BleepGuard.Infrastructure.csproj", "/AdminService/"]

RUN dotnet restore "AdminService"

RUN dotnet build "AdminService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminService.Api.dll"]
