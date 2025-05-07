using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace SQT.Deprecated;


public static class Utility
{

    public static DumpOptions GetDefaultDumpOptions() => new DumpOptions()
    {
        DumpStyle = DumpStyle.CSharp,
        IndentSize = 1,
        IndentChar = '\t',
        LineBreakChar = Environment.NewLine,
        SetPropertiesOnly = false,
        MaxLevel = 10, // int.MaxValue,
        ExcludeProperties = new HashSet<string>() { "Facets", "Tables", "Facet", "DomainFacet", "TargetFacet" },
        PropertyOrderBy = null,
        IgnoreDefaultValues = false
    };

    public static string Dump(object instance, DumpOptions options = null)
    {
        options ??= GetDefaultDumpOptions();
        var data = ObjectDumper.Dump(instance, options);
        return data;
    }

    public static void Dump(object instance, string filename, DumpOptions options = null)
    {
        options ??= GetDefaultDumpOptions();
        var data = ObjectDumper.Dump(instance, options);
        using (StreamWriter file = new StreamWriter(filename))
        {
            file.Write(data);
        }
    }

    // public static void DumpUriObject(string uri, object value, string folder)
    // {
    //     dynamic expando = new ExpandoObject();
    //     expando.Uri = uri;
    //     expando.ValueType = value.GetType().Name;
    //     expando.Value = value;
    //     Utility.Dump(expando, Path.Combine(folder, $"{value.GetType().Name}_{uri.Replace(":", "#").Replace("/", "+")}.cs"));
    // }
}
