using Microsoft.EntityFrameworkCore;
using Npgsql;
using SeadQueryCore;
using System;
using System.Diagnostics;

namespace DataAccessPostgreSqlProvider {
    public class FacetContext : DbContext, IFacetContext
    {
        public StoreSetting Settings { get; set; }
        public string ConnectionString { get; set; }

        public FacetContext(IQueryBuilderSetting config)
        {
            Settings = config.Store;
            ConnectionString = new NpgsqlConnectionStringBuilder {
                Host = Settings.Host,
                Database = Settings.Database,
                Username = Settings.Username,
                Password = Settings.Password,
                Port = Convert.ToInt32(Settings.Port),
                Pooling = false
            }.ConnectionString;
        }

        public FacetContext(DbContextOptions<FacetContext> options) : base(options) {
        }

        public DbSet<ResultAggregate> ResultDefinitions { get; set; }
        public DbSet<ResultField> ResultFields { get; set; }
        public DbSet<ResultFieldType> ResultFieldTypes { get; set; }
        public DbSet<Facet> Facets { get; set; }
        public DbSet<TableRelation> TableRelations { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ViewState> ViewStates { get; set; }
        public DbSet<ResultViewType> ViewTypes { get; set; }
        public DbSet<FacetType> FacetTypes { get; set; }
        public DbSet<FacetGroup> FacetGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Table>().ToTable("table", "facet").HasKey(b => b.TableId);
            builder.Entity<Table>().Property(b => b.TableId).HasColumnName("table_id").IsRequired();
            builder.Entity<Table>().Property(b => b.TableOrUdfName).HasColumnName("table_or_udf_name").IsRequired();
            builder.Entity<Table>().Property(b => b.IsUdf).HasColumnName("is_udf").IsRequired();
            builder.Entity<Table>().Property(b => b.PrimaryKeyName).HasColumnName("primary_key_name").IsRequired();

            builder.Entity<TableRelation>().ToTable("table_relation", "facet").HasKey(b => b.TableRelationId);
            builder.Entity<TableRelation>().Property(b => b.TableRelationId).HasColumnName("table_relation_id").IsRequired();
            builder.Entity<TableRelation>().Property(b => b.SourceTableId).HasColumnName("source_table_id").IsRequired();
            builder.Entity<TableRelation>().Property(b => b.TargetTableId).HasColumnName("target_table_id").IsRequired();
            builder.Entity<TableRelation>().Property(b => b.Weight).HasColumnName("weight").IsRequired();
            builder.Entity<TableRelation>().Property(b => b.SourceColumName).HasColumnName("source_column_name").IsRequired();
            builder.Entity<TableRelation>().Property(b => b.TargetColumnName).HasColumnName("target_column_name").IsRequired();
            builder.Entity<TableRelation>().HasOne<Table>(x => x.SourceTable).WithMany().HasForeignKey(p => p.SourceTableId);
            builder.Entity<TableRelation>().HasOne<Table>(x => x.TargetTable).WithMany().HasForeignKey(p => p.TargetTableId);

            builder.Entity<FacetType>().ToTable("facet_type", "facet").HasKey(b => b.FacetTypeId);
            builder.Entity<FacetType>().Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
            builder.Entity<FacetType>().Property(b => b.FacetTypeName).HasColumnName("facet_type_name").IsRequired();
            builder.Entity<FacetType>().Property(b => b.ReloadAsTarget).HasColumnName("reload_as_target").IsRequired();

            builder.Entity<Facet>().ToTable("facet", "facet").HasKey(b => b.FacetId);
            builder.Entity<Facet>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<Facet>().Property(b => b.FacetCode).HasColumnName("facet_code").IsRequired();
            builder.Entity<Facet>().Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
            builder.Entity<Facet>().Property(b => b.Description).HasColumnName("description").IsRequired();
            builder.Entity<Facet>().Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
            builder.Entity<Facet>().Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
            builder.Entity<Facet>().Property(b => b.CategoryIdExpr).HasColumnName("category_id_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.CategoryNameExpr).HasColumnName("category_name_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.SortExpr).HasColumnName("sort_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<Facet>().Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();
            builder.Entity<Facet>().Property(b => b.AggregateType).HasColumnName("aggregate_type").IsRequired();
            builder.Entity<Facet>().Property(b => b.AggregateTitle).HasColumnName("aggregate_title").IsRequired();
            builder.Entity<Facet>().Property(b => b.AggregateFacetId).HasColumnName("aggregate_facet_id").IsRequired();
            // builder.Entity<Facet>().Ignore(z => z.ExtraTables).Ignore(z => z.TargetTable);

            builder.Entity<Facet>().HasOne<FacetType>(x => x.FacetType).WithMany().HasForeignKey(p => p.FacetTypeId);
            builder.Entity<Facet>().HasOne<FacetGroup>(x => x.FacetGroup).WithMany(x => x.Facets).HasForeignKey(p => p.FacetGroupId);

            builder.Entity<FacetGroup>().ToTable("facet_group", "facet").HasKey(b => b.FacetGroupId);
            builder.Entity<FacetGroup>().Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.FacetGroupKey).HasColumnName("facet_group_key").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.Description).HasColumnName("description").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();

            builder.Entity<FacetClause>().ToTable("facet_clause", "facet").HasKey(b => b.FacetClauseId);
            builder.Entity<FacetClause>().Property(b => b.FacetClauseId).HasColumnName("facet_clause_id").IsRequired();
            builder.Entity<FacetClause>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetClause>().Property(b => b.Clause).HasColumnName("clause").IsRequired();
            builder.Entity<FacetClause>().HasOne<Facet>(x => x.Facet).WithMany(x => x.Clauses).HasForeignKey(x => x.FacetId);

