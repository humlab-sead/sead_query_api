using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SeadQueryCore
{

    public interface IRepositoryRegistry : IDisposable
    {

        IEdgeRepository Edges { get; }
        IFacetRepository Facets { get; }
        INodeRepository Nodes { get; }
        IResultRepository Results { get; }
        IFacetGroupRepository FacetGroups { get; }
        IFacetTypeRepository FacetTypes { get; }
        IViewStateRepository ViewStates { get; }


        int Commit();

        List<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2);

        T                     QueryRow<T>(string sql, Func<DbDataReader, T> selector = null);

        List<T>               QueryRows<T>(string sql, Func<DbDataReader, T> selector);

        DbDataReader          Query(string sql);

    }

}
