using SeadQueryCore.Model;
using System.Collections.Generic;

namespace SeadQueryTest.Mocks
{

    internal static class ResultConfigFactory
    {
        public static ResultConfig Create(string viewTypeId, List<string> resultKeys, string sessionId = "1")
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

        public static ResultConfig Create(string viewTypeId, string resultKey, string sessionId = "1")
        {
            return Create(viewTypeId,  new List<string>() { resultKey }, sessionId);
        }
    }
}
