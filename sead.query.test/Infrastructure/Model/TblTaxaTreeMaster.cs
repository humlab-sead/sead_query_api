using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaTreeMaster
    {
        public TblTaxaTreeMaster()
        {
            TblAbundances = new HashSet<TblAbundances>();
            TblDatingMaterial = new HashSet<TblDatingMaterial>();
            TblEcocodes = new HashSet<TblEcocodes>();
            TblImportedTaxaReplacements = new HashSet<TblImportedTaxaReplacements>();
            TblMcrdataBirmbeetledat = new HashSet<TblMcrdataBirmbeetledat>();
            TblRdb = new HashSet<TblRdb>();
            TblSpeciesAssociations = new HashSet<TblSpeciesAssociations>();
            TblTaxaCommonNames = new HashSet<TblTaxaCommonNames>();
            TblTaxaImages = new HashSet<TblTaxaImages>();
            TblTaxaMeasuredAttributes = new HashSet<TblTaxaMeasuredAttributes>();
            TblTaxaReferenceSpecimens = new HashSet<TblTaxaReferenceSpecimens>();
            TblTaxaSeasonality = new HashSet<TblTaxaSeasonality>();
            TblTaxaSynonyms = new HashSet<TblTaxaSynonyms>();
            TblTaxonomicOrder = new HashSet<TblTaxonomicOrder>();
            TblTaxonomyNotes = new HashSet<TblTaxonomyNotes>();
            TblTextBiology = new HashSet<TblTextBiology>();
            TblTextDistribution = new HashSet<TblTextDistribution>();
            TblTextIdentificationKeys = new HashSet<TblTextIdentificationKeys>();
        }

        public int TaxonId { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? GenusId { get; set; }
        public string Species { get; set; }

        public virtual TblTaxaTreeAuthors Author { get; set; }
        public virtual TblTaxaTreeGenera Genus { get; set; }
        public virtual TblMcrNames TblMcrNames { get; set; }
        public virtual TblMcrSummaryData TblMcrSummaryData { get; set; }
        public virtual ICollection<TblAbundances> TblAbundances { get; set; }
        public virtual ICollection<TblDatingMaterial> TblDatingMaterial { get; set; }
        public virtual ICollection<TblEcocodes> TblEcocodes { get; set; }
        public virtual ICollection<TblImportedTaxaReplacements> TblImportedTaxaReplacements { get; set; }
        public virtual ICollection<TblMcrdataBirmbeetledat> TblMcrdataBirmbeetledat { get; set; }
        public virtual ICollection<TblRdb> TblRdb { get; set; }
        public virtual ICollection<TblSpeciesAssociations> TblSpeciesAssociations { get; set; }
        public virtual ICollection<TblTaxaCommonNames> TblTaxaCommonNames { get; set; }
        public virtual ICollection<TblTaxaImages> TblTaxaImages { get; set; }
        public virtual ICollection<TblTaxaMeasuredAttributes> TblTaxaMeasuredAttributes { get; set; }
        public virtual ICollection<TblTaxaReferenceSpecimens> TblTaxaReferenceSpecimens { get; set; }
        public virtual ICollection<TblTaxaSeasonality> TblTaxaSeasonality { get; set; }
        public virtual ICollection<TblTaxaSynonyms> TblTaxaSynonyms { get; set; }
        public virtual ICollection<TblTaxonomicOrder> TblTaxonomicOrder { get; set; }
        public virtual ICollection<TblTaxonomyNotes> TblTaxonomyNotes { get; set; }
        public virtual ICollection<TblTextBiology> TblTextBiology { get; set; }
        public virtual ICollection<TblTextDistribution> TblTextDistribution { get; set; }
        public virtual ICollection<TblTextIdentificationKeys> TblTextIdentificationKeys { get; set; }
    }
}
