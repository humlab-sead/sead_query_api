using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using System.Collections.Generic;

namespace SQT.Scaffolding.Json;

public static class Utility
{
    public static DbContextOptionsBuilder<FacetContext> GetDbContextOptionBuilder(string hostName, string databaseName)
    {
        var connectionString = ConnectionStringFactory.Create(hostName, databaseName);
        var optionsBuilder = new DbContextOptionsBuilder<FacetContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder;
    }
}
