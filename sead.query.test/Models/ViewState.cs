using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ViewState
    {
        public string ViewStateKey { get; set; }
        public string ViewStateData { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
