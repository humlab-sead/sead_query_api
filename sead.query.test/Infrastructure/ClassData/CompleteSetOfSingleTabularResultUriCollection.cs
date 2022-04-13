using KellermanSoftware.CompareNetObjects;
using System.Collections;
using System.Collections.Generic;

namespace SQT.ClassData
{

    public class CompleteSetOfSingleTabularResultUriCollection : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var uri in UriCollections.CompleteSetOfSingleDomainFacetUrisWithNoPicks)
            {
                yield return new object[] { uri, "result_facet", "site_level", "tabular" };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CompleteSetOfSingleMapResultUriCollection : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var uri in UriCollections.CompleteSetOfSingleDomainFacetUrisWithNoPicks)
            {
                yield return new object[] { uri, "map_result", "site_level", "map" };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
