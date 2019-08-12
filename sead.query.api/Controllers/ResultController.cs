using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeadQueryCore;
using SeadQueryCore.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SeadQueryAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        public IUnitOfWork Context { get; private set; }
        private Services.ILoadResultService ResultService { get; set; }

        public ResultController(IUnitOfWork context, Services.ILoadResultService resultService)
        {
            Context = context;
            ResultService = resultService;
        }

        // GET api/values
        [HttpGet("definition")]
        public IEnumerable<ResultAggregate> Get()
        {
            return Context.Results.GetAll().ToList();
        }

        // GET api/values/5
        [HttpGet("definition/{id}")]
        public ResultAggregate Get(int id)
        {
            return Context.Results.Get(id);
        }

        [HttpPost("load")]
        [Produces("application/json", Type = typeof(ResultContentSet))]
        [Consumes("application/json")]
        public ResultContentSet Load([FromBody]JObject data)
        {
            FacetsConfig2 facetsConfig = data["facetsConfig"].ToObject<FacetsConfig2>();
            ResultConfig resultConfig = data["resultConfig"].ToObject<ResultConfig>();
            facetsConfig.SetContext(Context);
            var result = ResultService.Load(facetsConfig, resultConfig);

            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            //string json = JsonConvert.SerializeObject(result, settings);
            foreach (var z in result.Data.DataCollection)
                for (var i = 0; i < z.Length; i++)
                    if (z[i] == DBNull.Value)
                        z[i] = null;
            return result;
        }

        //[HttpPost("load3")]
        //[Produces("application/json", Type = typeof(ResultContentSet))]
        //[Consumes("application/json")]
        //public string /*ResultContentSet*/ Load3([FromBody]JObject data) //[FromBody]FacetsConfig2 facetsConfig, [FromBody]ResultConfig resultConfig)
        //{
        //    FacetsConfig2 facetsConfig = data["facetsConfig"].ToObject<FacetsConfig2>();
        //    ResultConfig resultConfig = data["resultConfig"].ToObject<ResultConfig>();
        //    facetsConfig.SetContext(Context);
        //    var result = ResultService.Load(facetsConfig, resultConfig);

        //    var settings = new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore,
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };
        //    string json = JsonConvert.SerializeObject(result, settings);
        //    return json;
        //    //return result;
        //}
    }

    //[JsonConverter(typeof(CustomDBNullConverter))]
    class CustomDBNullConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue.GetType().Name == "DBNull") {
                return null;
            }
            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DBNull);
        }
    }

    //public static class BugFixExtensions
    //{
    //    public static string[][] AsStringArrays(this object[][] objs)
    //    {
    //        var strs = new string[objs.Length][];
    //        for (var i = 0; i < objs.Length; i++) {
    //            strs[i] = new string[objs[i].Length];
    //            for (var j = 0; j < objs[i].Length; j++) {
    //                try {
    //                    var flag = objs[i][j].GetType().Name == "DBNull";
    //                    strs[i][j] = flag ? null : objs[i][j].ToString();
    //                } catch (Exception) {
    //                    return strs;
    //                }
    //            }
    //        }
    //        return strs;
    //    }
    //}
}
