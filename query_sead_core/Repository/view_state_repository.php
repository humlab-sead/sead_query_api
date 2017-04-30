<?php

require_once __DIR__ . "/config/bootstrap_application.php";
require_once __DIR__ . "/connection_helper.php";

class ViewStateRepository {

    public static function save($session_id, $view_state)
    {
        global $view_state_table;
        $q = "Insert Into $view_state_table (view_state, session_id) Values ('$view_state', '$session_id') Returning view_state_id";
        $rs = ConnectionHelper::query($q);
        $row = pg_fetch_assoc($rs);
        $view_state_id = $row["view_state_id"];
        return $view_state_id;
    }

    public static function getSessionIndex($session_id)
    {
        global $view_state_table;
        $q = "select * from $view_state_table where session_id = '$session_id' order by view_state_id desc limit 5";
        $index = ConnectionHelper::queryRows($q);
        return $index;
    }

    public static function get($view_state_id)
    {
        global $view_state_table;
        $q = "select * from $view_state_table where view_state_id = $view_state_id";
        $row = ConnectionHelper::queryRow($q);
        return $row ? $row["view_state"] : null;
    }
}

?>