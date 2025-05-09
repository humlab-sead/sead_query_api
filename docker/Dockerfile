# Base image for cloning the repository
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim AS repo

RUN apt-get update && apt-get install -y git && rm -rf /var/lib/apt/lists/*
ARG BRANCH
WORKDIR /repo
RUN git clone https://github.com/humlab-sead/sead_query_api.git --branch $BRANCH --depth 1

# SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
WORKDIR /src

ARG BUILD_MODE=Release

# Copying only csproj files and doing dotnet restore separately to take advantage of Docker cache
COPY --from=repo /repo/sead_query_api/sead.query.api/sead.query.api.csproj sead.query.api/
COPY --from=repo /repo/sead_query_api/sead.query.core/sead.query.core.csproj sead.query.core/
COPY --from=repo /repo/sead_query_api/sead.query.infra/sead.query.infra.csproj sead.query.infra/
RUN dotnet restore "sead.query.api/sead.query.api.csproj"
COPY --from=repo /repo/sead_query_api .

RUN dotnet publish "sead.query.api/sead.query.api.csproj" -c $BUILD_MODE -o /app/publish --no-restore

# RUN cd sead.query.api \
#     && dotnet build   sead.query.api.csproj -c $BUILD_MODE \
#     && dotnet publish sead.query.api.csproj -c $BUILD_MODE -o /src/app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim

LABEL MAINTAINER Roger Mähler <roger dot mahler at umu dot se>

RUN mkdir -p /data /app /app/logs /app/config

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sead.query.api.dll"]
