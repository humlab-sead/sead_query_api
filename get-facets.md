create or replace function site_name_enhancer(site_name text, national_site_identifier text) returns text
as $body$
	declare raa_code text;
begin
	if national_site_identifier is null or length(national_site_identifier) = 0 then
		return site_name;
	end if;

	if site_name ilike '%raä%' then
		-- site_name = regexp_replace(site_name, 'raä.*', '', 'gi');
		-- return site_name || ' ' || national_site_identifier;
		return site_name;
	end if;
	raa_code = (regexp_matches(national_site_identifier, '(\d+(:\d+)?)', 'g')::text[])[1];
	if raa_code is null then
		return site_name;
	end if;
	return site_name || ' Raä ' || raa_code;
end $body$ language 'plpgsql';

select site_name, national_site_identifier, tbl_locations.* --, site_name_enhancer(site_name, national_site_identifier)
from tbl_sites
	join tbl_site_locations using (site_id)
	join tbl_locations using (location_id)
where national_site_identifier is not null
  and site_name not ilike '%raä%'
  and location_id = 205

Common tasks:
 - [x] Remove EPickType enum (not needed)
 - [x] Remove FacetConfigPick.CreateLowerUpper (not needed)
 - [x] Rename FacetConfigPick.CreateDiscrete to CreateByList

GeoFacets:
 - [ ] Create "site_polygon" facet
 - [x] Create "Within" utility compiler: (pointExpr, values) => string
 - [ ] Create "WithinPolygonPickCompiler"

Intersect Facet:
 - [x] Create "site_age_interval" facet
 - [ ] Create "Intersect" utility compiler: (rangeExpr, values) => string
 - [ ] Create "IntersectPickCompiler"

```php
    "map_result" =>
        array(
            "id_column" => "tbl_sites.site_id",
            "name_column" => "tbl_sites.site_name",
            "display_title" => "Site",
            "applicable" => "0",
            "icon_id_column" => "tbl_sites.site_id",
            "sort_column" => "tbl_sites.site_name",
            "facet_type" => "discrete",
            "table" => "tbl_sites",
            "query_cond_table" => "",
            "query_cond" => "",
            "default" => 0,
            "parents" => array("ROOT") //$facet_category[0]
        ),
```


```php
function get_geo_box_from_selection($current_selection_group) {
    $current_selection_group = (array) $current_selection_group;
    $count_boxes = 0;
    foreach ($current_selection_group as $key2 => $current_selection) {
        if (isset($current_selection)) {
            // loop through the different types of values  in the group
            foreach ($current_selection as $key5 => $selection_t) {

                $selection_t = (array) $selection_t;
                $this_selection_a[$selection_t["selection_type"]] = $selection_t["selection_value"];
            }

            //$query_where .= "$query_column>=".$this_selection_a["e1"]." and $query_column <=".$this_selection_a["n1"]." and ";

            $e1 = $this_selection_a["e1"];
            $n1 = $this_selection_a["n1"];
            $e2 = $this_selection_a["e2"];
            $n2 = $this_selection_a["n2"];
            $box_list[] = " SetSRID('BOX3D($e1 $n1,$e2 $n2)'::box3d,4326) ";
            $id_list[] = $this_selection_a["rectangle_id"];
            $count_boxes++;
        }
    }

    //	return $box_list;
    $return_obj["box_list"] = $box_list;
    $return_obj["id_list"] = $id_list;
    $return_obj["count_boxes"] = $count_boxes;

    return $return_obj;
}

/*
  function: get_geo_filter_clauses
  loop through selections and builds the sql for geo_filter, one or many areas (rectangles representative by 2 pair of coordinates
 */

function get_geo_filter_clauses($f_code, $skey, $current_selection_group) {
    global $facet_definition;

    //	print_r($current_selection_group);
    $box_list = get_geo_box_from_selection($current_selection_group);
    $box_list = $box_list["box_list"];
    $query_temp = "";

    $geo_column = $facet_definition[$skey]["geo_filter_column"];
    foreach ($box_list as $box_key => $box) {
        $query_temp.="\n $geo_column && " . $box . " and ST_Intersects( $geo_column," . $box . ") \n  or ";
    }

    $query_temp = substr($query_temp, 0, -3);
  //  $or_include=" or  $geo_column is null";
   $or_include="";

   $query_where.="(( " . $query_temp . " ) $or_include)  ";

    return $query_where;
}

```

