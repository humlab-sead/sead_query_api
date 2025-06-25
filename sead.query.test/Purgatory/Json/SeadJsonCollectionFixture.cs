using System.Data.Common;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using SQT.Mocks;
using SQT.Scaffolding;
using Xunit;

namespace SQT.Infrastructure
{

    [CollectionDefinition("InMemoryFacetContext")]
    public class InMemoryFacetContextCollection : ICollectionFixture<InMemoryFacetContext>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
    
    // [CollectionDefinition("SqliteFacetContextFixture")]
    // public class SqliteFacetContextFixtureCollection : ICollectionFixture<SqliteFacetContextFixture>
    // {
    //     // This class has no code, and is never created. Its purpose is simply
    //     // to be the place to apply [CollectionDefinition] and all the
    //     // ICollectionFixture<> interfaces.
    // }

    // public class StudyDbSqliteFacetContext : InMemoryFacetContext
    // {
    //     public StudyDbSqliteFacetContext() : base(ScaffoldUtility.GetInMemoryDataFolder("StudyDb/Json"))
    //     {
    //     }
    // }

    // [CollectionDefinition("StudyDbSqliteFacetContext")]
    // public class StudyDbSqliteFacetContextCollection : ICollectionFixture<StudyDbSqliteFacetContext>
    // {
    // }

}
