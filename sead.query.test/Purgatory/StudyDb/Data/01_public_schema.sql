

-- select 'drop table if exists ' || tablename || ' cascade;'
-- from pg_tables
-- where schemaname = 'public'

drop table if exists subject_note cascade;
drop table if exists subject cascade;
drop table if exists project_property cascade;
drop table if exists project_residence cascade;
drop table if exists project_report cascade;
drop table if exists project cascade;
drop table if exists study cascade;
drop table if exists cohort_description cascade;
drop table if exists cohort cascade;
drop table if exists report cascade;
drop table if exists method cascade;
drop table if exists residence cascade;

drop table if exists observed_count cascade;
drop table if exists observed_span cascade;
drop table if exists observed_magnitude cascade;
drop table if exists experiment cascade;
drop table if exists observable cascade;

drop table if exists residence_type cascade;
drop table if exists method_type cascade;
drop table if exists project_type cascade;
drop table if exists study_type cascade;
drop table if exists subject_type cascade;


create table "study_type" (
  "study_type_id" serial primary key not null,
  "name" varchar(25) collate "default" default null :: character varying,
  "description" text collate "default"
);

create table "residence_type" (
  "residence_type_id" serial primary key not null,
  "description" text collate "default"
);

create table "project_type" (
  "project_type_id" serial primary key not null,
  "type_name" varchar(60) collate "default" default null :: character varying
);

create table "subject_type" (
  "subject_type_id" serial primary key not null,
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
  "author" varchar collate "default"
);

create table "residence" (
  "residence_id" serial primary key not null,
  "name" varchar(255) collate "default" not null,
  "residence_type_id" int4 not null references residence_type (residence_type_id)
);

create table "method" (
  "method_id" serial primary key not null,
  "report_id" int4 not null references report (report_id),
  "description" text collate "default" not null,
  "method_name" varchar(50) collate "default" not null,
  "method_type_id" int4 references method_type (method_type_id)
);

create table "project" (
  "project_id" serial primary key not null,
  "project_type_id" int4 not null references project_type (project_type_id),
  "latitude_dd" numeric(18, 10) not null,
  "longitude_dd" numeric(18, 10) not null,
  "name" varchar(60) collate "default" default null :: character varying
);

create table "project_property" (
  "project_property_id" serial primary key not null,
  "property_name" varchar(80) collate "default",
  "property_value" int4 not null,
  "project_id" int4 default 0 references project (project_id)
);

create table "project_residence" (
  "project_residence_id" serial primary key not null,
  "residence_id" int4 not null references residence (residence_id),
  "latitude_dd" numeric(18, 10),
  "longitude_dd" numeric(18, 10),
  "project_id" int4 default 0 references project (project_id)
);

create table "project_report" (
  "project_report_id" serial primary key not null,
  "project_id" int4 default 0 references project (project_id),
  "report_id" int4 default 0 references report (report_id)
);

create table "study" (
  "study_id" serial primary key not null,
  "study_type_id" int4 not null references study_type (study_type_id),
  "method_id" int4 not null references method (method_id),
  "report_id" int4 not null references report (report_id),
  "study_name" varchar(50) collate "default" not null
);

create table "cohort" (
  "cohort_id" serial primary key not null,
  "project_id" int4 default 0 references project (project_id),
  "method_id" int4 not null references method (method_id),
  "name" varchar(100) collate "default" default null :: character varying
);

create table "cohort_description" (
  "cohort_description_id" serial primary key not null,
  "description" varchar collate "default",
  "cohort_id" int4 not null references cohort (cohort_id)
);

create table "subject" (
  "subject_id" serial primary key not null,
  "cohort_id" int4 not null default 0 references cohort (cohort_id),
  "subject_type_id" int4 not null references subject_type (subject_type_id),
  "name" varchar(50) collate "default" not null
);

create table "subject_note" (
  "subject_note_id" serial primary key not null,
  "subject_id" int4 not null references subject (subject_id),
  "note_type" varchar collate "default",
  "note" text collate "default" not null
);

create table "experiment" (
  "experiment_id" serial primary key not null,
  "subject_id" int4 not null references subject (subject_id),
  "study_id" int4 not null references study (study_id)
);

create table "observable" (
  "observable_id" serial primary key not null,
  "name" numeric(20, 10) not null
);

create table "observed_count" (
  "observed_count_id" serial primary key not null,
  "experiment_id" int4 not null default 0 references experiment (experiment_id),
  "observable_id" int4 not null references observable (observable_id),
  "count" int4 not null default 0
);

create table "observed_span" (
  "observed_span_id" serial primary key not null,
  "low" numeric(20, 5),
  "high" numeric(20, 5),
  "observable_id" int4 not null references observable (observable_id),
  "experiment_id" int4 not null default 0 references experiment (experiment_id)
);

create table "observed_magnitude" (
  "observed_magnitude_id" serial primary key not null,
  "experiment_id" int4 not null references experiment (experiment_id),
  "observable_id" int4 not null references observable (observable_id),
  "value" numeric(20, 10) not null
);
