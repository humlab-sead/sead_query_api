//using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    #region __SQL field compilers__

    public interface ISqlFieldCompiler
    {
        string Compile(string expr);
    }

#endregion
#region IResultSqlQueryCompiler
#endregion
}