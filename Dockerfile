FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-stretch-slim AS repo

ARG BRANCH=master

RUN apt-get update && apt-get install -y \
  git

WORKDIR /repo

ADD https://api.github.com/repos/humlab-sead/sead_query_api/git/refs/heads/$BRANCH version.json
RUN git clone -b$BRANCH https://github.com/humlab-sead/sead_query_api.git

COPY appsettings.Production.json /repo/sead_query_api/conf/appsettings.Production.json
#COPY hosting.json /repo/sead_query_api/conf/hosting.json

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build

WORKDIR /src

COPY --from=repo ["/repo/sead_query_api/sead.query.api/sead.query.api.csproj", "sead.query.api/"]
COPY --from=repo ["/repo/sead_query_api/sead.query.core/sead.query.core.csproj", "sead.query.core/"]
COPY --from=repo ["/repo/sead_query_api/sead.query.infra/sead.query.infra.csproj", "sead.query.infra/"]

RUN dotnet restore "sead.query.api/sead.query.api.csproj"

COPY --from=repo /repo/sead_query_api .

RUN cd sead.query.api \
    && dotnet build   sead.query.api.csproj -c Release \
    && dotnet publish sead.query.api.csproj -c Release -o /src/app --no-restore
    #dotnet test    "sead.query.api.csproj" -c Release -o /src/app --no-build --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-stretch-slim

LABEL MAINTAINER Roger Mähler <roger dot mahler at umu dot se>

WORKDIR /data
WORKDIR /app

COPY --from=build /src/app .

ENTRYPOINT ["dotnet", "sead.query.api.dll"]
