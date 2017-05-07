using QuerySeadDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;

namespace QuerySeadAPI.Services {

    using IResultServiceIndex = IIndex<EResultViewType, IResultService>;

    public interface ILoadResultService {
        FacetResult Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }

    public class LoadResultService : AppServiceBase, ILoadResultService {

        public IResultServiceIndex ContentServices { get; private set; }

        public LoadResultService(IQueryBuilderSetting config, IUnitOfWork context, IQueryCache cache,
            IResultServiceIndex services) : base(config, context, cache) {
            ContentServices = services;
        }

        public FacetResult Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var cacheId = facetsConfig.GetCacheId();
            var resultCacheId = resultConfig.GetCacheId(facetsConfig);
            var facetResult = ResultCache.Get(resultCacheId);
            //deleteBogusPicks()
            var isCacheable = resultConfig.ViewType == EResultViewType.Tabular;
            if (facetResult == null) {
                facetResult = ContentServices[resultConfig.ViewType].Load(facetsConfig, resultConfig, cacheId);
                ConfigCache.Put(cacheId, facetsConfig);
                if (isCacheable)
                    ResultCache.Put(resultCacheId, facetResult);
            }
            var pickData = facetsConfig.CollectUserPicks();
            return facetResult;

            //if (empty(serialized_data)) {
            //    //        facetCacheId  = $_REQUEST['facet_state_id'];
            //    //        if (empty($cache_id)) {
            //    //            facetCacheId = CacheIdGenerator::generateFacetStateId();
            //    //            CacheHelper::put_facet_xml($cache_id, self::getFacetXml());
            //    //        }
            //    //        CacheHelper::put_result_xml($cache_id, self::getResultXml());
            //    data = loader.load(facetsConfig, resultConfig, facetCacheId);
            //    serialized_data = serializer.serialize(data['iterator'], facetsConfig, resultConfig, facetCacheId, data['payload']);
            //}


        }
    }
//    class LoadResultHelper {

//    private static $loaders = [
//        "map" => "MapResultDataLoader",
//        "mapxml" => "MapResultDataLoader",
//        "listxml" => "ResultDataLoader",
//        "listhtml" => "ResultDataLoader",
//        "list" => "ResultDataLoader"
//    ];

//    private static $cacheables = [ "map" => false, "mapxml" => false, "listxml" => false, "listhtml" => true, "list" => true ];

//    public static function getLoader($requestType)
//    {
//        $loader_class = self::$loaders[$requestType];
//        return new $loader_class();
//    }

//    public static function isCacheableResultData($requestType)
//    {
//        return self::$cacheables[$requestType];
//    }

//    public static function getFacetXml()
//    {
//        $facetCacheId = $_REQUEST['facet_state_id'];
//        return !empty($facetCacheId) ? CacheHelper::get_facet_xml($facetCacheId) : $_REQUEST['facet_xml'];
//    }

//    public static function getResultXml()
//    {
//        $facetCacheId = $_REQUEST['facet_state_id'];
//        return !empty($facetCacheId) ? CacheHelper::get_result_xml($facetCacheId) : $_REQUEST['result_xml'];
//    }

//    public static function cacheInputData()
//    {
//        $cache_id  = $_REQUEST['facet_state_id'];
//        if (empty($cache_id)) {
//            $cache_id = CacheIdGenerator::generateFacetStateId();
//            CacheHelper::put_facet_xml($cache_id, self::getFacetXml());
//        }
//        CacheHelper::put_result_xml($cache_id, self::getResultXml());
//        return $cache_id;
//    }
//}

// FIXME: Convert to JSON instead om XML
//header("Content-Type: text/xml");
//header("Character-Encoding: UTF-8");
//echo "<xml>";
//echo   "<response>";
//echo       $serialized_data;
//echo   "</response>";
//echo   "<current_selections>";
//echo       "<![CDATA[", $pick_data, "]]>";
//echo   "</current_selections>";
//echo   "<request_id>";
//echo       $resultConfig.request_id;
//echo   "</request_id>";
//echo "</xml>";


}