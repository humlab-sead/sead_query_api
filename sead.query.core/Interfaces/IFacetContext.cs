using Microsoft.EntityFrameworkCore;
using System;

namespace SeadQueryCore
{
    public interface IFacetContext : IDisposable
    {
        // StoreSetting Settings { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet<TableRelation> TableRelations { get; set; }
        DbSet<Facet> Facets { get; set; }
        DbSet<FacetGroup> FacetGroups { get; set; }
        DbSet<FacetType> FacetTypes { get; set; }
        DbSet<Table> Tables { get; set; }
        DbSet<ResultAggregate> ResultDefinitions { get; set; }
        DbSet<ResultField> ResultFields { get; set; }
        DbSet<ResultFieldType> ResultFieldTypes { get; set; }
        DbSet<ViewState> ViewStates { get; set; }
        DbSet<ResultViewType> ViewTypes { get; set; }

        int SaveChanges();

        ITypedQueryProxy TypedQueryProxy { get; set; }
        IDynamicQueryProxy DynamicQueryProxy { get; set; }

    }
}