            builder.Entity<FacetTable>().ToTable("facet_table", "facet").HasKey(b => b.FacetTableId);
            builder.Entity<FacetTable>().Property(b => b.FacetTableId).HasColumnName("facet_table_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.TableId).HasColumnName("table_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.UdfCallArguments).HasColumnName("udf_call_arguments");
            builder.Entity<FacetTable>().Property(b => b.Alias).HasColumnName("alias");
            builder.Entity<FacetTable>().HasOne<Facet>(x => x.Facet).WithMany(x => x.Tables).HasForeignKey(x => x.FacetId);
            builder.Entity<FacetTable>().HasOne<Table>(x => x.Table).WithMany().HasForeignKey(p => p.TableId);

            builder.Entity<ResultViewType>().ToTable("result_view_type", "facet").HasKey(b => b.ViewTypeId);
            builder.Entity<ResultViewType>().Property(b => b.ViewTypeId).HasColumnName("view_type_id").IsRequired();
            builder.Entity<ResultViewType>().Property(b => b.ViewName).HasColumnName("view_name").IsRequired();
            builder.Entity<ResultViewType>().Property(b => b.IsCachable).HasColumnName("is_cachable").IsRequired();

            builder.Entity<ResultField>().ToTable("result_field", "facet").HasKey(b => b.ResultFieldId);
            builder.Entity<ResultField>().Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
            builder.Entity<ResultField>().Property(b => b.ResultFieldKey).HasColumnName("result_field_key").IsRequired();
            builder.Entity<ResultField>().Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();
            builder.Entity<ResultField>().Property(b => b.TableName).HasColumnName("table_name"); //.IsRequired();
            builder.Entity<ResultField>().Property(b => b.ColumnName).HasColumnName("column_name").IsRequired();
            builder.Entity<ResultField>().Property(b => b.DisplayText).HasColumnName("display_text").IsRequired();
            builder.Entity<ResultField>().Property(b => b.Activated).HasColumnName("activated").IsRequired();
            builder.Entity<ResultField>().Property(b => b.LinkUrl).HasColumnName("link_url");
            builder.Entity<ResultField>().Property(b => b.LinkLabel).HasColumnName("link_label");
            builder.Entity<ResultField>().HasOne(x => x.FieldType).WithMany().HasForeignKey(p => p.FieldTypeId);

            builder.Entity<ResultFieldType>().ToTable("result_field_type", "facet").HasKey(b => b.FieldTypeId);
            builder.Entity<ResultFieldType>().Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();
            builder.Entity<ResultFieldType>().Property(b => b.IsResultValue).HasColumnName("is_result_value").IsRequired();
            builder.Entity<ResultFieldType>().Property(b => b.SqlFieldCompiler).HasColumnName("sql_field_compiler").IsRequired();
            builder.Entity<ResultFieldType>().Property(b => b.IsSortField).HasColumnName("is_sort_field").IsRequired();
            builder.Entity<ResultFieldType>().Property(b => b.IsAggregateField).HasColumnName("is_aggregate_field").IsRequired();
            builder.Entity<ResultFieldType>().Property(b => b.IsItemField).HasColumnName("is_item_field").IsRequired();
            builder.Entity<ResultFieldType>().Property(b => b.SqlTemplate).HasColumnName("sql_template").IsRequired();

            builder.Entity<ResultAggregate>().ToTable("result_aggregate", "facet").HasKey(b => b.AggregateId);
            builder.Entity<ResultAggregate>().Property(b => b.AggregateId).HasColumnName("aggregate_id").IsRequired();
            builder.Entity<ResultAggregate>().Property(b => b.AggregateKey).HasColumnName("aggregate_key").IsRequired();
            builder.Entity<ResultAggregate>().Property(b => b.DisplayText).HasColumnName("display_text").IsRequired();
            builder.Entity<ResultAggregate>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<ResultAggregate>().Property(b => b.IsActivated).HasColumnName("is_activated").IsRequired();
            builder.Entity<ResultAggregate>().Property(b => b.InputType).HasColumnName("input_type").IsRequired();
            builder.Entity<ResultAggregate>().Property(b => b.HasSelector).HasColumnName("has_selector").IsRequired();
            builder.Entity<ResultAggregate>().HasMany<ResultAggregateField>(x => x.Fields).WithOne(x => x.Aggregate).HasForeignKey(p => p.AggregateId);

            builder.Entity<ResultAggregateField>().ToTable("result_aggregate_field", "facet").HasKey(b => b.AggregateFieldId);
            builder.Entity<ResultAggregateField>().Property(b => b.AggregateFieldId).HasColumnName("aggregate_field_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.AggregateId).HasColumnName("aggregate_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();
            builder.Entity<ResultAggregateField>().HasOne<ResultFieldType>(x => x.FieldType).WithMany().HasForeignKey(p => p.FieldTypeId);
            builder.Entity<ResultAggregateField>().HasOne<ResultField>(x => x.ResultField).WithMany().HasForeignKey(p => p.ResultFieldId);

            builder.Entity<ViewState>().ToTable("view_state", "facet").HasKey(b => b.Key);
            builder.Entity<ViewState>().Property(b => b.Key).HasColumnName("view_state_key").IsRequired();
            builder.Entity<ViewState>().Property(b => b.Data).HasColumnName("view_state_data").IsRequired();

            base.OnModelCreating(builder);
        }


        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
