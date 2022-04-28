using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class FacetContext : DbContext, IFacetContext
    {
        public ITypedQueryProxy TypedQueryProxy { get; set; }
        public IDynamicQueryProxy DynamicQueryProxy { get; set; }

        public FacetContext(DbContextOptions options) : base(options)
        {
            TypedQueryProxy = new DatabaseQueryProxy(this);
            DynamicQueryProxy = new DatabaseQueryProxy(this);
        }

        public virtual DbSet<ResultSpecification> ResultSpecifications { get; set; }
        public virtual DbSet<ResultSpecificationField> ResultSpecificationFields { get; set; }
        public virtual DbSet<ResultField> ResultFields { get; set; }
        public virtual DbSet<ResultFieldType> ResultFieldTypes { get; set; }
        public virtual DbSet<Facet> Facets { get; set; }
        public virtual DbSet<TableRelation> TableRelations { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<ViewState> ViewStates { get; set; }
        public virtual DbSet<ResultViewType> ViewTypes { get; set; }
        public virtual DbSet<FacetType> FacetTypes { get; set; }
        public virtual DbSet<FacetGroup> FacetGroups { get; set; }
        public virtual DbSet<FacetClause> FacetClauses { get; set; }
        public virtual DbSet<FacetTable> FacetTables { get; set; }
        public virtual DbSet<FacetChild> FacetChildren { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Table>(entity =>
            {
                entity.ToTable("table", "facet").HasKey(b => b.TableId);
                entity.Property(b => b.TableId).HasColumnName("table_id").IsRequired();
                entity.Property(b => b.TableOrUdfName).HasColumnName("table_or_udf_name").IsRequired();
                entity.Property(b => b.IsUdf).HasColumnName("is_udf").IsRequired();
                entity.Property(b => b.PrimaryKeyName).HasColumnName("primary_key_name").IsRequired();
            });

            builder.Entity<TableRelation>(entity =>
            {
                entity.ToTable("table_relation", "facet").HasKey(b => b.TableRelationId);
                entity.Property(b => b.TableRelationId).HasColumnName("table_relation_id").IsRequired();
                entity.Property(b => b.SourceTableId).HasColumnName("source_table_id").IsRequired();
                entity.Property(b => b.TargetTableId).HasColumnName("target_table_id").IsRequired();
                entity.Property(b => b.Weight).HasColumnName("weight").IsRequired();
                entity.Property(b => b.SourceColumName).HasColumnName("source_column_name").IsRequired();
                entity.Property(b => b.TargetColumnName).HasColumnName("target_column_name").IsRequired();
                // FIXME: Add alter table table_relation add column extra_constraint varchar null
                // entity.Property(b => b.ExtraConstraint).HasColumnName("extra_constraint");
                entity.HasOne<Table>(x => x.SourceTable).WithMany().HasForeignKey(p => p.SourceTableId);
                entity.HasOne<Table>(x => x.TargetTable).WithMany().HasForeignKey(p => p.TargetTableId);
            });

            builder.Entity<FacetType>(entity =>
            {
                entity.ToTable("facet_type", "facet").HasKey(b => b.FacetTypeId);
                entity.Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
                entity.Property(b => b.FacetTypeName).HasColumnName("facet_type_name").IsRequired();
                entity.Property(b => b.ReloadAsTarget).HasColumnName("reload_as_target").IsRequired();
            });

            builder.Entity<FacetChild>(entity =>
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

                entity.HasOne(d => d.Child).WithMany()
                    .HasPrincipalKey(d => d.FacetCode)
                    .HasForeignKey(d => d.ChildFacetCode);

                entity.HasOne(d => d.Facet)
                    .WithMany(p => p.Children)
                    .HasPrincipalKey(p => p.FacetCode)
                    .HasForeignKey(d => d.FacetCode);

            });

            builder.Entity<Facet>(entity =>
            {
                entity.ToTable("facet", "facet").HasKey(b => b.FacetId);
                entity.HasAlternateKey(b => b.FacetCode);
                entity.Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
                entity.Property(b => b.FacetCode).HasColumnName("facet_code").IsRequired();
                entity.Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
                entity.Property(b => b.Description).HasColumnName("description").IsRequired();
                entity.Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
                entity.Property(b => b.FacetTypeId).HasColumnName("facet_type_id").IsRequired();
                entity.Property(b => b.CategoryIdExpr).HasColumnName("category_id_expr").IsRequired();
                entity.Property(b => b.CategoryNameExpr).HasColumnName("category_name_expr").IsRequired();
                entity.Property(b => b.SortExpr).HasColumnName("sort_expr").IsRequired();
                entity.Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
                entity.Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();
                entity.Property(b => b.AggregateType).HasColumnName("aggregate_type").IsRequired();
                entity.Property(b => b.AggregateTitle).HasColumnName("aggregate_title").IsRequired();
                entity.Property(b => b.AggregateFacetId).HasColumnName("aggregate_facet_id").IsRequired();
                entity.HasOne<FacetType>(x => x.FacetType).WithMany().HasForeignKey(p => p.FacetTypeId);
                entity.HasOne<FacetGroup>(x => x.FacetGroup).WithMany().HasForeignKey(p => p.FacetGroupId);
            });

            builder.Entity<FacetGroup>(entity =>
            {
                entity.ToTable("facet_group", "facet").HasKey(b => b.FacetGroupId);
                entity.Property(b => b.FacetGroupId).HasColumnName("facet_group_id").IsRequired();
                entity.Property(b => b.FacetGroupKey).HasColumnName("facet_group_key").IsRequired();
                entity.Property(b => b.DisplayTitle).HasColumnName("display_title").IsRequired();
                entity.Property(b => b.Description).HasColumnName("description").IsRequired();
                entity.Property(b => b.IsApplicable).HasColumnName("is_applicable").IsRequired();
                entity.Property(b => b.IsDefault).HasColumnName("is_default").IsRequired();
            });

            builder.Entity<FacetClause>(entity =>
            {
                entity.ToTable("facet_clause", "facet").HasKey(b => b.FacetClauseId);
                entity.Property(b => b.FacetClauseId).HasColumnName("facet_clause_id").IsRequired();
                entity.Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
                entity.Property(b => b.Clause).HasColumnName("clause").IsRequired();
                entity.Property(b => b.EnforceConstraint).HasColumnName("enforce_constraint").IsRequired();
                entity.HasOne<Facet>(x => x.Facet).WithMany(x => x.Clauses).HasForeignKey(x => x.FacetId);
            });

            builder.Entity<FacetTable>(entity =>
            {
                entity.ToTable("facet_table", "facet").HasKey(b => b.FacetTableId);
                entity.Property(b => b.FacetTableId).HasColumnName("facet_table_id").IsRequired();
                entity.Property(b => b.FacetId).HasColumnName("facet_id").IsRequired();
                entity.Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
                entity.Property(b => b.TableId).HasColumnName("table_id").IsRequired();
                entity.Property(b => b.UdfCallArguments).HasColumnName("udf_call_arguments");
                entity.Property(b => b.Alias).HasColumnName("alias");
                entity.HasOne<Facet>(x => x.Facet).WithMany(x => x.Tables).HasForeignKey(x => x.FacetId);
                entity.HasOne<Table>(x => x.Table).WithMany().HasForeignKey(p => p.TableId);
            });

            builder.Entity<ResultViewType>(entity =>
            {
                entity.ToTable("result_view_type", "facet").HasKey(b => b.ViewTypeId);
                entity.Property(b => b.ViewTypeId).HasColumnName("view_type_id").IsRequired();
                entity.Property(b => b.ViewName).HasColumnName("view_name").IsRequired();
                entity.Property(b => b.SpecificationKey).HasColumnName("specification_key").IsRequired();
                entity.Property(b => b.SqlCompiler).HasColumnName("sql_compiler").IsRequired();
                entity.Property(b => b.ResultFacetCode).HasColumnName("result_facet_code").IsRequired();
                entity.Property(b => b.IsCachable).HasColumnName("is_cachable").IsRequired();
            });

            builder.Entity<ResultField>(entity =>
            {
                entity.ToTable("result_field", "facet").HasKey(b => b.ResultFieldId);
                entity.Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
                entity.Property(b => b.ResultFieldKey).HasColumnName("result_field_key").IsRequired();
                entity.Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();
                entity.Property(b => b.TableName).HasColumnName("table_name"); //.IsRequired();
                entity.Property(b => b.ColumnName).HasColumnName("column_name").IsRequired();
                entity.Property(b => b.DisplayText).HasColumnName("display_text").IsRequired();
                entity.Property(b => b.Activated).HasColumnName("activated").IsRequired();
                entity.Property(b => b.DataType).HasColumnName("datatype");
                entity.HasOne(x => x.FieldType).WithMany().HasForeignKey(p => p.FieldTypeId);
            });

            builder.Entity<ResultFieldType>(entity =>
            {
                entity.ToTable("result_field_type", "facet").HasKey(b => b.FieldTypeId);
                entity.Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();
                entity.Property(b => b.IsResultValue).HasColumnName("is_result_value").IsRequired();
                entity.Property(b => b.SqlFieldCompiler).HasColumnName("sql_field_compiler").IsRequired();
                entity.Property(b => b.IsSortField).HasColumnName("is_sort_field").IsRequired();
                entity.Property(b => b.IsAggregateField).HasColumnName("is_aggregate_field").IsRequired();
                entity.Property(b => b.IsItemField).HasColumnName("is_item_field").IsRequired();
                entity.Property(b => b.SqlTemplate).HasColumnName("sql_template").IsRequired();
            });

            builder.Entity<ResultSpecification>(entity =>
            {
                entity.ToTable("result_specification", "facet").HasKey(b => b.SpecificationId);
                entity.Property(b => b.SpecificationId).HasColumnName("specification_id").IsRequired();
                entity.Property(b => b.SpecificationKey).HasColumnName("specification_key").IsRequired();
                entity.Property(b => b.DisplayText).HasColumnName("display_text").IsRequired();
                entity.Property(b => b.IsActivated).HasColumnName("is_activated").IsRequired();
                entity.HasMany<ResultSpecificationField>(x => x.Fields).WithOne(x => x.Specification).HasForeignKey(p => p.SpecificationId);
            });

            builder.Entity<ResultSpecificationField>(entity =>
            {
                entity.ToTable("result_specification_field", "facet").HasKey(b => b.SpecificationFieldId);
                entity.Property(b => b.SpecificationFieldId).HasColumnName("specification_field_id").IsRequired();
                entity.Property(b => b.SequenceId).HasColumnName("sequence_id").IsRequired();
                entity.Property(b => b.SpecificationId).HasColumnName("specification_id").IsRequired();
                entity.Property(b => b.ResultFieldId).HasColumnName("result_field_id").IsRequired();
                entity.Property(b => b.FieldTypeId).HasColumnName("field_type_id").IsRequired();
                entity.HasOne<ResultFieldType>(x => x.FieldType).WithMany().HasForeignKey(p => p.FieldTypeId);
                entity.HasOne<ResultField>(x => x.ResultField).WithMany().HasForeignKey(p => p.ResultFieldId);
            });

            builder.Entity<ViewState>(entity =>
            {
                entity.ToTable("view_state", "facet").HasKey(b => b.Key);
                entity.Property(b => b.Key).HasColumnName("view_state_key").IsRequired();
                entity.Property(b => b.Data).HasColumnName("view_state_data").IsRequired();
            });

            base.OnModelCreating(builder);

        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
