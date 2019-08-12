using SeadQueryCore;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeadQueryTest.fixtures
{

    public class ResultConfigGenerator
    {

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
