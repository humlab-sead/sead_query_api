
// /* TestStartup.cs*/
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Autofac;
// using Autofac.Extensions.DependencyInjection;


// /* Fixture.cs */
// using DotNet.Testcontainers.Builders;
// using DotNet.Testcontainers.Containers;
// using DotNet.Testcontainers.Configurations;
// using System.Threading.Tasks;
// using Xunit;


// public class PostgresTestcontainerFixture : IAsyncLifetime
// {
//     public PostgreSqlTestcontainer Container { get; private set; }
//     public string ConnectionString => Container.ConnectionString;

//     public async Task InitializeAsync()
//     {
//         var config = new PostgreSqlTestcontainerConfiguration
//         {
//             Database = "testdb",
//             Username = "postgres",
//             Password = "password",
//         };

//         Container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
//             .WithDatabase(config)
//             .WithImage("postgres:15-alpine")
//             .WithCleanUp(true)
//             .Build();

//         await Container.StartAsync();
//     }

//     public async Task DisposeAsync()
//     {
//         await Container.StopAsync();
//     }
// }

// /* TestStartup.cs*/

// public class TestStartup : Startup
// {
//     private readonly string _testConnectionString;

//     public TestStartup(IConfiguration configuration, string testConnectionString)
//         : base(configuration)
//     {
//         _testConnectionString = testConnectionString;
//     }

//     public override void ConfigureContainer(ContainerBuilder builder)
//     {
//         base.ConfigureContainer(builder);

//         // Here you override your application's services for testing
//         builder.RegisterInstance(new DbOptions { ConnectionString = _testConnectionString })
//                .AsSelf()
//                .SingleInstance();

//         // Or if you register DbContext, replace it
//         builder.Register(ctx =>
//         {
//             var options = new DbContextOptionsBuilder<YourDbContext>();
//             options.UseNpgsql(_testConnectionString);
//             return new YourDbContext(options.Options);
//         }).AsSelf().InstancePerLifetimeScope();
//     }
// }
