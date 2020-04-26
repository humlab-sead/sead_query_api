using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblLanguages
    {
        public TblLanguages()
        {
            TblTaxaCommonNames = new HashSet<TblTaxaCommonNames>();
        }

        public int LanguageId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string LanguageNameEnglish { get; set; }
        public string LanguageNameNative { get; set; }

        public virtual ICollection<TblTaxaCommonNames> TblTaxaCommonNames { get; set; }
    }
}
