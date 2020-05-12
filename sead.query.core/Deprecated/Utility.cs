using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace SeadQueryCore
{
    public static class Utility {


        public static string ToJson(object value)
        {
            var resolver = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(value, Formatting.Indented, resolver);
        }

        public static void SaveAsJson(object value, string file_prefix, string logDir)
        {
            var timestamp = DateTime.Now.ToString("yyyyddM_HHmmss");
            var filename = Path.Combine(logDir, string.Format("{0}_{1}.json", file_prefix, timestamp));
            string data = Utility.ToJson(value);
            File.WriteAllText(filename, data);
        }
    }
}
