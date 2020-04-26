using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblColours
    {
        public TblColours()
        {
            TblSampleColours = new HashSet<TblSampleColours>();
        }

        public int ColourId { get; set; }
        public string ColourName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int MethodId { get; set; }
        public int? Rgb { get; set; }

        public virtual TblMethods Method { get; set; }
        public virtual ICollection<TblSampleColours> TblSampleColours { get; set; }
    }
}
