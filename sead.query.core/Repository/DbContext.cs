using Microsoft.EntityFrameworkCore;
using Npgsql.Logging;
using SeadQueryCore;
using System.Diagnostics;

namespace DataAccessPostgreSqlProvider {
    public class DomainModelDbContext : DbContext {
        public StoreSetting Settings { get; set; }

        public DomainModelDbContext(IQueryBuilderSetting config)
        {
            Settings = config.Store;
        }

        public DbSet<ResultAggregate> ResultDefinitions { get; set; }
        public DbSet<ResultField> ResultFields { get; set; }
        public DbSet<ResultFieldType> ResultFieldTypes { get; set; }
        public DbSet<FacetDefinition> FacetDefinitions { get; set; }
        public DbSet<GraphEdge> Edges { get; set; }
        public DbSet<GraphNode> Nodes { get; set; }
        public DbSet<ViewState> ViewStates { get; set; }
        public DbSet<ResultViewType> ViewTypes { get; set; }

        public DbSet<FacetType> FacetTypes { get; set; }
        public DbSet<FacetGroup> FacetGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FacetType>().ToTable("facet_type", "facet").HasKey(b => b.FacetTypeId);
            builder.Entity<FacetType>().Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
            builder.Entity<FacetType>().Property(b => b.FacetTypeName).HasColumnName("facet_type_name").IsRequired();
            builder.Entity<FacetType>().Property(b => b.ReloadAsTarget).HasColumnName("reload_as_target").IsRequired();

            builder.Entity<FacetDefinition>().ToTable("facet", "facet").HasKey(b => b.FacetId);
            builder.Entity<FacetDefinition>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.FacetCode).HasColumnName("facet_key").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.CategoryIdExpr).HasColumnName("category_id_expr").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.CategoryNameExpr).HasColumnName("category_name_expr").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.IconIdExpr).HasColumnName("icon_id_expr").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.SortExpr).HasColumnName("sort_expr").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.AggregateType).HasColumnName("aggregate_type").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.AggregateTitle).HasColumnName("aggregate_title").IsRequired();
            builder.Entity<FacetDefinition>().Property(b => b.AggregateFacetId).HasColumnName("aggregate_facet_id").IsRequired();
            builder.Entity<FacetDefinition>().Ignore(z => z.ExtraTables).Ignore(z => z.TargetTable);

            builder.Entity<FacetDefinition>().HasOne<FacetType>(x => x.FacetType).WithMany().HasForeignKey(p => p.FacetTypeId);
            builder.Entity<FacetDefinition>().HasOne<FacetGroup>(x => x.FacetGroup).WithMany(x => x.Items).HasForeignKey(p => p.FacetGroupId);
            //builder.Entity<FacetDefinition>().HasMany<FacetTable>(x => x.Tables).WithOne(z => z.FacetDefinition).HasForeignKey(p => p.FacetId);

            builder.Entity<FacetGroup>().ToTable("facet_group", "facet").HasKey(b => b.FacetGroupId);
            builder.Entity<FacetGroup>().Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.FacetGroupKey).HasColumnName("facet_group_key").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();

            builder.Entity<FacetConditionClause>().ToTable("facet_condition_clause", "facet").HasKey(b => b.FacetSourceTableId);
            builder.Entity<FacetConditionClause>().Property(b => b.FacetSourceTableId).HasColumnName("facet_source_table_id").IsRequired();
            builder.Entity<FacetConditionClause>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetConditionClause>().Property(b => b.Clause).HasColumnName("clause").IsRequired();
            builder.Entity<FacetConditionClause>().HasOne<FacetDefinition>(x => x.FacetDefinition).WithMany(x => x.Clauses).HasForeignKey(x => x.FacetId);

            builder.Entity<FacetTable>().ToTable("facet_table", "facet").HasKey(b => b.FacetTableId);
            builder.Entity<FacetTable>().Property(b => b.FacetTableId).HasColumnName("facet_table_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.SchemaName).HasColumnName("schema_name").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.TableName).HasColumnName("table_name").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.Alias).HasColumnName("alias").IsRequired();
            //builder.Entity<FacetTable>().HasOne<FacetDefinition>(x => x.FacetDefinition).WithMany(x => x.Tables).HasForeignKey(x => x.FacetId);
            builder.Entity<FacetTable>().HasOne<FacetDefinition>(x => x.FacetDefinition).WithMany(x => x.Tables).HasForeignKey(x => x.FacetId);

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

            builder.Entity<ResultAggregate>()
                .HasMany<ResultAggregateField>(x => x.Fields)
                .WithOne(x => x.Aggregate)
                .HasForeignKey(p => p.AggregateId);

            builder.Entity<ResultAggregateField>().ToTable("result_aggregate_field", "facet").HasKey(b => b.AggregateFieldId);
            builder.Entity<ResultAggregateField>().Property(b => b.AggregateFieldId).HasColumnName("aggregate_field_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.AggregateId).HasColumnName("aggregate_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
            builder.Entity<ResultAggregateField>().Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();

            builder.Entity<ResultAggregateField>()
                .HasOne<ResultFieldType>(x => x.FieldType)
                .WithMany()
                .HasForeignKey(p => p.FieldTypeId);

            builder.Entity<ResultAggregateField>()
                .HasOne<ResultField>(x => x.ResultField)
                .WithMany()
                .HasForeignKey(p => p.ResultFieldId);

            builder.Entity<GraphNode>().ToTable("graph_table", "facet").HasKey(b => b.TableId);
            builder.Entity<GraphNode>().Property(b => b.TableId).HasColumnName("table_id").IsRequired();
            builder.Entity<GraphNode>().Property(b => b.TableName).HasColumnName("table_name").IsRequired();

            builder.Entity<GraphEdge>().ToTable("graph_table_relation", "facet").HasKey(b => b.RelationId);
            builder.Entity<GraphEdge>().Property(b => b.RelationId).HasColumnName("relation_id").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.SourceTableId).HasColumnName("source_table_id").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.TargetTableId).HasColumnName("target_table_id").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.Weight).HasColumnName("weight").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.SourceColumnName).HasColumnName("source_column_name").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.TargetColumnName).HasColumnName("target_column_name").IsRequired();
            builder.Entity<GraphEdge>().HasOne<GraphNode>(x => x.SourceTable).WithMany().HasForeignKey(p => p.SourceTableId);
            builder.Entity<GraphEdge>().HasOne<GraphNode>(x => x.TargetTable).WithMany().HasForeignKey(p => p.TargetTableId);

            builder.Entity<ViewState>().ToTable("view_state", "facet").HasKey(b => b.Key);
            builder.Entity<ViewState>().Property(b => b.Key).HasColumnName("view_state_key").IsRequired();
            builder.Entity<ViewState>().Property(b => b.Data).HasColumnName("view_state_data").IsRequired();

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            //updateUpdatedProperty<FacetDefinition>();
            return base.SaveChanges();
        }

        //private void updateUpdatedProperty<T>() where T : class
        //{
        //    var modifiedSourceInfo =
        //        ChangeTracker.Entries<T>()
        //            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        //}

    }
}
