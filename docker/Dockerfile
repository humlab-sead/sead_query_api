FROM mcr.microsoft.com/dotnet/core/aspnet:8.0-bullseye-slim AS repo

RUN apt-get update && apt-get install -y \
  git

ARG BRANCH

WORKDIR /repo

RUN echo "info: using tag $BRANCH" \
    && git clone https://github.com/humlab-sead/sead_query_api.git --branch $BRANCH --depth 1

FROM mcr.microsoft.com/dotnet/core/sdk:8.0-bullseye-slim AS build

WORKDIR /src

COPY --from=repo ["/repo/sead_query_api/sead.query.api/sead.query.api.csproj", "sead.query.api/"]
COPY --from=repo ["/repo/sead_query_api/sead.query.core/sead.query.core.csproj", "sead.query.core/"]
COPY --from=repo ["/repo/sead_query_api/sead.query.infra/sead.query.infra.csproj", "sead.query.infra/"]

RUN dotnet restore "sead.query.api/sead.query.api.csproj"

COPY --from=repo /repo/sead_query_api .
COPY conf /src/conf

RUN cd sead.query.api \
    && dotnet build   sead.query.api.csproj -c Release \
    && dotnet publish sead.query.api.csproj -c Release -o /src/app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:8.0-bullseye-slim

LABEL MAINTAINER Roger Mähler <roger dot mahler at umu dot se>

WORKDIR /data
WORKDIR /app

COPY --from=build /src/app .

ENTRYPOINT ["dotnet", "sead.query.api.dll"]
