using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore
{
    public interface IDatabaseQueryProxy
    {
        DbContext Context { get; }

        IDataReader Query(string sql);
        List<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2);
        T QueryRow<T>(string sql, Func<IDataReader, T> selector = null);
        List<T> QueryRows<T>(string sql, Func<IDataReader, T> selector);
    }
}