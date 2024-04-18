
using System;
using System.Data;
namespace SeadQueryCore;

public static class IDataReaderExtensions
{

    public static string Category2String(this IDataReader x, int ordinal)
    {
        if (x.IsDBNull(ordinal))
            return null;
        var type_name = x.GetDataTypeName(ordinal);
        if (type_name == "numeric")
            return String.Format("{0:0.####}", x.GetDecimal(ordinal));
        if (type_name.StartsWith("text") || type_name.StartsWith("varchar") || type_name.StartsWith("char"))
            return x.GetString(ordinal);
        // if (type_name.Contains("geometry")){
        //     var geometry = x.GetFieldValue<PostgisGeometry>(ordinal);
        //     return geometry.AsText();
        // }

        return x.GetInt64(ordinal).ToString();
    }
}

