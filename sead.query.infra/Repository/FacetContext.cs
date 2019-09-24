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
        public DbSet<GraphEdge> Edges { get; set; }
        public DbSet<GraphNode> Nodes { get; set; }
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
            builder.Entity<FacetType>().ToTable("facet_type", "facet").HasKey(b => b.FacetTypeId);
            builder.Entity<FacetType>().Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
            builder.Entity<FacetType>().Property(b => b.FacetTypeName).HasColumnName("facet_type_name").IsRequired();
            builder.Entity<FacetType>().Property(b => b.ReloadAsTarget).HasColumnName("reload_as_target").IsRequired();

            builder.Entity<Facet>().ToTable("facet", "facet").HasKey(b => b.FacetId);
            builder.Entity<Facet>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<Facet>().Property(b => b.FacetCode).HasColumnName("facet_code").IsRequired();
            builder.Entity<Facet>().Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
            builder.Entity<Facet>().Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
            builder.Entity<Facet>().Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
            builder.Entity<Facet>().Property(b => b.CategoryIdExpr).HasColumnName("category_id_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.CategoryNameExpr).HasColumnName("category_name_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.IconIdExpr).HasColumnName("icon_id_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.SortExpr).HasColumnName("sort_expr").IsRequired();
            builder.Entity<Facet>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<Facet>().Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();
            builder.Entity<Facet>().Property(b => b.AggregateType).HasColumnName("aggregate_type").IsRequired();
            builder.Entity<Facet>().Property(b => b.AggregateTitle).HasColumnName("aggregate_title").IsRequired();
            builder.Entity<Facet>().Property(b => b.AggregateFacetId).HasColumnName("aggregate_facet_id").IsRequired();
            builder.Entity<Facet>().Ignore(z => z.ExtraTables).Ignore(z => z.TargetTable);

            builder.Entity<Facet>().HasOne<FacetType>(x => x.FacetType).WithMany().HasForeignKey(p => p.FacetTypeId);
            builder.Entity<Facet>().HasOne<FacetGroup>(x => x.FacetGroup).WithMany(x => x.Facets).HasForeignKey(p => p.FacetGroupId);

            builder.Entity<FacetGroup>().ToTable("facet_group", "facet").HasKey(b => b.FacetGroupId);
            builder.Entity<FacetGroup>().Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.FacetGroupKey).HasColumnName("facet_group_key").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
            builder.Entity<FacetGroup>().Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();

            builder.Entity<FacetClause>().ToTable("facet_clause", "facet").HasKey(b => b.FacetSourceTableId);
            builder.Entity<FacetClause>().Property(b => b.FacetSourceTableId).HasColumnName("facet_source_table_id").IsRequired();
            builder.Entity<FacetClause>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetClause>().Property(b => b.Clause).HasColumnName("clause").IsRequired();
            builder.Entity<FacetClause>().HasOne<Facet>(x => x.Facet).WithMany(x => x.Clauses).HasForeignKey(x => x.FacetId);

            builder.Entity<FacetTable>().ToTable("facet_table", "facet").HasKey(b => b.FacetTableId);
            builder.Entity<FacetTable>().Property(b => b.FacetTableId).HasColumnName("facet_table_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.SchemaName).HasColumnName("schema_name").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.ObjectName).HasColumnName("table_or_udf_name").IsRequired();
            builder.Entity<FacetTable>().Property(b => b.ObjectArgs).HasColumnName("udf_call_arguments");
            builder.Entity<FacetTable>().Property(b => b.Alias).HasColumnName("alias").IsRequired();
            builder.Entity<FacetTable>().HasOne<Facet>(x => x.Facet).WithMany(x => x.Tables).HasForeignKey(x => x.FacetId);

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

            builder.Entity<GraphNode>().ToTable("graph_table", "facet").HasKey(b => b.NodeId);
            builder.Entity<GraphNode>().Property(b => b.NodeId).HasColumnName("table_id").IsRequired();
            builder.Entity<GraphNode>().Property(b => b.TableName).HasColumnName("table_name").IsRequired();

            builder.Entity<GraphEdge>().ToTable("graph_table_relation", "facet").HasKey(b => b.EdgeId);
            builder.Entity<GraphEdge>().Property(b => b.EdgeId).HasColumnName("relation_id").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.SourceNodeId).HasColumnName("source_table_id").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.TargetNodeId).HasColumnName("target_table_id").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.Weight).HasColumnName("weight").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.SourceKeyName).HasColumnName("source_column_name").IsRequired();
            builder.Entity<GraphEdge>().Property(b => b.TargetKeyName).HasColumnName("target_column_name").IsRequired();
            builder.Entity<GraphEdge>().HasOne<GraphNode>(x => x.SourceNode).WithMany().HasForeignKey(p => p.SourceNodeId);
            builder.Entity<GraphEdge>().HasOne<GraphNode>(x => x.TargetNode).WithMany().HasForeignKey(p => p.TargetNodeId);

            builder.Entity<ViewState>().ToTable("view_state", "facet").HasKey(b => b.Key);
            builder.Entity<ViewState>().Property(b => b.Key).HasColumnName("view_state_key").IsRequired();
            builder.Entity<ViewState>().Property(b => b.Data).HasColumnName("view_state_data").IsRequired();

            base.OnModelCreating(builder);
        }

        //protected void OnModelCreatingScaffolded(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasPostgresExtension("pgcrypto")
        //        .HasPostgresExtension("tablefunc")
        //        .HasPostgresExtension("uuid-ossp")
        //        .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

        //    modelBuilder.Entity<Facet>(entity =>
        //    {
        //        entity.ToTable("facet", "facet");

        //        entity.Property(e => e.FacetId)
        //            .HasColumnName("facet_id")
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.AggregateFacetId).HasColumnName("aggregate_facet_id");

        //        entity.Property(e => e.AggregateTitle)
        //            .IsRequired()
        //            .HasColumnName("aggregate_title")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.AggregateType)
        //            .IsRequired()
        //            .HasColumnName("aggregate_type")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.CategoryIdExpr)
        //            .IsRequired()
        //            .HasColumnName("category_id_expr")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.CategoryNameExpr)
        //            .IsRequired()
        //            .HasColumnName("category_name_expr")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.DisplayTitle)
        //            .IsRequired()
        //            .HasColumnName("display_title")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.FacetGroupId).HasColumnName("facet_group_id");

        //        entity.Property(e => e.FacetCode)
        //            .IsRequired()
        //            .HasColumnName("facet_code")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.FacetTypeId).HasColumnName("facet_type_id");

        //        entity.Property(e => e.IconIdExpr)
        //            .IsRequired()
        //            .HasColumnName("icon_id_expr")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");

        //        entity.Property(e => e.IsDefault).HasColumnName("is_default");

        //        entity.Property(e => e.SortExpr)
        //            .IsRequired()
        //            .HasColumnName("sort_expr")
        //            .HasMaxLength(256);

        //        entity.HasOne(d => d.FacetGroup)
        //            .WithMany(p => p.Facets)
        //            .HasForeignKey(d => d.FacetGroupId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("facet_facet_group_id_fkey");

        //        entity.HasOne(d => d.FacetType)
        //            .WithMany()
        //            .HasForeignKey(d => d.FacetTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("facet_facet_type_id_fkey");
        //    });

        //    modelBuilder.Entity<FacetClause>(entity =>
        //    {
        //        entity.HasKey(e => e.FacetSourceTableId)
        //            .HasName("facet_condition_clause_pkey");

        //        entity.ToTable("facet_condition_clause", "facet");

        //        entity.Property(e => e.FacetSourceTableId)
        //            .HasColumnName("facet_source_table_id")
        //            .HasDefaultValueSql("nextval('facet.facet_condition_clause_facet_source_table_id_seq'::regclass)");

        //        entity.Property(e => e.Clause)
        //            .HasColumnName("clause")
        //            .HasMaxLength(512);

        //        entity.Property(e => e.FacetId).HasColumnName("facet_id");
        //        entity.HasOne<Facet>()
        //            .WithMany(p => p.Clauses)
        //            .HasForeignKey(d => d.FacetId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("facet_condition_clause_facet_id_fkey");
        //    });

        //    modelBuilder.Entity<FacetGroup>(entity =>
        //    {
        //        entity.ToTable("facet_group", "facet");

        //        entity.Property(e => e.FacetGroupId)
        //            .HasColumnName("facet_group_id")
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.DisplayTitle)
        //            .IsRequired()
        //            .HasColumnName("display_title")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.FacetGroupKey)
        //            .IsRequired()
        //            .HasColumnName("facet_group_key")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");

        //        entity.Property(e => e.IsDefault).HasColumnName("is_default");
        //    });

        //    modelBuilder.Entity<FacetTable>(entity =>
        //    {
        //        entity.ToTable("facet_table", "facet");

        //        entity.Property(e => e.FacetTableId)
        //            .HasColumnName("facet_table_id")
        //            .HasDefaultValueSql("nextval('facet.facet_table_facet_table_id_seq'::regclass)");

        //        entity.Property(e => e.Alias)
        //            .HasColumnName("alias")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.FacetId).HasColumnName("facet_id");

        //        entity.Property(e => e.SchemaName)
        //            .HasColumnName("schema_name")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.SequenceId).HasColumnName("sequence_id");

        //        entity.Property(e => e.TableName)
        //            .HasColumnName("table_name")
        //            .HasMaxLength(80);

        //        entity.HasOne<Facet>(p => p.Facet)
        //            .WithMany(p => p.Tables)
        //            .HasForeignKey(d => d.FacetId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("facet_table_facet_id_fkey");
        //    });

        //    modelBuilder.Entity<FacetType>(entity =>
        //    {
        //        entity.ToTable("facet_type", "facet");

        //        entity.Property(e => e.FacetTypeId)
        //            .HasColumnName("facet_type_id")
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.FacetTypeName)
        //            .IsRequired()
        //            .HasColumnName("facet_type_name")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.ReloadAsTarget).HasColumnName("reload_as_target");
        //    });

        //    modelBuilder.Entity<GraphNode>(entity =>
        //    {
        //        entity.HasKey(e => e.TableId)
        //            .HasName("graph_table_pkey");

        //        entity.ToTable("graph_table", "facet");

        //        entity.Property(e => e.TableId)
        //            .HasColumnName("table_id")
        //            .HasDefaultValueSql("nextval('facet.graph_table_table_id_seq'::regclass)");

        //        entity.Property(e => e.TableName)
        //            .IsRequired()
        //            .HasColumnName("table_name")
        //            .HasColumnType("character varying");
        //    });

        //    modelBuilder.Entity<GraphEdge>(entity =>
        //    {
        //        entity.HasKey(e => e.EdgeId)
        //            .HasName("graph_table_relation_pkey");

        //        entity.ToTable("graph_table_relation", "facet");

        //        entity.HasIndex(e => e.SourceNodeId)
        //            .HasName("idx_graph_table_relation_fk1");

        //        entity.HasIndex(e => e.TargetNodeId)
        //            .HasName("idx_graph_table_relation_fk2");

        //        entity.Property(e => e.EdgeId)
        //            .HasColumnName("relation_id")
        //            .HasDefaultValueSql("nextval('facet.graph_table_relation_relation_id_seq'::regclass)");

        //        entity.Property(e => e.SourceKeyName)
        //            .IsRequired()
        //            .HasColumnName("source_column_name")
        //            .HasColumnType("character varying");

        //        entity.Property(e => e.SourceNodeId).HasColumnName("source_table_id");

        //        entity.Property(e => e.TargetKeyName)
        //            .IsRequired()
        //            .HasColumnName("target_column_name")
        //            .HasColumnType("character varying");

        //        entity.Property(e => e.TargetNodeId).HasColumnName("target_table_id");

        //        entity.Property(e => e.Weight).HasColumnName("weight");

        //        entity.HasOne(d => d.SourceNode)
        //            .WithMany() // p => p.GraphTableRelationSourceTable)
        //            .HasForeignKey(d => d.SourceNodeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("graph_table_relation_source_table_id_fkey");

        //        entity.HasOne(d => d.TargetNode)
        //            .WithMany() // p => p.GraphTableRelationTargetTable)
        //            .HasForeignKey(d => d.TargetNodeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("graph_table_relation_target_table_id_fkey");
        //    });

        //    modelBuilder.Entity<ResultAggregate>(entity =>
        //    {
        //        entity.HasKey(e => e.AggregateId)
        //            .HasName("result_aggregate_pkey");

        //        entity.ToTable("result_aggregate", "facet");

        //        entity.Property(e => e.AggregateId)
        //            .HasColumnName("aggregate_id")
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.AggregateKey)
        //            .IsRequired()
        //            .HasColumnName("aggregate_key")
        //            .HasMaxLength(40);

        //        entity.Property(e => e.DisplayText)
        //            .IsRequired()
        //            .HasColumnName("display_text")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.HasSelector)
        //            .IsRequired()
        //            .HasColumnName("has_selector")
        //            .HasDefaultValueSql("true");

        //        entity.Property(e => e.InputType)
        //            .IsRequired()
        //            .HasColumnName("input_type")
        //            .HasMaxLength(40)
        //            .HasDefaultValueSql("'checkboxes'::character varying");

        //        entity.Property(e => e.IsActivated)
        //            .IsRequired()
        //            .HasColumnName("is_activated")
        //            .HasDefaultValueSql("true");

        //        entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");
        //    });

        //    modelBuilder.Entity<ResultAggregateField>(entity =>
        //    {
        //        entity.HasKey(e => e.AggregateFieldId)
        //            .HasName("result_aggregate_field_pkey");

        //        entity.ToTable("result_aggregate_field", "facet");

        //        entity.Property(e => e.AggregateFieldId)
        //            .HasColumnName("aggregate_field_id")
        //            .HasDefaultValueSql("nextval('facet.result_aggregate_field_aggregate_field_id_seq'::regclass)");

        //        entity.Property(e => e.AggregateId).HasColumnName("aggregate_id");

        //        entity.Property(e => e.FieldTypeId)
        //            .IsRequired()
        //            .HasColumnName("field_type_id")
        //            .HasMaxLength(40)
        //            .HasDefaultValueSql("'single_item'::character varying");

        //        entity.Property(e => e.ResultFieldId).HasColumnName("result_field_id");

        //        entity.Property(e => e.SequenceId).HasColumnName("sequence_id");

        //        entity.HasOne(d => d.Aggregate)
        //            .WithMany()
        //            .HasForeignKey(d => d.AggregateId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("result_aggregate_field_aggregate_id_fkey");

        //        entity.HasOne(d => d.FieldType)
        //            .WithMany()
        //            .HasForeignKey(d => d.FieldTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("result_aggregate_field_field_type_id_fkey");

        //        entity.HasOne(d => d.ResultField)
        //            .WithMany()
        //            .HasForeignKey(d => d.ResultFieldId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("result_aggregate_field_result_field_id_fkey");
        //    });

        //    modelBuilder.Entity<ResultField>(entity =>
        //    {
        //        entity.ToTable("result_field", "facet");

        //        entity.Property(e => e.ResultFieldId)
        //            .HasColumnName("result_field_id")
        //            .HasDefaultValueSql("nextval('facet.result_field_result_field_id_seq'::regclass)");

        //        entity.Property(e => e.Activated).HasColumnName("activated");

        //        entity.Property(e => e.ColumnName)
        //            .IsRequired()
        //            .HasColumnName("column_name")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.DisplayText)
        //            .IsRequired()
        //            .HasColumnName("display_text")
        //            .HasMaxLength(80);

        //        entity.Property(e => e.FieldTypeId)
        //            .IsRequired()
        //            .HasColumnName("field_type_id")
        //            .HasMaxLength(20);

        //        entity.Property(e => e.LinkLabel)
        //            .HasColumnName("link_label")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.LinkUrl)
        //            .HasColumnName("link_url")
        //            .HasMaxLength(256);

        //        entity.Property(e => e.ResultFieldKey)
        //            .IsRequired()
        //            .HasColumnName("result_field_key")
        //            .HasMaxLength(40);

        //        entity.Property(e => e.TableName)
        //            .HasColumnName("table_name")
        //            .HasMaxLength(80);

        //        entity.HasOne(d => d.FieldType)
        //            .WithMany() // p => p.ResultField)
        //            .HasForeignKey(d => d.FieldTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("result_field_field_type_id_fkey");
        //    });

        //    modelBuilder.Entity<ResultFieldType>(entity =>
        //    {
        //        entity.HasKey(e => e.FieldTypeId)
        //            .HasName("result_field_type_pkey");

        //        entity.ToTable("result_field_type", "facet");

        //        entity.Property(e => e.FieldTypeId)
        //            .HasColumnName("field_type_id")
        //            .HasMaxLength(40)
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.IsAggregateField).HasColumnName("is_aggregate_field");

        //        entity.Property(e => e.IsItemField).HasColumnName("is_item_field");

        //        entity.Property(e => e.IsResultValue)
        //            .IsRequired()
        //            .HasColumnName("is_result_value")
        //            .HasDefaultValueSql("true");

        //        entity.Property(e => e.IsSortField).HasColumnName("is_sort_field");

        //        entity.Property(e => e.SqlFieldCompiler)
        //            .IsRequired()
        //            .HasColumnName("sql_field_compiler")
        //            .HasMaxLength(40)
        //            .HasDefaultValueSql("''::character varying");

        //        entity.Property(e => e.SqlTemplate)
        //            .IsRequired()
        //            .HasColumnName("sql_template")
        //            .HasMaxLength(256)
        //            .HasDefaultValueSql("'{0}'::character varying");
        //    });

        //    modelBuilder.Entity<ResultViewType>(entity =>
        //    {
        //        entity.HasKey(e => e.ViewTypeId)
        //            .HasName("result_view_type_pkey");

        //        entity.ToTable("result_view_type", "facet");

        //        entity.Property(e => e.ViewTypeId)
        //            .HasColumnName("view_type_id")
        //            .HasMaxLength(40)
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.IsCachable)
        //            .IsRequired()
        //            .HasColumnName("is_cachable")
        //            .HasDefaultValueSql("true");

        //        entity.Property(e => e.ViewName)
        //            .IsRequired()
        //            .HasColumnName("view_name")
        //            .HasMaxLength(40);
        //    });

        //    modelBuilder.Entity<ViewState>(entity =>
        //    {
        //        entity.HasKey(e => e.Key)
        //            .HasName("view_state_pkey");

        //        entity.ToTable("view_state", "facet");

        //        entity.Property(e => e.Key)
        //            .HasColumnName("view_state_key")
        //            .HasMaxLength(80)
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.Data)
        //            .IsRequired()
        //            .HasColumnName("view_state_data");
        //    });

        //    modelBuilder.HasSequence<int>("facet_condition_clause_facet_source_table_id_seq");

        //    modelBuilder.HasSequence<int>("facet_table_facet_table_id_seq");

        //    modelBuilder.HasSequence<int>("graph_table_relation_relation_id_seq");

        //    modelBuilder.HasSequence<int>("graph_table_table_id_seq");

        //    modelBuilder.HasSequence<int>("result_aggregate_field_aggregate_field_id_seq");

        //    modelBuilder.HasSequence<int>("result_field_result_field_id_seq");
        //}

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
