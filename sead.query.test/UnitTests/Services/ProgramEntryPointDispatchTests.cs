using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SeadQueryAPI.Cli;
using SeadQueryAPI.Services;
using SeadQueryCore;
using Xunit;

namespace SQT.UnitTests.Services;

public class ProgramEntryPointDispatchTests
{
    [Fact]
    public void Run_WithValidationCommand_DispatchesToValidationServiceUsingHostFactory()
    {
        var importer = new Mock<IFacetRouteConfigurationImporter>(MockBehavior.Strict);
        importer
            .Setup(service => service.ValidateFile(It.IsAny<string>()))
            .Verifiable();

        var services = new ServiceCollection();
        services.AddSingleton(importer.Object);
        services.AddTransient<FacetRouteConfigurationValidationCommand>();

        var serviceProvider = services.BuildServiceProvider();

        string[] receivedHostArgs = null;
        IHost HostFactory(string[] hostArgs)
        {
            receivedHostArgs = hostArgs;
            return new TestHost(serviceProvider);
        }

        var configurationPath = "sead.query.composer/Templates/route_v1.yaml";
        var command = new ValidateFacetConfigCommand(configurationPath, ["--urls", "http://localhost:5000"]);

        var exitCode = ProgramEntryPoint.Run(
            ["ignored-by-injected-parser"],
            _ => command,
            StartupCommandDispatcher.Run,
            HostFactory
        );

        exitCode.Should().Be(0);
        receivedHostArgs.Should().Equal("--urls", "http://localhost:5000");
        importer.Verify(service => service.ValidateFile(Path.GetFullPath(configurationPath)), Times.Once);
    }

    private sealed class TestHost : IHost
    {
        public TestHost(IServiceProvider services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceProvider Services { get; }

        public void Dispose()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
