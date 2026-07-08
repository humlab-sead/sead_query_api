namespace SeadQueryAPI.Cli;

public abstract record StartupCommand(string[] HostArgs);

public sealed record RunWebHostCommand(string[] HostArgs) : StartupCommand(HostArgs);

public sealed record ImportFacetConfigCommand(string ConfigurationFilePath, string[] HostArgs) : StartupCommand(HostArgs);

public sealed record ValidateFacetConfigCommand(string ConfigurationFilePath, string[] HostArgs) : StartupCommand(HostArgs);

public sealed record PrintFacetSqlCommand(string FacetUrl, string[] HostArgs) : StartupCommand(HostArgs);

public sealed record PrintResultSqlCommand(string FacetUrl, string ViewTypeId, string ResultCode, string[] HostArgs)
    : StartupCommand(HostArgs);
