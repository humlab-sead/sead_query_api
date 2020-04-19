using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace SQT.Infrastructure
{
    public static class DataTableUtility
    {

        public static DataTable CreateDataTable(List<(string Name, Type Type)> fields, object[,] values)
        {
            var dt = new DataTable();
            foreach (var p in fields) {
                dt.Columns.Add(p.Name, p.Type);
            }
            for (var row = 0; row < values.GetLength(0); row++) {
                var dtrow = dt.NewRow();
                for (var column = 0; column < values.GetLength(1); column++) {
                    dtrow[column] = values[row, column];
                }
                dt.Rows.Add(dtrow);
            }
            return dt;
        }

    }
}
