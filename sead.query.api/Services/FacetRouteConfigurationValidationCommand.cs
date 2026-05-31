using System;
using System.IO;
using SeadQueryCore;

namespace SeadQueryAPI.Services;

public sealed class FacetRouteConfigurationValidationCommand
{
    private readonly IFacetRouteConfigurationImporter _importer;

    public FacetRouteConfigurationValidationCommand(IFacetRouteConfigurationImporter importer)
    {
        _importer = importer ?? throw new ArgumentNullException(nameof(importer));
    }

    public void Run(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Facet route configuration validation requires a file path.", nameof(filePath));

        _importer.ValidateFile(Path.GetFullPath(filePath));
    }
}
