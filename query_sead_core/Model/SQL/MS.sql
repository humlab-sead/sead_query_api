with F as ( 
  select 
  lower, 
  upper, 
  facet_term, 
  count(group_column) as direct_count 
from 
  (
    select 
      COALESCE(
        lower || '=>' || upper, 'data missing'
      ) as facet_term, 
      group_column, 
      lower, 
      upper 
    from 
      (
        select 
          lower, 
          upper, 
          tbl_analysis_entities.analysis_entity_id as group_column 
        from 
          metainformation.tbl_denormalized_measured_values 
          left join (
            select 
              0 as lower, 
              24 as upper, 
              '0=>24' :: text as id, 
              '' as name 
            union all 
            select 
              24 as lower, 
              48 as upper, 
              '24=>48' :: text as id, 
              '' as name 
            union all 
            select 
              48 as lower, 
              72 as upper, 
              '48=>72' :: text as id, 
              '' as name 
            union all 
            select 
              72 as lower, 
              96 as upper, 
              '72=>96' :: text as id, 
              '' as name 
            union all 
            select 
              96 as lower, 
              120 as upper, 
              '96=>120' :: text as id, 
              '' as name 
            union all 
            select 
              120 as lower, 
              144 as upper, 
              '120=>144' :: text as id, 
              '' as name 
            union all 
            select 
              144 as lower, 
              168 as upper, 
              '144=>168' :: text as id, 
              '' as name 
            union all 
            select 
              168 as lower, 
              192 as upper, 
              '168=>192' :: text as id, 
              '' as name 
            union all 
            select 
              192 as lower, 
              216 as upper, 
              '192=>216' :: text as id, 
              '' as name 
            union all 
            select 
              216 as lower, 
              240 as upper, 
              '216=>240' :: text as id, 
              '' as name 
            union all 
            select 
              240 as lower, 
              264 as upper, 
              '240=>264' :: text as id, 
              '' as name 
            union all 
            select 
              264 as lower, 
              288 as upper, 
              '264=>288' :: text as id, 
              '' as name 
            union all 
            select 
              288 as lower, 
              312 as upper, 
              '288=>312' :: text as id, 
              '' as name 
            union all 
            select 
              312 as lower, 
              336 as upper, 
              '312=>336' :: text as id, 
              '' as name 
            union all 
            select 
              336 as lower, 
              360 as upper, 
              '336=>360' :: text as id, 
              '' as name 
            union all 
            select 
              360 as lower, 
              384 as upper, 
              '360=>384' :: text as id, 
              '' as name 
            union all 
            select 
              384 as lower, 
              408 as upper, 
              '384=>408' :: text as id, 
              '' as name 
            union all 
            select 
              408 as lower, 
              432 as upper, 
              '408=>432' :: text as id, 
              '' as name 
            union all 
            select 
              432 as lower, 
              456 as upper, 
              '432=>456' :: text as id, 
              '' as name 
            union all 
            select 
              456 as lower, 
              480 as upper, 
              '456=>480' :: text as id, 
              '' as name 
            union all 
            select 
              480 as lower, 
              504 as upper, 
              '480=>504' :: text as id, 
              '' as name 
            union all 
            select 
              504 as lower, 
              528 as upper, 
              '504=>528' :: text as id, 
              '' as name 
            union all 
            select 
              528 as lower, 
              552 as upper, 
              '528=>552' :: text as id, 
              '' as name 
            union all 
            select 
              552 as lower, 
              576 as upper, 
              '552=>576' :: text as id, 
              '' as name 
            union all 
            select 
              576 as lower, 
              600 as upper, 
              '576=>600' :: text as id, 
              '' as name 
            union all 
            select 
              600 as lower, 
              624 as upper, 
              '600=>624' :: text as id, 
              '' as name 
            union all 
            select 
              624 as lower, 
              648 as upper, 
              '624=>648' :: text as id, 
              '' as name 
            union all 
            select 
              648 as lower, 
              672 as upper, 
              '648=>672' :: text as id, 
              '' as name 
            union all 
            select 
              672 as lower, 
              696 as upper, 
              '672=>696' :: text as id, 
              '' as name 
            union all 
            select 
              696 as lower, 
              720 as upper, 
              '696=>720' :: text as id, 
              '' as name 
            union all 
            select 
              720 as lower, 
              744 as upper, 
              '720=>744' :: text as id, 
              '' as name 
            union all 
            select 
              744 as lower, 
              768 as upper, 
              '744=>768' :: text as id, 
              '' as name 
            union all 
            select 
              768 as lower, 
              792 as upper, 
              '768=>792' :: text as id, 
              '' as name 
            union all 
            select 
              792 as lower, 
              816 as upper, 
              '792=>816' :: text as id, 
              '' as name 
            union all 
            select 
              816 as lower, 
              840 as upper, 
              '816=>840' :: text as id, 
              '' as name 
            union all 
            select 
              840 as lower, 
              864 as upper, 
              '840=>864' :: text as id, 
              '' as name 
            union all 
            select 
              864 as lower, 
              888 as upper, 
              '864=>888' :: text as id, 
              '' as name 
            union all 
            select 
              888 as lower, 
              912 as upper, 
              '888=>912' :: text as id, 
              '' as name 
            union all 
            select 
              912 as lower, 
              936 as upper, 
              '912=>936' :: text as id, 
              '' as name 
            union all 
            select 
              936 as lower, 
              960 as upper, 
              '936=>960' :: text as id, 
              '' as name 
            union all 
            select 
              960 as lower, 
              984 as upper, 
              '960=>984' :: text as id, 
              '' as name 
            union all 
            select 
              984 as lower, 
              1008 as upper, 
              '984=>1008' :: text as id, 
              '' as name 
            union all 
            select 
              1008 as lower, 
              1032 as upper, 
              '1008=>1032' :: text as id, 
              '' as name 
            union all 
            select 
              1032 as lower, 
              1056 as upper, 
              '1032=>1056' :: text as id, 
              '' as name 
            union all 
            select 
              1056 as lower, 
              1080 as upper, 
              '1056=>1080' :: text as id, 
              '' as name 
            union all 
            select 
              1080 as lower, 
              1104 as upper, 
              '1080=>1104' :: text as id, 
              '' as name 
            union all 
            select 
              1104 as lower, 
              1128 as upper, 
              '1104=>1128' :: text as id, 
              '' as name 
            union all 
            select 
              1128 as lower, 
              1152 as upper, 
              '1128=>1152' :: text as id, 
              '' as name 
            union all 
            select 
              1152 as lower, 
              1176 as upper, 
              '1152=>1176' :: text as id, 
              '' as name 
            union all 
            select 
              1176 as lower, 
              1200 as upper, 
              '1176=>1200' :: text as id, 
              '' as name 
            union all 
            select 
              1200 as lower, 
              1224 as upper, 
              '1200=>1224' :: text as id, 
              '' as name 
            union all 
            select 
              1224 as lower, 
              1248 as upper, 
              '1224=>1248' :: text as id, 
              '' as name 
            union all 
            select 
              1248 as lower, 
              1272 as upper, 
              '1248=>1272' :: text as id, 
              '' as name 
            union all 
            select 
              1272 as lower, 
              1296 as upper, 
              '1272=>1296' :: text as id, 
              '' as name 
            union all 
            select 
              1296 as lower, 
              1320 as upper, 
              '1296=>1320' :: text as id, 
              '' as name 
            union all 
            select 
              1320 as lower, 
              1344 as upper, 
              '1320=>1344' :: text as id, 
              '' as name 
            union all 
            select 
              1344 as lower, 
              1368 as upper, 
              '1344=>1368' :: text as id, 
              '' as name 
            union all 
            select 
              1368 as lower, 
              1392 as upper, 
              '1368=>1392' :: text as id, 
              '' as name 
            union all 
            select 
              1392 as lower, 
              1416 as upper, 
              '1392=>1416' :: text as id, 
              '' as name 
            union all 
            select 
              1416 as lower, 
              1440 as upper, 
              '1416=>1440' :: text as id, 
              '' as name 
            union all 
            select 
              1440 as lower, 
              1464 as upper, 
              '1440=>1464' :: text as id, 
              '' as name 
            union all 
            select 
              1464 as lower, 
              1488 as upper, 
              '1464=>1488' :: text as id, 
              '' as name 
            union all 
            select 
              1488 as lower, 
              1512 as upper, 
              '1488=>1512' :: text as id, 
              '' as name 
            union all 
            select 
              1512 as lower, 
              1536 as upper, 
              '1512=>1536' :: text as id, 
              '' as name 
            union all 
            select 
              1536 as lower, 
              1560 as upper, 
              '1536=>1560' :: text as id, 
              '' as name 
            union all 
            select 
              1560 as lower, 
              1584 as upper, 
              '1560=>1584' :: text as id, 
              '' as name 
            union all 
            select 
              1584 as lower, 
              1608 as upper, 
              '1584=>1608' :: text as id, 
              '' as name 
            union all 
            select 
              1608 as lower, 
              1632 as upper, 
              '1608=>1632' :: text as id, 
              '' as name 
            union all 
            select 
              1632 as lower, 
              1656 as upper, 
              '1632=>1656' :: text as id, 
              '' as name 
            union all 
            select 
              1656 as lower, 
              1680 as upper, 
              '1656=>1680' :: text as id, 
              '' as name 
            union all 
            select 
              1680 as lower, 
              1704 as upper, 
              '1680=>1704' :: text as id, 
              '' as name 
            union all 
            select 
              1704 as lower, 
              1728 as upper, 
              '1704=>1728' :: text as id, 
              '' as name 
            union all 
            select 
              1728 as lower, 
              1752 as upper, 
              '1728=>1752' :: text as id, 
              '' as name 
            union all 
            select 
              1752 as lower, 
              1776 as upper, 
              '1752=>1776' :: text as id, 
              '' as name 
            union all 
            select 
              1776 as lower, 
              1800 as upper, 
              '1776=>1800' :: text as id, 
              '' as name 
            union all 
            select 
              1800 as lower, 
              1824 as upper, 
              '1800=>1824' :: text as id, 
              '' as name 
            union all 
            select 
              1824 as lower, 
              1848 as upper, 
              '1824=>1848' :: text as id, 
              '' as name 
            union all 
            select 
              1848 as lower, 
              1872 as upper, 
              '1848=>1872' :: text as id, 
              '' as name 
            union all 
            select 
              1872 as lower, 
              1896 as upper, 
              '1872=>1896' :: text as id, 
              '' as name 
            union all 
            select 
              1896 as lower, 
              1920 as upper, 
              '1896=>1920' :: text as id, 
              '' as name 
            union all 
            select 
              1920 as lower, 
              1944 as upper, 
              '1920=>1944' :: text as id, 
              '' as name 
            union all 
            select 
              1944 as lower, 
              1968 as upper, 
              '1944=>1968' :: text as id, 
              '' as name 
            union all 
            select 
              1968 as lower, 
              1992 as upper, 
              '1968=>1992' :: text as id, 
              '' as name 
            union all 
            select 
              1992 as lower, 
              2016 as upper, 
              '1992=>2016' :: text as id, 
              '' as name 
            union all 
            select 
              2016 as lower, 
              2040 as upper, 
              '2016=>2040' :: text as id, 
              '' as name 
            union all 
            select 
              2040 as lower, 
              2064 as upper, 
              '2040=>2064' :: text as id, 
              '' as name 
            union all 
            select 
              2064 as lower, 
              2088 as upper, 
              '2064=>2088' :: text as id, 
              '' as name 
            union all 
            select 
              2088 as lower, 
              2112 as upper, 
              '2088=>2112' :: text as id, 
              '' as name 
            union all 
            select 
              2112 as lower, 
              2136 as upper, 
              '2112=>2136' :: text as id, 
              '' as name 
            union all 
            select 
              2136 as lower, 
              2160 as upper, 
              '2136=>2160' :: text as id, 
              '' as name 
            union all 
            select 
              2160 as lower, 
              2184 as upper, 
              '2160=>2184' :: text as id, 
              '' as name 
            union all 
            select 
              2184 as lower, 
              2208 as upper, 
              '2184=>2208' :: text as id, 
              '' as name 
            union all 
            select 
              2208 as lower, 
              2232 as upper, 
              '2208=>2232' :: text as id, 
              '' as name 
            union all 
            select 
              2232 as lower, 
              2256 as upper, 
              '2232=>2256' :: text as id, 
              '' as name 
            union all 
            select 
              2256 as lower, 
              2280 as upper, 
              '2256=>2280' :: text as id, 
              '' as name 
            union all 
            select 
              2280 as lower, 
              2304 as upper, 
              '2280=>2304' :: text as id, 
              '' as name 
            union all 
            select 
              2304 as lower, 
              2328 as upper, 
              '2304=>2328' :: text as id, 
              '' as name 
            union all 
            select 
              2328 as lower, 
              2352 as upper, 
              '2328=>2352' :: text as id, 
              '' as name 
            union all 
            select 
              2352 as lower, 
              2376 as upper, 
              '2352=>2376' :: text as id, 
              '' as name 
            union all 
            select 
              2376 as lower, 
              2400 as upper, 
              '2376=>2400' :: text as id, 
              '' as name 
            union all 
            select 
              2400 as lower, 
              2424 as upper, 
              '2400=>2424' :: text as id, 
              '' as name 
            union all 
            select 
              2424 as lower, 
              2448 as upper, 
              '2424=>2448' :: text as id, 
              '' as name 
            union all 
            select 
              2448 as lower, 
              2472 as upper, 
              '2448=>2472' :: text as id, 
              '' as name 
            union all 
            select 
              2472 as lower, 
              2496 as upper, 
              '2472=>2496' :: text as id, 
              '' as name 
            union all 
            select 
              2496 as lower, 
              2520 as upper, 
              '2496=>2520' :: text as id, 
              '' as name 
            union all 
            select 
              2520 as lower, 
              2544 as upper, 
              '2520=>2544' :: text as id, 
              '' as name 
            union all 
            select 
              2544 as lower, 
              2568 as upper, 
              '2544=>2568' :: text as id, 
              '' as name 
            union all 
            select 
              2568 as lower, 
              2592 as upper, 
              '2568=>2592' :: text as id, 
              '' as name 
            union all 
            select 
              2592 as lower, 
              2616 as upper, 
              '2592=>2616' :: text as id, 
              '' as name 
            union all 
            select 
              2616 as lower, 
              2640 as upper, 
              '2616=>2640' :: text as id, 
              '' as name 
            union all 
            select 
              2640 as lower, 
              2664 as upper, 
              '2640=>2664' :: text as id, 
              '' as name 
            union all 
            select 
              2664 as lower, 
              2688 as upper, 
              '2664=>2688' :: text as id, 
              '' as name 
            union all 
            select 
              2688 as lower, 
              2712 as upper, 
              '2688=>2712' :: text as id, 
              '' as name 
            union all 
            select 
              2712 as lower, 
              2736 as upper, 
              '2712=>2736' :: text as id, 
              '' as name 
            union all 
            select 
              2736 as lower, 
              2760 as upper, 
              '2736=>2760' :: text as id, 
              '' as name 
            union all 
            select 
              2760 as lower, 
              2784 as upper, 
              '2760=>2784' :: text as id, 
              '' as name 
            union all 
            select 
              2784 as lower, 
              2808 as upper, 
              '2784=>2808' :: text as id, 
              '' as name 
            union all 
            select 
              2808 as lower, 
              2832 as upper, 
              '2808=>2832' :: text as id, 
              '' as name 
            union all 
            select 
              2832 as lower, 
              2856 as upper, 
              '2832=>2856' :: text as id, 
              '' as name 
            union all 
            select 
              2856 as lower, 
              2880 as upper, 
              '2856=>2880' :: text as id, 
              '' as name 
            union all 
            select 
              2880 as lower, 
              2904 as upper, 
              '2880=>2904' :: text as id, 
              '' as name
          ) as temp_interval on metainformation.tbl_denormalized_measured_values.value_33_0 :: integer >= lower 
          and metainformation.tbl_denormalized_measured_values.value_33_0 :: integer < upper 
          inner join tbl_physical_samples on tbl_physical_samples."physical_sample_id" = metainformation.tbl_denormalized_measured_values."physical_sample_id" 
          left join tbl_analysis_entities on tbl_analysis_entities."physical_sample_id" = tbl_physical_samples."physical_sample_id" 
          and (
            (
              metainformation.tbl_denormalized_measured_values.value_33_0 >= 0 
              and metainformation.tbl_denormalized_measured_values.value_33_0 <= 2981
            )
          ) -- Make nice 
        group by 
          lower, 
          upper, 
          tbl_analysis_entities.analysis_entity_id 
        order by 
          lower
      ) as tmp4 
    group by 
      lower, 
      upper, 
      group_column
  ) as tmp3 
where 
  lower is not null 
  and upper is not null 
group by 
  lower, 
  upper, 
  facet_term 
order by 
  lower, 
  upper),
