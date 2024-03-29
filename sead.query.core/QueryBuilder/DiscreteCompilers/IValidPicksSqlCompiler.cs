﻿using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IValidPicksSqlCompiler : ISqlCompiler
    {
        string Compile(QuerySetup query, List<int> picks);
    }
}