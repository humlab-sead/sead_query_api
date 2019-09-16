using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class FacetConfig2 {

        public string FacetCode { get; set; } = "";
        public int Position { get; set; } = 0;
        public string TextFilter { get; set; } = "";

        // FIXM Refactor away dependency to Context
        [JsonIgnore]
        public IRepositoryRegistry Context { get; set; }    // FIXME Remove dependecy to Context

        public List<FacetConfigPick> Picks { get; set; }

        [JsonIgnore]
        public Facet Facet { get => Context?.Facets?.GetByCode(FacetCode); }

        [JsonConstructor]
        public FacetConfig2(IRepositoryRegistry context)
        {
            Context = context;
        }

        public FacetConfig2(string facetCode, int position, string filter, List<FacetConfigPick> picks)
        {
            FacetCode = facetCode;
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
            var tables = Facet.ExtraTables.Select(z => z.ObjectName).ToList();
            tables.Add(Facet.ResolvedName);
            return tables;
        }

    }
}
