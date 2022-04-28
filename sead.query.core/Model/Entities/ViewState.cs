using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{
    public class ViewState
    {
        public string Key { get; set; }
        public string Data { get; set; }
    }
}
