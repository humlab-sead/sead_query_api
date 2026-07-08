using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SeadQueryAPI.Cli;

public sealed class CommandHostRunner
{
    private readonly Func<string[], IHost> _hostFactory;

    public CommandHostRunner(Func<string[], IHost> hostFactory)
    {
        _hostFactory = hostFactory ?? throw new ArgumentNullException(nameof(hostFactory));
    }

    public int RunScoped<TCommand>(string[] hostArgs, Action<TCommand> action)
        where TCommand : notnull
    {
        ArgumentNullException.ThrowIfNull(hostArgs);
        ArgumentNullException.ThrowIfNull(action);

        using var host = _hostFactory(hostArgs);
        using var scope = host.Services.CreateScope();

        var command = scope.ServiceProvider.GetRequiredService<TCommand>();
        action(command);

        return 0;
    }
}
