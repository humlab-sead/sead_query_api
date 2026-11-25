# Built from the checkout it sits in: the build context is the repository root, so
# this builds whatever is checked out - sead-deployment checks out the release's tag.
#   podman build -f docker/Dockerfile .

# SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
WORKDIR /src

ARG BUILD_MODE=Release

# Copying only csproj files and doing dotnet restore separately to take advantage of Docker cache
COPY sead.query.api/sead.query.api.csproj sead.query.api/
COPY sead.query.core/sead.query.core.csproj sead.query.core/
COPY sead.query.infra/sead.query.infra.csproj sead.query.infra/
RUN dotnet restore "sead.query.api/sead.query.api.csproj"
COPY . .

RUN dotnet publish "sead.query.api/sead.query.api.csproj" -c $BUILD_MODE -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim

LABEL MAINTAINER Roger Mähler <roger dot mahler at umu dot se>

RUN mkdir -p /data /app /app/logs /app/config

WORKDIR /app
COPY --from=build /app/publish .

# What was built, as 'git describe' put it; reported by sead-deployment's 'deploy.sh versions'
ARG SOURCE_VERSION=
ENV SOURCE_VERSION=${SOURCE_VERSION}

ENTRYPOINT ["dotnet", "sead.query.api.dll"]
