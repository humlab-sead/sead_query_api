using System;
using Microsoft.Extensions.Hosting;

namespace SeadQueryAPI.Cli;

public static class ProgramEntryPoint
{
    public static int Run(string[] args)
    {
        return Run(args, parseCommand: null, dispatchCommand: null, hostFactory: null);
    }

    public static int Run(
        string[] args,
        Func<string[], StartupCommand> parseCommand,
        Func<StartupCommand, Func<string[], IHost>, int> dispatchCommand,
        Func<string[], IHost> hostFactory
    )
    {
        ArgumentNullException.ThrowIfNull(args);

        parseCommand ??= values => new StartupCommandParser().Parse(values);
        dispatchCommand ??= StartupCommandDispatcher.Run;
        hostFactory ??= Program.CreateHost;

        var command = parseCommand(args);

        return dispatchCommand(command, hostFactory);
    }
}
