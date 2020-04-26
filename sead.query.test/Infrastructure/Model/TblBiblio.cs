using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblBiblio
    {
        public TblBiblio()
        {
            TblAggregateDatasets = new HashSet<TblAggregateDatasets>();
            TblDatasetMasters = new HashSet<TblDatasetMasters>();
            TblDatasets = new HashSet<TblDatasets>();
            TblEcocodeSystems = new HashSet<TblEcocodeSystems>();
            TblGeochronRefs = new HashSet<TblGeochronRefs>();
            TblMethods = new HashSet<TblMethods>();
            TblRdbSystems = new HashSet<TblRdbSystems>();
            TblRelativeAgeRefs = new HashSet<TblRelativeAgeRefs>();
            TblSampleGroupReferences = new HashSet<TblSampleGroupReferences>();
            TblSiteOtherRecords = new HashSet<TblSiteOtherRecords>();
            TblSiteReferences = new HashSet<TblSiteReferences>();
            TblSpeciesAssociations = new HashSet<TblSpeciesAssociations>();
            TblTaxaSynonyms = new HashSet<TblTaxaSynonyms>();
            TblTaxonomicOrderBiblio = new HashSet<TblTaxonomicOrderBiblio>();
            TblTaxonomyNotes = new HashSet<TblTaxonomyNotes>();
            TblTephraRefs = new HashSet<TblTephraRefs>();
            TblTextBiology = new HashSet<TblTextBiology>();
            TblTextDistribution = new HashSet<TblTextDistribution>();
            TblTextIdentificationKeys = new HashSet<TblTextIdentificationKeys>();
        }

        public int BiblioId { get; set; }
        public string BugsReference { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Doi { get; set; }
        public string Isbn { get; set; }
        public string Notes { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Authors { get; set; }
        public string FullReference { get; set; }
        public string Url { get; set; }

        public virtual ICollection<TblAggregateDatasets> TblAggregateDatasets { get; set; }
        public virtual ICollection<TblDatasetMasters> TblDatasetMasters { get; set; }
        public virtual ICollection<TblDatasets> TblDatasets { get; set; }
        public virtual ICollection<TblEcocodeSystems> TblEcocodeSystems { get; set; }
        public virtual ICollection<TblGeochronRefs> TblGeochronRefs { get; set; }
        public virtual ICollection<TblMethods> TblMethods { get; set; }
        public virtual ICollection<TblRdbSystems> TblRdbSystems { get; set; }
        public virtual ICollection<TblRelativeAgeRefs> TblRelativeAgeRefs { get; set; }
        public virtual ICollection<TblSampleGroupReferences> TblSampleGroupReferences { get; set; }
        public virtual ICollection<TblSiteOtherRecords> TblSiteOtherRecords { get; set; }
        public virtual ICollection<TblSiteReferences> TblSiteReferences { get; set; }
        public virtual ICollection<TblSpeciesAssociations> TblSpeciesAssociations { get; set; }
        public virtual ICollection<TblTaxaSynonyms> TblTaxaSynonyms { get; set; }
        public virtual ICollection<TblTaxonomicOrderBiblio> TblTaxonomicOrderBiblio { get; set; }
        public virtual ICollection<TblTaxonomyNotes> TblTaxonomyNotes { get; set; }
        public virtual ICollection<TblTephraRefs> TblTephraRefs { get; set; }
        public virtual ICollection<TblTextBiology> TblTextBiology { get; set; }
        public virtual ICollection<TblTextDistribution> TblTextDistribution { get; set; }
        public virtual ICollection<TblTextIdentificationKeys> TblTextIdentificationKeys { get; set; }
    }
}
