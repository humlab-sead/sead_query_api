using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class ViewAbundance
    {
        public int? AnalysisEntityId { get; set; }
        public int? TaxonId { get; set; }
        public string ElementsPartMod { get; set; }
        public int? Abundance { get; set; }
    }
}
