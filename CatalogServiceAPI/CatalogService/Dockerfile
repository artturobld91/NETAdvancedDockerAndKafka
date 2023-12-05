#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CatalogServiceAPI/CatalogService/CatalogService.csproj", "CatalogServiceAPI/CatalogService/"]
COPY ["CatalogServiceAPI/CatalogService.Application/CatalogService.Application.csproj", "CatalogServiceAPI/CatalogService.Application/"]
COPY ["CatalogServiceAPI/CatalogService.Domain/CatalogService.Domain.csproj", "CatalogServiceAPI/CatalogService.Domain/"]
COPY ["CatalogServiceAPI/CatalogService.Infrastructure/CatalogService.Infrastructure.csproj", "CatalogServiceAPI/CatalogService.Infrastructure/"]
RUN dotnet restore "CatalogServiceAPI/CatalogService/CatalogService.csproj"
COPY . .
WORKDIR "/src/CatalogServiceAPI/CatalogService"
RUN dotnet build "CatalogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatalogService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.dll"]