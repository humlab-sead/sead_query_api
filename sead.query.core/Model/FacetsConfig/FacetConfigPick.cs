using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace SeadQueryCore
{
    public class FacetConfigPick
    {
        private static readonly CultureInfo cultureInfo = new CultureInfo("en-US");

        public string PickValue { get; set; }
        public string Text { get; set; } = "";

        /// <summary>
        /// Index of the polygon (ring) that this pick belongs to. Only relevant for geopolygon facets,
        /// where the flat pick list is a concatenation of one or more polygons' coordinate pairs.
        /// Defaults to 0 i.e. all picks belong to the same (single) polygon.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int PolygonIndex { get; set; } = 0;

        public FacetConfigPick()
        {
        }

        [JsonConstructor]
        public FacetConfigPick(string value, string text = "")
        {
            PickValue = value;
            Text = text;
        }

        public FacetConfigPick(decimal value, int polygonIndex) : this(value)
        {
            PolygonIndex = polygonIndex;
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
