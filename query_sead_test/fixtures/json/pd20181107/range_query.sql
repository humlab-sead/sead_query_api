with temp_interval as (
    select 110 as lower, 133 as upper, '110=>133' :: text as id, '' as name
    union all
    select
      133 as lower,
      156 as upper, '133=>156' :: text as id,
      '' as name
    union all
    select
      156 as lower,
      179 as upper, '156=>179' :: text as id,
      '' as name
    union all
    select
      179 as lower,
      202 as upper, '179=>202' :: text as id,
      '' as name
    union all
    select
      202 as lower,
      225 as upper, '202=>225' :: text as id,
      '' as name
    union all
    select
      225 as lower,
      248 as upper, '225=>248' :: text as id,
      '' as name
    union all
    select
      248 as lower,
      271 as upper, '248=>271' :: text as id,
      '' as name
    union all
    select
      271 as lower,
      294 as upper, '271=>294' :: text as id,
      '' as name
    union all
    select
      294 as lower,
      317 as upper, '294=>317' :: text as id,
      '' as name
    union all
    select
      317 as lower,
      340 as upper, '317=>340' :: text as id,
      '' as name
    union all
    select
      340 as lower,
      363 as upper, '340=>363' :: text as id,
      '' as name
    union all
    select
      363 as lower,
      386 as upper, '363=>386' :: text as id,
      '' as name
    union all
    select
      386 as lower,
      409 as upper, '386=>409' :: text as id,
      '' as name
    union all
    select
      409 as lower,
      432 as upper, '409=>432' :: text as id,
      '' as name
    union all
    select
      432 as lower,
      455 as upper, '432=>455' :: text as id,
      '' as name
    union all
    select
      455 as lower,
      478 as upper, '455=>478' :: text as id,
      '' as name
    union all
    select
      478 as lower,
      501 as upper, '478=>501' :: text as id,
      '' as name
    union all
    select
      501 as lower,
      524 as upper, '501=>524' :: text as id,
      '' as name
    union all
    select
      524 as lower,
      547 as upper, '524=>547' :: text as id,
      '' as name
    union all
    select
      547 as lower,
      570 as upper, '547=>570' :: text as id,
      '' as name
    union all
    select
      570 as lower,
      593 as upper, '570=>593' :: text as id,
      '' as name
    union all
    select
      593 as lower,
      616 as upper, '593=>616' :: text as id,
      '' as name
    union all
    select
      616 as lower,
      639 as upper, '616=>639' :: text as id,
      '' as name
    union all
    select
      639 as lower,
      662 as upper, '639=>662' :: text as id,
      '' as name
    union all
    select
      662 as lower,
      685 as upper, '662=>685' :: text as id,
      '' as name
    union all
    select
      685 as lower,
      708 as upper, '685=>708' :: text as id,
      '' as name
    union all
    select
      708 as lower,
      731 as upper, '708=>731' :: text as id,
      '' as name
    union all
    select
      731 as lower,
      754 as upper, '731=>754' :: text as id,
      '' as name
    union all
    select
      754 as lower,
      777 as upper, '754=>777' :: text as id,
      '' as name
    union all
    select
      777 as lower,
      800 as upper, '777=>800' :: text as id,
      '' as name
    union all
    select
      800 as lower,
      823 as upper, '800=>823' :: text as id,
      '' as name
    union all
    select
      823 as lower,
      846 as upper, '823=>846' :: text as id,
      '' as name
    union all
    select
      846 as lower,
      869 as upper, '846=>869' :: text as id,
      '' as name
    union all
    select
      869 as lower,
      892 as upper, '869=>892' :: text as id,
      '' as name
    union all
    select
      892 as lower,
      915 as upper, '892=>915' :: text as id,
      '' as name
    union all
    select
      915 as lower,
      938 as upper, '915=>938' :: text as id,
      '' as name
    union all
    select
      938 as lower,
      961 as upper, '938=>961' :: text as id,
      '' as name
    union all
    select
      961 as lower,
      984 as upper, '961=>984' :: text as id,
      '' as name
    union all
    select
      984 as lower,
      1007 as upper, '984=>1007' :: text as id,
      '' as name
    union all
    select
      1007 as lower,
      1030 as upper, '1007=>1030' :: text as id,
      '' as name
    union all
    select
      1030 as lower,
      1053 as upper, '1030=>1053' :: text as id,
      '' as name
    union all
    select
      1053 as lower,
      1076 as upper, '1053=>1076' :: text as id,
      '' as name
    union all
    select
      1076 as lower,
      1099 as upper, '1076=>1099' :: text as id,
      '' as name
    union all
    select
      1099 as lower,
      1122 as upper, '1099=>1122' :: text as id,
      '' as name
    union all
    select
      1122 as lower,
      1145 as upper, '1122=>1145' :: text as id,
      '' as name
    union all
    select
      1145 as lower,
      1168 as upper, '1145=>1168' :: text as id,
      '' as name
    union all
    select
      1168 as lower,
      1191 as upper, '1168=>1191' :: text as id,
      '' as name
    union all
    select
      1191 as lower,
      1214 as upper, '1191=>1214' :: text as id,
      '' as name
    union all
    select
      1214 as lower,
      1237 as upper, '1214=>1237' :: text as id,
      '' as name
    union all
    select
      1237 as lower,
      1260 as upper, '1237=>1260' :: text as id,
      '' as name
    union all
    select
      1260 as lower,
      1283 as upper, '1260=>1283' :: text as id,
      '' as name
    union all
    select
      1283 as lower,
      1306 as upper, '1283=>1306' :: text as id,
      '' as name
    union all
    select
      1306 as lower,
      1329 as upper, '1306=>1329' :: text as id,
      '' as name
    union all
    select
      1329 as lower,
      1352 as upper, '1329=>1352' :: text as id,
      '' as name
    union all
    select
      1352 as lower,
      1375 as upper, '1352=>1375' :: text as id,
      '' as name
    union all
    select
      1375 as lower,
      1398 as upper, '1375=>1398' :: text as id,
      '' as name
    union all
    select
      1398 as lower,
      1421 as upper, '1398=>1421' :: text as id,
      '' as name
    union all
    select
      1421 as lower,
      1444 as upper, '1421=>1444' :: text as id,
      '' as name
    union all
    select
      1444 as lower,
      1467 as upper, '1444=>1467' :: text as id,
      '' as name
    union all
    select
      1467 as lower,
      1490 as upper, '1467=>1490' :: text as id,
      '' as name
    union all
    select
      1490 as lower,
      1513 as upper, '1490=>1513' :: text as id,
      '' as name
    union all
    select
      1513 as lower,
      1536 as upper, '1513=>1536' :: text as id,
      '' as name
    union all
    select
      1536 as lower,
      1559 as upper, '1536=>1559' :: text as id,
      '' as name
    union all
    select
      1559 as lower,
      1582 as upper, '1559=>1582' :: text as id,
      '' as name
    union all
    select
      1582 as lower,
      1605 as upper, '1582=>1605' :: text as id,
      '' as name
    union all
    select
      1605 as lower,
      1628 as upper, '1605=>1628' :: text as id,
      '' as name
    union all
    select
      1628 as lower,
      1651 as upper, '1628=>1651' :: text as id,
      '' as name
    union all
    select
      1651 as lower,
      1674 as upper, '1651=>1674' :: text as id,
      '' as name
    union all
    select
      1674 as lower,
      1697 as upper, '1674=>1697' :: text as id,
      '' as name
    union all
    select
      1697 as lower,
      1720 as upper, '1697=>1720' :: text as id,
      '' as name
    union all
    select
      1720 as lower,
      1743 as upper, '1720=>1743' :: text as id,
      '' as name
    union all
    select
      1743 as lower,
      1766 as upper, '1743=>1766' :: text as id,
      '' as name
    union all
    select
      1766 as lower,
      1789 as upper, '1766=>1789' :: text as id,
      '' as name
    union all
    select
      1789 as lower,
      1812 as upper, '1789=>1812' :: text as id,
      '' as name
    union all
    select
      1812 as lower,
      1835 as upper, '1812=>1835' :: text as id,
      '' as name
    union all
    select
      1835 as lower,
      1858 as upper, '1835=>1858' :: text as id,
      '' as name
    union all
    select
      1858 as lower,
      1881 as upper, '1858=>1881' :: text as id,
      '' as name
    union all
    select
      1881 as lower,
      1904 as upper, '1881=>1904' :: text as id,
      '' as name
    union all
    select
      1904 as lower,
      1927 as upper, '1904=>1927' :: text as id,
      '' as name
    union all
    select
      1927 as lower,
      1950 as upper, '1927=>1950' :: text as id,
      '' as name
    union all
    select
      1950 as lower,
      1973 as upper, '1950=>1973' :: text as id,
      '' as name
    union all
    select
      1973 as lower,
      1996 as upper, '1973=>1996' :: text as id,
      '' as name
    union all
    select
      1996 as lower,
      2019 as upper, '1996=>2019' :: text as id,
      '' as name
    union all
    select
      2019 as lower,
      2042 as upper, '2019=>2042' :: text as id,
      '' as name
    union all
    select
      2042 as lower,
      2065 as upper, '2042=>2065' :: text as id,
      '' as name
    union all
    select
      2065 as lower,
      2088 as upper, '2065=>2088' :: text as id,
      '' as name
    union all
    select
      2088 as lower,
      2111 as upper, '2088=>2111' :: text as id,
      '' as name
    union all
    select
      2111 as lower,
      2134 as upper, '2111=>2134' :: text as id,
      '' as name
    union all
    select
      2134 as lower,
      2157 as upper, '2134=>2157' :: text as id,
      '' as name
    union all
    select
      2157 as lower,
      2180 as upper, '2157=>2180' :: text as id,
      '' as name
    union all
    select
      2180 as lower,
      2203 as upper, '2180=>2203' :: text as id,
      '' as name
    union all
    select
      2203 as lower,
      2226 as upper, '2203=>2226' :: text as id,
      '' as name
    union all
    select
      2226 as lower,
      2249 as upper, '2226=>2249' :: text as id,
      '' as name
    union all
    select
      2249 as lower,
      2272 as upper, '2249=>2272' :: text as id,
      '' as name
    union all
    select
      2272 as lower,
      2295 as upper, '2272=>2295' :: text as id,
      '' as name
    union all
    select
      2295 as lower,
      2318 as upper, '2295=>2318' :: text as id,
      '' as name
    union all
    select
      2318 as lower,
      2341 as upper, '2318=>2341' :: text as id,
      '' as name
    union all
    select
      2341 as lower,
      2364 as upper, '2341=>2364' :: text as id,
      '' as name
    union all
    select
      2364 as lower,
      2387 as upper, '2364=>2387' :: text as id,
      '' as name
    union all
    select
      2387 as lower,
      2410 as upper, '2387=>2410' :: text as id,
      '' as name
    union all
    select
      2410 as lower,
      2433 as upper, '2410=>2433' :: text as id,
      '' as name
    union all
    select
      2433 as lower,
      2456 as upper, '2433=>2456' :: text as id,
      '' as name
    union all
    select
      2456 as lower,
      2479 as upper, '2456=>2479' :: text as id,
      '' as name
    union all
    select
      2479 as lower,
      2502 as upper, '2479=>2502' :: text as id,
      '' as name
    union all
    select
      2502 as lower,
      2525 as upper, '2502=>2525' :: text as id,
      '' as name
    union all
    select
      2525 as lower,
      2548 as upper, '2525=>2548' :: text as id,
      '' as name
    union all
    select
      2548 as lower,
      2571 as upper, '2548=>2571' :: text as id,
      '' as name
    union all
    select
      2571 as lower,
      2594 as upper, '2571=>2594' :: text as id,
      '' as name
    union all
    select
      2594 as lower,
      2617 as upper, '2594=>2617' :: text as id,
      '' as name
    union all
    select
      2617 as lower,
      2640 as upper, '2617=>2640' :: text as id,
      '' as name
    union all
    select
      2640 as lower,
      2663 as upper, '2640=>2663' :: text as id,
      '' as name
    union all
    select
      2663 as lower,
      2686 as upper, '2663=>2686' :: text as id,
      '' as name
    union all
    select
      2686 as lower,
      2709 as upper, '2686=>2709' :: text as id,
      '' as name
    union all
    select
      2709 as lower,
      2732 as upper, '2709=>2732' :: text as id,
      '' as name
    union all
    select
      2732 as lower,
      2755 as upper, '2732=>2755' :: text as id,
      '' as name
    union all
    select
      2755 as lower,
      2778 as upper, '2755=>2778' :: text as id,
      '' as name
    union all
    select
      2778 as lower,
      2801 as upper, '2778=>2801' :: text as id,
      '' as name
    union all
    select
      2801 as lower,
      2824 as upper, '2801=>2824' :: text as id,
      '' as name
    union all
    select
      2824 as lower,
      2847 as upper, '2824=>2847' :: text as id,
      '' as name
    union all
    select
      2847 as lower,
      2870 as upper, '2847=>2870' :: text as id,
      '' as name
    union all
    select
      2870 as lower,
      2893 as upper, '2870=>2893' :: text as id,
      '' as name
)
select lower, upper, facet_term, count(facet_term) as direct_count
from
  (
    select COALESCE(lower || '=>' || upper, 'data missing') as facet_term, group_column, lower, upper
    from (
        select lower, upper, tbl_analysis_entities.analysis_entity_id as group_column
        from metainformation.tbl_denormalized_measured_values
        left join temp_interval
            on metainformation.tbl_denormalized_measured_values.value_33_0 :: integer >= lower
           and metainformation.tbl_denormalized_measured_values.value_33_0 :: integer < upper
          inner join tbl_physical_samples
            on tbl_physical_samples."physical_sample_id" = metainformation.tbl_denormalized_measured_values."physical_sample_id"
          left join tbl_analysis_entities
            on tbl_analysis_entities."physical_sample_id" = tbl_physical_samples."physical_sample_id"
          and (metainformation.tbl_denormalized_measured_values.value_33_0 >= 110
          and  metainformation.tbl_denormalized_measured_values.value_33_0 <= 2904)
        group by lower, upper, tbl_analysis_entities.analysis_entity_id
        order by lower
      ) as tmp4
    group by lower, upper, group_column
  ) as tmp3
where lower is not null
  and upper is not null
group by lower, upper, facet_term
order by lower, upper;