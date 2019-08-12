# query_sead_api_core
.NET Core port of Query SEAD API

# Dependencies / technologhy stack

- Autofac
- Autofac.Extras.AggregateService
- Redis e.g. StackExchange.Redis 
- Microsoft.Extensions.Caching.Redis
- Npgsql.EntityFrameworkCore.PostgreSQL

# Redis install
Windows:
1. Run "Install-Package Redis-64" (https://www.nuget.org/packages/Redis-64/)
   The package is installed in %HOMEPATH%\.nuget\packages\redis-64
2. Run cmd and "CD %HOMEPATH%\.nuget\packages\redis-64\3.0.503\tools" (replace version number with installed version)
-. Edit conf-files
3. Run "redis-server /path/to/config.con" to start server - accept appropriate firewall exceptions - uses default config (found in tools-folder).
-. Please make sure interface is bound to 127.0.0.1 in config file before running the server.
-.     "redis-server --help" for command line help
-.     "redis-server --service-install redis.windows-service.conf" too install as service
-.     "redis-server --service-uninstall"

ex:

%HOMEPATH%\.nuget\packages\redis-64\3.0.503\tools\redis-server.exe %HOMEPATH%\.nuget\packages\redis-64\3.0.503\tools\redis.windows.conf

Debian: Download source code and compile it on target machine, install as daemon (see doc)


