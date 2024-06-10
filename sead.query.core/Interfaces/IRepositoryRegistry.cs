using System;

namespace SeadQueryCore;

public interface IRepositoryRegistry : IDisposable
{
    IFacetContext Context { get; }
    IEdgeRepository Relations { get; }
    IFacetRepository Facets { get; }
    INodeRepository Tables { get; }
    IResultSpecificationRepository Results { get; }
    IFacetGroupRepository FacetGroups { get; }
    IFacetTypeRepository FacetTypes { get; }
    IFacetTableRepository FacetTables { get; }
    IViewStateRepository ViewStates { get; }

    int Commit();

}
