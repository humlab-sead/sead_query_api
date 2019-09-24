using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    public struct Key2Value<K, V>
    {
        public Key2Value(K k, V v1, V v2)
        {
            Key = k; Value1 = v1; Value2 = v2;
        }
        K Key { get; set; }
        V Value1 { get; set; }
        V Value2 { get; set; }
    }
}
