using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Compilers;

/// <summary>
/// Locator service for finding the appropriate facet predicate resolver based on facet type.
/// Uses Autofac's IIndex for dependency injection and service location.
/// </summary>
public interface IContentCompilerLocator
{
    /// <summary>
    /// Gets the appropriate resolver for the given facet type
    /// </summary>
    /// <param name="facetType">The type of facet</param>
    /// <returns>The resolver for that facet type</returns>
    IContentCompiler GetResolver(EFacetType facetType);
}
