using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSpeciesAssociationTypes
    {
        public TblSpeciesAssociationTypes()
        {
            TblSpeciesAssociations = new HashSet<TblSpeciesAssociations>();
        }

        public int AssociationTypeId { get; set; }
        public string AssociationTypeName { get; set; }
        public string AssociationDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblSpeciesAssociations> TblSpeciesAssociations { get; set; }
    }
}
