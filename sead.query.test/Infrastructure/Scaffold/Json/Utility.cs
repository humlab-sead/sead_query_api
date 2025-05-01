using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using System.Collections.Generic;

namespace SQT.Scaffolding.Json;

public static class Utility
{

    public static DbContextOptionsBuilder<FacetContext> GetDbContextOptionBuilder(string hostName, string databaseName)
    {
        var defaultSettings = new Dictionary<string, string>
        {
            { "QueryBuilderSetting:Store:Host",     hostName     },
            { "QueryBuilderSetting:Store:Database", databaseName }
        };
        var connectionString = ConnectionStringFactory.Create(defaultSettings);
        var optionsBuilder = new DbContextOptionsBuilder<FacetContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder;
    }


}
