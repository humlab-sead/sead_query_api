

-- select 'drop table if exists ' || tablename || ' cascade;'
-- from pg_tables
-- where schemaname = 'public'

drop table if exists player_note cascade;
drop table if exists player cascade;
drop table if exists enterprise_property cascade;
drop table if exists enterprise_arena cascade;
drop table if exists enterprise_report cascade;
drop table if exists enterprise cascade;
drop table if exists dataset cascade;
drop table if exists team_description cascade;
drop table if exists team cascade;
drop table if exists report cascade;
drop table if exists method cascade;
drop table if exists arena cascade;

drop table if exists repetition cascade;
drop table if exists endurance cascade;
drop table if exists strength cascade;
drop table if exists test cascade;

drop table if exists arena_type cascade;
drop table if exists method_type cascade;
drop table if exists enterprise_type cascade;
drop table if exists dataset_type cascade;
drop table if exists player_type cascade;


create table "dataset_type" (
  "dataset_type_id" serial primary key not null,
  "name" varchar(25) collate "default" default null :: character varying,
  "description" text collate "default"
);

create table "arena_type" (
  "arena_type_id" serial primary key not null,
  "description" text collate "default"
);

create table "enterprise_type" (
  "enterprise_type_id" serial primary key not null,
  "type_name" varchar(60) collate "default" default null :: character varying
);

create table "player_type" (
  "player_type_id" serial primary key not null,
  "description" text collate "default"
);

create table "method_type" (
  "method_type_id" int4  primary key not null,
  "name" varchar(50) collate "default" default null :: character varying
);

create table "report" (
  "report_id" serial primary key not null,
  "title" varchar collate "default",
  "year" varchar(255) collate "default" default null :: character varying,
  "physician" varchar collate "default"
);

create table "arena" (
  "arena_id" serial primary key not null,
  "name" varchar(255) collate "default" not null,
  "arena_type_id" int4 not null references arena_type (arena_type_id)
);

create table "method" (
  "method_id" serial primary key not null,
  "report_id" int4 not null references report (report_id),
  "description" text collate "default" not null,
  "method_name" varchar(50) collate "default" not null,
  "method_type_id" int4 references method_type (method_type_id)
);

create table "enterprise" (
  "enterprise_id" serial primary key not null,
  "enterprise_type_id" int4 not null references enterprise_type (enterprise_type_id),
  "latitude_dd" numeric(18, 10),
  "longitude_dd" numeric(18, 10),
  "name" varchar(60) collate "default" default null :: character varying
);

create table "enterprise_property" (
  "enterprise_property_id" serial primary key not null,
  "property_name" varchar(80) collate "default",
  "property_value" int4 not null,
  "enterprise_id" int4 default 0 references enterprise (enterprise_id)
);

create table "enterprise_arena" (
  "enterprise_arena_id" serial primary key not null,
  "arena_id" int4 not null references arena (arena_id),
  "enterprise_id" int4 default 0 references enterprise (enterprise_id)
);

create table "enterprise_report" (
  "enterprise_report_id" serial primary key not null,
  "enterprise_id" int4 default 0 references enterprise (enterprise_id),
  "report_id" int4 default 0 references report (report_id)
);

create table "dataset" (
  "dataset_id" serial primary key not null,
  "dataset_type_id" int4 not null references dataset_type (dataset_type_id),
  "method_id" int4 not null references method (method_id),
  "report_id" int4 not null references report (report_id),
  "dataset_name" varchar(50) collate "default" not null
);

create table "team" (
  "team_id" serial primary key not null,
  "enterprise_id" int4 default 0 references enterprise (enterprise_id),
  "method_id" int4 not null references method (method_id),
  "name" varchar(100) collate "default" default null :: character varying
);

create table "team_description" (
  "team_description_id" serial primary key not null,
  "description" varchar collate "default",
  "team_id" int4 not null references team (team_id)
);

create table "player" (
  "player_id" serial primary key not null,
  "team_id" int4 not null default 0 references team (team_id),
  "player_type_id" int4 not null references player_type (player_type_id),
  "name" varchar(50) collate "default" not null
);

create table "player_note" (
  "player_note_id" serial primary key not null,
  "player_id" int4 not null references player (player_id),
  "note_type" varchar collate "default",
  "note" text collate "default" not null
);

create table "fitness_test" (
  "fitness_test_id" serial primary key not null,
  "player_id" int4 not null references player (player_id),
  "dataset_id" int4 not null references dataset (dataset_id)
);

create table "repetition" (
  "repetition_id" serial primary key not null,
  "fitness_test_id" int4 not null default 0 references fitness_test (fitness_test_id),
  "count" int4 not null default 0
);

create table "endurance" (
  "endurance_id" serial primary key not null,
  "mean" numeric(20, 5),
  "low" numeric(20, 5),
  "high" numeric(20, 5),
  "fitness_test_id" int4 not null default 0 references fitness_test (fitness_test_id)
);

create table "strength" (
  "strength_id" serial primary key not null,
  "fitness_test_id" int4 not null references fitness_test (fitness_test_id),
  "value" numeric(20, 10) not null
);

