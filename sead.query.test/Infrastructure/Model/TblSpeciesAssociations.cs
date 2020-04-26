using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSpeciesAssociations
    {
        public int SpeciesAssociationId { get; set; }
        public int AssociatedTaxonId { get; set; }
        public int? BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int TaxonId { get; set; }
        public int? AssociationTypeId { get; set; }
        public string ReferencingType { get; set; }

        public virtual TblSpeciesAssociationTypes AssociationType { get; set; }
        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
