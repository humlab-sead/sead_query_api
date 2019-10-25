using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class FacetConfig2 {

        public string FacetCode { get; set; } = "";
        public int Position { get; set; } = 0;
        public string TextFilter { get; set; } = "";

        public List<FacetConfigPick> Picks { get; set; }

        [JsonIgnore]
        public Facet Facet { get; set; }

        [JsonConstructor]
        public FacetConfig2()
        {
        }

        public FacetConfig2(Facet facet, int position, string filter, List<FacetConfigPick> picks)
        {
            // FacetCode = facetCode;
            FacetCode = facet.FacetCode;
            Facet = facet;
            Position = position;
            TextFilter = filter;
            Picks = picks ?? new List<FacetConfigPick>();
        }

        public bool HasPicks() => (Picks?.Count ?? 0) > 0;
        public void ClearPicks() => Picks.Clear();

        public List<decimal> GetPickValues(bool sort = false)
        {
            List<decimal> values = Picks.Select(x => x.ToDecimal()).ToList();
            if (sort)
                values.Sort();
            return values;
        }

        public List<string> GetJoinTables()
        {
            // FIXME  Bugg? Alias should never be treated as table in joins??
            var tables = Facet.ExtraTables.Select(z => z.TableOrUdfName).ToList();
            tables.Add(Facet.ResolvedName);
            return tables;
        }

    }
}
