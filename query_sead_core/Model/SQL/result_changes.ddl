
select *
from facet.result_definition

select *
from facet.result_definition_field

select array_agg(result_field_key)
from facet.result_field

alter table facet.result_field_type rename column result_field_type to field_type;
alter table facet.result_field_type rename column result_field_type_id to field_type_id;

alter table facet.result_field rename column result_type_key to result_type_key;

drop table facet.result_field_type;
create table facet.result_field_type (
	field_type_id varchar(40) not null,
	is_result_value boolean not null default(true),
	CONSTRAINT result_field_type_pk PRIMARY KEY (field_type_id)
);

 
ALTER TABLE facet.result_field
  ADD CONSTRAINT result_fieldfk3 FOREIGN KEY (field_type_id)
      REFERENCES facet.result_field_type (field_type_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION;

select * from facet.result_definition_field
select * from facet.result_field_type;
alter table facet.result_definition_field drop column result_type_id

update facet.result_definition_field
    set field_type_id = 
        Case When result_type_id = 1 then 'single_item'
             When result_type_id = 2 then 'text_agg_item'
             When result_type_id = 3 then 'count_item'
             When result_type_id = 4 then 'link_item'
             When result_type_id = 5 then 'sort_item'
             When result_type_id = 6 then 'link_item_filtered' Else '' End;

insert into facet.result_field_type values ('single_item', TRUE), ('text_agg_item', TRUE), ('count_item', TRUE), ('link_item', TRUE), ('sort_item', FALSE), ('link_item_filtered', TRUE);


GRANT SELECT ON TABLE facet.result_definition TO seadread;
GRANT SELECT ON TABLE facet.result_definition_field TO seadread;
GRANT SELECT ON TABLE facet.result_type TO seadread;
GRANT SELECT ON TABLE facet.result_field TO seadread;


select * from facet.result_field;
select * from facet.result_field_type;

alter table facet.result_field_type add column is_result_value boolean not null default(true);
alter table facet.result_field_type add column sql_field_compiler character varying(40) not null default('');
alter table facet.result_field_type add column is_item_field boolean not null default(FALSE);
alter table facet.result_field_type add column is_aggregate_field boolean not null default(FALSE);
alter table facet.result_field_type add column is_sort_field boolean not null default(FALSE);
alter table facet.result_field_type add column sql_template character varying(256) not null default('{0}');

insert into facet.result_field_type values ('sum_item',true)
insert into facet.result_field_type values ('avg_item',true)

update facet.result_field_type set is_sort_field = field_type_id = 'sort_item';
update facet.result_field_type set is_sort_field = field_type_id <> 'sort_item';
update facet.result_field_type set is_aggregate_field = field_type_id in ('avg_item', 'text_agg_item', 'sum_item', 'count_item');
update facet.result_field_type set is_item_field = field_type_id in ('single_item', 'link_item', 'link_item_filtered');

update facet.result_field_type set sql_field_compiler = 'TemplateFieldCompiler';

--update facet.result_field_type set sql_field_compiler = 'SumFieldCompiler' where field_type_id = 'sum_item';
--update facet.result_field_type set sql_field_compiler = 'CountFieldCompiler' where field_type_id = 'count_item';
--update facet.result_field_type set sql_field_compiler = 'AvgFieldCompiler' where field_type_id = 'avg_item';
--update facet.result_field_type set sql_field_compiler = 'TextAggFieldCompiler' where field_type_id = 'text_agg_item';
--update facet.result_field_type set sql_field_compiler = 'DefaultFieldCompiler' where field_type_id = 'single_item';
--update facet.result_field_type set sql_field_compiler = 'DefaultFieldCompiler' where field_type_id = 'link_item';
--update facet.result_field_type set sql_field_compiler = 'DefaultFieldCompiler' where field_type_id = 'link_item_filtered';

update facet.result_field_type set sql_template = 'SUM({0}::double precision) AS sum_of_{0}' where field_type_id = 'sum_item';
update facet.result_field_type set sql_template = 'COUNT({0}) AS count_of_{0}' where field_type_id = 'count_item';
update facet.result_field_type set sql_template = 'AVG({0}) AS avg_of_{0}' where field_type_id = 'avg_item';
update facet.result_field_type set sql_template = 'ARRAY_TO_STRING(ARRAY_AGG(DISTINCT {0}),'','') AS text_agg_of_{0}' where field_type_id = 'text_agg_item';
update facet.result_field_type set sql_template = '{0}' where field_type_id in ('single_item', 'link_item',  'link_item_filtered'); 
  
  CONSTRAINT result_definition_fieldfk1 FOREIGN KEY (result_definition_id)
      REFERENCES facet.result_definition (result_definition_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
      
  CONSTRAINT result_definition_fieldfk2 FOREIGN KEY (result_field_id)
      REFERENCES facet.result_field (result_field_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
      
  CONSTRAINT result_definition_fieldfk3 FOREIGN KEY (result_type_id)
      REFERENCES facet.result_type (result_type_id) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION


select * --d.result_definition_key, f.*
from facet.result_definition d
join facet.result_definition_field f
  on f.result_definition_id = d.result_definition_id

"site_level", "aggregate_all", "sample_group_level"

alter table facet.result_definition drop column aggregation_type
alter table facet.result_definition rename column result_definition_key to aggregation_key

alter table facet.result_definition rename column result_definition_id to aggregate_id;
alter table facet.result_definition rename column aggregation_key to aggregate_key;
alter table facet.result_definition rename column has_aggregation_selector to has_selector;

alter table facet.result_definition_field rename column result_definition_field_id to aggregate_field_id;
alter table facet.result_definition_field rename column result_definition_id to aggregate_id;

alter table facet.result_definition rename to result_aggregate;
alter table facet.result_definition_field rename to result_aggregate_field;



GRANT SELECT ON TABLE facet.result_aggregate TO seadread;
GRANT SELECT ON TABLE facet.result_aggregate_field TO seadread;
GRANT SELECT ON TABLE facet.result_field_type TO seadread;
GRANT SELECT ON TABLE facet.result_field TO seadread;

GRANT SELECT ON TABLE facet.result_aggregate TO seadworker;
GRANT SELECT ON TABLE facet.result_aggregate_field TO seadworker;
GRANT SELECT ON TABLE facet.result_field_type TO seadworker;
GRANT SELECT ON TABLE facet.result_field TO seadworker;

create table facet.result_view_type (
	view_type_id varchar(40) not null,
	view_name varchar(40) not null,
	is_cachable boolean not null default(true),
	CONSTRAINT view_type_pk PRIMARY KEY (view_type_id)
);

insert into facet.result_view_type (values ('tabular', 'Tabular', TRUE), ('map', 'Map', FALSE))

select * from facet.result_view_type