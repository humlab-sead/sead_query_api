using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ViewAbundancesByTaxonAnalysisEntity
    {
        public int? TaxonId { get; set; }
        public int? AnalysisEntityId { get; set; }
        public int? AbundanceM3 { get; set; }
        public int? AbundanceM8 { get; set; }
        public int? AbundanceM111 { get; set; }
    }
}
