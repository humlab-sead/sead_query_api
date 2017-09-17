using Microsoft.EntityFrameworkCore;
using Npgsql.Logging;
using QuerySeadDomain;
using System.Diagnostics;

namespace DataAccessPostgreSqlProvider {
 
    public class DomainModelDbContext : DbContext {

        public StoreSetting Settings { get; set; }

        public DomainModelDbContext(IQueryBuilderSetting config)
        {
            Settings = config.Store;
        }

        public DbSet<ResultDefinition> ResultDefinitions { get; set; }
        public DbSet<FacetDefinition> FacetDefinitions { get; set; }
        public DbSet<GraphEdge> Edges { get; set; }
        public DbSet<GraphNode> Nodes { get; set; }
        public DbSet<ViewState> ViewStates { get; set; }

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

            builder.Entity<ResultField>().ToTable("result_field", "facet").HasKey(b => b.ResultFieldId);
            builder.Entity<ResultField>().Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
            builder.Entity<ResultField>().Property(b => b.ResultFieldKey).HasColumnName("result_field_key").IsRequired();
            builder.Entity<ResultField>().Property(b => b.TableName).HasColumnName("table_name").IsRequired();
            builder.Entity<ResultField>().Property(b => b.ColumnName).HasColumnName("column_name").IsRequired();
            builder.Entity<ResultField>().Property(b => b.DisplayText).HasColumnName("display_text").IsRequired();
            builder.Entity<ResultField>().Property(b => b.ResultType).HasColumnName("result_type").IsRequired();
            builder.Entity<ResultField>().Property(b => b.Activated).HasColumnName("activated").IsRequired();
            builder.Entity<ResultField>().Property(b => b.LinkUrl).HasColumnName("link_url");
            builder.Entity<ResultField>().Property(b => b.LinkLabel).HasColumnName("link_label");

            builder.Entity<ResultType>().ToTable("result_type", "facet").HasKey(b => b.ResultTypeId);
            builder.Entity<ResultType>().Property(b => b.ResultTypeId).HasColumnName("result_type_id").IsRequired();
            builder.Entity<ResultType>().Property(b => b.ResultTypeName).HasColumnName("result_type").IsRequired();

            builder.Entity<ResultDefinition>().ToTable("result_definition", "facet").HasKey(b => b.ResultDefinitionId);
            builder.Entity<ResultDefinition>().Property(b => b.ResultDefinitionId).HasColumnName("result_definition_id").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.Key).HasColumnName("result_definition_key").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.DisplayText).HasColumnName("display_text").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.IsActivated).HasColumnName("is_activated").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.AggregationType).HasColumnName("aggregation_type").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.InputType).HasColumnName("input_type").IsRequired();
            builder.Entity<ResultDefinition>().Property(b => b.HasAggregationSelector).HasColumnName("has_aggregation_selector").IsRequired();

            builder.Entity<ResultDefinition>()
                .HasMany<ResultDefinitionField>(x => x.Fields)
                .WithOne(x => x.ResultDefinition)
                .HasForeignKey(p => p.ResultDefinitionId);

            builder.Entity<ResultDefinitionField>().ToTable("result_definition_field", "facet").HasKey(b => b.ResultDefinitionFieldId);
            builder.Entity<ResultDefinitionField>().Property(b => b.ResultDefinitionFieldId).HasColumnName("result_definition_field_id").IsRequired();
            builder.Entity<ResultDefinitionField>().Property(b => b.ResultDefinitionId).HasColumnName("result_definition_id").IsRequired();
            builder.Entity<ResultDefinitionField>().Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
            builder.Entity<ResultDefinitionField>().Property(b => b.ResultTypeId).HasColumnName("result_type_id").IsRequired();

            builder.Entity<ResultDefinitionField>()
                .HasOne<ResultType>(x => x.ResultType)
                .WithMany()
                .HasForeignKey(p => p.ResultTypeId);

            builder.Entity<ResultDefinitionField>()
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

            builder.Entity<ViewState>().ToTable("tbl_view_states", "metainformation").HasKey(b => b.ViewStateId);
            builder.Entity<ViewState>().Property(b => b.ViewStateId).HasColumnName("view_state_id").ValueGeneratedOnAdd().IsRequired();
            builder.Entity<ViewState>().Property(b => b.SessionId).HasColumnName("session_id").IsRequired();
            builder.Entity<ViewState>().Property(b => b.Data).HasColumnName("view_state").IsRequired();
            builder.Entity<ViewState>().Property(b => b.CreateTime).HasColumnName("creatation_date").IsRequired();

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
