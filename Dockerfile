FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /portfolio
EXPOSE 5116

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Portfolio.API/Portfolio.API.csproj", "Portfolio.API/"]
COPY ["Portfolio.Models/Portfolio.Models.csproj", "Portfolio.Models/"]
COPY ["Portfolio.Repository/Portfolio.Repository.csproj", "Portfolio.Repository/"]
COPY ["Portfolio.Service/Portfolio.Service.csproj", "Portfolio.Service/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Portfolio.API/Portfolio.API.csproj"
COPY . .
WORKDIR "/src/Portfolio.API"
RUN dotnet build "Portfolio.API.csproj" -c $BUILD_CONFIGURATION -o /portfolio/build

RUN dotnet tool install --global dotnet-ef

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Portfolio.API.csproj" -c $BUILD_CONFIGURATION -o /portfolio/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /portfolio
COPY --from=publish /portfolio/publish .
ENTRYPOINT ["dotnet", "Portfolio.API.dll"]