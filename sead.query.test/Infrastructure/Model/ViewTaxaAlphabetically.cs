using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class ViewTaxaAlphabetically
    {
        public int? OrderId { get; set; }
        public string Order { get; set; }
        public int? FamilyId { get; set; }
        public string Family { get; set; }
        public int? GenusId { get; set; }
        public string Genus { get; set; }
        public int? TaxonId { get; set; }
        public string Species { get; set; }
        public int? AuthorId { get; set; }
        public string Author { get; set; }
    }
}
