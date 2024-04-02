
using System;
using System.Data;

namespace SeadQueryCore;

public static class IDataReaderExtensions
{

    public static string Category2String(this IDataReader x, int ordinal)
    {
        var type_name = x.GetDataTypeName(ordinal);
        if (type_name == "numeric")
            return String.Format("{0:0.####}", x.GetDecimal(ordinal));
        if (type_name == "text")
            return x.GetString(ordinal);
        return x.GetInt32(ordinal).ToString();
    }
}

