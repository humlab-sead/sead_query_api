using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace SeadQueryCore
{
    public class FacetConfigPick
    {

        private static CultureInfo cultureInfo = new CultureInfo("en-US");

        public EPickType PickType { get; set; }
        public string PickValue { get; set; }
        public string Text { get; set; }

        public FacetConfigPick()
        {
        }

        [JsonConstructor]
        public FacetConfigPick(EPickType type, string value, string text)
        {
            PickType = type;
            PickValue = value;
            Text = text;
        }

        public FacetConfigPick(EPickType type, string value) : this(type, value, value)
        {
        }

        public FacetConfigPick(EPickType type, int value) : this(type, value.ToString(), value.ToString())
        {
        }

        public FacetConfigPick(EPickType type, decimal value)
            : this(type, value.ToString(cultureInfo), value.ToString(cultureInfo))
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

        public static List<FacetConfigPick> CreateDiscrete(List<int> ids)
        {
            return ids.Select(z => new FacetConfigPick(EPickType.discrete, z)).ToList();
        }

        public static List<FacetConfigPick> CreateLowerUpper(decimal lower, decimal upper)
        {
            return new List<FacetConfigPick>() {
                new FacetConfigPick(EPickType.lower, lower),
                new FacetConfigPick(EPickType.upper, upper)
            };
        }
    }
}
