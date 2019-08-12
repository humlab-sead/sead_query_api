FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["query_sead_net/query.sead.api.csproj", "query_sead_net/"]
COPY ["query_sead_core/query.sead.domain.csproj", "query_sead_core/"]
RUN dotnet restore "query_sead_net/query.sead.api.csproj"
COPY . .
WORKDIR "/src/query_sead_net"
RUN dotnet build "query.sead.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "query.sead.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "query.sead.api.dll"]