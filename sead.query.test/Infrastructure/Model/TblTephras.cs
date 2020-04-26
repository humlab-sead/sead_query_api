using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTephras
    {
        public TblTephras()
        {
            TblTephraDates = new HashSet<TblTephraDates>();
            TblTephraRefs = new HashSet<TblTephraRefs>();
        }

        public int TephraId { get; set; }
        public decimal? C14Age { get; set; }
        public decimal? C14AgeOlder { get; set; }
        public decimal? C14AgeYounger { get; set; }
        public decimal? CalAge { get; set; }
        public decimal? CalAgeOlder { get; set; }
        public decimal? CalAgeYounger { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Notes { get; set; }
        public string TephraName { get; set; }

        public virtual ICollection<TblTephraDates> TblTephraDates { get; set; }
        public virtual ICollection<TblTephraRefs> TblTephraRefs { get; set; }
    }
}
