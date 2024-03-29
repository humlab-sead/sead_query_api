using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace SeadQueryCore
{
    public class FacetConfigPick
    {
        private static CultureInfo cultureInfo = new CultureInfo("en-US");

        public string PickValue { get; set; }
        public string Text { get; set; }

        public FacetConfigPick()
        {
        }

        [JsonConstructor]
        public FacetConfigPick(string value, string text)
        {
            PickValue = value;
            Text = text;
        }

        public FacetConfigPick(string value) : this(value, value)
        {
        }

        public FacetConfigPick(int value) : this(value.ToString(), value.ToString())
        {
        }

        public FacetConfigPick(decimal value)
            : this(value.ToString(cultureInfo), value.ToString(cultureInfo))
        {
        }

        public decimal ToDecimal()
        {
            var cultureInfo = new CultureInfo("en-US");
            return decimal.Parse(PickValue, NumberStyles.Any, cultureInfo);
        }

        public int ToInt()
        {
            return int.Parse(PickValue);
        }

        public static List<FacetConfigPick> CreateByList(List<int> ids)
        {
            return ids.Select(z => new FacetConfigPick(z)).ToList();
        }

    }
}
