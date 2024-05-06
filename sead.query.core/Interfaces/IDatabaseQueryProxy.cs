using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore
{
    public interface ITypedQueryProxy
    {
        DbContext Context { get; }

        List<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2);
        T QueryRow<T>(string sql, Func<IDataReader, T> selector = null);
        List<T> QueryScalars<T>(string sql);
        List<T> QueryRows<T>(string sql, Func<IDataReader, T> selector);
    }

    public interface IDynamicQueryProxy
    {
        DbContext Context { get; }
        IDataReader Query(string sql);
    }
}
