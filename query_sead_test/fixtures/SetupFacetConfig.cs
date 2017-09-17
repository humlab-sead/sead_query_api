using QuerySeadDomain;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;

namespace QuerySeadTests.fixtures
{
    public class SetupFacetsConfig
    {

        public IContainer Container { get; set; }
        public IUnitOfWork Context { get; set; }

        public SetupFacetsConfig()
        {
            Container = new TestDependencyService().Register();
            Context = Container.Resolve<IUnitOfWork>();
        }


        public FacetsConfig2 GenerateFacetsConfig(string targetCode, string triggerCode, List<FacetConfig2> facetConfigs)
        {
            return new FacetsConfig2(Context)
            {
                RequestId = "1",
                Language = "en_GB",
                TargetCode = targetCode,
                RequestType = "populate",
                TriggerCode = triggerCode,
                FacetConfigs = facetConfigs
            };
        }

        public FacetConfig2 GenerateFacetConfig(string facetCode, int position, List<FacetConfigPick> picks = null, string filter = "")
        {
            return new FacetConfig2(Context)
            {
                FacetCode = facetCode,
                Position = position,
                StartRow = 0,
                RowCount = 150,
                TextFilter = filter,
                Picks = picks ?? new List<FacetConfigPick>()
            };
        }

        public List<FacetConfigPick> GenerateDiscreteFacetPicks(List<int> ids)
        {
            return ids.Select(z => new FacetConfigPick(EFacetPickType.discrete, z, z.ToString())).ToList();
        }

        //public List<FacetConfigPick> GenerateRangeFacetPicks(decimal lower, decimal upper)
        //{
        //    return new List<FacetConfigPick>() {
        //        new FacetConfigPick(EFacetPickType.lower, lower, lower.ToString())),
        //        new FacetConfigPick(EFacetPickType.lower, lower, lower.ToString())),
        //    };
        //}

        public FacetsConfig2 GenerateSingleFacetsConfigWithoutPicks(string facetCode)
        {
            return GenerateFacetsConfig(facetCode, facetCode, new List<FacetConfig2>() { GenerateFacetConfig(facetCode, 0, new List<FacetConfigPick>()) });
        }

        //public string GetJSON()
        //{
        //        dynamic jsonObject = new JObject();
        //        jsonObject.A = "a value";
        //        jsonObject.B = "b value";

        //        /*            JObject o = JObject.Parse(@"{
        //                      'CPU': 'Intel',
        //                      'Drives': [
        //                        'DVD read/writer',
        //                        '500 gigabyte hard drive'
        //                      ]
        //                    }");

        //                    JObject o = JObject.FromObject(new
        //                    {
        //                        channel = new
        //                        {
        //                            title = "James Newton-King",
        //                            link = "http://james.newtonking.com",
        //                            description = "James Newton-King's blog.",
        //                            item =
        //                                from p in posts
        //                                orderby p.Title
        //                                select new
        //                                {
        //                                    title = p.Title,
        //                                    description = p.Description,
        //                                    link = p.Link,
        //                                    category = p.Categories
        //                                }
        //                        }
        //                    });
        //        */

        //   }
    }
}
