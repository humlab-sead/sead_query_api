using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using static SeadQueryCore.Utility;

namespace SeadQueryCore.Services
{
    public class ReportService : QueryServiceBase
    {
        public string FacetCode { get; protected set; }

        public ReportService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
            FacetCode = "distinct_expr";
        }

        //public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        //{

//    string sql = CompileSql(facetsConfig, resultConfig);
//    return empty(sql) ? null : new TabularResultContentSet(resultConfig, GetResultFields(resultConfig), Context.Query(sql));
//}
/*
create view facet.report_site as
    select
        tbl_sites.site_id                                                               as id,
        tbl_sites.site_id                                                               as "site_id",
        site_name                                                                       as "Site name",
        site_description                                                                as "Site description",
        natgridref                                                                      as "National grid ref",
        array_to_string(array_agg(location_name order by location_type_id desc), ',')   as Places,
        preservation_status_or_threat                                                   as "Preservation status or threat",
        latitude_dd                                                                     as site_lat,
        longitude_dd                                                                    as site_lng
    from tbl_sites
    left join tbl_site_locations on tbl_site_locations.site_id = tbl_sites.site_id
    left join tbl_site_natgridrefs on tbl_site_natgridrefs.site_id = tbl_sites.site_id
    left join tbl_site_preservation_status on tbl_site_preservation_status.site_preservation_status_id = tbl_sites.site_preservation_status_id
    left join tbl_locations on tbl_locations.location_id = tbl_site_locations.location_id
    group by tbl_sites.site_id, site_name, site_description, natgridref, site_lat, site_lng, preservation_status_or_threat;


with facet_query as (
    -- $facet_identity_query
    select 1 as id
)select report_site.*
 from facet.report_site
 join facet_query
   on report_site.id = facet_query.id
 where report_site.id = 1

select * from clearing_house.tbl_clearinghouse_users
c0d3rs "test_admin"
update clearing_house.tbl_clearinghouse_users set password='$2y$10$/u3RCeK8Q.2s75UsZmvQ4.4TOxvLNKH8EoH4k6NYYtkAMavjP.dry'
 */
    }
}
