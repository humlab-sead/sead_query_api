FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["sead.query.api/sead.query.api.csproj", "sead.query.api/"]
COPY ["sead.query.core/sead.query.core.csproj", "sead.query.core/"]
RUN dotnet restore "sead.query.api/sead.query.api.csproj"
COPY . .
WORKDIR "/src/sead.query.api"
RUN dotnet build "sead.query.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "sead.query.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sead.query.api.dll"]
