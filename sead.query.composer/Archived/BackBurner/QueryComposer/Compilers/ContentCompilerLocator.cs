using System;
using Autofac.Features.Indexed;

namespace SeadQueryComposer.QueryComposer.Compilers;

// TODOC: Move this to infrastructure project

/// <summary>
/// Implementation using Autofac's IIndex for service location
/// </summary>
public class ContentCompilerLocator(IIndex<EFacetType, IContentCompiler> resolvers) : IContentCompilerLocator
{
    private readonly IIndex<EFacetType, IContentCompiler> _resolvers = resolvers;

    public IContentCompiler GetResolver(EFacetType facetType)
    {
        if (_resolvers.TryGetValue(facetType, out var resolver))
        {
            return resolver;
        }

        throw new NotSupportedException($"No resolver found for facet type: {facetType}");
    }
}
