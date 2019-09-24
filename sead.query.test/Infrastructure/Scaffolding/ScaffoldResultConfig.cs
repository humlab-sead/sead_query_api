using SeadQueryCore;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text.RegularExpressions;
using SeadQueryCore.Model;

namespace SeadQueryTest.Fixtures
{

    public class ScaffoldResultConfig
    {

        public ResultConfig Scaffold(string viewTypeId, List<string> resultKeys, string sessionId = "1")
        {
            var resultConfig =  new ResultConfig()
            {
                ViewTypeId = viewTypeId,
                RequestId = "1",
                SessionId = sessionId,
                AggregateKeys = resultKeys  // "site_level"
            };
            return resultConfig;
        }

        public ResultConfig GenerateConfig(string viewTypeId, string resultKey, string sessionId = "1")
        {
            return Scaffold(viewTypeId,  new List<string>() { resultKey }, sessionId);
        }


    }
}
