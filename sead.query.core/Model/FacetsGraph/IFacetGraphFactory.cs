using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, Table>;
    using NodesDictI = Dictionary<int, Table>;
    using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

    public interface IFacetGraphFactory
    {
        IFacetsGraph Build();

    }
}