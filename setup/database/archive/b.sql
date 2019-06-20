CREATE VIEW "facet"."report_site" AS  SELECT tbl_sites.site_id AS id,
    tbl_sites.site_id,
    tbl_sites.site_name AS "Site name",
    tbl_sites.site_description AS "Site description",
    tbl_site_natgridrefs.natgridref AS "National grid ref",
    array_to_string(array_agg(tbl_locations.location_name ORDER BY tbl_locations.location_type_id DESC), ','::text) AS places,
    tbl_site_preservation_status.preservation_status_or_threat AS "Preservation status or threat",
    tbl_sites.latitude_dd AS site_lat,
    tbl_sites.longitude_dd AS site_lng
   FROM ((((tbl_sites
   LEFT JOIN tbl_site_locations ON ((tbl_site_locations.site_id = tbl_sites.site_id)))
   LEFT JOIN tbl_site_natgridrefs ON ((tbl_site_natgridrefs.site_id = tbl_sites.site_id)))
   LEFT JOIN tbl_site_preservation_status ON ((tbl_site_preservation_status.site_preservation_status_id = tbl_sites.site_preservation_status_id)))
   LEFT JOIN tbl_locations ON ((tbl_locations.location_id = tbl_site_locations.location_id)))
  GROUP BY tbl_sites.site_id, tbl_sites.site_name, tbl_sites.site_description, tbl_site_natgridrefs.natgridref, tbl_sites.latitude_dd, tbl_sites.longitude_dd, tbl_site_preservation_status.preservation_status_or_threat;
