﻿using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    public class ResultViewType
    {
        public string ViewTypeId { get; set; }
        public string ViewName { get; set; }
        public string ResultFacetCode { get; set; }
        public string SqlCompiler { get; set; }
        public string SpecificationKey { get; set; }
        public bool IsCachable { get; set; }
    }
}
