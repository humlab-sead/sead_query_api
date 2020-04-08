using System;
using System.Collections.Generic;
using System.IO;

namespace SeadQueryCore
{
    public static class DumpUtility
    {
        public static DumpOptions GetDefaultDumpOptions() => new DumpOptions()
        {
            DumpStyle = DumpStyle.CSharp,
            IndentSize = 1,
            IndentChar = '\t',
            LineBreakChar = Environment.NewLine,
            SetPropertiesOnly = false,
            MaxLevel = 10, 
            ExcludeProperties = new HashSet<string>() {
            },
            PropertyOrderBy = null,
            IgnoreDefaultValues = false
        };

        public static string Dump(object instance, DumpOptions options = null)
        {
            options ??= GetDefaultDumpOptions();
            var data = ObjectDumper.Dump(instance, options);
            return data;
        }

        public static void Dump(string filename, DumpOptions options = null, params object[] instances)
        {
            options ??= GetDefaultDumpOptions();
            using (StreamWriter file = new StreamWriter(filename)) {
                foreach (var instance in instances) {
                    var data = ObjectDumper.Dump(instance, options);
                    file.Write($@"\n\n// {instance.GetType().Name}\n\n");
                    file.Write(data);
                }
            }
        }

        public static void Dump(
            string filename,
            bool setPropertiesOnly = false,
            int maxLevel = 10,
            ICollection<string> excludeProperties = null,
            bool ignoreDefaultValues = false,
            params object[] instances)
        {
            var options = GetDefaultDumpOptions();
            options.SetPropertiesOnly = setPropertiesOnly;
            options.MaxLevel = maxLevel;
            options.ExcludeProperties = excludeProperties;
            options.IgnoreDefaultValues = ignoreDefaultValues;
            using (StreamWriter file = new StreamWriter(filename)) {
                foreach (var instance in instances) {
                    var data = ObjectDumper.Dump(instance, options);
                    file.Write($@"\n\n// {instance.GetType().Name}\n\n");
                    file.Write(data);
                }
            }
        }
    }
}
