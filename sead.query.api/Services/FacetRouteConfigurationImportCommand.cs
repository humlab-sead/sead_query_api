using System;
using System.IO;
using SeadQueryCore;

namespace SeadQueryAPI.Services;

public sealed class FacetRouteConfigurationImportCommand
{
    private readonly IFacetRouteConfigurationImporter _importer;

    public FacetRouteConfigurationImportCommand(IFacetRouteConfigurationImporter importer)
    {
        _importer = importer ?? throw new ArgumentNullException(nameof(importer));
    }

    public void Run(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Facet route configuration import requires a file path.", nameof(filePath));

        _importer.ImportFromFile(Path.GetFullPath(filePath));
    }
}
