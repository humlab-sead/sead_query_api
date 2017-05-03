# query_sead_api_core
.NET Core port of Query SEAD API

# Dependencies / technologhy stack

- Autofac
- Autofac.Extras.AggregateService
- StackExchange.Redis 
- Microsoft.Extensions.Caching.Redis
- Npgsql.EntityFrameworkCore.PostgreSQL

# Redis install
Windows:
1. Run "Install-Package Redis-64""
   The package is installed in %HOMEPATH%\.nuget\packages\redis-64
2. Run cmd and "CD %HOMEPATH%\.nuget\packages\redis-64\3.0.503\tools" (replace version number with installed version)
-. Edit conf-files
3. Run "redis-server" to start server - accept appropriate firewall exceptions - uses default config (found in tools-folder).
-.     "redis-server --help" for command line help
-.     "redis-server --service-install redis.windows-service.conf" too install as service
-.     "redis-server --service-uninstall"

Debian: Download source code and compile it on target machine, install as daemon (see doc)