FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base

LABEL MAINTAINER Roger Mähler <roger dot mahler at umu dot se>

RUN apt-get update && apt-get install -y \
  git

WORKDIR /app

EXPOSE 8089

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build

WORKDIR /src

RUN git clone https://github.com/humlab-sead/sead_query_api.git

WORKDIR /src/sead_query_api

#COPY ["sead.query.api/sead.query.api.csproj", "sead.query.api/"]
#COPY ["sead.query.core/sead.query.core.csproj", "sead.query.core/"]
#COPY ["sead.query.infra/sead.query.infra.csproj", "sead.query.infra/"]

RUN ls -al \
    && dotnet restore "sead.query.api/sead.query.api.csproj"

#COPY . .
WORKDIR /src/sead_query_api/sead.query.api
RUN dotnet build "sead.query.api.csproj" -c Release -o /src/app

FROM build AS publish
RUN dotnet publish "sead.query.api.csproj" -c Release -o /src/app

FROM base AS final
WORKDIR /app

ENV PGPASSFILE="/app/.pgpass"

COPY --from=publish /src/app .
COPY ./conf/api/appsettings.Release.json /app/appsettings.json
COPY ./conf/api/hosting.json /app/hosting.json

ENTRYPOINT ["dotnet", "sead.query.api.dll"]
