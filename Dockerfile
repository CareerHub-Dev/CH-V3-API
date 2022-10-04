FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY CH-V3-API.sln ./
COPY Domain/*.csproj ./Domain/
COPY Application/*.csproj ./Application/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Application.IntegrationTests/*.csproj ./Application.IntegrationTests/
COPY Application.UnitTests/*.csproj ./Application.UnitTests/
COPY WebUI/*.csproj ./WebUI/

RUN dotnet restore
COPY . .

WORKDIR /src/Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/Application
RUN dotnet build -c Release -o /app

WORKDIR /src/Infrastructure
RUN dotnet build -c Release -o /app

WORKDIR /src/WebUI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebUI.dll"]