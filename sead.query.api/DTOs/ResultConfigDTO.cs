using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryAPI.DTO
{

    public class ResultConfigDTO
    {
        public string RequestId { get; set; }
        public string SessionId { get; set; }
        public string FacetCode { get; set; }
        public string ViewTypeId { get; set; }
        public List<string> AggregateKeys { get; set; } = new List<string>();

        public ResultConfigDTO()
        {
        }
    }
}