N as (

            WITH categories(category, lower, upper) AS ((VALUES ('0 => 24', 0, 24)
,('24 => 48', 24, 48)
,('48 => 72', 48, 72)
,('72 => 96', 72, 96)
,('96 => 120', 96, 120)
,('120 => 144', 120, 144)
,('144 => 168', 144, 168)
,('168 => 192', 168, 192)
,('192 => 216', 192, 216)
,('216 => 240', 216, 240)
,('240 => 264', 240, 264)
,('264 => 288', 264, 288)
,('288 => 312', 288, 312)
,('312 => 336', 312, 336)
,('336 => 360', 336, 360)
,('360 => 384', 360, 384)
,('384 => 408', 384, 408)
,('408 => 432', 408, 432)
,('432 => 456', 432, 456)
,('456 => 480', 456, 480)
,('480 => 504', 480, 504)
,('504 => 528', 504, 528)
,('528 => 552', 528, 552)
,('552 => 576', 552, 576)
,('576 => 600', 576, 600)
,('600 => 624', 600, 624)
,('624 => 648', 624, 648)
,('648 => 672', 648, 672)
,('672 => 696', 672, 696)
,('696 => 720', 696, 720)
,('720 => 744', 720, 744)
,('744 => 768', 744, 768)
,('768 => 792', 768, 792)
,('792 => 816', 792, 816)
,('816 => 840', 816, 840)
,('840 => 864', 840, 864)
,('864 => 888', 864, 888)
,('888 => 912', 888, 912)
,('912 => 936', 912, 936)
,('936 => 960', 936, 960)
,('960 => 984', 960, 984)
,('984 => 1008', 984, 1008)
,('1008 => 1032', 1008, 1032)
,('1032 => 1056', 1032, 1056)
,('1056 => 1080', 1056, 1080)
,('1080 => 1104', 1080, 1104)
,('1104 => 1128', 1104, 1128)
,('1128 => 1152', 1128, 1152)
,('1152 => 1176', 1152, 1176)
,('1176 => 1200', 1176, 1200)
,('1200 => 1224', 1200, 1224)
,('1224 => 1248', 1224, 1248)
,('1248 => 1272', 1248, 1272)
,('1272 => 1296', 1272, 1296)
,('1296 => 1320', 1296, 1320)
,('1320 => 1344', 1320, 1344)
,('1344 => 1368', 1344, 1368)
,('1368 => 1392', 1368, 1392)
,('1392 => 1416', 1392, 1416)
,('1416 => 1440', 1416, 1440)
,('1440 => 1464', 1440, 1464)
,('1464 => 1488', 1464, 1488)
,('1488 => 1512', 1488, 1512)
,('1512 => 1536', 1512, 1536)
,('1536 => 1560', 1536, 1560)
,('1560 => 1584', 1560, 1584)
,('1584 => 1608', 1584, 1608)
,('1608 => 1632', 1608, 1632)
,('1632 => 1656', 1632, 1656)
,('1656 => 1680', 1656, 1680)
,('1680 => 1704', 1680, 1704)
,('1704 => 1728', 1704, 1728)
,('1728 => 1752', 1728, 1752)
,('1752 => 1776', 1752, 1776)
,('1776 => 1800', 1776, 1800)
,('1800 => 1824', 1800, 1824)
,('1824 => 1848', 1824, 1848)
,('1848 => 1872', 1848, 1872)
,('1872 => 1896', 1872, 1896)
,('1896 => 1920', 1896, 1920)
,('1920 => 1944', 1920, 1944)
,('1944 => 1968', 1944, 1968)
,('1968 => 1992', 1968, 1992)
,('1992 => 2016', 1992, 2016)
,('2016 => 2040', 2016, 2040)
,('2040 => 2064', 2040, 2064)
,('2064 => 2088', 2064, 2088)
,('2088 => 2112', 2088, 2112)
,('2112 => 2136', 2112, 2136)
,('2136 => 2160', 2136, 2160)
,('2160 => 2184', 2160, 2184)
,('2184 => 2208', 2184, 2208)
,('2208 => 2232', 2208, 2232)
,('2232 => 2256', 2232, 2256)
,('2256 => 2280', 2256, 2280)
,('2280 => 2304', 2280, 2304)
,('2304 => 2328', 2304, 2328)
,('2328 => 2352', 2328, 2352)
,('2352 => 2376', 2352, 2376)
,('2376 => 2400', 2376, 2400)
,('2400 => 2424', 2400, 2424)
,('2424 => 2448', 2424, 2448)
,('2448 => 2472', 2448, 2472)
,('2472 => 2496', 2472, 2496)
,('2496 => 2520', 2496, 2520)
,('2520 => 2544', 2520, 2544)
,('2544 => 2568', 2544, 2568)
,('2568 => 2592', 2568, 2592)
,('2592 => 2616', 2592, 2616)
,('2616 => 2640', 2616, 2640)
,('2640 => 2664', 2640, 2664)
,('2664 => 2688', 2664, 2688)
,('2688 => 2712', 2688, 2712)
,('2712 => 2736', 2712, 2736)
,('2736 => 2760', 2736, 2760)
,('2760 => 2784', 2760, 2784)
,('2784 => 2808', 2784, 2808)
,('2808 => 2832', 2808, 2832)
,('2832 => 2856', 2832, 2856)
,('2856 => 2880', 2856, 2880)
,('2880 => 2904', 2880, 2904)))
                SELECT category, lower, upper, count(tbl_analysis_entities.analysis_entity_id) AS count_column
                FROM categories
                LEFT JOIN metainformation.tbl_denormalized_measured_values 
                  ON metainformation.tbl_denormalized_measured_values.value_33_0::integer >= lower
                 AND metainformation.tbl_denormalized_measured_values.value_33_0::integer < upper
                 LEFT JOIN tbl_physical_samples  ON tbl_physical_samples."physical_sample_id" = metainformation.tbl_denormalized_measured_values."physical_sample_id"  LEFT JOIN tbl_analysis_entities  ON tbl_analysis_entities."physical_sample_id" = tbl_physical_samples."physical_sample_id" 
                
                GROUP BY category, lower, upper
                ORDER BY lower
)
select '  { ''' || category || ''', ' || count_column::text || ' }, '
from n
/*SELECT n.category, f.direct_count, n.count_column, COALESCE(f.direct_count, 0) - n.count_column
  FROM F f
  FULL OUTER JOIN N n
    ON f.lower = n.lower
   AND f.upper = n.upper
  WHERE COALESCE(f.direct_count, 0) - n.count_column <> 0
*/
