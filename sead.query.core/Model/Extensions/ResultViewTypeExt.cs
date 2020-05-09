using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore {
    public static class ResultViewTypeExt
    {
        public static IResultSqlCompiler GetSqlCompiler(this ResultViewType viewType)
        {
            return (IResultSqlCompiler)Activator.CreateInstance(
                Type.GetType($"SeadQueryCore.{viewType.SqlCompiler}"), viewType
            );
        }
    }
}
