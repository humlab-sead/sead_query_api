﻿//using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{

    public interface ICategoryBoundSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, string facetCode);
    }

}