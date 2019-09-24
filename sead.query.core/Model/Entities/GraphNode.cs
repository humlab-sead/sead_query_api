using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    public class GraphNode {
        public int NodeId { get; set; }
        public string TableName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as GraphNode);
        }

        public bool Equals(GraphNode obj)
        {
            return obj != null && TableName == obj.TableName;
        }

        public override int GetHashCode()
        {
            return TableName.GetHashCode();
        }
    }
}
