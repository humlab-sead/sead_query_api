using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IFacetRepository : IRepository<Facet, int>
    {
        IEnumerable<Facet> FindThoseWithAlias();
        // string GenerateStateId();
        Facet GetByCode(string facetCode);
        IEnumerable<Facet> GetOfType(EFacetType type);
        (decimal, decimal) GetUpperLowerBounds(Facet facet);
        IEnumerable<Facet> Parents();
        IEnumerable<Facet> Children(string facetCode);
    }


    public interface IFacetTypeRepository : IRepository<FacetType, int>
    {
    }

    public interface IFacetGroupRepository : IRepository<FacetGroup, int>
    {
    }

    public interface IFacetTableRepository : IRepository<FacetTable, int>
    {
        IEnumerable<FacetTable> FindThoseWithAlias();
        Dictionary<string, FacetTable> AliasTablesDict();
    }
}