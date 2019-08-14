using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IFacetRepository : IRepository<FacetDefinition, int>
    {
        IEnumerable<FacetDefinition> FindThoseWithAlias();
        string GenerateStateId();
        FacetDefinition GetByCode(string facetCode);
        IEnumerable<FacetDefinition> GetOfType(EFacetType type);
        (decimal, decimal) GetUpperLowerBounds(FacetDefinition facet);
        Dictionary<string, FacetDefinition> ToDictionary();
    }


    public interface IFacetTypeRepository : IRepository<FacetType, int>
    {
    }

    public interface IFacetGroupRepository : IRepository<FacetGroup, int>
    {
    }

}