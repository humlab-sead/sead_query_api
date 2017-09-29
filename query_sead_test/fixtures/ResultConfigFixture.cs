using QuerySeadDomain;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;

namespace QuerySeadTests.fixtures
{
    
    public class TestResultConfig
    {
        /// <summary>
        /// Uri format: "taget-facet[@trigger-facet]:(facet-code[@picks])+
        /// </summary>
        public string UriConfig { get; set; }

        public List<List<string>> ExpectedRoutes { get; set; }
        public int ExpectedCount { get; set; }
        public List<(int, int)> ExpectedData { get; set; }

        // Derived properties
        public string TargetCode { get; set; }
        public string TriggerCode { get; set; }
        public Dictionary<string, List<int>> FacetCodes { get; set; }

        public TestResultConfig(string uriConfig, int expectedCount = 0)
        {
            UriConfig = uriConfig;
            ExpectedCount = expectedCount;
            var parts = uriConfig.Split(':').ToList();
            var codes = parts[0].Split("@");
            TargetCode = codes[0];
            TriggerCode = codes.Length > 1 ? codes[1] : TargetCode;
            FacetCodes = parts[1]
                .Split('/')
                .Where(z => z.Trim() != "")
                .Select(z => z.Split("@"))
                .Select(z => (FacetCode: z[0].Trim(), Picks: z.Length > 1 ? ParsePicks(z[1]) : null))
                .ToDictionary(z => z.FacetCode, z => z.Picks);
        }

        private List<int> ParsePicks(string data)
        {
            return data.Split(",").Select(z => Int32.TryParse(z.Trim(), out int x) ? x : -1).Where(z => z >= 0).ToList();
        }
    }

    public class ResultConfigFixture
    {

        public IContainer Container { get; set; }
        public IUnitOfWork Context { get; set; }
        public ResultConfigFixtureData Data { get; set; }

        public ResultConfigFixture()
        {
            Container = new TestDependencyService().Register();
            Context = Container.Resolve<IUnitOfWork>();
            Data = new ResultConfigFixtureData();
        }

        public ResultConfig GenerateConfig(string viewTypeId, List<string> resultKeys, string sessionId = "1")
        {
            var resultConfig =  new ResultConfig()
            {
                ViewTypeId = viewTypeId,
                RequestId = "1",
                SessionId = sessionId,
                AggregateKeys = resultKeys
            };
            return resultConfig;
        }

        public ResultConfig GenerateConfig(string viewTypeId, string resultKey, string sessionId = "1")
        {
            return GenerateConfig(viewTypeId,  new List<string>() { resultKey }, sessionId);
        }
    }
}
