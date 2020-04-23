using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SeadQueryCore
{
    public interface IRepositoryRegistry : IDisposable
    {
        ITableRelationRepository TableRelations { get; }
        IFacetRepository Facets { get; }
        ITableRepository Tables { get; }
        IResultRepository Results { get; }
        IFacetGroupRepository FacetGroups { get; }
        IFacetTypeRepository FacetTypes { get; }
        IFacetTableRepository FacetTables { get; }
        IViewStateRepository ViewStates { get; }

        int Commit();

    }

}