```php
/*
  function: get_geo_count_query
  get sql-query for counting of geo-facet
  l
 */
function get_geo_count_query($facet_params, $f_code, $direct_count_table, $direct_count_column) {
    global $facet_definition;
    $f_selected = derive_selections($facet_params);
    $geo_selection = $f_selected[$f_code]["selection_group"];

    $data_tables[] = $direct_count_table;
    $f_list = derive_facet_list($facet_params);
    $mod_params = $facet_params;

    $mod_params["facet_collection"][$f_code]["selection_groups"] = ""; //empty the selections for geo // removed the geo_selection
    $query_table = $facet_definition[$f_code]["table"];
    $data_tables[] = $query_table;

    $temp_table = "temp_poly_" . md5($direct_count_column . derive_selections_string($facet_params));
    //maka tempory table with geo-filter only to make a temp output for the polygons
    $temp_geo_sql = "select * into temp " . $temp_table . " from  " . $facet_definition[$f_code]["table"];
    //get the selection as sql for the geo_selection
    $box_list = get_geo_box_from_selection($geo_selection);
    $box_list = $box_list["box_list"];

    $query_temp = "";
    if (isset($box_list)) {
        $query_temp = " where  (";
        foreach ($box_list as $box_key => $box) {

            $query_temp.="\n " . $facet_definition[$f_code]["geo_filter_column"] . " && " . $box . " and ST_Intersects( " . $facet_definition[$f_code]["geo_filter_column"] . "," . $box . ") \n  or ";
        }
        $query_temp = substr($query_temp, 0, -4); // remove last or
        $query_temp.=" ) ";
    }

    $temp_geo_sql.=$query_temp . "\n;";

    $query = get_query_clauses($facet_params, $f_code, $data_tables, $f_list);
    $query_modified = get_query_clauses($mod_params, $f_code, $data_tables, $f_list);
    //replace ships_polygons with temp out but with the alias "ships_polygons"
    $query["tables"] = str_replace($facet_definition[$f_code]["table"], $temp_table . " as " . $facet_definition[$f_code]["table"], $query["tables"]);
    // optimiziation replace ships_polygons with a subset of ships_polygon based on the area name that temp table as ships_polygon

    $new_sql = $temp_geo_sql; // add temp table query to the main query

    $sql_geo_boxes = "";
    $geo_boxes = get_geo_box_from_selection($geo_selection);

    $geo_box_id_list = $geo_boxes["id_list"];
    $geo_boxes = $geo_boxes["box_list"];
    foreach ($geo_boxes as $tkey => $the_geo_box) {
        $rect_id = $geo_box_id_list[$tkey];
        $sql_geo_boxes.=" select  '$rect_id'::text as rectangle_id, " . $the_geo_box . "    union    ";
    }

    $sql_geo_boxes = substr($sql_geo_boxes, 0, -10);

// make temp table for performance:
    $temp_table = "temp_" . md5(derive_selections_string($facet_params) . $direct_count_column);
    $new_sql.= "select * into temp $temp_table from  (" . $sql_geo_boxes . ") as geotemp2;";
    $new_sql.="select  rectangle_id as facet_term, area(setsrid),count($direct_count_column) as direct_count from  ";
    if ($query["joins"] != "") {
        $extra_join = $query["joins"] . "  ";
    }

    $new_sql.=" $temp_table, " . $query["tables"] . "  $extra_join  where " . $facet_definition[$f_code]["geo_filter_column"] . " && setsrid   ";
    if ($query_modified["where"] != '') {
        $new_sql.=" and  " . $query_modified["where"] . " ";
    }
    $new_sql.=" group by rectangle_id,setsrid";

    return SqlFormatter::format($new_sql,false);
}


/*
  Function:  get_facet_content

  function to obtain content of facets to the right of the current one
  Parameters:
  conn , database connection
  params - all view-state info.

  Returns:
 * facet query report, sql only
 * Selection in the particular  facets
 * start row of the rows requested
 * number of rows requested
 * total number of rows available
  row structure:
 *   row id
 *   display name
 *   (direct) count 1 number of geo-units or what is being defined in fb_def.php
 *   (indirect count 2 number of time-periods or  what is being defined in fb_def.php

  Preperation function:
  <fb_process_params>

  Functions used by interval facets:


  <get_range_query>

  Functions used by discrete and geo facets (geo):
  <get_discrete_query>

  function used all types of facet:
  <get_counts>

  Post functions:
  <build_xml_response>



 */

function get_facet_content($conn, $params) {

    global $last_facet_query, $direct_count_table, $direct_count_column, $indirect_count_table, $indirect_count_column,
    $facet_definition;

    $facet_content = $search_string = $query_column_name = "";
    $f_code = $params["requested_facet"];

    $query_column = $facet_definition[$f_code]["id_column"];
    $sort_column = $query_column;
    // compute the intervall for a number of histogram items if it is defined as range facet

    switch ($facet_definition[$f_code]["facet_type"]) {
        case "range":
            // get the limits from the client if existing otherwize get the min and max from the database
            // this is need to match the client handling of the intervals in facet.range.js
            // use the limits to define the size of the interval
            $limits = get_lower_upper_limit($conn, $params, $f_code);
            $min_value = $limits["lower"];
            $max_value = $limits["upper"];
            $interval_count = 120; // should it be lower maybe??? when there is no data
            $interval = floor(($limits["upper"] - $limits["lower"]) / $interval_count);
            if ($interval <= 0) {
                $interval = 1;
            }

            // derive the interval as sql-query although it is not using a table...only a long sql with select the values of the interval
            $q1 = get_range_query($interval, $min_value, $max_value, $interval_count);
            $interval_query = $q1; // use the query later to check how many obesveration for each item
            break;
        case "discrete":
            $q1 = get_discrete_query($params). "  ";
            break;
        case "geo":
            // compute boxes and how many observations in each rectangle in the map.
            $f_code = $params["requested_facet"];
            $f_selected = derive_selections($params);
            $geo_selection = $f_selected[$f_code]["selection_group"];
            $box_check = get_geo_box_from_selection($geo_selection); // use this function to check if there are any selections
            //print_r($box_check);
            if (($box_check["count_boxes"]) > 0)
                $q1 = get_geo_query($params);
            else
                $q1 = "select 'null' as id, 'null' as name ;";
            break;
    }

    $last_facet_query.="--- Facet load of $f_code \n" . $q1 . ";\n";

    if (($rs2 = pg_query($conn, $q1)) <= 0) {
        echo "Error: cannot execute query2. get facet content ".SqlFormatter::format($q1,false)."  \n";

        pg_close($conn);
        exit;
    }
//	echo  $q1;
    $facet_contents[$f_code]['f_code'] = $f_code;
    $facet_contents[$f_code]['range_interval'] = $interval;

//	echo $params['f_action'][1];
    $facet_contents[$f_code]['f_action'] = $params['f_action'][1];
    $facet_contents[$f_code]['start_row'] = $params[$f_code]['facet_start_row'];
    $facet_contents[$f_code]['rows_num'] = $params[$f_code]['facet_number_of_rows'];
    $facet_contents[$f_code]['total_number_of_rows'] = pg_numrows($rs2);

    pg_result_seek($rs2, 0);
    if (isset($direct_count_column) && !empty($direct_count_column)) {
        if ($facet_definition[$f_code]["facet_type"] == "range") {
            // use a special function of the counting in ranges since it using other parameters
            $direct_counts = get_range_counts($conn, $params, $interval_query, $f_code, $query, $direct_count_column, $direct_count_table);
            //print_r($direct_counts);
        } else {
            $direct_counts = get_counts($conn, $f_code, $params, $interval, $direct_count_table, $direct_count_column);

        }
    }
    // add extra information to a facet
    if (isset($facet_definition[$f_code]["extra_row_info_facet"])) {
        $extra_row_info = get_extra_row_info($facet_definition[$f_code]["extra_row_info_facet"], $conn, $params);
    }
    $count_of_selections = derive_count_of_selection($params, $f_code);
    if ($count_of_selections != 0) {
        $tooltip_text = derive_selections_to_html($params, $f_code);
    }
    else
        $tooltip_text = "";

    $tooltip_xml = "";

    $facet_contents[$f_code]['report'] = " Aktuella filter   <BR>  ". $tooltip_text."  ".SqlFormatter::format($q1,false)."  ; \n  ".$direct_counts["sql"]." ;\n";
    ;
    $facet_contents[$f_code]['report_html'] = $tooltip_text;
    $facet_contents[$f_code]['count_of_selections'] = $count_of_selections;
    $facet_contents[$f_code]['report_xml'] = $tooltip_xml;
    pg_result_seek($rs2, 0);
    $row_counter = 0;
    while ($row = pg_fetch_assoc($rs2)) {
        $au_id = $row["id"];

        // add values and values types
        // if range the use additional fields from DB

        switch ($facet_definition[$f_code]["facet_type"]) {
            case "range":
                $facet_contents[$f_code]['rows'][$row_counter]['values'] = array("lower" => $row["lower"], "upper" => $row["upper"]);
                $name_to_display = $row["lower"] . "  " . t("till", $params["client_language"]) . " " . $row["upper"];
//            $name_to_display=$row["lower"]." "."till". " ".$row["upper"];
                $facet_contents[$f_code]['report'].="\n" . $name_to_display;
                break;
            case "discrete":
                $facet_contents[$f_code]['rows'][$row_counter]['values'] = array("discrete" => $row["id"]);
                $name_to_display = $row["name"];
                if (!empty($extra_row_info)) {
                    $name_to_display.="(" . $extra_row_info . ")";
                }

                break;
            case "geo":
                $facet_contents[$f_code]['rows'][$row_counter]['values'] = array("discrete" => $row["id"]);
                $name_to_display = $row["id"];
                if (!empty($extra_row_info)) {
                    $name_to_display.="(" . $extra_row_info . ")";
                }
                break;
        }
        $facet_contents[$f_code]['rows'][$row_counter]['name'] = $name_to_display;
//		echo $name_to_display;
        if (isset($direct_counts["list"]["$au_id"]))
            $facet_contents[$f_code]['rows'][$row_counter]['direct_counts'] = $direct_counts["list"]["$au_id"];
        else
            $facet_contents[$f_code]['rows'][$row_counter]['direct_counts'] = "0";


        $row_counter++;
    }
    //2012-04-18 print_r($facet_contents[$f_code]);
    return $facet_contents;
}


//***********************************************************************************************************************************************************************
/*
  Function: get_counts

  Arguments:
 * table over which to do counting
 * column over which to do counting
 * interval for range facets

  Returns:
  associative array with counts, the keys are the facet_term i.e the unique id of the row

 */

function get_counts($conn, $f_code, $facet_params, $interval = 1, $direct_count_table, $direct_count_column) {

    global $last_facet_query, $facet_definition;
    $direct_counts = array();
    $combined_list = array();
    $row_counter = 1;
    $data_tables[] = $direct_count_table;
   // $query_table = $facet_definition[$f_code]["table"];
    $query_table =isset($facet_definition["alias_table"]) ? $facet_definition["alias_table"] :$facet_definition[$f_code]["table"];
    $data_tables[] = $query_table;
    $f_list = derive_facet_list($facet_params);

    //check if f_code exist in list, if not add it. since counting can be done also in result area and then using "abstract facet" that are not normally part of the list
    if (isset($f_list)) {
        if (!in_array($f_code, $f_list)) {
            $f_list[] = $f_code;
        }
    }


    // use the id's to compute direct resource counts
    // filter is not being used be needed as parameter
   // $query = get_query_clauses($facet_params, $f_code, $data_tables, $f_list);
    // echo $facet_definition[$f_code]["facet_type"];
    switch ($facet_definition[$f_code]["facet_type"]) {
        case "discrete":
           $count_facet="result_facet";
            $summarize_type="count";
            if (isset($facet_definition[$f_code]["count_facet"]))
            {
                $count_facet=$facet_definition[$f_code]["count_facet"];
            }
            if (isset($facet_definition[$f_code]["summarize_type"]))
            {
               $summarize_type=$facet_definition[$f_code]["summarize_type"];;
            }

            $q=get_discrete_count_query2($count_facet,$facet_params,$summarize_type);

           // $q = get_discrete_count_query($f_code, $query, $direct_count_column);
		//echo "QUERY ".$q;exit;
            break;
        case "geo":
            $f_selected = derive_selections($facet_params);
            $geo_selection = $f_selected[$f_code]["selection_group"];
            $count_facet="result_facet";
            $box_check = get_geo_box_from_selection($geo_selection); // use this function to check if there are any selections
            if ($box_check["count_boxes"] > 0 && isset($geo_selection)) {
                $q = get_geo_count_query($facet_params, $f_code, $direct_count_table, $direct_count_column);
            } else {
                $q = "select 'null' as facet_term, 0 as direct_count ;";
            }


            break;
    }

//	echo "count query ".$q;exit;
    $max_count = 0;
    $min_count = 99999999999999999;

    if (($rs = pg_exec($conn, $q)) <= 0) {
        echo "Error: cannot execute query2b. direct counts  ".SqlFormatter::format($q,false). "\n";
        pg_close($conn);
        exit;
    }
    while ($row = pg_fetch_assoc($rs)) {
        $facet_term = $row["facet_term"];
        if ($row["direct_count"] > $max_count)
            $max_count = $row["direct_count"];
        if ($row["direct_count"] < $min_count)
            $min_count = $row["direct_count"];

        $direct_counts["$facet_term"] = $row["direct_count"];
    }
    $last_facet_query.="-- direct counts query \n  $q; \n";

    $combined_list["list"] = $direct_counts;
    $combined_list["sql"]=  $q ;
    return $combined_list;
}


    public function get_query_information($edge_list,$numeric_edge_list,$lookup_list_tables, $facet_definition, $facet_params, $f_code, $extra_tables, $f_list) {

          global $list_of_alias_tables;
        $query = array();
        $table_list = array();
        $facet_selections = derive_selections($facet_params);
        //print_r($facet_params);
        if (isset($facet_definition[$f_code]["query_cond_table"]) && !empty($facet_definition[$f_code]["query_cond_table"])) {
            foreach ($facet_definition[$f_code]["query_cond_table"] as $cond_key => $cond_table) {
                $extra_tables[] = $cond_table;
            }
        }

        if (isset($f_list)) {
            $f_list_positions = array_flip($f_list);
            // the list needs to be set to be flipped key to values
        }

        $query_column = $query_where = "";

        if (isset($f_list)) {
            // list must exist, ie there must be some filters in order build a query
            foreach ($f_list as $pos => $facet) {
                //echo $facet."\n";
                if (isset($facet_selections[$facet])) {
                    while (list($skey1, $selection_group) = each($facet_selections[$facet])) {
                        // tricky condition here!2009-11-28
                        if ($f_list_positions[$f_code] > $f_list_positions[$facet] || ($facet_definition[$f_code]["facet_type"] == "range" &&
                                $f_list_positions[$f_code] == $f_list_positions[$facet]) || ($facet_definition[$f_code]["facet_type"] == "geo" &&
                                $f_list_positions[$f_code] == $f_list_positions[$facet]))
                            {               // only being affected to the down but always for itself for ranges since we need to update the histogram and map filter with new intervals

                            $table_with_selections=  isset($facet_definition[$facet]["alias_table"]) ? $facet_definition[$facet]["alias_table"] : $facet_definition[$facet]["table"] ;

                            switch ($facet_definition[$facet]["facet_type"]) {
                                case "range":
                                    $query_where.=get_range_selection_clauses($f_code, $facet, $selection_group)."      AND ";
                                    //echo count($facet_definition[$facet]["query_cond_table"]);
                                    if (!empty($facet_definition[$facet]["query_cond_table"]) )
                                    {
                                        foreach ($facet_definition[$facet]["query_cond_table"] as $cond_table)
                                        {
                                             $table_list[$cond_table] = true;
                                        }
                                    }
                                    $query_where_list[$table_with_selections][]=get_range_selection_clauses($f_code, $facet, $selection_group);
                                    $subselect_where[$table_with_selections]=get_range_selection_clauses($f_code, $facet, $selection_group). "   AND ";
                                    break;
                                case "discrete":
                                    // echo "antal selection ".count($sval);
                                    if (!empty($selection_group)) {
                                        $query_where.= get_discrete_selection_clauses($f_code, $facet, $selection_group). "     AND ";
                                         if (!empty($facet_definition[$facet]["query_cond_table"]) )
                                    {
                                        foreach ($facet_definition[$facet]["query_cond_table"] as $cond_table)
                                        {
                                             $table_list[$cond_table] = true;
                                        }
                                    }

                                        $query_where_list[$table_with_selections][]=get_discrete_selection_clauses($f_code, $facet, $selection_group);
                                        $subselect_where[$table_with_selections]=get_discrete_selection_clauses($f_code, $facet, $selection_group). "   AND ";


                                    }
                                    break;
                                case "geo":
                                    //print_r($sval);
                                    $query_where.=get_geo_filter_clauses($f_code, $facet, $selection_group). "     AND " ;
                                    $query_where_list[$table_with_selections][]=get_geo_filter_clauses($f_code, $facet, $selection_group);
                                    $subselect_where[$table_with_selections]=get_geo_filter_clauses($f_code, $facet, $selection_group). "  AND ";

                                    break;
                            }

                            $table_list[$table_with_selections] = true; // set table to use to true, is used later when picking the graph
                           // $table_list_outer[$facet_definition[$facet]["table"]] = true;
                            $subselect_where[$table_with_selections] = substr($subselect_where[$table_with_selections], 0,- 5); //remove last AND
                        }  // end of check if selection should affect facet.
                    }

                }
            }
        }
// array_flip
        if (!empty($extra_tables))
            foreach ($extra_tables as $value) {
                $table_list[$value] = true; // data table is added to the list of tables makes unique
            }


            $query_where = substr($query_where, 0, strlen($query_where) - 5); //remove last AND
          //  print_r($query_where_list);
            if (isset($query_where_list))
            {
             foreach ($query_where_list as $table_with_criteria =>$sql_criteria_list)
             {
                 $new_query_where.="( ".implode(" AND ", $sql_criteria_list). " )   -- Make nice \n   AND ";
             }
         //
            }
          $query_where = substr($new_query_where, 0, -5); //remove last AND
//

        // put the where condition to the return object
        $query["where"] = $query_where . "";
        if (!empty($facet_definition[$f_code]["query_cond"])) {
            if ($query["where"] == "") {
                $query["where"].="   " . $facet_definition[$f_code]["query_cond"];
            } else {
                $query["where"].="  and " . $facet_definition[$f_code]["query_cond"];
            }
        }

        $query["select"] = $facet_definition[$f_code]["id_column"].",".$facet_definition[$f_code]["name_column"];


        $current_table=  isset($facet_definition[$f_code]["alias_table"]) ?  $facet_definition[$f_code]["table"]. " as ".$facet_definition[$f_code]["alias_table"] : $facet_definition[$f_code]["table"] ;
        $query["tables"] =  $current_table;


        // Join clauses between tables
        // adds extra tables from argument
        // Join clauses between tables and tables names

        $counter = 0;

         $alias_table=  isset($facet_definition[$f_code]["alias_table"]) ?  $facet_definition[$f_code]["alias_table"] : $facet_definition[$f_code]["table"] ;
        //$start_table = $facet_definition[$f_code]["table"];
         $start_table =  $alias_table;


        $table_list_outer[$start_table]=true;
        $table_list[$start_table]=true;
       // print_r($table_list_outer );
      //  print_r($table_list);
         foreach ($table_list_outer as $start_table =>$value1)
         {
            foreach ($table_list as $key2 => $value2) {
                if ( $start_table != $key2) {
                    $destination_table = $key2;
                    //echo $start_table ." - ".$destination_table ."\n";
                    $routes[] = $this->get_joins_information($lookup_list_tables, $numeric_edge_list, $start_table, $destination_table);
                 }
            }
         }

        $none_reduced_routes=$routes;
        $sub_selects=$this->make_sub_selects($routes, $edge_list,$subselect_where) ;
        //print_r($sub_selects);

        if (isset($routes)) {
            $routes = $this->route_reducer($routes);
            $route_counter = 0;
            foreach ($routes as $key => $route) {
                // print_r($route);
                $query["joins"].="";//-- Reduced route # $route_counter \n";
                 foreach ($route as $edge_key => $edge) {
                    //    print_r($edge);
                   $join_type=" left ";
                   if (isset($subselect_where[$edge["to_table"]]))
                    {
                          //$query["joins"].= " and " . $subselect_where[$edge["to_table"]]. "\n";
                          $filter_clause.=" and " . $subselect_where[$edge["to_table"]]. "\n";
                          $join_type=" inner ";


                    }

                    if (isset($subselect_where[$edge["from_table"]]))
                    {
                          //$query["joins"].= " and " . $subselect_where[$edge["from_table"]]. "\n";
                          $filter_clause.=" and " . $subselect_where[$edge["from_table"]]. "\n";
                          $join_type=" inner ";
                    }


                    // check if the table in route is the start or destination table
                    // then use the correct alias...
                   // print_r($list_of_alias_tables);
                    if (isset($list_of_alias_tables[$edge["to_table"]]))
                    {
                    // echo
                     $table_to_be_joined=$list_of_alias_tables[$edge["to_table"]] ;//. " as " . $edge["to_table"];
                     $alias_to_be_used=$edge["to_table"];// ."_".$route_counter;
                    }
                    else
                    {
                      $table_to_be_joined= $edge["to_table"] ;
                      $alias_to_be_used=$edge["to_table"];// <<<<<<<<<<<<<<<<<<<<<<<."_".$route_counter;
                    }

                    $route_alias_list_search[$route_counter][] = $table_to_be_joined;
                    $route_alias_list_replace[$route_counter][]=$alias_to_be_used;


                    if ($table_to_be_joined!=$alias_to_be_used)
                    {
                        $query["joins"].="  $join_type join " . $table_to_be_joined . " as ".$alias_to_be_used." \n";
                    }
                    else
                    {
                        $query["joins"].="  $join_type join " . $table_to_be_joined . " \n";
                    }

                 //  $query["where"]=str_replace($route_alias_list_search[$route_counter],$route_alias_list_replace[$route_counter],$query["where"]);
                    $query["joins"].=" on ";
                    foreach ($edge_list[$edge["to_table"]][$edge["from_table"]]["home_columns"] as $key_c1 => $home_column) {

                        $remote_column=$edge_list[$edge["to_table"]][$edge["from_table"]]["remote_columns"][$key_c1];
                       // $home_column=str_replace($route_alias_list_search[$route_counter],$route_alias_list_replace[$route_counter],$home_column);
                     //   $remote_column=str_replace($route_alias_list_search[$route_counter],$route_alias_list_replace[$route_counter],$remote_column);
                        $query["joins"].= $home_column. " = " . $remote_column . "\n";


                    }


                    if (isset($edge_list[$edge["to_table"]][$edge["from_table"]]["extra_condition"])) {
                        $filter_clause.=" and " . $edge_list[$edge["to_table"]][$edge["from_table"]]["extra_condition"];
                        $query["joins"].= " and " . $edge_list[$edge["to_table"]][$edge["from_table"]]["extra_condition"] . "\n";
                    }



                    // print_r ($edge_list[$edge["to_table"]][$edge["from_table"]]);

                }
                $route_counter++;
            }
        }

       // echo "where \n ".$query_where. "\n";
        //print_r($routes);

// add extra condition
        // Merge list of tables needed for queries and joining
         //print_r($query);exit;
        $query["none_reduced_routes"]=$none_reduced_routes;
        $query["reduced_routes"]=$routes;
        $query["sub_selects"]=$sub_selects;
        return $query;
    }


```
