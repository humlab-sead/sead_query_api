namespace SeadQueryCore;

public interface IFacetRouteConfigurationImporter
{
    void ValidateFile(string filePath);

    void ImportFromFile(string filePath);
}
