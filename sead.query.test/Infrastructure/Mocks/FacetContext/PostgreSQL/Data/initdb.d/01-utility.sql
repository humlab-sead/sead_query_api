--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1 (Debian 16.1-1.pgdg110+1)
-- Dumped by pg_dump version 16.8 (Debian 16.8-1.pgdg110+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: sead_utility; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA sead_utility;


--
-- Name: allocate_system_id(text, text, text, text, text, jsonb); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.allocate_system_id(p_submission_identifier text, p_change_request_identifier text, p_table_name text, p_column_name text, p_system_id text DEFAULT NULL::text, p_data jsonb DEFAULT NULL::jsonb) RETURNS integer
    LANGUAGE plpgsql
    AS $$
        declare 
            v_next_id int;
        begin

            v_next_id = sead_utility.get_next_system_id(p_table_name, p_column_name);
            insert into sead_utility.system_id_allocations (
                table_name,
                column_name,
                submission_identifier,
                change_request_identifier,
                external_system_id,
                external_data,
                alloc_system_id
            ) values (
                p_table_name,
                p_column_name,
                p_submission_identifier,
                p_change_request_identifier,
                p_system_id,
                p_data,
                v_next_id
            );
            return v_next_id;
        end;
        $$;


--
-- Name: audit_schema(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.audit_schema(p_table_schema text) RETURNS void
    LANGUAGE plpgsql
    AS $$
declare
    v_record record;
    v_table_name text;
    v_table_audit_view text;
begin
    for v_record in
    select
        t.table_name
    from
        information_schema.tables t
    left join pg_trigger g on not g.tgisinternal
        and g.tgrelid =(t.table_schema || '.' || t.table_name)::regclass
where
    t.table_schema = p_table_schema
        and t.table_type = 'BASE TABLE'
        and g.tgrelid is null
    order by
        1 loop
            v_table_name = p_table_schema || '.' || v_record.table_name;
            perform
                audit.audit_table(v_table_name);
            -- v_table_audit_view_sql = clearing_house.fn_script_audit_views(p_table_schema, v_record.table_name);
            -- Execute v_table_audit_view_sql;
            raise notice 'done: %', v_table_name;
        end loop;

end
$$;


--
-- Name: chown(character varying, character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.chown(in_schema character varying, new_owner character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
        declare
            object_types varchar[];
            object_classes varchar[];
            object_type record;
            r record;
        begin
            object_types = '{type,table,table,sequence,index,view}';
            object_classes = '{c,t,r,S,i,v}';

            for object_type in
                select unnest(object_types) type_name,
                            unnest(object_classes) code
            loop
                for r in
                    select n.nspname, c.relname
                    from pg_class c, pg_namespace n
                    where n.oid = c.relnamespace
                        and nspname = in_schema
                        and relkind = object_type.code
                loop
                raise notice 'Changing ownership of % %.% to %',
                            object_type.type_name,
                            r.nspname, r.relname, new_owner;
                execute format(
                    'alter %s %I.%I owner to %I'
                    , object_type.type_name, r.nspname, r.relname,new_owner);
                end loop;
            end loop;

            for r in
                select  p.proname, n.nspname,
                pg_catalog.pg_get_function_identity_arguments(p.oid) args
                from    pg_catalog.pg_namespace n
                join    pg_catalog.pg_proc p
                on      p.pronamespace = n.oid
                where   n.nspname = in_schema
            loop
                raise notice 'Changing ownership of function %.%(%) to %',
                            r.nspname, r.proname, r.args, new_owner;
                execute format(
                'alter function %I.%I (%s) owner to %I', r.nspname, r.proname, r.args, new_owner);
            end loop;

            for r in
                select *
                from pg_catalog.pg_namespace n
                join pg_catalog.pg_ts_dict d
                on d.dictnamespace = n.oid
                where n.nspname = in_schema
            loop
                execute format(
                'alter text search dictionary %I.%I owner to %I', r.nspname, r.dictname, new_owner );
            end loop;
        end $$;


--
-- Name: column_exists(text, text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.column_exists(p_schema_name text, p_table_name text, p_column_name text) RETURNS boolean
    LANGUAGE sql
    AS $$
        select exists (
            select 1
            from pg_catalog.pg_attribute as a
            join pg_catalog.pg_class as c
              on a.attrelid = c.oid
            join pg_catalog.pg_namespace as ns
              on c.relnamespace = ns.oid
            where c.oid::regclass::text in ( p_table_name, p_schema_name || '.' || p_table_name )
              and a.attname = p_column_name
              and ns.nspname = p_schema_name
              and attnum > 0
        );
    $$;


--
-- Name: constraint_exists(text, text, text[]); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.constraint_exists(s_schema_name text, s_table_name text, VARIADIC v_columns text[]) RETURNS text
    LANGUAGE plpgsql
    AS $$
            declare v_constraint_name text;
        begin

        v_constraint_name = null;

        select tc.constraint_name into v_constraint_name
        from information_schema.table_constraints as tc 
        join information_schema.key_column_usage as kcu
          on tc.constraint_name = kcu.constraint_name
        join information_schema.constraint_column_usage as ccu
          on ccu.constraint_name = tc.constraint_name
        where tc.constraint_type = 'UNIQUE'
          and tc.constraint_schema = s_schema_name
          and tc.table_name=s_table_name
          and kcu.column_name = any(v_columns)
        group by tc.constraint_name, tc.table_name, kcu.column_name
        having count(*) = array_length(v_columns, 1);

        return v_constraint_name;

    end
    $$;


--
-- Name: create_consolidated_references_view(character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.create_consolidated_references_view(p_fk_column_name character varying) RETURNS text
    LANGUAGE plpgsql
    AS $$
    declare
        v_sql text;
        v_table_name varchar;
        v_view_name varchar;
        v_pk_column_name varchar;
    begin
        v_sql = '';
        v_view_name = 'sead_utility.view_consolidated_' || p_fk_column_name;
        for v_table_name, v_pk_column_name in
            select fk.table_name, pk.column_name as pk_column_name
            from sead_utility.table_columns fk
            join sead_utility.table_columns pk
              on pk.table_name = fk.table_name
             and pk.is_pk = 'YES'
            where fk.is_fk = 'YES'
              and fk.column_name = p_fk_column_name
        loop
                if v_sql <> '' then
                    v_sql = v_sql || '    union all' || E'\n';
                end if;
                v_sql = v_sql || '    select ''' || v_table_name || ''' as table_name, ''' || v_pk_column_name || ''' as pk_column_name, ' ||
                    v_pk_column_name || ' as pk_id, ' ||
                    p_fk_column_name || ' as fk_id ' ||
                    'from ' || v_table_name || E'\n';
                raise notice '%', v_table_name;
        end loop;
        v_sql = 'create or replace view ' || v_view_name || ' as (' || E'\n' || v_sql || ');' || E'\n';
        raise notice '%', v_sql;
        return v_sql;
    end $$;


--
-- Name: create_postgrest_default_api_schema(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.create_postgrest_default_api_schema(p_schema_name text DEFAULT 'postgrest_default_api'::text) RETURNS void
    LANGUAGE plpgsql
    AS $_$
    Declare x record;
    Declare create_sql text;
    Declare grant_sql text;
Begin
    /*
        Create `postgrest_default_api` schema. Views for all tables are added to this schema.
    */

    if not exists(
            select schema_name
            from information_schema.schemata
            where schema_name = p_schema_name
        ) then

        if not exists (select from pg_catalog.pg_roles where rolname = 'anonymous_rest_user') then
            create role anonymous_rest_user nologin;
            grant anonymous_rest_user to humlab_admin, humlab_read;
        end if;

        create_sql = format('create schema if not exists %s authorization humlab_read', p_schema_name);

        grant_sql = format('
            grant usage on schema %1$s, public to anonymous_rest_user, humlab_read, humlab_admin;
            grant select on all tables in schema %1$s to humlab_read, humlab_admin, anonymous_rest_user;
            grant execute on all functions in schema %1$s, public to humlab_read, humlab_admin, anonymous_rest_user;
            grant execute on all functions in schema public to anonymous_rest_user;
        ', p_schema_name);

        execute grant_sql;

    end if;

    For x In (
        select distinct table_name
        from information_schema.tables
        where table_schema = 'public'
          and table_type = 'BASE TABLE'
    ) Loop
        perform sead_utility.create_postgrest_default_api_view(x.table_name, p_schema_name);
        Raise Notice 'Done: %', x.table_name;
    End Loop;
End $_$;


--
-- Name: create_postgrest_default_api_view(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.create_postgrest_default_api_view(p_table_name text, p_schema_name text DEFAULT 'postgrest_default_api'::text) RETURNS void
    LANGUAGE plpgsql
    AS $_$
    Declare entity_name text;
    Declare drop_sql text;
    Declare create_sql text;
    Declare owner_sql text;
Begin
    /*
        Create a default postgrest api view for given table.
    */
    entity_name = replace(p_table_name, 'tbl_', '');

    If entity_name Like '%entities' Then
        entity_name = replace(entity_name, 'entities', 'entity');
    ElseIf entity_name Like '%ies' Then
        entity_name = regexp_replace(entity_name, 'ies$', 'y');
    ElseIf Not entity_name Like '%status' Then
        entity_name = rtrim(entity_name, 's');
    End If;

    drop_sql = format('drop view if exists %s.%s;', p_schema_name, entity_name);
    create_sql = format('create or replace view %s.%s as select * from public.%s;', p_schema_name, entity_name, p_table_name);
    owner_sql = format('alter table %s.%s owner to humlab_read;', p_schema_name, entity_name);

    Execute drop_sql;
    Execute create_sql;
    Execute owner_sql;

    Raise Notice 'Done: %', entity_name;

End $_$;


--
-- Name: create_typed_audit_views(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.create_typed_audit_views(p_table_schema text DEFAULT 'public'::text) RETURNS void
    LANGUAGE plpgsql
    AS $$
declare
    v_record record;
    v_view_dml text;
begin
    for v_record in select distinct
        t.table_name
    from
        information_schema.tables t
        join pg_trigger g on not g.tgisinternal
            and g.tgrelid =(t.table_schema || '.' || t.table_name)::regclass
    where
        t.table_schema = p_table_schema
        and t.table_type = 'BASE TABLE'
    order by
        1 loop
            v_view_dml = sead_utility.fn_script_audit_views(p_table_schema, v_record.table_name);
            execute v_view_dml;
            raise notice 'done: %', v_record.table_name;
        end loop;
end
$$;


--
-- Name: drop_table(text, boolean); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.drop_table(IN p_table_name text, IN p_cascade boolean DEFAULT true)
    LANGUAGE plpgsql
    AS $$
        declare
            v_schema_name text;
        begin
            if position('.' in p_table_name) > 0 then
                v_schema_name = split_part(p_table_name, '.', 1);
                p_table_name = split_part(p_table_name, '.', 2);
            else
                v_schema_name = current_schema();
            end if;
            if sead_utility.table_exists(v_schema_name, p_table_name) then
                execute format('drop table %I.%I %s;', v_schema_name, p_table_name, 
                    case when p_cascade then 'cascade' else '' end);
            end if;
        end $$;


--
-- Name: drop_table(text, text, boolean); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.drop_table(IN p_schema_name text, IN p_table_name text, IN p_cascade boolean DEFAULT true)
    LANGUAGE plpgsql
    AS $$
        begin
            if sead_utility.table_exists(p_schema_name, p_table_name) then
                execute format('drop table %I.%I %s;', p_schema_name, p_table_name, 
                    case when p_cascade then 'cascade' else '' end);
            end if;
        end $$;


--
-- Name: drop_udf(text, text); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.drop_udf(IN schema_name text, IN func_name text)
    LANGUAGE plpgsql
    AS $$
        declare
            r record;
        begin
            for r in
                select n.nspname as schema_name, p.proname as function_name,
                    pg_catalog.pg_get_function_identity_arguments(p.oid) as arguments
                from pg_catalog.pg_proc p
                join pg_catalog.pg_namespace n on n.oid = p.pronamespace
                where p.proname = func_name
                and n.nspname = schema_name
            loop
                execute format('drop function if exists %I.%I(%s);', r.schema_name, r.function_name, r.arguments);
            end loop;
        end $$;


--
-- Name: drop_view(text, boolean); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.drop_view(IN p_view_name text, IN p_cascade boolean DEFAULT true)
    LANGUAGE plpgsql
    AS $$
        declare
            v_schema_name text;
        begin
            if position('.' in p_view_name) > 0 then
                v_schema_name = split_part(p_view_name, '.', 1);
                p_view_name = split_part(p_view_name, '.', 2);
            else
                v_schema_name = current_schema();
            end if;
            if sead_utility.view_exists(v_schema_name, p_view_name) then
                execute format('drop view %I.%I %s;', v_schema_name, p_view_name, 
                    case when p_cascade then 'cascade' else '' end);
            end if;
        end $$;


--
-- Name: drop_view(text, text, boolean); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.drop_view(IN p_schema_name text, IN p_view_name text, IN p_cascade boolean DEFAULT true)
    LANGUAGE plpgsql
    AS $$
        begin
            if sead_utility.view_exists(p_schema_name, p_view_name) then
                execute format('drop view %I.%I %s;', p_schema_name, p_view_name, 
                    case when p_cascade then 'cascade' else '' end);
            end if;
        end $$;


--
-- Name: fn_ecocode_crosstab_2_2(integer, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_ecocode_crosstab_2_2(p_master_set_id integer, p_sum_or_count text) RETURNS TABLE(agg_type text, physical_sample_id integer, "Aquatics" integer, "Carrion" integer, "Disturbed/arable" integer, "Dry dead wood" integer, "Dung/foul habitats" integer, "Ectoparasite" integer, "General synanthropic" integer, "Halotolerant" integer, "Heathland & moorland" integer, "Indicators: Coniferous" integer, "Indicators: Deciduous" integer, "Indicators: Dung" integer, "Indicators: Running water" integer, "Indicators: Standing water" integer, "Meadowland" integer, "Mould beetles" integer, "Open wet habitats" integer, "Pasture/Dung" integer, "Sandy/dry disturbed/arable" integer, "Stored grain pest" integer, "Wetlands/marshes" integer, "Wood and trees" integer)
    LANGUAGE plpgsql
    AS $$
        declare
            v_sql text;
            v_abundance_index int;
        begin
            v_abundance_index = case when p_sum_or_count = 'count' then 1 else 2 end;
            return query
                select p_sum_or_count::text as agg_type, x.physical_sample_id, x."Aquatics" , x."Carrion" , x."Disturbed/arable" , x."Dry dead wood" , x."Dung/foul habitats" , x."Ectoparasite" , x."General synanthropic" , x."Halotolerant" , x."Heathland & moorland" , x."Indicators: Coniferous" , x."Indicators: Deciduous" , x."Indicators: Dung" , x."Indicators: Running water" , x."Indicators: Standing water" , x."Meadowland" , x."Mould beetles" , x."Open wet habitats" , x."Pasture/Dung" , x."Sandy/dry disturbed/arable" , x."Stored grain pest" , x."Wetlands/marshes" , x."Wood and trees" 
                from crosstab(format('
        select physical_sample_id, ecocode_name::text, (array[abundance_count, abundance_sum])[%s]::int as abundance
        from sead_utility.fn_sample_ecocode_abundances(2, 2, %s)
        order by 1, 2
     ', v_abundance_index, p_master_set_id), 'values (''Aquatics''::text), (''Carrion''::text), (''Disturbed/arable''::text), (''Dry dead wood''::text), (''Dung/foul habitats''::text), (''Ectoparasite''::text), (''General synanthropic''::text), (''Halotolerant''::text), (''Heathland & moorland''::text), (''Indicators: Coniferous''::text), (''Indicators: Deciduous''::text), (''Indicators: Dung''::text), (''Indicators: Running water''::text), (''Indicators: Standing water''::text), (''Meadowland''::text), (''Mould beetles''::text), (''Open wet habitats''::text), (''Pasture/Dung''::text), (''Sandy/dry disturbed/arable''::text), (''Stored grain pest''::text), (''Wetlands/marshes''::text), (''Wood and trees''::text)') as x (physical_sample_id int, "Aquatics" int, "Carrion" int, "Disturbed/arable" int, "Dry dead wood" int, "Dung/foul habitats" int, "Ectoparasite" int, "General synanthropic" int, "Halotolerant" int, "Heathland & moorland" int, "Indicators: Coniferous" int, "Indicators: Deciduous" int, "Indicators: Dung" int, "Indicators: Running water" int, "Indicators: Standing water" int, "Meadowland" int, "Mould beetles" int, "Open wet habitats" int, "Pasture/Dung" int, "Sandy/dry disturbed/arable" int, "Stored grain pest" int, "Wetlands/marshes" int, "Wood and trees" int);
        end;
        $$;


--
-- Name: fn_ecocode_dating_2_2(integer, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_ecocode_dating_2_2(p_master_set_id integer, p_sum_or_count text) RETURNS TABLE(agg_type text, physical_sample_id integer, "Aquatics" integer, "Carrion" integer, "Disturbed/arable" integer, "Dry dead wood" integer, "Dung/foul habitats" integer, "Ectoparasite" integer, "General synanthropic" integer, "Halotolerant" integer, "Heathland & moorland" integer, "Indicators: Coniferous" integer, "Indicators: Deciduous" integer, "Indicators: Dung" integer, "Indicators: Running water" integer, "Indicators: Standing water" integer, "Meadowland" integer, "Mould beetles" integer, "Open wet habitats" integer, "Pasture/Dung" integer, "Sandy/dry disturbed/arable" integer, "Stored grain pest" integer, "Wetlands/marshes" integer, "Wood and trees" integer, location_name character varying, site_id integer, site_name character varying, latitude_dd numeric, longitude_dd numeric, dating_type integer, age_older numeric, age_younger numeric, age_name character varying, age_abbreviation character varying, sample_group_id integer, alt_ref_type_id integer, sample_type_id integer, sample_name character varying, date_updated timestamp with time zone, date_sampled character varying)
    LANGUAGE plpgsql
    AS $$
        declare
            v_sql text;
            v_abundance_index int;
        begin
            return query
                select  x.agg_type,
                        x.physical_sample_id,
                        x."Aquatics" , x."Carrion" , x."Disturbed/arable" , x."Dry dead wood" , x."Dung/foul habitats" , x."Ectoparasite" , x."General synanthropic" , x."Halotolerant" , x."Heathland & moorland" , x."Indicators: Coniferous" , x."Indicators: Deciduous" , x."Indicators: Dung" , x."Indicators: Running water" , x."Indicators: Standing water" , x."Meadowland" , x."Mould beetles" , x."Open wet habitats" , x."Pasture/Dung" , x."Sandy/dry disturbed/arable" , x."Stored grain pest" , x."Wetlands/marshes" , x."Wood and trees" ,
                        d.location_name, d.site_id, d.site_name, d.latitude_dd, d.longitude_dd,
                        d.dating_type, d.age_older, d.age_younger, d.age_name, d.age_abbreviation,
                        ps.sample_group_id, ps.alt_ref_type_id, ps.sample_type_id, ps.sample_name, ps.date_updated, ps.date_sampled
                from sead_utility.fn_ecocode_crosstab_2_2(p_master_set_id, p_sum_or_count) x
                join sead_utility.physical_sample_dating d USING (physical_sample_id)
                join public.tbl_physical_samples ps USING (physical_sample_id);
        end;
        $$;


--
-- Name: fn_ecocode_dating_geojson_2_2(integer, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_ecocode_dating_geojson_2_2(p_master_set_id integer, p_sum_or_count text) RETURNS TABLE(agg_type text, ecocode_json json)
    LANGUAGE plpgsql
    AS $$
        begin

             return query
                select  p_sum_or_count as agg_type,
                        json_build_object(
                            'type', 'FeatureCollection',
                            'features', json_agg(
                                json_build_object(
                                    'type', 'Feature',
                                    'id', e.physical_sample_id,
                                    'geometry', json_build_object('type', 'Point', 'coordinates', json_build_array(e.longitude_dd, e.latitude_dd)),
                                    'properties', json_build_object(
                                        'id', e.physical_sample_id,
                                        'country', e.location_name,
                                        'sampleData', json_build_object(
                                            'site_id', e.site_id,
                                            'site_name', e.site_name,
                                            'sample_name', e.sample_name,
                                            'sample_group_id', e.sample_group_id,
                                            'dating_type', (ARRAY['Calibrated radiocarbon dates'::text, 'Relative dates'::text])[e.dating_type],
                                            'start', e.age_older,
                                            'end', e.age_younger,
                                            'age_name', e.age_name,
                                            'age_abbreviation', e.age_abbreviation),
                                            'indicators', json_build_object('Aquatics', e."Aquatics", 'Carrion', e."Carrion", 'Disturbed/arable', e."Disturbed/arable", 'Dry dead wood', e."Dry dead wood", 'Dung/foul habitats', e."Dung/foul habitats", 'Ectoparasite', e."Ectoparasite", 'General synanthropic', e."General synanthropic", 'Halotolerant', e."Halotolerant", 'Heathland & moorland', e."Heathland & moorland", 'Indicators: Coniferous', e."Indicators: Coniferous", 'Indicators: Deciduous', e."Indicators: Deciduous", 'Indicators: Dung', e."Indicators: Dung", 'Indicators: Running water', e."Indicators: Running water", 'Indicators: Standing water', e."Indicators: Standing water", 'Meadowland', e."Meadowland", 'Mould beetles', e."Mould beetles", 'Open wet habitats', e."Open wet habitats", 'Pasture/Dung', e."Pasture/Dung", 'Sandy/dry disturbed/arable', e."Sandy/dry disturbed/arable", 'Stored grain pest', e."Stored grain pest", 'Wetlands/marshes', e."Wetlands/marshes", 'Wood and trees', e."Wood and trees")
                                        )
                                    )
                                )
                            ) AS ecocode_json
                from sead_utility.fn_ecocode_dating_2_2(p_master_set_id, p_sum_or_count) e;
        end;
        $$;


--
-- Name: fn_generate_ecocode_crosstab_function(integer, integer); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_generate_ecocode_crosstab_function(p_ecocode_system_id integer, p_ecocode_group_id integer) RETURNS text
    LANGUAGE plpgsql
    AS $_$
declare
    v_ecocodes character varying[];
    v_fields text;
    v_typed_fields text;
    v_cx_sql text;
    v_data_sql text;
    v_category_sql text;
    v_udf_sql text;
    v_udf_name text;
begin
    v_ecocodes = (
        select array_agg(ed.name order by ed.name )
        from tbl_ecocode_definitions ed
        join tbl_ecocode_groups eg using (ecocode_group_id)
        where eg.ecocode_system_id = p_ecocode_system_id
          and eg.ecocode_group_id = p_ecocode_group_id
    );

    /* Generate list of EcoCode field names for selected system and group */
    v_fields = array_to_string(array(select 'cx."' || ecocode_name || '" ' from unnest(v_ecocodes) as ecocode_name), ', '::text);

    /* Generate list of typed EcoCode field names for selected system and group */
    v_typed_fields = array_to_string(array(select '"' || ecocode_name || '" int' from unnest(v_ecocodes) as ecocode_name), ', '::text);

    /* Generate list of EcoCode names as string values. This specifies categories in crosstab (i.e. columns) */
    v_category_sql = 'values ' || array_to_string(array(select '(''''' || ecocode_name || '''''::text)' from unnest(v_ecocodes) as ecocode_name), ', '::text);

    /* Cross tab data query. The sort order is required since crosstab function expects
       data to  be sorted in row, category order (othwerwise crosstab will display wrong values ). */
    v_data_sql = format('
        select physical_sample_id, ecocode_name::text, (array[abundance_count, abundance_sum])[%%s]::int as abundance
        from sead_utility.fn_sample_ecocode_abundances(%s, %s, %%s)
        order by 1, 2
    ', p_ecocode_system_id, p_ecocode_group_id);

    /* Final crosstab query parameterized by master set id and aggregate type (1:'count', 2:'sum') */
    v_cx_sql = format('
        select p_sum_or_count::text as agg_type, cx.physical_sample_id, %s
        from crosstab(format(''%s'', v_abundance_index, p_master_set_id), ''%s'') as cx (physical_sample_id int, %s)
    ', v_fields, v_data_sql, v_category_sql, v_typed_fields);

    v_udf_name = format('sead_utility.fn_ecocode_crosstab_%s_%s', p_ecocode_system_id, p_ecocode_group_id);

    v_udf_sql  = format('
drop function if exists %s(text);
create or replace function %s(p_master_set_id int, p_sum_or_count text)
/*
    Note! This function was automatically generated by fn_generate_ecocode_crosstab_function.
*/

returns table (
  agg_type text,
  physical_sample_id integer,
  %s
) as
$body$
declare
  v_sql text;
  v_abundance_index int;
begin
    v_abundance_index = case when p_sum_or_count = ''count'' then 1 else 2 end;
    return query
        %s;
end;
$body$
language ''plpgsql'';
', v_udf_name, v_udf_name, v_typed_fields, v_cx_sql);

    raise notice '%', v_udf_sql;
    execute v_udf_sql;
    return v_udf_sql;
end $_$;


--
-- Name: fn_generate_ecocode_crosstab_function(integer, integer, boolean); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_generate_ecocode_crosstab_function(p_ecocode_system_id integer, p_ecocode_group_id integer, p_dry_run boolean DEFAULT true) RETURNS text
    LANGUAGE plpgsql
    AS $_$
declare
    v_ecocodes character varying[];
    v_fields text;
    v_typed_fields text;
    v_data_sql text;
    v_category_sql text;
    v_udf_sql text;
    v_udf_name text;
    v_valid_groups text;
    v_ecocode_fields text;
begin
    v_ecocodes = (
        select array_agg(ed.name order by ed.name )
        from public.tbl_ecocode_definitions ed
        join public.tbl_ecocode_groups eg using (ecocode_group_id)
        where eg.ecocode_system_id = p_ecocode_system_id
          and eg.ecocode_group_id = p_ecocode_group_id
    );

    if v_ecocodes is null then

        select string_agg(ecocode_group_id::text, ',')
            into v_valid_groups from public.tbl_ecocode_groups
        where ecocode_system_id = p_ecocode_system_id;

        v_valid_groups = coalesce(v_valid_groups, 'none');

        raise exception 'Illegal combination: system_id %, group_id %', p_ecocode_system_id, p_ecocode_group_id
            using hint = 'Please check ecocode_group_id. Valid groups for ecocode_system_id ' || p_ecocode_system_id::text || ': ' ||  v_valid_groups;

        return '';
    end if;

    /* Generate list of EcoCode field names for selected system and group */
    v_fields = array_to_string(array(select 'x."' || ecocode_name || '" ' from unnest(v_ecocodes) as ecocode_name), ', '::text);

    /* Generate list of typed EcoCode field names for selected system and group */
    v_typed_fields = array_to_string(array(select '"' || ecocode_name || '" int' from unnest(v_ecocodes) as ecocode_name), ', '::text);

    /* Generate list of EcoCode names as string values. This specifies categories in crosstab (i.e. columns) */
    v_category_sql = 'values ' || array_to_string(array(select '(''''' || ecocode_name || '''''::text)' from unnest(v_ecocodes) as ecocode_name), ', '::text);

    /* Cross tab data query. The sort order is required since crosstab function expects
       data to  be sorted in row, category order (othwerwise crosstab will display wrong values ). */
    v_data_sql = format('
        select physical_sample_id, ecocode_name::text, (array[abundance_count, abundance_sum])[%%s]::int as abundance
        from sead_utility.fn_sample_ecocode_abundances(%1$s, %2$s, %%s)
        order by 1, 2
     ', p_ecocode_system_id, p_ecocode_group_id);

    /* Final crosstab query parameterized by master set id and aggregate type (1:'count', 2:'sum') */

    v_udf_sql  = format('
        -- drop function if exists sead_utility.fn_ecocode_crosstab_%1$s_%2$s(int, text);
        create or replace function sead_utility.fn_ecocode_crosstab_%1$s_%2$s(p_master_set_id int, p_sum_or_count text)
        /*
            Note! This function was automatically generated by fn_generate_ecocode_crosstab_function.
        */

        returns table (
            agg_type text,
            physical_sample_id integer,
            %3$s
        ) as
        $body$
        declare
            v_sql text;
            v_abundance_index int;
        begin
            v_abundance_index = case when p_sum_or_count = ''count'' then 1 else 2 end;
            return query
                select p_sum_or_count::text as agg_type, x.physical_sample_id, %4$s
                from crosstab(format(''%5$s'', v_abundance_index, p_master_set_id), ''%6$s'') as x (physical_sample_id int, %3$s);
        end;
        $body$
        language ''plpgsql'';
        ', p_ecocode_system_id, p_ecocode_group_id, v_typed_fields, v_fields, v_data_sql, v_category_sql);

    if p_dry_run = TRUE then
        raise notice '%', v_udf_sql;
    else
        execute v_udf_sql;
    end if;

    v_udf_sql  = format('

        -- drop function if exists sead_utility.fn_ecocode_dating_%1$s_%2$s(int, text);

        create or replace function sead_utility.fn_ecocode_dating_%1$s_%2$s(p_master_set_id int, p_sum_or_count text)
            /*
                Note! This function was automatically generated by fn_generate_ecocode_crosstab_function.
            */
        returns table (
            agg_type            text,
            physical_sample_id  integer,
            %3$s,
            location_name       character varying(255),
            site_id             integer,
            site_name           character varying(60),
            latitude_dd         numeric(18, 10),
            longitude_dd        numeric(18, 10),
            dating_type         integer,
            age_older           numeric(20, 5),
            age_younger         numeric(20, 5),
            age_name            character varying,
            age_abbreviation    character varying,
            sample_group_id     integer,
            alt_ref_type_id     integer,
            sample_type_id      integer,
            sample_name         character varying(50),
            date_updated        timestamp with time zone,
            date_sampled        character varying
        ) as
        $body$
        declare
            v_sql text;
            v_abundance_index int;
        begin
            return query
                select  x.agg_type,
                        x.physical_sample_id,
                        %4$s,
                        d.location_name, d.site_id, d.site_name, d.latitude_dd, d.longitude_dd,
                        d.dating_type, d.age_older, d.age_younger, d.age_name, d.age_abbreviation,
                        ps.sample_group_id, ps.alt_ref_type_id, ps.sample_type_id, ps.sample_name, ps.date_updated, ps.date_sampled
                from sead_utility.fn_ecocode_crosstab_%1$s_%2$s(p_master_set_id, p_sum_or_count) x
                join sead_utility.physical_sample_dating d USING (physical_sample_id)
                join public.tbl_physical_samples ps USING (physical_sample_id);
        end;
        $body$
        language ''plpgsql'';
    ', p_ecocode_system_id, p_ecocode_group_id, v_typed_fields, v_fields);

    if p_dry_run = TRUE then
        raise notice '%', v_udf_sql;
    else
        execute v_udf_sql;
    end if;


    select string_agg(format('''%1$s'', e."%1$s"', x.a), ', ') into v_ecocode_fields
    from (
    	select unnest(v_ecocodes) as a
    ) as x;


    v_udf_sql  = format('

        -- drop function if exists sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(int, text);

        create or replace function sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(p_master_set_id int, p_sum_or_count text)
            /*
                Note! This function was automatically generated by fn_generate_ecocode_crosstab_function.
            */
        returns table (
            agg_type      text,
            ecocode_json  json
        ) as
        $body$
        begin

             return query
                select  p_sum_or_count as agg_type,
                        json_build_object(
                            ''type'', ''FeatureCollection'',
                            ''features'', json_agg(
                                json_build_object(
                                    ''type'', ''Feature'',
                                    ''id'', e.physical_sample_id,
                                    ''geometry'', json_build_object(''type'', ''Point'', ''coordinates'', json_build_array(e.longitude_dd, e.latitude_dd)),
                                    ''properties'', json_build_object(
                                        ''id'', e.physical_sample_id,
                                        ''country'', e.location_name,
                                        ''sampleData'', json_build_object(
                                            ''site_id'', e.site_id,
                                            ''site_name'', e.site_name,
                                            ''sample_name'', e.sample_name,
                                            ''sample_group_id'', e.sample_group_id,
                                            ''dating_type'', (ARRAY[''Calibrated radiocarbon dates''::text, ''Relative dates''::text])[e.dating_type],
                                            ''start'', e.age_older,
                                            ''end'', e.age_younger,
                                            ''age_name'', e.age_name,
                                            ''age_abbreviation'', e.age_abbreviation),
                                            ''indicators'', json_build_object(%3$s)
                                        )
                                    )
                                )
                            ) AS ecocode_json
                from sead_utility.fn_ecocode_dating_%1$s_%2$s(p_master_set_id, p_sum_or_count) e;
        end;
        $body$
        language ''plpgsql'';
    ', p_ecocode_system_id, p_ecocode_group_id, v_ecocode_fields);


    if p_dry_run = TRUE then
        raise notice '%', v_udf_sql;
    else
        execute v_udf_sql;
    end if;

    /* REST api views */

    v_udf_sql = format('

        /* Public API: /rpc/fn_ecocode_dating_geojson_x_y_sum */
        drop function if exists  postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_sum;
        create or replace function postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_sum() returns json language sql
        as $body$
         	select ecocode_json
          	from sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(1, ''sum''::text);
        $body$;

        grant all on function postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_sum() to humlab_admin;
        grant execute on function postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_sum() to public;

        /* Public API: /rpc/fn_ecocode_dating_geojson_x_y_count */
        drop function if exists  postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_count;
        create or replace function postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_count() returns json language sql
        as $body$
         	select ecocode_json
          	from sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(1, ''count''::text);
        $body$;

        grant all on function postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_sum() to humlab_admin;
        grant execute on function postgrest_api.fn_ecocode_dating_geojson_%1$s_%2$s_sum() to public;


        /* Public API: /ecocode_dating_geojson_x_y_count DEPRECATED (creates invalid GeoJSON caused by a wrapped JSON) */
        drop view if exists postgrest_api.ecocode_dating_geojson_%1$s_%2$s_sum;
        drop view if exists postgrest_api.ecocode_dating_geojson_%1$s_%2$s_count;
        -- create or replace view postgrest_api.ecocode_dating_geojson_%1$s_%2$s_sum as
        --     select ecocode_json
        --     from sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(1, ''sum'');
        -- create or replace view postgrest_api.ecocode_dating_geojson_%1$s_%2$s_count as
        --     select ecocode_json
        --     from sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(1, ''count'');
        -- grant select on table postgrest_api.ecocode_dating_geojson_%1$s_%2$s_sum to public;
        ---grant select on table postgrest_api.ecocode_dating_geojson_%1$s_%2$s_count to public;


    ', p_ecocode_system_id, p_ecocode_group_id);

    if p_dry_run = TRUE then
        raise notice '%', v_udf_sql;
    else
        execute v_udf_sql;
    end if;

    /* Permissions */

    v_udf_sql = format('

        grant select on table sead_utility.physical_sample_dating to public;
        grant execute on function sead_utility.fn_sample_ecocode_abundances(int,int,int) to public;

        grant execute on function sead_utility.fn_ecocode_crosstab_%1$s_%2$s(int, text) to public;
        grant execute on function sead_utility.fn_ecocode_dating_%1$s_%2$s(int, text) to public;
        grant execute on function sead_utility.fn_ecocode_dating_geojson_%1$s_%2$s(int, text) to public;

        grant usage on schema sead_utility, public to postgrest_anon;
        grant select on all tables in schema sead_utility to postgrest_anon;
        grant execute on all functions in schema sead_utility to postgrest_anon;

    ', p_ecocode_system_id, p_ecocode_group_id);

    if p_dry_run = TRUE then
        raise notice '%', v_udf_sql;
    else
        execute v_udf_sql;
    end if;

    return '';

end $_$;


--
-- Name: fn_sample_ecocode_abundances(integer, integer, integer); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_sample_ecocode_abundances(v_ecocode_system_id integer, v_ecocode_group_id integer, v_master_set_id integer) RETURNS TABLE(physical_sample_id integer, ecocode_name character varying, abundance_count integer, abundance_sum integer)
    LANGUAGE plpgsql
    AS $$
begin
    return query
    with ecocodes as (
        /* This is all unique ecocode names within selected system and group (used in cross join) */
        select ed.name as ecocode_name
        from public.tbl_ecocode_definitions ed
        join public.tbl_ecocode_groups eg using (ecocode_group_id)
        where eg.ecocode_system_id = v_ecocode_system_id
          and eg.ecocode_group_id = v_ecocode_group_id
        -- select unnest(v_ecocodes) as ecocode_name
    ), taxon_ecocodes as (
        /* This is just a taxon to eco-code mapping (for selected system and group) */
        select taxon_id, ed.name as ecocode_name
        from public.tbl_ecocode_groups eg
        join public.tbl_ecocode_definitions ed using (ecocode_group_id)
        join public.tbl_ecocodes e using (ecocode_definition_id)
        where eg.ecocode_system_id = v_ecocode_system_id
          and eg.ecocode_group_id = v_ecocode_group_id
    ), samples as (
        /* Only consider samples that belong to specified master dataset */
        select distinct samples.physical_sample_id
        from public.tbl_physical_samples samples
        join public.tbl_analysis_entities using (physical_sample_id)
        join public.tbl_datasets using (dataset_id)
        join public.tbl_dataset_masters using (master_set_id)
        where tbl_dataset_masters.master_set_id = v_master_set_id
          and tbl_dataset_masters.master_name =  'Bugs database'
    ), sample_ecocode_abundances as (
        /* Compute count and sum by sample and ecocode */
        select samples.physical_sample_id, taxon_ecocodes.ecocode_name, count(abundance_id) as abundance_count, sum(abundance) as abundance_sum
        from samples
        join public.tbl_analysis_entities using (physical_sample_id)
        join public.tbl_abundances using (analysis_entity_id)
        join taxon_ecocodes using (taxon_id)
        group by samples.physical_sample_id, taxon_ecocodes.ecocode_name
    ) select x.physical_sample_id,
             x.ecocode_name::character varying(150),
             coalesce(sample_ecocode_abundances.abundance_count, 0)::int,
             coalesce(sample_ecocode_abundances.abundance_sum, 0)::int
      from (
          select samples.physical_sample_id, ecocodes.ecocode_name
          from samples
          cross join ecocodes
      ) as x
      left join sample_ecocode_abundances using (physical_sample_id, ecocode_name);
end $$;


--
-- Name: fn_script_audit_views(character varying, character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_script_audit_views(source_schema character varying, p_table_name character varying) RETURNS text
    LANGUAGE plpgsql
    AS $$
declare
    v_template text;
    declare v_view_name text;
    declare v_view_dml text;
    declare v_column_list text;
    declare v_column_type text;
begin
    v_view_name = replace(p_table_name, 'tbl_', 'view_');

    v_template = '
    DROP VIEW IF EXISTS audit.#VIEW-NAME#;
    CREATE VIEW audit.#VIEW-NAME# AS
		SELECT #COLUMN-LIST#,
		transaction_id,
		action,
		session_user_name,
		action_tstamp_tx
		FROM audit.logged_actions
		WHERE table_name = ''#TABLE-NAME#''
	;';

    with table_columns as (
        select
            column_name,
            clearing_house.fn_create_schema_type_string(data_type, character_maximum_length, numeric_precision, numeric_scale, 'YES', null) as column_type
        from
            clearing_house.fn_dba_get_sead_public_db_schema('public', 'sead_master') s
        where
            s.table_schema = 'public'
            and s.table_name = 'tbl_locations'
        order by
            ordinal_position
)
    select
        string_agg('(row_data->''' || column_name || ''')::' || replace(column_type, ' null', '') || ' AS ' || column_name, ', ' || chr(13)) into v_column_list
    from
        table_columns;

    v_view_dml := v_template;
    v_view_dml := replace(v_view_dml, '#VIEW-NAME#', v_view_name);
    v_view_dml := replace(v_view_dml, '#TABLE-NAME#', p_table_name);
    v_view_dml := replace(v_view_dml, '#COLUMN-LIST#', v_column_list);

    return v_view_dml;

end
$$;


--
-- Name: fn_script_to_sqlite_columns(); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_script_to_sqlite_columns() RETURNS TABLE(table_name information_schema.sql_identifier, column_name information_schema.sql_identifier, ordinal_position information_schema.cardinal_number, data_type text, is_nullable information_schema.yes_or_no, is_pk information_schema.yes_or_no, is_fk information_schema.yes_or_no, fk_table_name information_schema.sql_identifier, fk_column_name information_schema.sql_identifier)
    LANGUAGE plpgsql
    AS $$
        begin
                return query
                    select
                        pg_tables.tablename::information_schema.sql_identifier  as table_name,
                        pg_attribute.attname::information_schema.sql_identifier as column_name,
                        pg_attribute.attnum::information_schema.cardinal_number as ordinal_position,
                        case
                        when pg_attribute.atttypid in (16, 20, 21, 23) then 'integer'
                        when pg_attribute.atttypid in (18, 19, 25, 1002, 1043, 1082, 1114, 1184, 12790, 12797) then 'text'
                        when pg_attribute.atttypid in (700, 1700) then 'numeric'
                        when pg_attribute.atttypid in (17, 1005, 1009, 1021) then 'blob'
                        else null end::text,
                        case pg_attribute.attnotnull when false then 'YES' else 'NO' end::information_schema.yes_or_no as is_nullable,
                        case when pk.contype is null then 'NO' else 'YES' end::information_schema.yes_or_no as is_pk,
                        case when fk.table_oid is null then 'NO' else 'YES' end::information_schema.yes_or_no as is_fk,
                        fk.f_table_name::information_schema.sql_identifier,
                        fk.f_column_name::information_schema.sql_identifier
                from pg_tables
                join pg_class
                on pg_class.relname = pg_tables.tablename
                join pg_namespace ns
                on ns.oid = pg_class.relnamespace
                and ns.nspname  = pg_tables.schemaname
                join pg_attribute
                on pg_class.oid = pg_attribute.attrelid
                and pg_attribute.attnum > 0
                left join pg_constraint pk
                on pk.contype = 'p'::"char"
                and pk.conrelid = pg_class.oid
                and (pg_attribute.attnum = any (pk.conkey))
                left join clearing_house.view_foreign_keys as fk
                on fk.table_oid = pg_class.oid
                and fk.attnum = pg_attribute.attnum
                where true
                and pg_tables.tableowner = 'sead_master'
                and pg_attribute.atttypid <> 0::oid
                and pg_tables.schemaname = 'public'
                order by table_name, ordinal_position asc;
        end
$$;


--
-- Name: fn_script_to_sqlite_table(character varying, character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_script_to_sqlite_table(p_source_schema character varying, p_table_name character varying) RETURNS text
    LANGUAGE plpgsql
    AS $$
            Declare sql_stmt text;
            Declare data_columns text;
            Declare foreign_key_columns text;
        Begin

            Select string_agg(
                format('%s %s%s%s', column_name, data_type,
                    case when is_nullable = 'NO' then ' NOT NULL' else '' end,
                    case when is_pk = 'YES' then ' PRIMARY' else '' end
                ), E',\n        ' ORDER BY ordinal_position ASC),
                string_agg(
                    case when is_fk = 'NO' then ''
                        else format(E'      , FOREIGN KEY (%s) REFERENCES %s (%s) ON DELETE NO ACTION ON UPDATE NO ACTION\n',
                                    column_name, fk_table_name, fk_column_name) end
                    , '')

            Into Strict data_columns, foreign_key_columns
            From sead_utility.fn_script_to_sqlite_columns() s
            Where table_name = p_table_name;

            sql_stmt = format('Create Table %s (
                %s
                %s
            );', p_table_name, data_columns, foreign_key_columns);

            Return sql_stmt;
        End
$$;


--
-- Name: fn_script_to_sqlite_tables(); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.fn_script_to_sqlite_tables() RETURNS character varying
    LANGUAGE plpgsql
    AS $$
        Declare x RECORD;
            Declare create_script text;
        Begin
            create_script := '';
            For x In (

                with recursive fk_tree as (
                -- All tables not referencing anything else
                select t.oid as reloid,
                        t.relname as table_name,
                        s.nspname as schema_name,
                        null::text as referenced_table_name,
                        null::text as referenced_schema_name,
                        1 as level
                from pg_class t
                    join pg_namespace s on s.oid = t.relnamespace
                where relkind = 'r'
                    and not exists (select * from pg_constraint where contype = 'f' and conrelid = t.oid)
                    and s.nspname = 'public' -- limit to one schema
                union all
                select ref.oid, ref.relname, rs.nspname, p.table_name, p.schema_name, p.level + 1
                from pg_class ref
                join pg_namespace rs on rs.oid = ref.relnamespace
                join pg_constraint c on c.contype = 'f' and c.conrelid = ref.oid
                join fk_tree p on p.reloid = c.confrelid
                where ref.oid != p.reloid  -- do not enter to tables referencing theirselves.
                ), all_tables as (
                -- this picks the highest level for each table
                select schema_name, table_name, level,
                        row_number() over (partition by schema_name, table_name order by level desc) as last_table_row
                from fk_tree
                )
                select schema_name, table_name
                from all_tables at
                where last_table_row = 1
                order by level

            )
            Loop
                create_script := create_script || E'\n' || sead_utility.fn_script_to_sqlite_table(x.schema_name::character varying, x.table_name::character varying);
                Raise Notice '%', create_script;
            End Loop;
            return create_script;
        End
        $$;


--
-- Name: generate_drop_table_script(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.generate_drop_table_script(p_table_name text) RETURNS text
    LANGUAGE plpgsql
    AS $_$
declare v_drop_script text;
declare v_entity_name text;
declare v_view_name text;
begin

    v_entity_name = clearing_house.fn_sead_table_entity_name(p_table_name::name)::character varying;
    v_view_name = replace(p_table_name, 'tbl_', 'view_')::character varying;

    begin
        
        if sead_utility.table_exists('public'::text, p_table_name) = FALSE then
            raise exception sqlstate 'GUARD';
        end if;

        v_drop_script = format('

drop function if exists clearing_house_commit.resolve_%3$s(int);
drop view if exists clearing_house.%2$s;
drop table if exists clearing_house.%1$s;
delete from clearing_house.tbl_clearinghouse_submission_tables where table_name_underscored = ''%1$s'';
delete from clearing_house_commit.tbl_sead_table_keys where table_name = ''%1$s'';

drop view if exists postgrest_api.%1$s;
drop view if exists postgrest_default_api.%3$s;

delete from facet.table_relation
    where source_table_id in (
        select table_id
        from facet.table
        where table_or_udf_name = ''%1$s''
    ) or 
    target_table_id in (
        select table_id
        from facet.table
        where table_or_udf_name = ''%1$s''
    )
    ;

delete from facet.facet_table
    where table_id in (
        select table_id
        from facet.table
        where table_or_udf_name = ''%1$s''
    );

delete from facet.table
    where table_or_udf_name = ''%1$s'';

drop table if exists public."%1$s";
            
        ', p_table_name, v_view_name, v_entity_name);

        -- raise notice '%', v_drop_script;
        return v_drop_script;


    exception when sqlstate 'GUARD' then
        raise notice 'ALREADY EXECUTED';
        return null;
    end;

end $_$;


--
-- Name: get_all_table_counts(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.get_all_table_counts(p_schema_name text DEFAULT 'public'::text) RETURNS TABLE(schema_name text, table_name text, count bigint)
    LANGUAGE plpgsql
    AS $$
    declare _t record;
    begin
        for _t in (select schemaname, tablename from pg_tables where schemaname = p_schema_name) loop
            return query execute format('select %L::text as schema_name, %L::text as table_name, count(*)::bigint as count from %I.%I',
                _t.schemaname,
                _t.tablename,
                _t.schemaname,
                _t.tablename
            );
        end loop;
    end $$;


--
-- Name: get_allocated_id(text, text, text, text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.get_allocated_id(p_submission_identifier text, p_change_request_identifier text, p_table_name text, p_column_name text, p_system_id text DEFAULT NULL::text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
        declare
            v_alloc_system_id integer;
        begin

            select max(alloc_system_id::int)
                into v_alloc_system_id
                    from sead_utility.system_id_allocations
                    where submission_identifier = p_submission_identifier
                    and change_request_identifier = p_change_request_identifier
                    and table_name = p_table_name
                    and column_name = p_column_name
                    and external_system_id = p_system_id
            ;
            return v_alloc_system_id;
        end;
        $$;


--
-- Name: get_column_type(text, text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.get_column_type(p_schema text, p_table text, p_column text) RETURNS text
    LANGUAGE plpgsql
    AS $$
        declare
            v_data_type text;
        begin
            select data_type into v_data_type
            from information_schema.columns
            where table_schema = p_schema
            and table_name = p_table
            and column_name = p_column;
            return v_data_type;
        end;
        $$;


--
-- Name: get_json_field(jsonb, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.get_json_field(json_data jsonb, field_name text) RETURNS text
    LANGUAGE plpgsql
    AS $_$
        declare
            v_value text;
        begin
            if json_data is null then
                return null;
            end if;
            execute format('select $1->>%L', field_name)
                into v_value using json_data;
            return result;
        end;
        $_$;


--
-- Name: get_max_sequence_id(text, text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.get_max_sequence_id(p_table_schema text, p_table_name text, p_column_name text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
	declare p_value int;
begin
	execute format('select max(%I) from %I.%I', p_column_name, p_table_schema, p_table_name) into p_value;
	return p_value;
end; $$;


--
-- Name: get_next_system_id(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.get_next_system_id(p_table_name text, p_column_name text) RETURNS integer
    LANGUAGE plpgsql
    AS $$
        declare
            v_next_id integer;
            v_query text;
        begin
            v_query := format('select max(%s) from %s', quote_ident(p_column_name), quote_ident(p_table_name));
            -- raise notice '%', v_query;
            execute v_query into v_next_id;

            select max(system_id) into v_next_id
            from (
                select coalesce(max(alloc_system_id), 0) as system_id
                from sead_utility.system_id_allocations
                where table_name = p_table_name
                and column_name = p_column_name
                union all
                values (v_next_id)
            );
            return v_next_id + 1;
        end;
        $$;


--
-- Name: import_pending_results_chronologies(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.import_pending_results_chronologies(p_change_request text) RETURNS void
    LANGUAGE plpgsql
    AS $$
    declare
        v_project_id int;
        v_method_id int;
        v_note text;
        v_count_sheet_code text;
        v_dataset_id int;
        v_dataset_name text;
        v_now timestamp with time zone;
        v_id int;
        v_sample_group_id int;
        v_physical_sample_id int;
        v_age_from int;
        v_age_to int;
        v_analysis_entity_id int;
        v_dating_specifier text;
    begin
        begin

            v_now = now();

            if (select count(*) from bugs_import.results_chronology_import where "change_request" = p_change_request) = 0 then
                raise notice 'No data to import for %', p_change_request;
                return;
            end if;

            /* Explode compound keys */
            update bugs_import.results_chronology_import 
                set "site_name" = split_part("identifier", '|', 1),
                    "count_sheet_code" = split_part("identifier", '|', 2),
                    "sample_name" = split_part("identifier", '|', 3),
                    "is_ok" = true
            where "change_request" = p_change_request;

            /* Add SEAD sample group identity */
            with bugs_translation as (
                select distinct "bugs_identifier" as count_sheet_code, "sead_reference_id" as sample_group_id
                from bugs_import."bugs_trace"
                where "bugs_table" = 'TCountsheet'
                  and "sead_table" = 'tbl_sample_groups'
                  and "manipulation_type" = 'INSERT'
            )
                update bugs_import.results_chronology_import d
                   set "sample_group_id" = t."sample_group_id"
                from bugs_translation t
                where d."change_request" = p_change_request
                  and t."count_sheet_code" = d."count_sheet_code";

            /* Assign SEAD identity to physical samples */
            update bugs_import.results_chronology_import d
               set physical_sample_id = t."physical_sample_id"
            from tbl_physical_samples t
            where d."change_request" = p_change_request
              and t."sample_group_id" = d."sample_group_id"
              and t."sample_name" = d."sample_name";

            update bugs_import.results_chronology_import
               set "is_ok" = false, "error" = 'No dating data'
            where "change_request" = p_change_request
              and "is_ok"
              and Coalesce("Chosen_C14", "Chosen_OtherRadio", "Chosen_Calendar", "Chosen_Period") is null;

            update bugs_import.results_chronology_import
                set "is_ok" = false, "error" = 'Unknown count sheet'
            where "change_request" = p_change_request
              and "is_ok"
              and "sample_group_id" is null;

            update bugs_import.results_chronology_import
                set "is_ok" = false, "error" = 'Unknown sample group'
            where "change_request" = p_change_request
              and "is_ok"
              and "physical_sample_id" is null;

            update bugs_import.results_chronology_import
                set "is_ok" = false, "error" = 'Invalid age range'
            where "AgeFrom" is not null
              and "AgeTo" is not null
              and coalesce("AgeTo"::int, "AgeFrom"::int) > coalesce("AgeFrom"::int, "AgeTo"::int);

            perform sead_utility.sync_sequences('public');

            v_project_id = (select project_id from tbl_projects where project_name = 'Swedish Biodiversity Data Infrastructure');
            v_method_id = (select method_id from tbl_methods where method_name = 'Composite chronology');

            for v_count_sheet_code in (
                select distinct count_sheet_code
                from bugs_import.results_chronology_import
                where "change_request" = p_change_request
                order by count_sheet_code
            )
            loop
                -- raise notice 'Count sheet: %', v_count_sheet_code;

                v_dataset_name = format('simpledate_%s', v_count_sheet_code);
                v_dataset_id = (select dataset_id from tbl_datasets where dataset_name = v_dataset_name);
                if v_dataset_id is null then

                    insert into tbl_datasets(master_set_id, data_type_id, method_id, project_id, dataset_name)
                        values (1 /* Bugs */, 44 /* Composite type */, v_method_id, v_project_id, v_dataset_name)
                            returning dataset_id into v_dataset_id;

                    v_note = 'Single data per sample for BugsCEP data as compiled from existing dating evidence by Francesca Pilotto and Philip Buckland for SBDI';
                    insert into tbl_dataset_submissions(dataset_id, submission_type_id, contact_id, date_submitted, notes, date_updated)
                        values (v_dataset_id, 8, 1, v_now, v_note, v_now );
                    
                    v_note = 'Compilation and R code selection of optimal dates from BugsCEP into Excel.';
                    insert into tbl_dataset_submissions(dataset_id, submission_type_id, contact_id, date_submitted, notes)
                        values (v_dataset_id, 3, 1, '2021-10-28', v_note);

                    insert into tbl_dataset_contacts(contact_id, contact_type_id, dataset_id)
                        values (1, 6, v_dataset_id);

                end if;

                for v_id, v_sample_group_id, v_physical_sample_id, v_age_from, v_age_to, v_dating_specifier in (
                    select "id",  "sample_group_id", "physical_sample_id", "AgeFrom"::int, "AgeTo"::int,
                        array_to_string(Array[
                            case when "Chosen_C14" = '1' or "Chosen_C14" = 'True' then 'Chosen_C14' else null end,
                            case when "Chosen_OtherRadio" = '1' or "Chosen_OtherRadio" = 'True' then 'Chosen_OtherRadio' else null end,
                            case when "Chosen_Calendar" = '1' or "Chosen_Calendar" = 'True' then 'Chosen_Calendar' else null end,
                            case when "Chosen_Period" = '1' or "Chosen_Period" = 'True' then 'Chosen_Period' else null end
                        ], ';', null)
                    from bugs_import.results_chronology_import
                    where "change_request" = p_change_request
                      and "count_sheet_code" = v_count_sheet_code
                      and "is_ok"
                      and "analysis_entity_id" is null
                      and "dataset_id" is null
                    order by "count_sheet_code"
                )
                loop

                    v_analysis_entity_id = (select max(analysis_entity_id) from tbl_analysis_entities where physical_sample_id = v_physical_sample_id and dataset_id = v_dataset_id);

                    if v_analysis_entity_id is not null then

                        raise notice 'Physical sample % already exists in dataset %', v_physical_sample_id, v_dataset_id;

                        update tbl_analysis_entity_ages
                            set "age_older" = v_age_from,
                                "age_younger" = v_age_to,
                                "dating_specifier" = v_dating_specifier
                        where "analysis_entity_id" = v_analysis_entity_id;

                        -- insert into tbl_analysis_entity_ages("analysis_entity_id", "age", "age_older", "age_younger", "chronology_id", "dating_specifier")
                        --     values (v_analysis_entity_id, null, v_age_from, v_age_to, null, v_dating_specifier)
                        --         on conflict ("analysis_entity_id") do update
                        --             set "age_older" = v_age_from,
                        --                 "age_younger" = v_age_to,
                        --                 "dating_specifier" = v_dating_specifier;

                        update bugs_import.results_chronology_import
                            set "is_ok" = false,
                                "error" = 'Physical sample already exists in dataset',
                                "analysis_entity_id" = (select max(analysis_entity_id) from tbl_analysis_entities where physical_sample_id = v_physical_sample_id and dataset_id = v_dataset_id)
                            where "id" = v_id;

                        continue;

                    else

                        insert into tbl_analysis_entities("physical_sample_id", "dataset_id")
                            values (v_physical_sample_id, v_dataset_id)
                                returning "analysis_entity_id" into v_analysis_entity_id;

                        begin

                            insert into tbl_analysis_entity_ages("analysis_entity_id", "age", "age_older", "age_younger", "chronology_id", "dating_specifier")
                                values (v_analysis_entity_id, null, v_age_from, v_age_to, null, v_dating_specifier);

                        exception
                            when others then
                                v_note = format('Failed to insert age (%s, %s) for %s: %s', v_age_from, v_age_to, v_analysis_entity_id, SQLERRM);
                                raise notice '%', v_note;
                                update bugs_import.results_chronology_import
                                    set "is_ok" = false, "error" = v_note
                                    where "id" = v_id;
                                continue;
                        end;
                        
                        update bugs_import.results_chronology_import
                            set "analysis_entity_id" = v_analysis_entity_id,
                                "dataset_id" = v_dataset_id
                        where "id" = v_id;
                        
                    end if;
                    
                end loop;

            end loop;

            perform sead_utility.sync_sequences('public');

        end;
    end $$;


--
-- Name: is_integer(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.is_integer(text) RETURNS boolean
    LANGUAGE plpgsql IMMUTABLE
    AS $_$
        begin
            perform $1::integer;
            return true;
        exception when others then
            return false;
        end;
        $_$;


--
-- Name: is_numeric(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.is_numeric(text) RETURNS boolean
    LANGUAGE plpgsql IMMUTABLE
    AS $_$
        begin
            perform $1::numeric;
            return true;
        exception when others then
            return false;
        end;
        $_$;


--
-- Name: pascal_case_to_underscore(character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.pascal_case_to_underscore(p_token character varying) RETURNS character varying
    LANGUAGE plpgsql
    AS $_$
    begin
        return lower(regexp_replace(p_token,'([[:lower:]]|[0-9])([[:upper:]]|[0-9]$)','\1_\2','g'));
    end 
    $_$;


--
-- Name: release_allocated_ids(text, text, text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.release_allocated_ids(p_submission_identifier text, p_change_request_identifier text DEFAULT NULL::text, p_table_name text DEFAULT NULL::text, p_column_name text DEFAULT NULL::text) RETURNS void
    LANGUAGE plpgsql
    AS $$
        begin
            delete from sead_utility.system_id_allocations
            where submission_identifier = p_submission_identifier   
            and (p_change_request_identifier is null or change_request_identifier = p_change_request_identifier)
            and (p_table_name is null or table_name = p_table_name)
            and (p_column_name is null or column_name = p_column_name)
            ;
        end;
        $$;


--
-- Name: restore_view_definition(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.restore_view_definition(p_schema_name text, p_view_name text) RETURNS void
    LANGUAGE plpgsql
    AS $$
    declare
        v_view_definition text;
    begin
        select view_definition
            into v_view_definition
        from sead_utility.temp_view_definitions
        where schema_name = p_schema_name
          and view_name = p_view_name;
        v_view_definition = format('CREATE OR REPLACE VIEW %s.%s AS %s', p_schema_name, p_view_name, v_view_definition);
        execute v_view_definition;
    end;
    $$;


--
-- Name: schema_exists(text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.schema_exists(p_schema_name text) RETURNS boolean
    LANGUAGE sql
    AS $$
        select exists (
            select 1 from information_schema.schemata where schema_name = p_schema_name
        );
    $$;


--
-- Name: set_as_serial(character varying, character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.set_as_serial(p_table_name character varying, p_column_name character varying) RETURNS integer
    LANGUAGE plpgsql
    AS $$
    declare
        v_start_with integer;
        v_sequence_name text;
        v_sql text;
    begin

        select pg_get_serial_sequence(p_table_name, p_column_name)
            into v_sequence_name;

        execute format('select coalesce(max(%s), 0) + 1 from %s;', p_column_name, p_table_name) into v_start_with;

        if v_sequence_name is null then
            v_sequence_name = format('%s_%s_seq', p_table_name, p_column_name);
            execute format('create sequence %s start with %s owned by %s.%s;', v_sequence_name, v_start_with, p_table_name, p_column_name);
            execute format('alter sequence %s owner to sead_master;', v_sequence_name);
            execute format('alter table %s alter column %s set default nextval(''%s'');', p_table_name, p_column_name, v_sequence_name);
        else
            execute format('alter sequence %s owned by %s.%s;', v_sequence_name, p_table_name, p_column_name);
        end if;
        return v_start_with;
    end;
    $$;


--
-- Name: set_fk_is_deferrable(text, boolean, boolean); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.set_fk_is_deferrable(p_schema text, p_is_deferrable boolean, dry_run boolean DEFAULT false) RETURNS void
    LANGUAGE plpgsql
    AS $$
    declare
        v_sql text;
        r record;
    begin

        for r in
            select distinct tc.table_schema, tc.table_name, tc.constraint_name
            from information_schema.table_constraints AS tc
            join information_schema.key_column_usage AS kcu
              on tc.constraint_name = kcu.constraint_name
             and tc.table_schema = kcu.table_schema
            join information_schema.constraint_column_usage AS ccu
              on ccu.constraint_name = tc.constraint_name
             and ccu.table_schema = tc.table_schema
            where tc.table_schema = p_schema
             and tc.constraint_type = 'FOREIGN KEY'
             and tc.is_deferrable = case when p_is_deferrable = TRUE then 'NO' else 'YES' end
        loop
            if p_is_deferrable = TRUE then
                v_sql = format('alter table %s."%s" alter constraint "%s" deferrable;', -- initially immediate
                    r.table_schema,
                    r.table_name,
                    r.constraint_name
                );
            else
                v_sql = format('alter table %s."%s" alter constraint "%s" not deferrable;',
                    r.table_schema,
                    r.table_name,
                    r.constraint_name
                );
            end if;
            if not dry_run then
                execute v_sql;
            else
	            raise notice '%', v_sql;
			end if;
        end loop;
    end $$;


--
-- Name: set_schema_privilege(text, text, text, text[]); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.set_schema_privilege(IN p_schema_name text, IN p_user_name text, IN level text, VARIADIC p_for_roles text[] DEFAULT NULL::text[])
    LANGUAGE plpgsql
    AS $_$
    declare
        command text;
        v_for_role text;
    begin

        if p_for_roles is null or array_length(p_for_roles, 1) < 1 or p_for_roles[0] is null then
             p_for_roles = array['current_user'];
        end if;

        foreach v_for_role in array p_for_roles loop
            -- Revoke all privileges first
            command := format('
                revoke all on all tables in schema %1$I from %2$I; 
                revoke all on all sequences in schema %1$I from %2$I; 
                revoke all on all functions in schema %1$I from %2$I; 
                alter default privileges in schema %1$I for role %3$s revoke all on tables from %2$I; 
                alter default privileges in schema %1$I for role %3$s revoke all on sequences from %2$I; 
                alter default privileges in schema %1$I for role %3$s revoke all on functions from %2$I;
            ', p_schema_name, p_user_name, v_for_role);
            execute command;

            -- Grant privileges based on the level
            if level = 'read' then
                command := format('
                    grant select on all tables in schema %1$I to %2$I; 
                    grant select on all sequences in schema %1$I to %2$I; 
                    grant execute on all functions in schema %1$I to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant select on tables to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant select on sequences to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant execute on functions to %2$I;
                ', p_schema_name, p_user_name, v_for_role);
            elsif level = 'read/write' then
                command := format('
                    grant all on all tables in schema %1$I to %2$I; 
                    grant all on all sequences in schema %1$I to %2$I; 
                    grant execute on all functions in schema %1$I to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant all on tables to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant all on sequences to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant execute on functions to %2$I;
                ', p_schema_name, p_user_name, v_for_role);
            elsif level = 'admin' then
                command := format('
                    grant all on schema %1$I to %2$I; 
                    grant all on all tables in schema %1$I to %2$I; 
                    grant all on all sequences in schema %1$I to %2$I; 
                    grant all on all functions in schema %1$I to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant all on tables to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant all on sequences to %2$I; 
                    alter default privileges in schema %1$I for role %3$s grant all on functions to %2$I;
                ', p_schema_name, p_user_name, v_for_role);
            end if;

            if command is not null then
                -- raise notice '%', command;
                execute command;
            end if;
        end loop;
    end;
    $_$;


--
-- Name: setup_comments(); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.setup_comments()
    LANGUAGE plpgsql
    AS $$
        begin
            drop table if exists sead_utility.temp_comments;
            create table sead_utility.temp_comments (
                schema_name text,
                table_name text,
                column_name text,
                comment text
            );
        end;
    $$;


--
-- Name: store_view_definition(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.store_view_definition(p_schema_name text, p_view_name text) RETURNS void
    LANGUAGE plpgsql
    AS $$
    declare
        v_view_definition text;
    begin
        if not exists (
                select 1
                from pg_class c
                join pg_namespace n on n.oid = c.relnamespace
                where c.relname = 'temp_view_definitions'
                and n.nspname = 'sead_utility'
                and c.relkind = 'r'
            ) then
            create table if not exists sead_utility.temp_view_definitions (
                schema_name text not null,
                view_name text not null,
                view_definition text not null,
                timestamp timestamp default now(),
                primary key (schema_name, view_name)
            );
        end if;
        select pg_get_viewdef(format('%s.%s', p_schema_name, p_view_name), true) into v_view_definition;
        insert into sead_utility.temp_view_definitions (schema_name, view_name, view_definition)
            values (p_schema_name, p_view_name, v_view_definition)
            on conflict (schema_name, view_name) do update set view_definition = v_view_definition; 
    end;
    $$;


--
-- Name: sync_comments(); Type: PROCEDURE; Schema: sead_utility; Owner: -
--

CREATE PROCEDURE sead_utility.sync_comments()
    LANGUAGE plpgsql
    AS $$
        declare r record;
            v_sql text;
        begin
            -- Use format function instead of concatenation to avoid SQL injection
            for r in select * from sead_utility.temp_comments loop
                if r.column_name = '' then
                    v_sql := format('comment on table "%I"."%I" is %L;', r.schema, r.table_name, r.comment);
                elsif r.table_name = '' then
                    v_sql := format('comment on schema "%I" is %L;', r.schema_name, r.comment);
                else
                    v_sql := format('comment on column "%I"."%I"."%I" is %L;', r.schema_name, r.table_name, r.column_name, r.comment);
                end if;
                execute v_sql;
            end loop;
        end;
    $$;


--
-- Name: sync_sequence(character varying, character varying, character varying); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.sync_sequence(p_schema_name character varying, p_table_name character varying, p_column_name character varying DEFAULT NULL::character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
    declare	v_sequence_name text;
begin

    if p_column_name is null then
        select column_name into p_column_name
        from sead_utility.column_sequences
        where table_schema = p_schema_name
          and table_name = p_table_name
        limit 1;
    end if;

    v_sequence_name = pg_get_serial_sequence(p_schema_name || '.' || p_table_name, p_column_name);
    if v_sequence_name is not null then
        execute (select format('select setval(''%s''::regclass, greatest(coalesce(max(%I), 1), 1)) from %s.%s', v_sequence_name, p_column_name, p_schema_name, p_table_name::regclass));
    else
        raise notice 'Column %.%.% has no sequence', p_schema_name, p_table_name, p_column_name;
    end if;
end;
$$;


--
-- Name: sync_sequences(text, text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.sync_sequences(p_table_schema text DEFAULT NULL::text, p_table_name text DEFAULT NULL::text, p_column_name text DEFAULT NULL::text) RETURNS void
    LANGUAGE plpgsql
    AS $$
    declare sql record;
begin
	for sql in
        select format('select setval(''%I.%I'', greatest(coalesce(max(%I), 1), 1)) from %I.%I;', sequence_namespace, sequence_name, column_name, table_schema, table_name) as fix_query
        from sead_utility.column_sequences
        where table_schema = coalesce(p_table_schema, table_schema)
          and table_name = coalesce(p_table_name, table_name)
          and column_name = coalesce(p_column_name, column_name)
        order by sequence_name
    loop
		execute sql.fix_query;
	end loop;
end;
$$;


--
-- Name: table_exists(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.table_exists(p_schema_name text, p_table_name text) RETURNS boolean
    LANGUAGE sql
    AS $$
        select exists (
            select 1
            from pg_catalog.pg_class as c
            join pg_catalog.pg_namespace as ns
              on c.relnamespace = ns.oid
            where c.oid::regclass::text = p_table_name
              and ns.nspname = p_schema_name
        );
    $$;


--
-- Name: underscore_to_entity_name(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.underscore_to_entity_name(p_str text, p_sep text DEFAULT ''::text) RETURNS text
    LANGUAGE plpgsql
    AS $$
            declare v_result text;
            begin
                v_result = replace(initcap(replace(p_str, 'tbl_', '')), '_', coalesce(p_sep, ''));
                return v_result;
            end;
    $$;


--
-- Name: underscore_to_pascal_case(text, boolean); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.underscore_to_pascal_case(str text, lower_first boolean DEFAULT false) RETURNS text
    LANGUAGE plpgsql
    AS $$
    declare v_result text;
    begin
        v_result = replace(initcap(str), '_', '');
        if lower_first = true then
            v_result = concat(lower(substring(v_result from 1 for 1)), substring(v_result from 2));
        end if;
        return v_result;
    end;
    $$;


--
-- Name: view_exists(text, text); Type: FUNCTION; Schema: sead_utility; Owner: -
--

CREATE FUNCTION sead_utility.view_exists(p_schema_name text, p_view_name text) RETURNS boolean
    LANGUAGE sql
    AS $$
        select exists (
            select 1 from information_schema.views
            where table_schema = p_schema_name
              and table_name = p_view_name
        );
    $$;


--
-- Name: table_columns; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.table_columns AS
 WITH fk_constraint AS (
         SELECT DISTINCT fk_1.conrelid,
            fk_1.confrelid,
            fk_1.conkey,
            ((fk_1.confrelid)::regclass)::information_schema.sql_identifier AS fk_table_name,
            (fkc.attname)::information_schema.sql_identifier AS fk_column_name
           FROM (pg_constraint fk_1
             JOIN pg_attribute fkc ON (((fkc.attrelid = fk_1.confrelid) AND (fkc.attnum = fk_1.confkey[1]))))
          WHERE ((fk_1.contype)::text = ('f'::character(1))::text)
        )
 SELECT (pg_tables.schemaname)::information_schema.sql_identifier AS table_schema,
    (pg_tables.tablename)::information_schema.sql_identifier AS table_name,
    (pg_attribute.attname)::information_schema.sql_identifier AS column_name,
    (pg_attribute.attnum)::information_schema.cardinal_number AS ordinal_position,
    (format_type(pg_attribute.atttypid, NULL::integer))::information_schema.character_data AS data_type,
    (
        CASE pg_attribute.atttypid
            WHEN 21 THEN 16
            WHEN 23 THEN 32
            WHEN 20 THEN 64
            WHEN 1700 THEN
            CASE
                WHEN (pg_attribute.atttypmod = '-1'::integer) THEN NULL::integer
                ELSE (((pg_attribute.atttypmod - 4) >> 16) & 65535)
            END
            WHEN 700 THEN 24
            WHEN 701 THEN 53
            ELSE NULL::integer
        END)::information_schema.cardinal_number AS numeric_precision,
    (
        CASE
            WHEN (pg_attribute.atttypid = ANY (ARRAY[(21)::oid, (23)::oid, (20)::oid])) THEN 0
            WHEN (pg_attribute.atttypid = (1700)::oid) THEN
            CASE
                WHEN (pg_attribute.atttypmod = '-1'::integer) THEN NULL::integer
                ELSE ((pg_attribute.atttypmod - 4) & 65535)
            END
            ELSE NULL::integer
        END)::information_schema.cardinal_number AS numeric_scale,
    (
        CASE
            WHEN ((pg_attribute.atttypid <> ALL (ARRAY[(1042)::oid, (1043)::oid])) OR (pg_attribute.atttypmod = '-1'::integer)) THEN NULL::integer
            ELSE (pg_attribute.atttypmod - 4)
        END)::information_schema.cardinal_number AS character_maximum_length,
    (
        CASE pg_attribute.attnotnull
            WHEN false THEN 'YES'::text
            ELSE 'NO'::text
        END)::information_schema.yes_or_no AS is_nullable,
    (
        CASE
            WHEN (pk.contype IS NULL) THEN 'NO'::text
            ELSE 'YES'::text
        END)::information_schema.yes_or_no AS is_pk,
    (
        CASE
            WHEN (fk.conrelid IS NULL) THEN 'NO'::text
            ELSE 'YES'::text
        END)::information_schema.yes_or_no AS is_fk,
    fk.fk_table_name,
    fk.fk_column_name
   FROM (((((pg_tables
     JOIN pg_class ON ((pg_class.relname = pg_tables.tablename)))
     JOIN pg_namespace ns ON (((ns.oid = pg_class.relnamespace) AND (ns.nspname = pg_tables.schemaname))))
     JOIN pg_attribute ON (((pg_class.oid = pg_attribute.attrelid) AND (pg_attribute.attnum > 0))))
     LEFT JOIN pg_constraint pk ON (((pk.contype = 'p'::"char") AND (pk.conrelid = pg_class.oid) AND (pg_attribute.attnum = ANY (pk.conkey)))))
     LEFT JOIN fk_constraint fk ON (((fk.conrelid = pg_class.oid) AND (pg_attribute.attnum = ANY (fk.conkey)))))
  WHERE (true AND (pg_attribute.atttypid <> (0)::oid) AND (pg_tables.schemaname = 'public'::name))
  ORDER BY (pg_tables.tablename)::information_schema.sql_identifier, (pg_attribute.attnum)::information_schema.cardinal_number;


--
-- Name: column_sequences; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.column_sequences AS
 SELECT n.nspname AS table_schema,
    t.relname AS table_name,
    a.attname AS column_name,
    q.nspname AS sequence_namespace,
    s.relname AS sequence_name
   FROM (((((pg_class s
     JOIN pg_depend d ON ((s.oid = d.objid)))
     JOIN pg_class t ON ((t.oid = d.refobjid)))
     JOIN pg_attribute a ON (((a.attrelid = t.oid) AND (d.refobjsubid = a.attnum))))
     JOIN pg_namespace n ON ((n.oid = t.relnamespace)))
     JOIN pg_namespace q ON ((q.oid = s.relnamespace)))
  WHERE (s.relkind = 'S'::"char")
  ORDER BY q.nspname, s.relname;


--
-- Name: encoded_dendro_analysis_values; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.encoded_dendro_analysis_values AS
 WITH regular_expressions AS (
         SELECT '^\d{3,4}\s*\-\s*\d{3,4}$'::text AS regex_is_year_range,
            '^(\d{3,4})\s*-\s*(\d{3,4})$'::text AS regex_year_range_extract,
            '^(-?\d+)\s*±\s*(\d+)$'::text AS regex_plus_minus_extract,
            '^-?\d+\s*±\s*\d+$'::text AS regex_is_plus_minus_range,
            '^V\s*\d{4}/\d{2,4}$'::text AS regex_is_winter_year,
            '(\d{4})/(\d{2,4})$'::text AS regex_winter_year_extract,
            '^E\s*\d{3,4}$'::text AS regex_is_after_year,
            '^[a-zA-ZåäöÅÄÖ]{2,}\s+\d{4}$'::text AS regex_is_year_with_specifier,
            '^([a-zA-ZåäöÅÄÖ]{2,})\s*(\d{4})$'::text AS regex_year_with_specifier_extract,
            ( SELECT format('^(%s)'::text, string_agg(regexp_replace(tbl_value_qualifier_symbols.symbol, '(\.|\^|\$|\||\(|\)|\{|\}|\*|\+|\?|\[|\]])'::text, '\\\1'::text, 'g'::text), '|'::text)) AS symbol
                   FROM public.tbl_value_qualifier_symbols) AS regex_is_qualifier
        ), analysis_entities AS (
         SELECT DISTINCT x.analysis_entity_id
           FROM ( SELECT tbl_dendro.analysis_entity_id
                   FROM public.tbl_dendro
                UNION
                 SELECT DISTINCT tbl_dendro_dates.analysis_entity_id
                   FROM public.tbl_dendro_dates) x
        ), analysis_values AS (
         SELECT vc_1.analysis_value_id,
            vc_1.analysis_entity_id,
            vc_1.value_class_id,
            vc_1.analysis_value
           FROM (public.tbl_analysis_values vc_1
             JOIN analysis_entities USING (analysis_entity_id))
        ), analysis_values_without_uncertanty AS (
         SELECT analysis_values.analysis_value_id,
            (analysis_values.analysis_value ~* '^eventuellt|\?$'::text) AS has_uncertainty_indicator,
            "substring"(analysis_values.analysis_value, '(?i)^eventuellt|\?$'::text) AS uncertainty_indicator,
            TRIM(BOTH FROM
                CASE
                    WHEN (analysis_values.analysis_value ~* '^eventuellt|\?$'::text) THEN regexp_replace(analysis_values.analysis_value, '(?i)^eventuellt|\?$'::text, ''::text, 'i'::text)
                    ELSE analysis_values.analysis_value
                END) AS value_without_uncertainty
           FROM analysis_values
        ), analysis_values_with_flags AS (
         SELECT analysis_values.analysis_value_id,
            analysis_values.value_class_id,
            analysis_values.analysis_value,
            vt_1.base_type,
            ((analysis_values_without_uncertanty.value_without_uncertainty ~* regular_expressions.regex_is_qualifier) OR (analysis_values_without_uncertanty.value_without_uncertainty ~* regular_expressions.regex_is_after_year)) AS has_qualifier,
                CASE
                    WHEN (analysis_values_without_uncertanty.value_without_uncertainty ~* regular_expressions.regex_is_qualifier) THEN "substring"(analysis_values_without_uncertanty.value_without_uncertainty, ('(?i)'::text || regular_expressions.regex_is_qualifier))
                    ELSE NULL::text
                END AS qualifier,
            analysis_values_without_uncertanty.has_uncertainty_indicator,
            analysis_values_without_uncertanty.uncertainty_indicator,
            ((analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_year_range) OR (analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_plus_minus_range)) AS is_range,
            (analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_year_range) AS is_lower_upper_range,
            (analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_plus_minus_range) AS is_plus_minus_range,
            (analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_winter_year) AS is_winter_year,
            (analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_after_year) AS is_after_year,
            (analysis_values_without_uncertanty.value_without_uncertainty ~ regular_expressions.regex_is_year_with_specifier) AS is_year_with_specifier,
            ((length(analysis_values_without_uncertanty.value_without_uncertainty) > 50) OR (vt_1.base_type = 'text'::text)) AS is_note
           FROM ((((analysis_values
             JOIN analysis_values_without_uncertanty USING (analysis_value_id))
             JOIN public.tbl_value_classes vc_1 USING (value_class_id))
             JOIN public.tbl_value_types vt_1 USING (value_type_id))
             CROSS JOIN regular_expressions)
        ), stripped_values AS (
         SELECT f.analysis_value_id,
            TRIM(BOTH FROM
                CASE
                    WHEN f.is_after_year THEN regexp_replace(analysis_values_without_uncertanty.value_without_uncertainty, '^E\s*'::text, ''::text, 'i'::text)
                    WHEN f.is_winter_year THEN regexp_replace(analysis_values_without_uncertanty.value_without_uncertainty, '^V\s*'::text, ''::text, 'i'::text)
                    WHEN f.has_qualifier THEN regexp_replace(analysis_values_without_uncertanty.value_without_uncertainty, regular_expressions.regex_is_qualifier, ''::text, 'i'::text)
                    ELSE analysis_values_without_uncertanty.value_without_uncertainty
                END) AS stripped_value
           FROM ((analysis_values_with_flags f
             JOIN analysis_values_without_uncertanty USING (analysis_value_id))
             CROSS JOIN regular_expressions)
        ), value_pattern AS (
         SELECT analysis_values_with_flags_1.analysis_value_id,
                CASE
                    WHEN analysis_values_with_flags_1.is_after_year THEN 'E YYYY'::text
                    WHEN analysis_values_with_flags_1.is_year_with_specifier THEN 'SPECIFIER YYYY'::text
                    WHEN analysis_values_with_flags_1.is_winter_year THEN 'V YYYY/YY'::text
                    WHEN analysis_values_with_flags_1.is_range THEN 'RANGE'::text
                    WHEN sead_utility.is_numeric(stripped_values.stripped_value) THEN 'INTEGER'::text
                    WHEN (lower(analysis_values_with_flags_1.analysis_value) = 'undefined'::text) THEN 'UNDEFINED'::text
                    WHEN analysis_values_with_flags_1.is_note THEN 'NOTE'::text
                    ELSE upper(stripped_values.stripped_value)
                END AS pattern
           FROM (analysis_values_with_flags analysis_values_with_flags_1
             JOIN stripped_values USING (analysis_value_id))
        ), intermediate_typed_values AS (
         SELECT analysis_values_with_flags_1.analysis_value_id,
            lower_upper_range_table.lower_range_value,
            lower_upper_range_table.upper_range_value,
            plus_minus_range_table.plus_minus_year_value,
            plus_minus_range_table.plus_minus_value,
            plus_minus_range_table.plus_minus_qualifier,
            after_year_table.after_year_value,
            after_year_table.after_qualifier,
            winter_table.winter_value,
            year_with_specifier_table.year_with_season_value,
            year_with_specifier_table.season_specifier
           FROM (((((((analysis_values_with_flags analysis_values_with_flags_1
             JOIN stripped_values USING (analysis_value_id))
             CROSS JOIN regular_expressions)
             LEFT JOIN LATERAL ( SELECT ((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_year_range_extract))[1])::integer AS lower_range_value,
                    ((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_year_range_extract))[2])::integer AS upper_range_value
                  WHERE analysis_values_with_flags_1.is_lower_upper_range) lower_upper_range_table ON (true))
             LEFT JOIN LATERAL ( SELECT ((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_plus_minus_extract))[1])::integer AS plus_minus_year_value,
                    ((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_plus_minus_extract))[2])::integer AS plus_minus_value,
                    '±'::text AS plus_minus_qualifier
                  WHERE analysis_values_with_flags_1.is_plus_minus_range) plus_minus_range_table ON (true))
             LEFT JOIN LATERAL ( SELECT ARRAY[((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_winter_year_extract))[1])::integer, ((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_winter_year_extract))[2])::integer] AS winter_value
                  WHERE analysis_values_with_flags_1.is_winter_year) winter_table ON (true))
             LEFT JOIN LATERAL ( SELECT ((regexp_matches(stripped_values.stripped_value, '^(\d{3,4})$'::text))[1])::integer AS after_year_value,
                    'efter'::text AS after_qualifier
                  WHERE analysis_values_with_flags_1.is_after_year) after_year_table ON (true))
             LEFT JOIN LATERAL ( SELECT (regexp_matches(stripped_values.stripped_value, regular_expressions.regex_year_with_specifier_extract))[1] AS season_specifier,
                    ((regexp_matches(stripped_values.stripped_value, regular_expressions.regex_year_with_specifier_extract))[2])::integer AS year_with_season_value
                  WHERE analysis_values_with_flags_1.is_year_with_specifier) year_with_specifier_table ON (true))
        ), typed_values AS (
         SELECT analysis_values_with_flags_1.analysis_value_id,
                CASE
                    WHEN ((analysis_values_with_flags_1.base_type = ANY (ARRAY['integer'::text, 'int4range'::text])) AND sead_utility.is_integer(stripped_values.stripped_value)) THEN (stripped_values.stripped_value)::integer
                    WHEN analysis_values_with_flags_1.is_year_with_specifier THEN intermediate_typed_values_1.year_with_season_value
                    WHEN (intermediate_typed_values_1.after_year_value IS NOT NULL) THEN intermediate_typed_values_1.after_year_value
                    WHEN (intermediate_typed_values_1.winter_value IS NOT NULL) THEN intermediate_typed_values_1.winter_value[1]
                    ELSE NULL::integer
                END AS integer_value,
                CASE
                    WHEN ((analysis_values_with_flags_1.base_type = 'numeric'::text) AND sead_utility.is_numeric(stripped_values.stripped_value)) THEN (stripped_values.stripped_value)::numeric(20,10)
                    ELSE NULL::numeric
                END AS decimal_value,
                CASE
                    WHEN (analysis_values_with_flags_1.base_type = 'boolean'::text) THEN
                    CASE
                        WHEN (lower(stripped_values.stripped_value) = 'yes'::text) THEN true
                        WHEN (lower(stripped_values.stripped_value) = 'true'::text) THEN true
                        WHEN (lower(stripped_values.stripped_value) = 'ja'::text) THEN true
                        WHEN (lower(stripped_values.stripped_value) = 'no'::text) THEN false
                        WHEN (lower(stripped_values.stripped_value) = 'false'::text) THEN false
                        WHEN (lower(stripped_values.stripped_value) = 'nej'::text) THEN false
                        ELSE NULL::boolean
                    END
                    ELSE NULL::boolean
                END AS boolean_value
           FROM ((analysis_values_with_flags analysis_values_with_flags_1
             JOIN stripped_values USING (analysis_value_id))
             JOIN intermediate_typed_values intermediate_typed_values_1 USING (analysis_value_id))
        )
 SELECT analysis_values_with_flags.analysis_value_id,
    analysis_values_with_flags.analysis_value,
    ss.stripped_value,
    analysis_values_with_flags.value_class_id,
    vc.name AS value_class_name,
    vt.name AS value_type_name,
    vt.base_type,
    analysis_values_with_flags.uncertainty_indicator,
    lower(COALESCE(analysis_values_with_flags.qualifier, intermediate_typed_values.plus_minus_qualifier, intermediate_typed_values.after_qualifier)) AS qualifier,
        CASE
            WHEN analysis_values_with_flags.is_winter_year THEN 'V'::text
            ELSE intermediate_typed_values.season_specifier
        END AS season_specifier,
    tv.decimal_value,
    tv.boolean_value,
    tv.integer_value,
    intermediate_typed_values.lower_range_value,
    intermediate_typed_values.upper_range_value,
    intermediate_typed_values.plus_minus_year_value,
    intermediate_typed_values.plus_minus_value,
    p.pattern
   FROM ((((((analysis_values_with_flags
     JOIN value_pattern p USING (analysis_value_id))
     JOIN stripped_values ss USING (analysis_value_id))
     JOIN typed_values tv USING (analysis_value_id))
     JOIN public.tbl_value_classes vc USING (value_class_id))
     JOIN public.tbl_value_types vt USING (value_type_id))
     JOIN intermediate_typed_values USING (analysis_value_id));


--
-- Name: foreign_key_columns; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.foreign_key_columns AS
 SELECT tc.table_schema,
    tc.constraint_name,
    tc.table_name,
    kcu.column_name,
    ccu.table_schema AS foreign_table_schema,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name
   FROM ((information_schema.table_constraints tc
     JOIN information_schema.key_column_usage kcu ON ((((tc.constraint_name)::name = (kcu.constraint_name)::name) AND ((tc.table_schema)::name = (kcu.table_schema)::name))))
     JOIN information_schema.constraint_column_usage ccu ON ((((ccu.constraint_name)::name = (tc.constraint_name)::name) AND ((ccu.table_schema)::name = (tc.table_schema)::name))))
  WHERE ((tc.constraint_type)::text = 'FOREIGN KEY'::text);


--
-- Name: foreign_keys_index_check; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.foreign_keys_index_check AS
 WITH fk_actions(code, action) AS (
         VALUES ('a'::text,'error'::text), ('r'::text,'restrict'::text), ('c'::text,'cascade'::text), ('n'::text,'set null'::text), ('d'::text,'set default'::text)
        ), fk_list AS (
         SELECT pg_constraint.oid AS fkoid,
            pg_constraint.conrelid,
            pg_constraint.confrelid AS parentid,
            pg_constraint.conname,
            pg_class.relname,
            pg_namespace.nspname,
            fk_actions_update.action AS update_action,
            fk_actions_delete.action AS delete_action,
            pg_constraint.conkey AS key_cols
           FROM ((((pg_constraint
             JOIN pg_class ON ((pg_constraint.conrelid = pg_class.oid)))
             JOIN pg_namespace ON ((pg_class.relnamespace = pg_namespace.oid)))
             JOIN fk_actions fk_actions_update ON (((pg_constraint.confupdtype)::text = fk_actions_update.code)))
             JOIN fk_actions fk_actions_delete ON (((pg_constraint.confdeltype)::text = fk_actions_delete.code)))
          WHERE (pg_constraint.contype = 'f'::"char")
        ), fk_attributes AS (
         SELECT fk_list.fkoid,
            fk_list.conrelid,
            pg_attribute.attname,
            pg_attribute.attnum
           FROM (fk_list
             JOIN pg_attribute ON (((fk_list.conrelid = pg_attribute.attrelid) AND (pg_attribute.attnum = ANY (fk_list.key_cols)))))
          ORDER BY fk_list.fkoid, pg_attribute.attnum
        ), fk_cols_list AS (
         SELECT fk_attributes.fkoid,
            array_agg(fk_attributes.attname) AS cols_list
           FROM fk_attributes
          GROUP BY fk_attributes.fkoid
        ), index_list AS (
         SELECT pg_index.indexrelid AS indexid,
            pg_class.relname AS indexname,
            pg_index.indrelid,
            pg_index.indkey,
            (pg_index.indpred IS NOT NULL) AS has_predicate,
            pg_get_indexdef(pg_index.indexrelid) AS indexdef
           FROM (pg_index
             JOIN pg_class ON ((pg_index.indexrelid = pg_class.oid)))
          WHERE pg_index.indisvalid
        ), fk_index_match AS (
         SELECT fk_list.fkoid,
            fk_list.conrelid,
            fk_list.parentid,
            fk_list.conname,
            fk_list.relname,
            fk_list.nspname,
            fk_list.update_action,
            fk_list.delete_action,
            fk_list.key_cols,
            index_list.indexid,
            index_list.indexname,
            (index_list.indkey)::integer[] AS indexatts,
            index_list.has_predicate,
            index_list.indexdef,
            array_length(fk_list.key_cols, 1) AS fk_colcount,
            array_length(index_list.indkey, 1) AS index_colcount,
            round(((pg_relation_size((fk_list.conrelid)::regclass))::numeric / (((1024)::double precision ^ (2)::double precision))::numeric)) AS table_mb,
            fk_cols_list.cols_list
           FROM ((fk_list
             JOIN fk_cols_list USING (fkoid))
             LEFT JOIN index_list ON (((fk_list.conrelid = index_list.indrelid) AND (((index_list.indkey)::smallint[])[0:(array_length(fk_list.key_cols, 1) - 1)] @> fk_list.key_cols))))
        ), fk_perfect_match AS (
         SELECT fk_index_match.fkoid
           FROM fk_index_match
          WHERE (((fk_index_match.index_colcount - 1) <= fk_index_match.fk_colcount) AND (NOT fk_index_match.has_predicate) AND (fk_index_match.indexdef ~~ '%USING btree%'::text))
        ), fk_index_check AS (
         SELECT 'no index'::text AS issue,
            fk_index_match.fkoid,
            fk_index_match.conrelid,
            fk_index_match.parentid,
            fk_index_match.conname,
            fk_index_match.relname,
            fk_index_match.nspname,
            fk_index_match.update_action,
            fk_index_match.delete_action,
            fk_index_match.key_cols,
            fk_index_match.indexid,
            fk_index_match.indexname,
            fk_index_match.indexatts,
            fk_index_match.has_predicate,
            fk_index_match.indexdef,
            fk_index_match.fk_colcount,
            fk_index_match.index_colcount,
            fk_index_match.table_mb,
            fk_index_match.cols_list,
            1 AS issue_sort
           FROM fk_index_match
          WHERE (fk_index_match.indexid IS NULL)
        UNION ALL
         SELECT 'questionable index'::text AS issue,
            fk_index_match.fkoid,
            fk_index_match.conrelid,
            fk_index_match.parentid,
            fk_index_match.conname,
            fk_index_match.relname,
            fk_index_match.nspname,
            fk_index_match.update_action,
            fk_index_match.delete_action,
            fk_index_match.key_cols,
            fk_index_match.indexid,
            fk_index_match.indexname,
            fk_index_match.indexatts,
            fk_index_match.has_predicate,
            fk_index_match.indexdef,
            fk_index_match.fk_colcount,
            fk_index_match.index_colcount,
            fk_index_match.table_mb,
            fk_index_match.cols_list,
            2
           FROM fk_index_match
          WHERE ((fk_index_match.indexid IS NOT NULL) AND (NOT (fk_index_match.fkoid IN ( SELECT fk_perfect_match.fkoid
                   FROM fk_perfect_match))))
        ), parent_table_stats AS (
         SELECT fk_list.fkoid,
            tabstats.relname AS parent_name,
            (((tabstats.n_tup_ins + tabstats.n_tup_upd) + tabstats.n_tup_del) + tabstats.n_tup_hot_upd) AS parent_writes,
            round(((pg_relation_size((fk_list.parentid)::regclass))::numeric / (((1024)::double precision ^ (2)::double precision))::numeric)) AS parent_mb
           FROM (pg_stat_user_tables tabstats
             JOIN fk_list ON ((tabstats.relid = fk_list.parentid)))
        ), fk_table_stats AS (
         SELECT fk_list.fkoid,
            (((tabstats.n_tup_ins + tabstats.n_tup_upd) + tabstats.n_tup_del) + tabstats.n_tup_hot_upd) AS writes,
            tabstats.seq_scan AS table_scans
           FROM (pg_stat_user_tables tabstats
             JOIN fk_list ON ((tabstats.relid = fk_list.conrelid)))
        )
 SELECT fk_index_check.nspname AS schema_name,
    fk_index_check.relname AS table_name,
    fk_index_check.conname AS fk_name,
    fk_index_check.issue,
    fk_index_check.table_mb,
    fk_table_stats.writes,
    fk_table_stats.table_scans,
    parent_table_stats.parent_name,
    parent_table_stats.parent_mb,
    parent_table_stats.parent_writes,
    fk_index_check.cols_list,
    fk_index_check.indexdef
   FROM ((fk_index_check
     JOIN parent_table_stats USING (fkoid))
     JOIN fk_table_stats USING (fkoid))
  WHERE true
  ORDER BY fk_index_check.issue_sort, fk_index_check.table_mb DESC, fk_index_check.relname, fk_index_check.conname;


--
-- Name: foreign_keys_index_check2; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.foreign_keys_index_check2 AS
 SELECT (c.connamespace)::regclass AS connamespace,
    (c.conrelid)::regclass AS table_with_fk,
    a.attname AS fk_column
   FROM (pg_attribute a
     JOIN pg_constraint c ON ((a.attnum = ANY (c.conkey))))
  WHERE ((c.confrelid > (0)::oid) AND (NOT (EXISTS ( SELECT 1
           FROM pg_index i
          WHERE ((i.indrelid = c.conrelid) AND (a.attnum = ANY ((i.indkey)::smallint[])))))) AND (a.attnum > 0) AND (NOT a.attisdropped) AND (a.attrelid = c.conrelid) AND (c.connamespace = ('public'::regnamespace)::oid));


--
-- Name: out_of_sync_sequences; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.out_of_sync_sequences AS
 WITH sequence_counter_values AS (
         SELECT column_sequences.table_schema,
            column_sequences.table_name,
            column_sequences.column_name,
            column_sequences.sequence_namespace,
            column_sequences.sequence_name,
            COALESCE(currval((pg_get_serial_sequence(format('%I.%I'::text, column_sequences.table_schema, column_sequences.table_name), (column_sequences.column_name)::text))::regclass), (1)::bigint) AS current_value,
            sead_utility.get_max_sequence_id((column_sequences.table_schema)::text, (column_sequences.table_name)::text, (column_sequences.column_name)::text) AS max_value
           FROM sead_utility.column_sequences
        )
 SELECT table_schema,
    table_name,
    column_name,
    sequence_namespace,
    sequence_name,
    current_value,
    max_value
   FROM sequence_counter_values
  WHERE (current_value <> COALESCE((max_value)::bigint, current_value));


--
-- Name: physical_sample_dating; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.physical_sample_dating AS
 WITH abundance_analysis AS (
         SELECT ae.physical_sample_id
           FROM ((public.tbl_analysis_entities ae
             JOIN public.tbl_abundances ab_1 USING (analysis_entity_id))
             JOIN public.tbl_taxa_tree_master tm USING (taxon_id))
          GROUP BY ae.physical_sample_id
        ), dating AS (
         SELECT a.physical_sample_id,
            1 AS dating_type,
            aea.age_older,
            aea.age_younger,
            NULL::character varying AS age_name,
            NULL::character varying AS age_abbreviation
           FROM (public.tbl_analysis_entities a
             JOIN public.tbl_analysis_entity_ages aea USING (analysis_entity_id))
          GROUP BY a.physical_sample_id, aea.age_older, aea.age_younger
        UNION
         SELECT ae.physical_sample_id,
            2 AS dating_type,
            ra.cal_age_older AS age_older,
            ra.cal_age_younger AS age_younger,
            ra.relative_age_name AS age_name,
            ra.abbreviation AS age_abbreviation
           FROM ((public.tbl_analysis_entities ae
             JOIN public.tbl_relative_dates rd ON ((rd.analysis_entity_id = ae.analysis_entity_id)))
             JOIN public.tbl_relative_ages ra ON ((rd.relative_age_id = ra.relative_age_id)))
          GROUP BY ae.physical_sample_id, 2::integer, ra.cal_age_older, ra.cal_age_younger, ra.relative_age_name, ra.abbreviation
        ), site_location AS (
         SELECT s.site_id,
            s.site_name,
            s.latitude_dd,
            s.longitude_dd,
            sg.sample_group_id,
            l.location_type_id,
            l.location_name
           FROM ((((public.tbl_sample_groups sg
             JOIN public.tbl_sites s ON ((s.site_id = sg.site_id)))
             JOIN public.tbl_site_locations sl_1 ON ((sl_1.site_id = s.site_id)))
             JOIN public.tbl_locations l ON ((l.location_id = sl_1.location_id)))
             JOIN public.tbl_location_types lt ON ((lt.location_type_id = l.location_type_id)))
          WHERE (NOT ((s.latitude_dd IS NULL) OR (s.longitude_dd IS NULL)))
          GROUP BY s.site_id, s.site_name, s.latitude_dd, s.longitude_dd, sg.sample_group_id, l.location_type_id, l.location_name
        )
 SELECT sl.location_name,
    sl.site_id,
    sl.site_name,
    sl.latitude_dd,
    sl.longitude_dd,
    ps.physical_sample_id,
    d.dating_type,
    d.age_older,
    d.age_younger,
    d.age_name,
    d.age_abbreviation
   FROM (((public.tbl_physical_samples ps
     JOIN site_location sl USING (sample_group_id))
     JOIN abundance_analysis ab USING (physical_sample_id))
     LEFT JOIN dating d USING (physical_sample_id))
  WHERE (sl.location_type_id = 1);


--
-- Name: sead_comments; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.sead_comments AS
 SELECT schema_name,
    table_name,
    column_name,
    objsubid,
    (description)::character varying(2048) AS description
   FROM ( SELECT pg_namespace.nspname AS schema_name,
            pg_class.relname AS table_name,
            ''::name AS column_name,
            pg_description.objsubid,
            pg_description.description
           FROM ((pg_description
             JOIN pg_class ON ((pg_description.objoid = pg_class.oid)))
             JOIN pg_namespace ON ((pg_class.relnamespace = pg_namespace.oid)))
        UNION ALL
         SELECT c_1.table_schema,
            c_1.table_name,
            c_1.column_name,
            d.objsubid,
            d.description
           FROM (pg_description d
             JOIN information_schema.columns c_1 ON ((((((((c_1.table_schema)::text || '.'::text) || (c_1.table_name)::text))::regclass)::oid = d.objoid) AND ((c_1.ordinal_position)::integer = d.objsubid))))) c
  WHERE ((COALESCE(description, ''::text) <> ''::text) AND (schema_name = 'public'::name))
  ORDER BY schema_name, table_name, column_name;


SET default_table_access_method = heap;

--
-- Name: system_id_allocations; Type: TABLE; Schema: sead_utility; Owner: -
--

CREATE TABLE sead_utility.system_id_allocations (
    uuid text null,
    table_name text NOT NULL,
    column_name text NOT NULL,
    submission_identifier text NOT NULL,
    change_request_identifier text NOT NULL,
    external_system_id text,
    external_data json,
    alloc_system_id integer NOT NULL
);


--
-- Name: table_dependencies; Type: VIEW; Schema: sead_utility; Owner: -
--

CREATE VIEW sead_utility.table_dependencies AS
 WITH RECURSIVE t AS (
         SELECT c.oid AS origin_id,
            ((c.oid)::regclass)::text AS origin_table,
            n.nspname AS origin_schema,
            c.oid AS referencing_id,
            ((c.oid)::regclass)::text AS referencing_table,
            c2.oid AS referenced_id,
            ((c2.oid)::regclass)::text AS referenced_table,
            ARRAY[(c.oid)::regclass, (c2.oid)::regclass] AS chain
           FROM (((pg_constraint co
             JOIN pg_class c ON ((c.oid = co.conrelid)))
             JOIN pg_namespace n ON ((n.oid = c.relnamespace)))
             JOIN pg_class c2 ON ((c2.oid = co.confrelid)))
        UNION ALL
         SELECT t_1.origin_id,
            t_1.origin_table,
            t_1.origin_schema,
            t_1.referenced_id AS referencing_id,
            t_1.referenced_table AS referencing_table,
            c3.oid AS referenced_id,
            ((c3.oid)::regclass)::text AS referenced_table,
            (t_1.chain || (c3.oid)::regclass) AS chain
           FROM ((pg_constraint co
             JOIN pg_class c3 ON ((c3.oid = co.confrelid)))
             JOIN t t_1 ON ((t_1.referenced_id = co.conrelid)))
          WHERE (NOT (ARRAY[t_1.chain[array_upper(t_1.chain, 1)]] <@ t_1.chain[1:(array_upper(t_1.chain, 1) - 1)]))
        )
 SELECT origin_table,
    origin_schema,
    referenced_table,
    array_upper(chain, 1) AS depth,
    array_to_string(chain, ','::text) AS chain
   FROM t;


--
-- Name: system_id_allocations system_id_allocations_pkey; Type: CONSTRAINT; Schema: sead_utility; Owner: -
--

ALTER TABLE ONLY sead_utility.system_id_allocations
    ADD CONSTRAINT system_id_allocations_pkey PRIMARY KEY (uuid);


--
-- PostgreSQL database dump complete
--

