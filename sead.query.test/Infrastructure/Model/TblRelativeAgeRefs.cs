using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRelativeAgeRefs
    {
        public int RelativeAgeRefId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int RelativeAgeId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblRelativeAges RelativeAge { get; set; }
    }
}
