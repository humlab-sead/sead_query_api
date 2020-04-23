using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SQT.Models
{
    public partial class TestFacetDbContext : DbContext
    {
        public TestFacetDbContext()
        {
        }

        public TestFacetDbContext(DbContextOptions<TestFacetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Facet> Facet { get; set; }
        public virtual DbSet<FacetChildren> FacetChildren { get; set; }
        public virtual DbSet<FacetClause> FacetClause { get; set; }
        public virtual DbSet<FacetDependency> FacetDependency { get; set; }
        public virtual DbSet<FacetGroup> FacetGroup { get; set; }
        public virtual DbSet<FacetTable> FacetTable { get; set; }
        public virtual DbSet<FacetType> FacetType { get; set; }
        public virtual DbSet<ReportSite> ReportSite { get; set; }
        public virtual DbSet<ResultAggregate> ResultAggregate { get; set; }
        public virtual DbSet<ResultAggregateField> ResultAggregateField { get; set; }
        public virtual DbSet<ResultField> ResultField { get; set; }
        public virtual DbSet<ResultFieldType> ResultFieldType { get; set; }
        public virtual DbSet<ResultViewType> ResultViewType { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<TableRelation> TableRelation { get; set; }
        public virtual DbSet<ViewAbundance> ViewAbundance { get; set; }
        public virtual DbSet<ViewAbundancesByTaxonAnalysisEntity> ViewAbundancesByTaxonAnalysisEntity { get; set; }
        public virtual DbSet<ViewSampleGroupReferences> ViewSampleGroupReferences { get; set; }
        public virtual DbSet<ViewSiteReferences> ViewSiteReferences { get; set; }
        public virtual DbSet<ViewState> ViewState { get; set; }
        public virtual DbSet<ViewTaxaBiblio> ViewTaxaBiblio { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new NotImplementedException("OptionsBuilder Is Not Configured");
                //optionsBuilder.UseNpgsql("Host=host;Database=database;Username=user;Password=password");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("tablefunc")
                .HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Facet>(entity =>
            {
                entity.ToTable("facet", "facet");

                entity.HasIndex(e => e.FacetCode)
                    .HasName("facet_facet_code_key")
                    .IsUnique();

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

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(256)
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.DisplayTitle)
                    .IsRequired()
                    .HasColumnName("display_title")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetCode)
                    .IsRequired()
                    .HasColumnName("facet_code")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetGroupId).HasColumnName("facet_group_id");

                entity.Property(e => e.FacetTypeId).HasColumnName("facet_type_id");

                entity.Property(e => e.IsApplicable).HasColumnName("is_applicable");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.SortExpr)
                    .IsRequired()
                    .HasColumnName("sort_expr")
                    .HasMaxLength(256);

                entity.HasOne(d => d.FacetGroup)
                    .WithMany(p => p.Facet)
                    .HasForeignKey(d => d.FacetGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_facet_group_id_fkey");

                entity.HasOne(d => d.FacetType)
                    .WithMany(p => p.Facet)
                    .HasForeignKey(d => d.FacetTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_facet_type_id_fkey");
            });

            modelBuilder.Entity<FacetChildren>(entity =>
            {
                entity.HasKey(e => new { e.FacetCode, e.ChildFacetCode })
                    .HasName("child_facet_pkey");

                entity.ToTable("facet_children", "facet");

                entity.Property(e => e.FacetCode)
                    .HasColumnName("facet_code")
                    .HasColumnType("character varying");

                entity.Property(e => e.ChildFacetCode)
                    .HasColumnName("child_facet_code")
                    .HasColumnType("character varying");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.HasOne(d => d.ChildFacetCodeNavigation)
                    .WithMany(p => p.FacetChildrenChildFacetCodeNavigation)
                    .HasPrincipalKey(p => p.FacetCode)
                    .HasForeignKey(d => d.ChildFacetCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_facet_children_child_facet_code_facet_code");

                entity.HasOne(d => d.FacetCodeNavigation)
                    .WithMany(p => p.FacetChildrenFacetCodeNavigation)
                    .HasPrincipalKey(p => p.FacetCode)
                    .HasForeignKey(d => d.FacetCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_facet_children_facet_code_facet_code");
            });

            modelBuilder.Entity<FacetClause>(entity =>
            {
                entity.ToTable("facet_clause", "facet");

                entity.Property(e => e.FacetClauseId).HasColumnName("facet_clause_id");

                entity.Property(e => e.Clause)
                    .HasColumnName("clause")
                    .HasMaxLength(512);

                entity.Property(e => e.FacetId).HasColumnName("facet_id");

                entity.HasOne(d => d.Facet)
                    .WithMany(p => p.FacetClause)
                    .HasForeignKey(d => d.FacetId)
                    .HasConstraintName("facet_clause_facet_id_fkey");
            });

            modelBuilder.Entity<FacetDependency>(entity =>
            {
                entity.ToTable("facet_dependency", "facet");

                entity.Property(e => e.FacetDependencyId).HasColumnName("facet_dependency_id");

                entity.Property(e => e.DependencyFacetId).HasColumnName("dependency_facet_id");

                entity.Property(e => e.FacetId).HasColumnName("facet_id");

                entity.HasOne(d => d.DependencyFacet)
                    .WithMany(p => p.FacetDependencyDependencyFacet)
                    .HasForeignKey(d => d.DependencyFacetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_dependency_dependency_facet_id_fkey");

                entity.HasOne(d => d.Facet)
                    .WithMany(p => p.FacetDependencyFacet)
                    .HasForeignKey(d => d.FacetId)
                    .HasConstraintName("facet_dependency_facet_id_fkey");
            });

            modelBuilder.Entity<FacetGroup>(entity =>
            {
                entity.ToTable("facet_group", "facet");

                entity.HasIndex(e => e.FacetGroupKey)
                    .HasName("facet_group_facet_group_key_key")
                    .IsUnique();

                entity.Property(e => e.FacetGroupId)
                    .HasColumnName("facet_group_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(256)
                    .HasDefaultValueSql("''::character varying");

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

                entity.HasIndex(e => e.Alias)
                    .HasName("facet_table_alias_key")
                    .IsUnique();

                entity.Property(e => e.FacetTableId).HasColumnName("facet_table_id");

                entity.Property(e => e.Alias)
                    .HasColumnName("alias")
                    .HasMaxLength(80);

                entity.Property(e => e.FacetId).HasColumnName("facet_id");

                entity.Property(e => e.SequenceId).HasColumnName("sequence_id");

                entity.Property(e => e.TableId).HasColumnName("table_id");

                entity.Property(e => e.UdfCallArguments)
                    .HasColumnName("udf_call_arguments")
                    .HasMaxLength(80);

                entity.HasOne(d => d.Facet)
                    .WithMany(p => p.FacetTable)
                    .HasForeignKey(d => d.FacetId)
                    .HasConstraintName("facet_table_facet_id_fkey");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.FacetTable)
                    .HasForeignKey(d => d.TableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("facet_table_table_id_fkey");
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

            modelBuilder.Entity<ReportSite>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("report_site", "facet");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NationalGridRef)
                    .HasColumnName("National grid ref")
                    .HasColumnType("character varying");

                entity.Property(e => e.Places).HasColumnName("places");

                entity.Property(e => e.PreservationStatusOrThreat)
                    .HasColumnName("Preservation status or threat")
                    .HasColumnType("character varying");

                entity.Property(e => e.SiteDescription).HasColumnName("Site description");

                entity.Property(e => e.SiteId).HasColumnName("site_id");

                entity.Property(e => e.SiteLat)
                    .HasColumnName("site_lat")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.SiteLng)
                    .HasColumnName("site_lng")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.SiteName)
                    .HasColumnName("Site name")
                    .HasMaxLength(60);
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

                entity.Property(e => e.AggregateFieldId).HasColumnName("aggregate_field_id");

                entity.Property(e => e.AggregateId).HasColumnName("aggregate_id");

                entity.Property(e => e.FieldTypeId)
                    .IsRequired()
                    .HasColumnName("field_type_id")
                    .HasMaxLength(40)
                    .HasDefaultValueSql("'single_item'::character varying");

                entity.Property(e => e.ResultFieldId).HasColumnName("result_field_id");

                entity.Property(e => e.SequenceId).HasColumnName("sequence_id");

                entity.HasOne(d => d.Aggregate)
                    .WithMany(p => p.ResultAggregateField)
                    .HasForeignKey(d => d.AggregateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_aggregate_field_aggregate_id_fkey");

                entity.HasOne(d => d.FieldType)
                    .WithMany(p => p.ResultAggregateField)
                    .HasForeignKey(d => d.FieldTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_aggregate_field_field_type_id_fkey");

                entity.HasOne(d => d.ResultField)
                    .WithMany(p => p.ResultAggregateField)
                    .HasForeignKey(d => d.ResultFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_aggregate_field_result_field_id_fkey");
            });

            modelBuilder.Entity<ResultField>(entity =>
            {
                entity.ToTable("result_field", "facet");

                entity.Property(e => e.ResultFieldId).HasColumnName("result_field_id");

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

                entity.Property(e => e.DataType)
                    .HasColumnName("datatype")
                    .HasMaxLength(40);

                entity.HasOne(d => d.FieldType)
                    .WithMany(p => p.ResultField)
                    .HasForeignKey(d => d.FieldTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("result_field_field_type_id_fkey");

                entity.HasOne(d => d.TableNameNavigation)
                    .WithMany(p => p.ResultField)
                    .HasPrincipalKey(p => p.TableOrUdfName)
                    .HasForeignKey(d => d.TableName)
                    .HasConstraintName("result_field_table_name_fkey");
            });

            modelBuilder.Entity<ResultFieldType>(entity =>
            {
                entity.HasKey(e => e.FieldTypeId)
                    .HasName("result_field_type_pkey");

                entity.ToTable("result_field_type", "facet");

                entity.Property(e => e.FieldTypeId)
                    .HasColumnName("field_type_id")
                    .HasMaxLength(40);

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
                    .HasMaxLength(40);

                entity.Property(e => e.IsCachable)
                    .IsRequired()
                    .HasColumnName("is_cachable")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ViewName)
                    .IsRequired()
                    .HasColumnName("view_name")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.ToTable("table", "facet");

                entity.HasIndex(e => e.TableOrUdfName)
                    .HasName("table_table_or_udf_name_key")
                    .IsUnique();

                entity.Property(e => e.TableId)
                    .HasColumnName("table_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsUdf).HasColumnName("is_udf");

                entity.Property(e => e.PrimaryKeyName)
                    .IsRequired()
                    .HasColumnName("primary_key_name")
                    .HasColumnType("character varying")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.SchemaName)
                    .IsRequired()
                    .HasColumnName("schema_name")
                    .HasColumnType("character varying")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.TableOrUdfName)
                    .IsRequired()
                    .HasColumnName("table_or_udf_name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TableRelation>(entity =>
            {
                entity.ToTable("table_relation", "facet");

                entity.HasIndex(e => e.SourceTableId)
                    .HasName("idx_table_relation_fk1");

                entity.HasIndex(e => e.TargetTableId)
                    .HasName("idx_table_relation_fk2");

                entity.Property(e => e.TableRelationId).HasColumnName("table_relation_id");

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
                    .WithMany(p => p.TableRelationSourceTable)
                    .HasForeignKey(d => d.SourceTableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("table_relation_source_table_id_fkey");

                entity.HasOne(d => d.TargetTable)
                    .WithMany(p => p.TableRelationTargetTable)
                    .HasForeignKey(d => d.TargetTableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("table_relation_target_table_id_fkey");
            });

            modelBuilder.Entity<ViewAbundance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("view_abundance", "facet");

                entity.Property(e => e.Abundance).HasColumnName("abundance");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.ElementsPartMod).HasColumnName("elements_part_mod");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");
            });

            modelBuilder.Entity<ViewAbundancesByTaxonAnalysisEntity>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("view_abundances_by_taxon_analysis_entity", "facet");

                entity.Property(e => e.AbundanceM111).HasColumnName("abundance_m111");

                entity.Property(e => e.AbundanceM3).HasColumnName("abundance_m3");

                entity.Property(e => e.AbundanceM8).HasColumnName("abundance_m8");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");
            });

            modelBuilder.Entity<ViewSampleGroupReferences>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("view_sample_group_references", "facet");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.BiblioLink).HasColumnName("biblio_link");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");
            });

            modelBuilder.Entity<ViewSiteReferences>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("view_site_references", "facet");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.BiblioLink).HasColumnName("biblio_link");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.SiteId).HasColumnName("site_id");
            });

            modelBuilder.Entity<ViewState>(entity =>
            {
                entity.HasKey(e => e.ViewStateKey)
                    .HasName("view_state_pkey");

                entity.ToTable("view_state", "facet");

                entity.Property(e => e.ViewStateKey)
                    .HasColumnName("view_state_key")
                    .HasMaxLength(80);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("clock_timestamp()");

                entity.Property(e => e.ViewStateData)
                    .IsRequired()
                    .HasColumnName("view_state_data");
            });

            modelBuilder.Entity<ViewTaxaBiblio>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("view_taxa_biblio", "facet");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
