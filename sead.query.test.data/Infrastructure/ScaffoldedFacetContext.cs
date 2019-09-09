using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using sead.query.test.data.Entities;

namespace sead.query.test.data.Infrastructure
{

    public partial class ScaffoldedFacetContext : DbContext
    {
        public ScaffoldedFacetContext(DbContextOptions<ScaffoldedFacetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Facet> Facet { get; set; }
        public virtual DbSet<FacetClause> FacetClause { get; set; }
        public virtual DbSet<FacetGroup> FacetGroup { get; set; }
        public virtual DbSet<FacetTable> FacetTable { get; set; }
        public virtual DbSet<FacetType> FacetType { get; set; }
        public virtual DbSet<GraphTable> GraphTable { get; set; }
        public virtual DbSet<GraphTableRelation> GraphTableRelation { get; set; }
        public virtual DbSet<ResultAggregate> ResultAggregate { get; set; }
        public virtual DbSet<ResultAggregateField> ResultAggregateField { get; set; }
        public virtual DbSet<ResultField> ResultField { get; set; }
        public virtual DbSet<ResultFieldType> ResultFieldType { get; set; }
        public virtual DbSet<ResultViewType> ResultViewType { get; set; }
        public virtual DbSet<ViewState> ViewState { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new NotSupportedException("You must supply an already configured DbContextOptionsBuilder!");
                // optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("tablefunc")
                .HasPostgresExtension("uuid-ossp")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Facet>(entity =>
            {
                entity.ToTable("facet", "facet");

                entity.Property(e => e.FacetId)
                    .HasColumnName("facet_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AggregateFacetId).HasColumnName("aggregate_facet_id");

                entity.Property(e => e.AggregateTitle)
                    .IsRequired()
                    .HasColumnName("aggregate_title")
                    .HasMaxLength(256);

                entity.Property(e => e.AggregateType)
                    .IsRequired()
                    .HasColumnName("aggregate_type")
                    .HasMaxLength(256);

                entity.Property(e => e.CategoryIdExpr)
                    .IsRequired()
                    .HasColumnName("category_id_expr")
                    .HasMaxLength(256);

                entity.Property(e => e.CategoryNameExpr)
                    .IsRequired()
                    .HasColumnName("category_name_expr")
                    .HasMaxLength(256);

                entity.Property(e => e.DisplayTitle)
                    .IsRequired()
                    .HasColumnName("display_title")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetGroupId).HasColumnName("facet_group_id");

                entity.Property(e => e.FacetCode)
                    .IsRequired()
                    .HasColumnName("facet_code")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetTypeId).HasColumnName("facet_type_id");

                entity.Property(e => e.IconIdExpr)
                    .IsRequired()
                    .HasColumnName("icon_id_expr")
                    .HasMaxLength(256);

                entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.SortExpr)
                    .IsRequired()
                    .HasColumnName("sort_expr")
                    .HasMaxLength(256);

                entity.HasOne(d => d.FacetGroup)
                    .WithMany(p => p.Facets)
                    .HasForeignKey(d => d.FacetGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_facet_group_id_fkey");

                entity.HasOne(d => d.FacetType)
                    .WithMany()
                    .HasForeignKey(d => d.FacetTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_facet_type_id_fkey");
            });

            modelBuilder.Entity<FacetClause>(entity =>
            {
                entity.HasKey(e => e.FacetSourceTableId)
                    .HasName("facet_condition_clause_pkey");

                entity.ToTable("facet_clause", "facet");

                entity.Property(e => e.FacetSourceTableId)
                    .HasColumnName("facet_source_table_id")
                    .HasDefaultValueSql("nextval('facet.facet_condition_clause_facet_source_table_id_seq'::regclass)");

                entity.Property(e => e.Clause)
                    .HasColumnName("clause")
                    .HasMaxLength(512);

                entity.Property(e => e.FacetId).HasColumnName("facet_id");

                entity.HasOne(d => d.Facet)
                    .WithMany(p => p.Clauses)
                    .HasForeignKey(d => d.FacetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_condition_clause_facet_id_fkey");
            });

            modelBuilder.Entity<FacetGroup>(entity =>
            {
                entity.ToTable("facet_group", "facet");

                entity.Property(e => e.FacetGroupId)
                    .HasColumnName("facet_group_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DisplayTitle)
                    .IsRequired()
                    .HasColumnName("display_title")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetGroupKey)
                    .IsRequired()
                    .HasColumnName("facet_group_key")
                    .HasMaxLength(80);

                entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");
            });

            modelBuilder.Entity<FacetTable>(entity =>
            {
                entity.ToTable("facet_table", "facet");

                entity.Property(e => e.FacetTableId)
                    .HasColumnName("facet_table_id")
                    .HasDefaultValueSql("nextval('facet.facet_table_facet_table_id_seq'::regclass)");

                entity.Property(e => e.Alias)
                    .HasColumnName("alias")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetId).HasColumnName("facet_id");

                entity.Property(e => e.SchemaName)
                    .HasColumnName("schema_name")
                    .HasMaxLength(80);

                entity.Property(e => e.SequenceId).HasColumnName("sequence_id");

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(80);

                entity.HasOne(d => d.Facet)
                    .WithMany(p => p.Tables)
                    .HasForeignKey(d => d.FacetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_table_facet_id_fkey");
            });

            modelBuilder.Entity<FacetType>(entity =>
            {
                entity.ToTable("facet_type", "facet");

                entity.Property(e => e.FacetTypeId)
                    .HasColumnName("facet_type_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.FacetTypeName)
                    .IsRequired()
                    .HasColumnName("facet_type_name")
                    .HasMaxLength(80);

                entity.Property(e => e.ReloadAsTarget).HasColumnName("reload_as_target");
            });

            modelBuilder.Entity<GraphTable>(entity =>
            {
                entity.HasKey(e => e.TableId)
                    .HasName("graph_table_pkey");

                entity.ToTable("graph_table", "facet");

                entity.Property(e => e.TableId)
                    .HasColumnName("table_id")
                    .HasDefaultValueSql("nextval('facet.graph_table_table_id_seq'::regclass)");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnName("table_name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<GraphTableRelation>(entity =>
            {
                entity.HasKey(e => e.RelationId)
                    .HasName("graph_table_relation_pkey");

                entity.ToTable("graph_table_relation", "facet");

                entity.HasIndex(e => e.SourceTableId)
                    .HasName("idx_graph_table_relation_fk1");

                entity.HasIndex(e => e.TargetTableId)
                    .HasName("idx_graph_table_relation_fk2");

                entity.Property(e => e.RelationId)
                    .HasColumnName("relation_id")
                    .HasDefaultValueSql("nextval('facet.graph_table_relation_relation_id_seq'::regclass)");

                entity.Property(e => e.SourceColumnName)
                    .IsRequired()
                    .HasColumnName("source_column_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.SourceTableId).HasColumnName("source_table_id");

                entity.Property(e => e.TargetColumnName)
                    .IsRequired()
                    .HasColumnName("target_column_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.TargetTableId).HasColumnName("target_table_id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.SourceTable)
                    .WithMany()
                    .HasForeignKey(d => d.SourceTableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graph_table_relation_source_table_id_fkey");

                entity.HasOne(d => d.TargetTable)
                    .WithMany()
                    .HasForeignKey(d => d.TargetTableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("graph_table_relation_target_table_id_fkey");
            });

            modelBuilder.Entity<ResultAggregate>(entity =>
            {
                entity.HasKey(e => e.AggregateId)
                    .HasName("result_aggregate_pkey");

                entity.ToTable("result_aggregate", "facet");

                entity.Property(e => e.AggregateId)
                    .HasColumnName("aggregate_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AggregateKey)
                    .IsRequired()
                    .HasColumnName("aggregate_key")
                    .HasMaxLength(40);

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasColumnName("display_text")
                    .HasMaxLength(80);

                entity.Property(e => e.HasSelector)
                    .IsRequired()
                    .HasColumnName("has_selector")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.InputType)
                    .IsRequired()
                    .HasColumnName("input_type")
                    .HasMaxLength(40)
                    .HasDefaultValueSql("'checkboxes'::character varying");

                entity.Property(e => e.IsActivated)
                    .IsRequired()
                    .HasColumnName("is_activated")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");
            });


            modelBuilder.Entity<ResultAggregateField>(entity =>
            {
                entity.HasKey(e => e.AggregateFieldId)
                    .HasName("result_aggregate_field_pkey");

                entity.ToTable("result_aggregate_field", "facet");

                entity.Property(e => e.AggregateFieldId)
                    .HasColumnName("aggregate_field_id")
                    .HasDefaultValueSql("nextval('facet.result_aggregate_field_aggregate_field_id_seq'::regclass)");

                entity.Property(e => e.AggregateId).HasColumnName("aggregate_id");

                entity.Property(e => e.FieldTypeId)
                    .IsRequired()
                    .HasColumnName("field_type_id")
                    .HasMaxLength(40)
                    .HasDefaultValueSql("'single_item'::character varying");

                entity.Property(e => e.ResultFieldId).HasColumnName("result_field_id");

                entity.Property(e => e.SequenceId).HasColumnName("sequence_id");

                //entity.HasOne(d => d.Aggregate)
                //    .WithMany()
                //    .HasForeignKey(d => d.AggregateId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("result_aggregate_field_aggregate_id_fkey");

                entity.HasOne(d => d.FieldType)
                    .WithMany()
                    .HasForeignKey(d => d.FieldTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_aggregate_field_field_type_id_fkey");

                entity.HasOne(d => d.ResultField)
                    .WithMany()
                    .HasForeignKey(d => d.ResultFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_aggregate_field_result_field_id_fkey");
            });

            modelBuilder.Entity<ResultField>(entity =>
            {
                entity.ToTable("result_field", "facet");

                entity.Property(e => e.ResultFieldId)
                    .HasColumnName("result_field_id")
                    .HasDefaultValueSql("nextval('facet.result_field_result_field_id_seq'::regclass)");

                entity.Property(e => e.Activated).HasColumnName("activated");

                entity.Property(e => e.ColumnName)
                    .IsRequired()
                    .HasColumnName("column_name")
                    .HasMaxLength(80);

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasColumnName("display_text")
                    .HasMaxLength(80);

                entity.Property(e => e.FieldTypeId)
                    .IsRequired()
                    .HasColumnName("field_type_id")
                    .HasMaxLength(20);

                entity.Property(e => e.LinkLabel)
                    .HasColumnName("link_label")
                    .HasMaxLength(256);

                entity.Property(e => e.LinkUrl)
                    .HasColumnName("link_url")
                    .HasMaxLength(256);

                entity.Property(e => e.ResultFieldKey)
                    .IsRequired()
                    .HasColumnName("result_field_key")
                    .HasMaxLength(40);

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(80);

                entity.HasOne(d => d.FieldType)
                    .WithMany()
                    .HasForeignKey(d => d.FieldTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_field_field_type_id_fkey");
            });

            modelBuilder.Entity<ResultFieldType>(entity =>
            {
                entity.HasKey(e => e.FieldTypeId)
                    .HasName("result_field_type_pkey");

                entity.ToTable("result_field_type", "facet");

                entity.Property(e => e.FieldTypeId)
                    .HasColumnName("field_type_id")
                    .HasMaxLength(40)
                    .ValueGeneratedNever();

                entity.Property(e => e.IsAggregateField).HasColumnName("is_aggregate_field");

                entity.Property(e => e.IsItemField).HasColumnName("is_item_field");

                entity.Property(e => e.IsResultValue)
                    .IsRequired()
                    .HasColumnName("is_result_value")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsSortField).HasColumnName("is_sort_field");

                entity.Property(e => e.SqlFieldCompiler)
                    .IsRequired()
                    .HasColumnName("sql_field_compiler")
                    .HasMaxLength(40)
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.SqlTemplate)
                    .IsRequired()
                    .HasColumnName("sql_template")
                    .HasMaxLength(256)
                    .HasDefaultValueSql("'{0}'::character varying");
            });

            modelBuilder.Entity<ResultViewType>(entity =>
            {
                entity.HasKey(e => e.ViewTypeId)
                    .HasName("result_view_type_pkey");

                entity.ToTable("result_view_type", "facet");

                entity.Property(e => e.ViewTypeId)
                    .HasColumnName("view_type_id")
                    .HasMaxLength(40)
                    .ValueGeneratedNever();

                entity.Property(e => e.IsCachable)
                    .IsRequired()
                    .HasColumnName("is_cachable")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ViewName)
                    .IsRequired()
                    .HasColumnName("view_name")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<ViewState>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("view_state_pkey");

                entity.ToTable("view_state", "facet");

                entity.Property(e => e.Key)
                    .HasColumnName("view_state_key")
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnName("view_state_data");
            });

            modelBuilder.HasSequence<int>("facet_condition_clause_facet_source_table_id_seq");

            modelBuilder.HasSequence<int>("facet_table_facet_table_id_seq");

            modelBuilder.HasSequence<int>("graph_table_relation_relation_id_seq");

            modelBuilder.HasSequence<int>("graph_table_table_id_seq");

            modelBuilder.HasSequence<int>("result_aggregate_field_aggregate_field_id_seq");

            modelBuilder.HasSequence<int>("result_field_result_field_id_seq");
        }
    }
}
