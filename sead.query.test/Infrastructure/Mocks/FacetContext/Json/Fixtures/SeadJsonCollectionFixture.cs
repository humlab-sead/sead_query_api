using System.Data.Common;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using SQT.Mocks;
using SQT.Scaffolding;
using Xunit;

namespace SQT.Infrastructure
{
    public class SeadJsonFacetContextFixture : JsonFacetContextDataFixture
    {
        public SeadJsonFacetContextFixture() : base(ScaffoldUtility.GetDataFolder("Json"))
        {
        }
    }

    [CollectionDefinition("SeadJsonFacetContextFixture")]
    public class SeadJsonCollectionFixture : ICollectionFixture<SeadJsonFacetContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class SqliteJsonFacetContext : InMemoryFacetContext
    {
        public SqliteJsonFacetContext(string jsonFolder) : this(
            new SqliteConnectionFactory().CreateDbContextOptionsAsync2().GetAwaiter().GetResult(),
            jsonFolder
            )
        {
        }
        public SqliteJsonFacetContext((DbContextOptions options, DbConnection connection) args, string jsonFolder) : base(
            args.options,
            new JsonFacetContextDataFixture(ScaffoldUtility.GetDataFolder(jsonFolder)),
            args.connection
        )
        {
        }
        public SqliteJsonFacetContext((DbContextOptions options, DbConnection connection, JsonFacetContextDataFixture fixture) args) : base(
            args.options,
            args.fixture,
            args.connection
        )
        {
        }
    }

    public class SqliteFacetContext : SqliteJsonFacetContext
    {
        public SqliteFacetContext() : base("Json")
        {
        }
    }

    [CollectionDefinition("SqliteFacetContext")]
    public class SqliteFacetContextCollection : ICollectionFixture<SqliteFacetContext>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }


    public class StudyDbSqliteFacetContext : SqliteJsonFacetContext
    {
        public StudyDbSqliteFacetContext() : base("StudyDb/Json")
        {
        }
    }

    [CollectionDefinition("StudyDbSqliteFacetContext")]
    public class StudyDbSqliteFacetContextCollection : ICollectionFixture<StudyDbSqliteFacetContext>
    {
    }

}
