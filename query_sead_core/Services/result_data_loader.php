<?php

error_reporting(E_ERROR | E_WARNING | E_PARSE); // | E_NOTICE);

require_once __DIR__ . "/config/environment.php";
require_once __DIR__ . "/config/bootstrap_application.php";
require_once __DIR__ . '/connection_helper.php';
require_once __DIR__ . '/facet_config.php';
require_once __DIR__ . '/result_sql_compiler.php';
require_once __DIR__ . '/category_distribution_loader.php';

class ResultDataLoader
{
    function __construct() {
    }

    public function load($facetsConfig, $resultConfig, $facetStateId)
    {
        $sql = $this->compileSql($facetsConfig, $resultConfig);
        if (empty($sql)) {
            return ['iterator' => NULL, 'payload' => NULL];
        }
        $data = ConnectionHelper::queryIter($sql);
        $extra = $this->getExtraPayload($facetsConfig, $resultConfig);
        return [ 'iterator' => $data, 'payload' => $extra ];
    }

    protected function getExtraPayload($facetsConfig, $resultConfig)
    {
        return NULL;
    }

    protected function compileSql($facetsConfig, $resultConfig)
    {
        return ResultSqlQueryCompiler::compile($facetsConfig, $resultConfig);
    }
}

class MapResultDataLoader extends ResultDataLoader {

    public $facetCode = NULL;
    public $extraLoader = NULL;

    function __construct() {
        parent::__construct();
        $this->facetCode = "map_result";
        $this->extraLoader = new DiscreteCategoryDistributionLoader();
    }

    private function loadDistribution($facetsConfig)
    {
        return $this->extraLoader->load($this->facetCode, $facetsConfig);
    }

    protected function getExtraPayload($facetsConfig, $resultConfig)
    {
        $data       = $this->loadDistribution($facetsConfig);
        $filtered   = $data['list'] ?: [ ];
        $unfiltered = $facetsConfig->hasPicks() ? $this->loadDistribution($facetsConfig->deleteUserPicks()) : $filteredDistribution;
        return [
            "filtered_count"    => $filtered,
            "un_filtered_count" => $unfiltered
        ];
    }

    protected function compileSql($facetsConfig, $resultConfig)
    {
        return MapResultSqlQueryCompiler::compile($facetsConfig, $this->facetCode);
    }
}

?>