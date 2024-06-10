using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IFacetRepository : IRepository<Facet, int>
    {
        IEnumerable<Facet> GetAllUserFacets();
        IEnumerable<Facet> FindThoseWithAlias();
        Facet GetByCode(string facetCode);
        IEnumerable<Facet> GetOfType(EFacetType type);
        IEnumerable<Facet> Parents();
        IEnumerable<Facet> Children(string facetCode);
        Dictionary<string, Facet> ToDictionary();
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
        FacetTable GetByAlias(string aliasName);
    }
}
