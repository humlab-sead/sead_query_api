using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    public class Table {
        public int TableId { get; set; }
        public string TableOrUdfName { get; set; }
        public string PrimaryKeyName { get; set; }
        public bool IsUdf { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Table);
        }

        public bool Equals(Table obj)
        {
            return obj != null && TableOrUdfName == obj.TableOrUdfName;
        }

        public override int GetHashCode()
        {
            return TableOrUdfName.GetHashCode();
        }
    }
}
