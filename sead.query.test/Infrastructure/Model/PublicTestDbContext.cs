using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SQT.Infrastructure.Model
{
    public partial class PublicTestDbContext : DbContext
    {
        public PublicTestDbContext()
        {
        }

        public PublicTestDbContext(DbContextOptions<PublicTestDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EcocodeDatingGeojson22Count> EcocodeDatingGeojson22Count { get; set; }
        public virtual DbSet<EcocodeDatingGeojson22Sum> EcocodeDatingGeojson22Sum { get; set; }
        public virtual DbSet<TblAbundanceElements> TblAbundanceElements { get; set; }
        public virtual DbSet<TblAbundanceIdentLevels> TblAbundanceIdentLevels { get; set; }
        public virtual DbSet<TblAbundanceModifications> TblAbundanceModifications { get; set; }
        public virtual DbSet<TblAbundances> TblAbundances { get; set; }
        public virtual DbSet<TblActivityTypes> TblActivityTypes { get; set; }
        public virtual DbSet<TblAgeTypes> TblAgeTypes { get; set; }
        public virtual DbSet<TblAggregateDatasets> TblAggregateDatasets { get; set; }
        public virtual DbSet<TblAggregateOrderTypes> TblAggregateOrderTypes { get; set; }
        public virtual DbSet<TblAggregateSampleAges> TblAggregateSampleAges { get; set; }
        public virtual DbSet<TblAggregateSamples> TblAggregateSamples { get; set; }
        public virtual DbSet<TblAltRefTypes> TblAltRefTypes { get; set; }
        public virtual DbSet<TblAnalysisEntities> TblAnalysisEntities { get; set; }
        public virtual DbSet<TblAnalysisEntityAges> TblAnalysisEntityAges { get; set; }
        public virtual DbSet<TblAnalysisEntityDimensions> TblAnalysisEntityDimensions { get; set; }
        public virtual DbSet<TblAnalysisEntityPrepMethods> TblAnalysisEntityPrepMethods { get; set; }
        public virtual DbSet<TblBiblio> TblBiblio { get; set; }
        public virtual DbSet<TblCeramics> TblCeramics { get; set; }
        public virtual DbSet<TblCeramicsLookup> TblCeramicsLookup { get; set; }
        public virtual DbSet<TblCeramicsMeasurements> TblCeramicsMeasurements { get; set; }
        public virtual DbSet<TblChronControlTypes> TblChronControlTypes { get; set; }
        public virtual DbSet<TblChronControls> TblChronControls { get; set; }
        public virtual DbSet<TblChronologies> TblChronologies { get; set; }
        public virtual DbSet<TblColours> TblColours { get; set; }
        public virtual DbSet<TblContactTypes> TblContactTypes { get; set; }
        public virtual DbSet<TblContacts> TblContacts { get; set; }
        public virtual DbSet<TblCoordinateMethodDimensions> TblCoordinateMethodDimensions { get; set; }
        public virtual DbSet<TblDataTypeGroups> TblDataTypeGroups { get; set; }
        public virtual DbSet<TblDataTypes> TblDataTypes { get; set; }
        public virtual DbSet<TblDatasetContacts> TblDatasetContacts { get; set; }
        public virtual DbSet<TblDatasetMasters> TblDatasetMasters { get; set; }
        public virtual DbSet<TblDatasetMethods> TblDatasetMethods { get; set; }
        public virtual DbSet<TblDatasetSubmissionTypes> TblDatasetSubmissionTypes { get; set; }
        public virtual DbSet<TblDatasetSubmissions> TblDatasetSubmissions { get; set; }
        public virtual DbSet<TblDatasets> TblDatasets { get; set; }
        public virtual DbSet<TblDatingLabs> TblDatingLabs { get; set; }
        public virtual DbSet<TblDatingMaterial> TblDatingMaterial { get; set; }
        public virtual DbSet<TblDatingUncertainty> TblDatingUncertainty { get; set; }
        public virtual DbSet<TblDendro> TblDendro { get; set; }
        public virtual DbSet<TblDendroDateNotes> TblDendroDateNotes { get; set; }
        public virtual DbSet<TblDendroDates> TblDendroDates { get; set; }
        public virtual DbSet<TblDendroLookup> TblDendroLookup { get; set; }
        public virtual DbSet<TblDendroMeasurements> TblDendroMeasurements { get; set; }
        public virtual DbSet<TblDimensions> TblDimensions { get; set; }
        public virtual DbSet<TblEcocodeDefinitions> TblEcocodeDefinitions { get; set; }
        public virtual DbSet<TblEcocodeGroups> TblEcocodeGroups { get; set; }
        public virtual DbSet<TblEcocodeSystems> TblEcocodeSystems { get; set; }
        public virtual DbSet<TblEcocodes> TblEcocodes { get; set; }
        public virtual DbSet<TblErrorUncertainties> TblErrorUncertainties { get; set; }
        public virtual DbSet<TblFeatureTypes> TblFeatureTypes { get; set; }
        public virtual DbSet<TblFeatures> TblFeatures { get; set; }
        public virtual DbSet<TblGeochronRefs> TblGeochronRefs { get; set; }
        public virtual DbSet<TblGeochronology> TblGeochronology { get; set; }
        public virtual DbSet<TblHorizons> TblHorizons { get; set; }
        public virtual DbSet<TblIdentificationLevels> TblIdentificationLevels { get; set; }
        public virtual DbSet<TblImageTypes> TblImageTypes { get; set; }
        public virtual DbSet<TblImportedTaxaReplacements> TblImportedTaxaReplacements { get; set; }
        public virtual DbSet<TblIsotopeMeasurements> TblIsotopeMeasurements { get; set; }
        public virtual DbSet<TblIsotopeStandards> TblIsotopeStandards { get; set; }
        public virtual DbSet<TblIsotopeTypes> TblIsotopeTypes { get; set; }
        public virtual DbSet<TblIsotopeValueSpecifiers> TblIsotopeValueSpecifiers { get; set; }
        public virtual DbSet<TblIsotopes> TblIsotopes { get; set; }
        public virtual DbSet<TblLanguages> TblLanguages { get; set; }
        public virtual DbSet<TblLithology> TblLithology { get; set; }
        public virtual DbSet<TblLocationTypes> TblLocationTypes { get; set; }
        public virtual DbSet<TblLocations> TblLocations { get; set; }
        public virtual DbSet<TblMcrNames> TblMcrNames { get; set; }
        public virtual DbSet<TblMcrSummaryData> TblMcrSummaryData { get; set; }
        public virtual DbSet<TblMcrdataBirmbeetledat> TblMcrdataBirmbeetledat { get; set; }
        public virtual DbSet<TblMeasuredValueDimensions> TblMeasuredValueDimensions { get; set; }
        public virtual DbSet<TblMeasuredValues> TblMeasuredValues { get; set; }
        public virtual DbSet<TblMethodGroups> TblMethodGroups { get; set; }
        public virtual DbSet<TblMethods> TblMethods { get; set; }
        public virtual DbSet<TblModificationTypes> TblModificationTypes { get; set; }
        public virtual DbSet<TblPhysicalSampleFeatures> TblPhysicalSampleFeatures { get; set; }
        public virtual DbSet<TblPhysicalSamples> TblPhysicalSamples { get; set; }
        public virtual DbSet<TblProjectStages> TblProjectStages { get; set; }
        public virtual DbSet<TblProjectTypes> TblProjectTypes { get; set; }
        public virtual DbSet<TblProjects> TblProjects { get; set; }
        public virtual DbSet<TblRdb> TblRdb { get; set; }
        public virtual DbSet<TblRdbCodes> TblRdbCodes { get; set; }
        public virtual DbSet<TblRdbSystems> TblRdbSystems { get; set; }
        public virtual DbSet<TblRecordTypes> TblRecordTypes { get; set; }
        public virtual DbSet<TblRelativeAgeRefs> TblRelativeAgeRefs { get; set; }
        public virtual DbSet<TblRelativeAgeTypes> TblRelativeAgeTypes { get; set; }
        public virtual DbSet<TblRelativeAges> TblRelativeAges { get; set; }
        public virtual DbSet<TblRelativeDates> TblRelativeDates { get; set; }
        public virtual DbSet<TblSampleAltRefs> TblSampleAltRefs { get; set; }
        public virtual DbSet<TblSampleColours> TblSampleColours { get; set; }
        public virtual DbSet<TblSampleCoordinates> TblSampleCoordinates { get; set; }
        public virtual DbSet<TblSampleDescriptionSampleGroupContexts> TblSampleDescriptionSampleGroupContexts { get; set; }
        public virtual DbSet<TblSampleDescriptionTypes> TblSampleDescriptionTypes { get; set; }
        public virtual DbSet<TblSampleDescriptions> TblSampleDescriptions { get; set; }
        public virtual DbSet<TblSampleDimensions> TblSampleDimensions { get; set; }
        public virtual DbSet<TblSampleGroupCoordinates> TblSampleGroupCoordinates { get; set; }
        public virtual DbSet<TblSampleGroupDescriptionTypeSamplingContexts> TblSampleGroupDescriptionTypeSamplingContexts { get; set; }
        public virtual DbSet<TblSampleGroupDescriptionTypes> TblSampleGroupDescriptionTypes { get; set; }
        public virtual DbSet<TblSampleGroupDescriptions> TblSampleGroupDescriptions { get; set; }
        public virtual DbSet<TblSampleGroupDimensions> TblSampleGroupDimensions { get; set; }
        public virtual DbSet<TblSampleGroupImages> TblSampleGroupImages { get; set; }
        public virtual DbSet<TblSampleGroupNotes> TblSampleGroupNotes { get; set; }
        public virtual DbSet<TblSampleGroupReferences> TblSampleGroupReferences { get; set; }
        public virtual DbSet<TblSampleGroupSamplingContexts> TblSampleGroupSamplingContexts { get; set; }
        public virtual DbSet<TblSampleGroups> TblSampleGroups { get; set; }
        public virtual DbSet<TblSampleHorizons> TblSampleHorizons { get; set; }
        public virtual DbSet<TblSampleImages> TblSampleImages { get; set; }
        public virtual DbSet<TblSampleLocationTypeSamplingContexts> TblSampleLocationTypeSamplingContexts { get; set; }
        public virtual DbSet<TblSampleLocationTypes> TblSampleLocationTypes { get; set; }
        public virtual DbSet<TblSampleLocations> TblSampleLocations { get; set; }
        public virtual DbSet<TblSampleNotes> TblSampleNotes { get; set; }
        public virtual DbSet<TblSampleTypes> TblSampleTypes { get; set; }
        public virtual DbSet<TblSeasonOrQualifier> TblSeasonOrQualifier { get; set; }
        public virtual DbSet<TblSeasonTypes> TblSeasonTypes { get; set; }
        public virtual DbSet<TblSeasons> TblSeasons { get; set; }
        public virtual DbSet<TblSiteImages> TblSiteImages { get; set; }
        public virtual DbSet<TblSiteLocations> TblSiteLocations { get; set; }
        public virtual DbSet<TblSiteNatgridrefs> TblSiteNatgridrefs { get; set; }
        public virtual DbSet<TblSiteOtherRecords> TblSiteOtherRecords { get; set; }
        public virtual DbSet<TblSitePreservationStatus> TblSitePreservationStatus { get; set; }
        public virtual DbSet<TblSiteReferences> TblSiteReferences { get; set; }
        public virtual DbSet<TblSites> TblSites { get; set; }
        public virtual DbSet<TblSpeciesAssociationTypes> TblSpeciesAssociationTypes { get; set; }
        public virtual DbSet<TblSpeciesAssociations> TblSpeciesAssociations { get; set; }
        public virtual DbSet<TblTaxaCommonNames> TblTaxaCommonNames { get; set; }
        public virtual DbSet<TblTaxaImages> TblTaxaImages { get; set; }
        public virtual DbSet<TblTaxaMeasuredAttributes> TblTaxaMeasuredAttributes { get; set; }
        public virtual DbSet<TblTaxaReferenceSpecimens> TblTaxaReferenceSpecimens { get; set; }
        public virtual DbSet<TblTaxaSeasonality> TblTaxaSeasonality { get; set; }
        public virtual DbSet<TblTaxaSynonyms> TblTaxaSynonyms { get; set; }
        public virtual DbSet<TblTaxaTreeAuthors> TblTaxaTreeAuthors { get; set; }
        public virtual DbSet<TblTaxaTreeFamilies> TblTaxaTreeFamilies { get; set; }
        public virtual DbSet<TblTaxaTreeGenera> TblTaxaTreeGenera { get; set; }
        public virtual DbSet<TblTaxaTreeMaster> TblTaxaTreeMaster { get; set; }
        public virtual DbSet<TblTaxaTreeOrders> TblTaxaTreeOrders { get; set; }
        public virtual DbSet<TblTaxonomicOrder> TblTaxonomicOrder { get; set; }
        public virtual DbSet<TblTaxonomicOrderBiblio> TblTaxonomicOrderBiblio { get; set; }
        public virtual DbSet<TblTaxonomicOrderSystems> TblTaxonomicOrderSystems { get; set; }
        public virtual DbSet<TblTaxonomyNotes> TblTaxonomyNotes { get; set; }
        public virtual DbSet<TblTemperatures> TblTemperatures { get; set; }
        public virtual DbSet<TblTephraDates> TblTephraDates { get; set; }
        public virtual DbSet<TblTephraRefs> TblTephraRefs { get; set; }
        public virtual DbSet<TblTephras> TblTephras { get; set; }
        public virtual DbSet<TblTextBiology> TblTextBiology { get; set; }
        public virtual DbSet<TblTextDistribution> TblTextDistribution { get; set; }
        public virtual DbSet<TblTextIdentificationKeys> TblTextIdentificationKeys { get; set; }
        public virtual DbSet<TblUnits> TblUnits { get; set; }
        public virtual DbSet<TblUpdatesLog> TblUpdatesLog { get; set; }
        public virtual DbSet<TblYearsTypes> TblYearsTypes { get; set; }
        public virtual DbSet<ViewTaxaAlphabetically> ViewTaxaAlphabetically { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EcocodeDatingGeojson22Count>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ecocode_dating_geojson_2_2_count");

                entity.Property(e => e.EcocodeJson)
                    .HasColumnName("ecocode_json")
                    .HasColumnType("json");
            });

            modelBuilder.Entity<EcocodeDatingGeojson22Sum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ecocode_dating_geojson_2_2_sum");

                entity.Property(e => e.EcocodeJson)
                    .HasColumnName("ecocode_json")
                    .HasColumnType("json");
            });

            modelBuilder.Entity<TblAbundanceElements>(entity =>
            {
                entity.HasKey(e => e.AbundanceElementId)
                    .HasName("pk_abundance_elements");

                entity.ToTable("tbl_abundance_elements");

                entity.Property(e => e.AbundanceElementId).HasColumnName("abundance_element_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ElementDescription)
                    .HasColumnName("element_description")
                    .HasComment("explanation of short name, e.g. minimum number of individuals, base of seed grain, covering of leaf or flower bud");

                entity.Property(e => e.ElementName)
                    .IsRequired()
                    .HasColumnName("element_name")
                    .HasMaxLength(100)
                    .HasComment("short name for element, e.g. mni, seed, leaf");

                entity.Property(e => e.RecordTypeId)
                    .HasColumnName("record_type_id")
                    .HasComment("used to restrict list of available elements according to record type. enables specific use of single term for multiple proxies whilst avoiding confusion, e.g. mni insects, mni seeds");

                entity.HasOne(d => d.RecordType)
                    .WithMany(p => p.TblAbundanceElements)
                    .HasForeignKey(d => d.RecordTypeId)
                    .HasConstraintName("fk_abundance_elements_record_type_id");
            });

            modelBuilder.Entity<TblAbundanceIdentLevels>(entity =>
            {
                entity.HasKey(e => e.AbundanceIdentLevelId)
                    .HasName("pk_abundance_ident_levels");

                entity.ToTable("tbl_abundance_ident_levels");

                entity.Property(e => e.AbundanceIdentLevelId).HasColumnName("abundance_ident_level_id");

                entity.Property(e => e.AbundanceId).HasColumnName("abundance_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IdentificationLevelId).HasColumnName("identification_level_id");

                entity.HasOne(d => d.Abundance)
                    .WithMany(p => p.TblAbundanceIdentLevels)
                    .HasForeignKey(d => d.AbundanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_abundance_ident_levels_abundance_id");

                entity.HasOne(d => d.IdentificationLevel)
                    .WithMany(p => p.TblAbundanceIdentLevels)
                    .HasForeignKey(d => d.IdentificationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_abundance_ident_levels_identification_level_id");
            });

            modelBuilder.Entity<TblAbundanceModifications>(entity =>
            {
                entity.HasKey(e => e.AbundanceModificationId)
                    .HasName("pk_abundance_modifications");

                entity.ToTable("tbl_abundance_modifications");

                entity.Property(e => e.AbundanceModificationId).HasColumnName("abundance_modification_id");

                entity.Property(e => e.AbundanceId).HasColumnName("abundance_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ModificationTypeId).HasColumnName("modification_type_id");

                entity.HasOne(d => d.Abundance)
                    .WithMany(p => p.TblAbundanceModifications)
                    .HasForeignKey(d => d.AbundanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_abundance_modifications_abundance_id");

                entity.HasOne(d => d.ModificationType)
                    .WithMany(p => p.TblAbundanceModifications)
                    .HasForeignKey(d => d.ModificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_abundance_modifications_modification_type_id");
            });

            modelBuilder.Entity<TblAbundances>(entity =>
            {
                entity.HasKey(e => e.AbundanceId)
                    .HasName("pk_abundances");

                entity.ToTable("tbl_abundances");

                entity.HasComment("20120503pib deleted column \"abundance_modification_id\" as appeared superfluous with \"abundance_id\" in tbl_adbundance_modifications");

                entity.Property(e => e.AbundanceId).HasColumnName("abundance_id");

                entity.Property(e => e.Abundance)
                    .HasColumnName("abundance")
                    .HasComment("usually count value (abundance) but can be presence (1) or catagorical or relative scale, as defined by tbl_data_types through tbl_datasets");

                entity.Property(e => e.AbundanceElementId)
                    .HasColumnName("abundance_element_id")
                    .HasComment("allows recording of different parts for single taxon, e.g. leaf, seed, mni etc.");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.AbundanceElement)
                    .WithMany(p => p.TblAbundances)
                    .HasForeignKey(d => d.AbundanceElementId)
                    .HasConstraintName("tbl_abundances_abundance_element_id_fkey");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblAbundances)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_abundances_analysis_entity_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblAbundances)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_abundances_taxon_id_fkey");
            });

            modelBuilder.Entity<TblActivityTypes>(entity =>
            {
                entity.HasKey(e => e.ActivityTypeId)
                    .HasName("pk_activity_types");

                entity.ToTable("tbl_activity_types");

                entity.Property(e => e.ActivityTypeId).HasColumnName("activity_type_id");

                entity.Property(e => e.ActivityType)
                    .HasColumnName("activity_type")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<TblAgeTypes>(entity =>
            {
                entity.HasKey(e => e.AgeTypeId)
                    .HasName("tbl_age_types_pkey");

                entity.ToTable("tbl_age_types");

                entity.Property(e => e.AgeTypeId).HasColumnName("age_type_id");

                entity.Property(e => e.AgeType)
                    .IsRequired()
                    .HasColumnName("age_type")
                    .HasMaxLength(150);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<TblAggregateDatasets>(entity =>
            {
                entity.HasKey(e => e.AggregateDatasetId)
                    .HasName("pk_aggregate_datasets");

                entity.ToTable("tbl_aggregate_datasets");

                entity.Property(e => e.AggregateDatasetId).HasColumnName("aggregate_dataset_id");

                entity.Property(e => e.AggregateDatasetName)
                    .HasColumnName("aggregate_dataset_name")
                    .HasMaxLength(255)
                    .HasComment("name of aggregated dataset");

                entity.Property(e => e.AggregateOrderTypeId).HasColumnName("aggregate_order_type_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("Notes explaining the purpose of the aggregated set of analysis entities");

                entity.HasOne(d => d.AggregateOrderType)
                    .WithMany(p => p.TblAggregateDatasets)
                    .HasForeignKey(d => d.AggregateOrderTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_aggregate_datasets_aggregate_order_type_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblAggregateDatasets)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("fk_aggregate_datasets_biblio_id");
            });

            modelBuilder.Entity<TblAggregateOrderTypes>(entity =>
            {
                entity.HasKey(e => e.AggregateOrderTypeId)
                    .HasName("pk_aggregate_order_types");

                entity.ToTable("tbl_aggregate_order_types");

                entity.HasComment("20120504pib: drop this? or replace with alternative?");

                entity.Property(e => e.AggregateOrderTypeId).HasColumnName("aggregate_order_type_id");

                entity.Property(e => e.AggregateOrderType)
                    .IsRequired()
                    .HasColumnName("aggregate_order_type")
                    .HasMaxLength(60)
                    .HasComment("aggregate order name, e.g. site name, age, sample depth, altitude");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("explanation of ordering system");
            });

            modelBuilder.Entity<TblAggregateSampleAges>(entity =>
            {
                entity.HasKey(e => e.AggregateSampleAgeId)
                    .HasName("pk_aggregate_sample_ages");

                entity.ToTable("tbl_aggregate_sample_ages");

                entity.Property(e => e.AggregateSampleAgeId).HasColumnName("aggregate_sample_age_id");

                entity.Property(e => e.AggregateDatasetId).HasColumnName("aggregate_dataset_id");

                entity.Property(e => e.AnalysisEntityAgeId).HasColumnName("analysis_entity_age_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.AggregateDataset)
                    .WithMany(p => p.TblAggregateSampleAges)
                    .HasForeignKey(d => d.AggregateDatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_aggregate_sample_ages_aggregate_dataset_id");

                entity.HasOne(d => d.AnalysisEntityAge)
                    .WithMany(p => p.TblAggregateSampleAges)
                    .HasForeignKey(d => d.AnalysisEntityAgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_aggregate_sample_ages_analysis_entity_age_id");
            });

            modelBuilder.Entity<TblAggregateSamples>(entity =>
            {
                entity.HasKey(e => e.AggregateSampleId)
                    .HasName("pk_aggregate_samples");

                entity.ToTable("tbl_aggregate_samples");

                entity.HasComment("20120504pib: can we drop aggregate sample name? seems excessive and unnecessary sample names can be traced.");

                entity.Property(e => e.AggregateSampleId).HasColumnName("aggregate_sample_id");

                entity.Property(e => e.AggregateDatasetId).HasColumnName("aggregate_dataset_id");

                entity.Property(e => e.AggregateSampleName)
                    .HasColumnName("aggregate_sample_name")
                    .HasMaxLength(50)
                    .HasComment("optional name for aggregated entity.");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.AggregateDataset)
                    .WithMany(p => p.TblAggregateSamples)
                    .HasForeignKey(d => d.AggregateDatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_aggregate_samples_aggregate_dataset_id");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblAggregateSamples)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_aggragate_samples_analysis_entity_id");
            });

            modelBuilder.Entity<TblAltRefTypes>(entity =>
            {
                entity.HasKey(e => e.AltRefTypeId)
                    .HasName("pk_alt_ref_types");

                entity.ToTable("tbl_alt_ref_types");

                entity.Property(e => e.AltRefTypeId).HasColumnName("alt_ref_type_id");

                entity.Property(e => e.AltRefType)
                    .IsRequired()
                    .HasColumnName("alt_ref_type")
                    .HasMaxLength(50);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<TblAnalysisEntities>(entity =>
            {
                entity.HasKey(e => e.AnalysisEntityId)
                    .HasName("pk_analysis_entities");

                entity.ToTable("tbl_analysis_entities");

                entity.HasComment(@"20120503pib deleted column preparation_method_id, but may need to cater for this in datasets...
20120506pib: deleted method_id and added table for multiple methods per entity");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DatasetId).HasColumnName("dataset_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.TblAnalysisEntities)
                    .HasForeignKey(d => d.DatasetId)
                    .HasConstraintName("tbl_analysis_entities_dataset_id_fkey");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblAnalysisEntities)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .HasConstraintName("tbl_analysis_entities_physical_sample_id_fkey");
            });

            modelBuilder.Entity<TblAnalysisEntityAges>(entity =>
            {
                entity.HasKey(e => e.AnalysisEntityAgeId)
                    .HasName("pk_sample_ages");

                entity.ToTable("tbl_analysis_entity_ages");

                entity.HasComment(@"20170911PIB: Changed numeric ranges of values to 20,5 to match tbl_relative_ages
20120504PIB: Should this be connected to physical sample instead of analysis entities? Allowing multiple ages (from multiple dates) for a sample. At the moment it requires a lot of backtracing to find a sample's age... but then again, it allows... what, exactly?");

                entity.Property(e => e.AnalysisEntityAgeId).HasColumnName("analysis_entity_age_id");

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.AgeOlder)
                    .HasColumnName("age_older")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.AgeYounger)
                    .HasColumnName("age_younger")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.ChronologyId).HasColumnName("chronology_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblAnalysisEntityAges)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .HasConstraintName("fk_analysis_entity_ages_analysis_entity_id");

                entity.HasOne(d => d.Chronology)
                    .WithMany(p => p.TblAnalysisEntityAges)
                    .HasForeignKey(d => d.ChronologyId)
                    .HasConstraintName("fk_analysis_entity_ages_chronology_id");
            });

            modelBuilder.Entity<TblAnalysisEntityDimensions>(entity =>
            {
                entity.HasKey(e => e.AnalysisEntityDimensionId)
                    .HasName("tbl_analysis_entity_dimensions_pkey");

                entity.ToTable("tbl_analysis_entity_dimensions");

                entity.Property(e => e.AnalysisEntityDimensionId).HasColumnName("analysis_entity_dimension_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DimensionId).HasColumnName("dimension_id");

                entity.Property(e => e.DimensionValue)
                    .HasColumnName("dimension_value")
                    .HasColumnType("numeric");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblAnalysisEntityDimensions)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .HasConstraintName("fk_analysis_entity_dimensions_analysis_entity_id");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.TblAnalysisEntityDimensions)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_analysis_entity_dimensions_dimension_id");
            });

            modelBuilder.Entity<TblAnalysisEntityPrepMethods>(entity =>
            {
                entity.HasKey(e => e.AnalysisEntityPrepMethodId)
                    .HasName("tbl_analysis_entity_prep_methods_pkey");

                entity.ToTable("tbl_analysis_entity_prep_methods");

                entity.HasComment(@"20170907PIB: Devolved due to problems in isolating measurement datasets with pretreatment/without. Many to many between datasets and methods used as replacement.
20120506PIB: created to cater for multiple preparation methods for analysis but maintaining simple dataset concept.");

                entity.Property(e => e.AnalysisEntityPrepMethodId)
                    .HasColumnName("analysis_entity_prep_method_id")
                    .HasDefaultValueSql("nextval('tbl_analysis_entity_prep_meth_analysis_entity_prep_method_i_seq'::regclass)");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId)
                    .HasColumnName("method_id")
                    .HasComment("preparation methods only");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblAnalysisEntityPrepMethods)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_analysis_entity_prep_methods_analysis_entity_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblAnalysisEntityPrepMethods)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_analysis_entity_prep_methods_method_id");
            });

            modelBuilder.Entity<TblBiblio>(entity =>
            {
                entity.HasKey(e => e.BiblioId)
                    .HasName("pk_biblio");

                entity.ToTable("tbl_biblio");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.Authors)
                    .HasColumnName("authors")
                    .HasColumnType("character varying");

                entity.Property(e => e.BugsReference)
                    .HasColumnName("bugs_reference")
                    .HasMaxLength(60)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Doi)
                    .HasColumnName("doi")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.FullReference)
                    .IsRequired()
                    .HasColumnName("full_reference")
                    .HasDefaultValueSql("''::text");

                entity.Property(e => e.Isbn)
                    .HasColumnName("isbn")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("character varying");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("character varying");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<TblCeramics>(entity =>
            {
                entity.HasKey(e => e.CeramicsId)
                    .HasName("tbl_ceramics_pkey");

                entity.ToTable("tbl_ceramics");

                entity.Property(e => e.CeramicsId).HasColumnName("ceramics_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.CeramicsLookupId).HasColumnName("ceramics_lookup_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MeasurementValue)
                    .IsRequired()
                    .HasColumnName("measurement_value")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblCeramics)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ceramics_analysis_entity_id");

                entity.HasOne(d => d.CeramicsLookup)
                    .WithMany(p => p.TblCeramics)
                    .HasForeignKey(d => d.CeramicsLookupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ceramics_ceramics_lookup_id");
            });

            modelBuilder.Entity<TblCeramicsLookup>(entity =>
            {
                entity.HasKey(e => e.CeramicsLookupId)
                    .HasName("tbl_ceramics_lookup_pkey");

                entity.ToTable("tbl_ceramics_lookup");

                entity.HasComment("Type=lookup");

                entity.Property(e => e.CeramicsLookupId).HasColumnName("ceramics_lookup_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblCeramicsLookup)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ceramics_lookup_method_id");
            });

            modelBuilder.Entity<TblCeramicsMeasurements>(entity =>
            {
                entity.HasKey(e => e.CeramicsMeasurementId)
                    .HasName("tbl_ceramics_measurements_pkey");

                entity.ToTable("tbl_ceramics_measurements");

                entity.HasComment("Type=lookup");

                entity.Property(e => e.CeramicsMeasurementId).HasColumnName("ceramics_measurement_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblCeramicsMeasurements)
                    .HasForeignKey(d => d.MethodId)
                    .HasConstraintName("fk_ceramics_measurements_method_id");
            });

            modelBuilder.Entity<TblChronControlTypes>(entity =>
            {
                entity.HasKey(e => e.ChronControlTypeId)
                    .HasName("pk_chron_control_types");

                entity.ToTable("tbl_chron_control_types");

                entity.Property(e => e.ChronControlTypeId).HasColumnName("chron_control_type_id");

                entity.Property(e => e.ChronControlType)
                    .HasColumnName("chron_control_type")
                    .HasMaxLength(50);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<TblChronControls>(entity =>
            {
                entity.HasKey(e => e.ChronControlId)
                    .HasName("pk_chron_controls");

                entity.ToTable("tbl_chron_controls");

                entity.Property(e => e.ChronControlId).HasColumnName("chron_control_id");

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.AgeLimitOlder)
                    .HasColumnName("age_limit_older")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.AgeLimitYounger)
                    .HasColumnName("age_limit_younger")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.ChronControlTypeId).HasColumnName("chron_control_type_id");

                entity.Property(e => e.ChronologyId).HasColumnName("chronology_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DepthBottom)
                    .HasColumnName("depth_bottom")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.DepthTop)
                    .HasColumnName("depth_top")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.HasOne(d => d.ChronControlType)
                    .WithMany(p => p.TblChronControls)
                    .HasForeignKey(d => d.ChronControlTypeId)
                    .HasConstraintName("fk_chron_controls_chron_control_type_id");

                entity.HasOne(d => d.Chronology)
                    .WithMany(p => p.TblChronControls)
                    .HasForeignKey(d => d.ChronologyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_chron_controls_chronology_id");
            });

            modelBuilder.Entity<TblChronologies>(entity =>
            {
                entity.HasKey(e => e.ChronologyId)
                    .HasName("pk_chronologies");

                entity.ToTable("tbl_chronologies");

                entity.HasComment(@"20170911PIB: Removed Not Null requirement for sample-group_id to allow for chronologies not tied to a single sample group (e.e. calibrated ages for DataArc or other projects)
Increased length of some fields.
20120504PIB: Note that the dropped age type recorded the type of dates (C14 etc) used in constructing the chronology... but is only one per chonology enough? Can a chronology not be made up of mulitple types of age? (No, years types can only be of one sort - need to calibrate if mixed?)");

                entity.Property(e => e.ChronologyId).HasColumnName("chronology_id");

                entity.Property(e => e.AgeBoundOlder).HasColumnName("age_bound_older");

                entity.Property(e => e.AgeBoundYounger).HasColumnName("age_bound_younger");

                entity.Property(e => e.AgeModel)
                    .HasColumnName("age_model")
                    .HasMaxLength(255);

                entity.Property(e => e.ChronologyName)
                    .HasColumnName("chronology_name")
                    .HasMaxLength(255);

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.DatePrepared)
                    .HasColumnName("date_prepared")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.RelativeAgeTypeId)
                    .HasColumnName("relative_age_type_id")
                    .HasComment("Constraint removed to obsolete table (tbl_age_types), replaced by non-binding id of relative_age_types - but not fully implemented. Notes should be used to inform on chronology years types and construction.");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblChronologies)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("fk_chronologies_contact_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblChronologies)
                    .HasForeignKey(d => d.SampleGroupId)
                    .HasConstraintName("fk_chronologies_sample_group_id");
            });

            modelBuilder.Entity<TblColours>(entity =>
            {
                entity.HasKey(e => e.ColourId)
                    .HasName("pk_colours");

                entity.ToTable("tbl_colours");

                entity.Property(e => e.ColourId).HasColumnName("colour_id");

                entity.Property(e => e.ColourName)
                    .IsRequired()
                    .HasColumnName("colour_name")
                    .HasMaxLength(30);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.Rgb).HasColumnName("rgb");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblColours)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_colours_method_id");
            });

            modelBuilder.Entity<TblContactTypes>(entity =>
            {
                entity.HasKey(e => e.ContactTypeId)
                    .HasName("pk_contact_types");

                entity.ToTable("tbl_contact_types");

                entity.Property(e => e.ContactTypeId).HasColumnName("contact_type_id");

                entity.Property(e => e.ContactTypeName)
                    .IsRequired()
                    .HasColumnName("contact_type_name")
                    .HasMaxLength(150);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<TblContacts>(entity =>
            {
                entity.HasKey(e => e.ContactId)
                    .HasName("pk_contacts");

                entity.ToTable("tbl_contacts");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.Address1)
                    .HasColumnName("address_1")
                    .HasMaxLength(255);

                entity.Property(e => e.Address2)
                    .HasColumnName("address_2")
                    .HasMaxLength(255);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("character varying");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(100);

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(50);

                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<TblCoordinateMethodDimensions>(entity =>
            {
                entity.HasKey(e => e.CoordinateMethodDimensionId)
                    .HasName("tbl_coordinate_method_dimensions_pkey");

                entity.ToTable("tbl_coordinate_method_dimensions");

                entity.Property(e => e.CoordinateMethodDimensionId)
                    .HasColumnName("coordinate_method_dimension_id")
                    .HasDefaultValueSql("nextval('tbl_coordinate_method_dimensi_coordinate_method_dimension_i_seq'::regclass)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DimensionId).HasColumnName("dimension_id");

                entity.Property(e => e.LimitLower)
                    .HasColumnName("limit_lower")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.LimitUpper)
                    .HasColumnName("limit_upper")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.TblCoordinateMethodDimensions)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_coordinate_method_dimensions_dimensions_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblCoordinateMethodDimensions)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_coordinate_method_dimensions_method_id");
            });

            modelBuilder.Entity<TblDataTypeGroups>(entity =>
            {
                entity.HasKey(e => e.DataTypeGroupId)
                    .HasName("pk_data_type_groups");

                entity.ToTable("tbl_data_type_groups");

                entity.Property(e => e.DataTypeGroupId).HasColumnName("data_type_group_id");

                entity.Property(e => e.DataTypeGroupName)
                    .HasColumnName("data_type_group_name")
                    .HasMaxLength(25);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<TblDataTypes>(entity =>
            {
                entity.HasKey(e => e.DataTypeId)
                    .HasName("pk_samplegroup_data_types");

                entity.ToTable("tbl_data_types");

                entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");

                entity.Property(e => e.DataTypeGroupId).HasColumnName("data_type_group_id");

                entity.Property(e => e.DataTypeName)
                    .HasColumnName("data_type_name")
                    .HasMaxLength(25)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.HasOne(d => d.DataTypeGroup)
                    .WithMany(p => p.TblDataTypes)
                    .HasForeignKey(d => d.DataTypeGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_data_types_data_type_group_id");
            });

            modelBuilder.Entity<TblDatasetContacts>(entity =>
            {
                entity.HasKey(e => e.DatasetContactId)
                    .HasName("pk_dataset_contacts");

                entity.ToTable("tbl_dataset_contacts");

                entity.Property(e => e.DatasetContactId).HasColumnName("dataset_contact_id");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.ContactTypeId).HasColumnName("contact_type_id");

                entity.Property(e => e.DatasetId).HasColumnName("dataset_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblDatasetContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_dataset_contacts_contact_id_fkey");

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.TblDatasetContacts)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_dataset_contacts_contact_type_id_fkey");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.TblDatasetContacts)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_dataset_contacts_dataset_id_fkey");
            });

            modelBuilder.Entity<TblDatasetMasters>(entity =>
            {
                entity.HasKey(e => e.MasterSetId)
                    .HasName("pk_dataset_masters");

                entity.ToTable("tbl_dataset_masters");

                entity.Property(e => e.MasterSetId).HasColumnName("master_set_id");

                entity.Property(e => e.BiblioId)
                    .HasColumnName("biblio_id")
                    .HasComment("primary reference for master dataset if available, e.g. buckland & buckland 2006 for bugscep");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MasterName)
                    .HasColumnName("master_name")
                    .HasMaxLength(100)
                    .HasComment("identification of master dataset, e.g. mal, bugscep, dendrolab");

                entity.Property(e => e.MasterNotes)
                    .HasColumnName("master_notes")
                    .HasComment("description of master dataset, its form (e.g. database, lab) and any other relevant information for tracing it.");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasComment("website or other url for master dataset, be it a project, lab or... other");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblDatasetMasters)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("fk_dataset_masters_biblio_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblDatasetMasters)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("fk_dataset_masters_contact_id");
            });

            modelBuilder.Entity<TblDatasetMethods>(entity =>
            {
                entity.HasKey(e => e.DatasetMethodId)
                    .HasName("tbl_dataset_methods_pkey");

                entity.ToTable("tbl_dataset_methods");

                entity.Property(e => e.DatasetMethodId).HasColumnName("dataset_method_id");

                entity.Property(e => e.DatasetId).HasColumnName("dataset_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp(6) with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.TblDatasetMethods)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dataset_methods_to_tbl_datasets");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblDatasetMethods)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dataset_methods_to_tbl_methods");
            });

            modelBuilder.Entity<TblDatasetSubmissionTypes>(entity =>
            {
                entity.HasKey(e => e.SubmissionTypeId)
                    .HasName("pk_dataset_submission_types");

                entity.ToTable("tbl_dataset_submission_types");

                entity.Property(e => e.SubmissionTypeId).HasColumnName("submission_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("explanation of submission type, explaining clearly data ingestion mechanism");

                entity.Property(e => e.SubmissionType)
                    .IsRequired()
                    .HasColumnName("submission_type")
                    .HasMaxLength(60)
                    .HasComment("descriptive name for type of submission, e.g. original submission, ingestion from another database");
            });

            modelBuilder.Entity<TblDatasetSubmissions>(entity =>
            {
                entity.HasKey(e => e.DatasetSubmissionId)
                    .HasName("pk_dataset_submissions");

                entity.ToTable("tbl_dataset_submissions");

                entity.Property(e => e.DatasetSubmissionId).HasColumnName("dataset_submission_id");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.DatasetId).HasColumnName("dataset_id");

                entity.Property(e => e.DateSubmitted)
                    .HasColumnName("date_submitted")
                    .HasColumnType("date");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasComment("any details of submission not covered by submission_type information, such as name of source from which submission originates if not covered elsewhere in database, e.g. from bugscep");

                entity.Property(e => e.SubmissionTypeId).HasColumnName("submission_type_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblDatasetSubmissions)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dataset_submissions_contact_id");

                entity.HasOne(d => d.Dataset)
                    .WithMany(p => p.TblDatasetSubmissions)
                    .HasForeignKey(d => d.DatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dataset_submissions_dataset_id");

                entity.HasOne(d => d.SubmissionType)
                    .WithMany(p => p.TblDatasetSubmissions)
                    .HasForeignKey(d => d.SubmissionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dataset_submission_submission_type_id");
            });

            modelBuilder.Entity<TblDatasets>(entity =>
            {
                entity.HasKey(e => e.DatasetId)
                    .HasName("pk_datasets");

                entity.ToTable("tbl_datasets");

                entity.Property(e => e.DatasetId).HasColumnName("dataset_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");

                entity.Property(e => e.DatasetName)
                    .IsRequired()
                    .HasColumnName("dataset_name")
                    .HasMaxLength(50)
                    .HasComment("something uniquely identifying the dataset for this site. may be same as sample group name, or created adhoc if necessary, but preferably with some meaning.");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MasterSetId).HasColumnName("master_set_id");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.UpdatedDatasetId).HasColumnName("updated_dataset_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblDatasets)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("tbl_datasets_biblio_id_fkey");

                entity.HasOne(d => d.DataType)
                    .WithMany(p => p.TblDatasets)
                    .HasForeignKey(d => d.DataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_datasets_data_type_id_fkey");

                entity.HasOne(d => d.MasterSet)
                    .WithMany(p => p.TblDatasets)
                    .HasForeignKey(d => d.MasterSetId)
                    .HasConstraintName("tbl_datasets_master_set_id_fkey");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblDatasets)
                    .HasForeignKey(d => d.MethodId)
                    .HasConstraintName("tbl_datasets_method_id_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TblDatasets)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("tbl_datasets_project_id_fkey");

                entity.HasOne(d => d.UpdatedDataset)
                    .WithMany(p => p.InverseUpdatedDataset)
                    .HasForeignKey(d => d.UpdatedDatasetId)
                    .HasConstraintName("tbl_datasets_updated_dataset_id_fkey");
            });

            modelBuilder.Entity<TblDatingLabs>(entity =>
            {
                entity.HasKey(e => e.DatingLabId)
                    .HasName("pk_dating_labs");

                entity.ToTable("tbl_dating_labs");

                entity.HasComment("20120504pib: reduced this table and linked to tbl_contacts for address related data");

                entity.Property(e => e.DatingLabId).HasColumnName("dating_lab_id");

                entity.Property(e => e.ContactId)
                    .HasColumnName("contact_id")
                    .HasComment("address details are stored in tbl_contacts");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.InternationalLabId)
                    .IsRequired()
                    .HasColumnName("international_lab_id")
                    .HasMaxLength(10)
                    .HasComment(@"international standard radiocarbon lab identifier.
from http://www.radiocarbon.org/info/labcodes.html");

                entity.Property(e => e.LabName)
                    .HasColumnName("lab_name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying")
                    .HasComment("international standard name of radiocarbon lab, from http://www.radiocarbon.org/info/labcodes.html");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblDatingLabs)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("fk_dating_labs_contact_id");
            });

            modelBuilder.Entity<TblDatingMaterial>(entity =>
            {
                entity.HasKey(e => e.DatingMaterialId)
                    .HasName("tbl_dating_material_pkey");

                entity.ToTable("tbl_dating_material");

                entity.HasComment("20130722PIB: Added field date_updated");

                entity.Property(e => e.DatingMaterialId).HasColumnName("dating_material_id");

                entity.Property(e => e.AbundanceElementId).HasColumnName("abundance_element_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.GeochronId).HasColumnName("geochron_id");

                entity.Property(e => e.MaterialDated)
                    .HasColumnName("material_dated")
                    .HasColumnType("character varying");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.AbundanceElement)
                    .WithMany(p => p.TblDatingMaterial)
                    .HasForeignKey(d => d.AbundanceElementId)
                    .HasConstraintName("fk_dating_material_abundance_elements_id");

                entity.HasOne(d => d.Geochron)
                    .WithMany(p => p.TblDatingMaterial)
                    .HasForeignKey(d => d.GeochronId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dating_material_geochronology_geochron_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblDatingMaterial)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("fk_dating_material_taxa_tree_master_taxon_id");
            });

            modelBuilder.Entity<TblDatingUncertainty>(entity =>
            {
                entity.HasKey(e => e.DatingUncertaintyId)
                    .HasName("tbl_dating_uncertainty_pkey");

                entity.ToTable("tbl_dating_uncertainty");

                entity.Property(e => e.DatingUncertaintyId).HasColumnName("dating_uncertainty_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Uncertainty)
                    .HasColumnName("uncertainty")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TblDendro>(entity =>
            {
                entity.HasKey(e => e.DendroId)
                    .HasName("tbl_dendro_pkey");

                entity.ToTable("tbl_dendro");

                entity.Property(e => e.DendroId).HasColumnName("dendro_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DendroLookupId).HasColumnName("dendro_lookup_id");

                entity.Property(e => e.MeasurementValue)
                    .IsRequired()
                    .HasColumnName("measurement_value")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblDendro)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dendro_analysis_entity_id");

                entity.HasOne(d => d.DendroLookup)
                    .WithMany(p => p.TblDendro)
                    .HasForeignKey(d => d.DendroLookupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dendro_dendro_lookup_id");
            });

            modelBuilder.Entity<TblDendroDateNotes>(entity =>
            {
                entity.HasKey(e => e.DendroDateNoteId)
                    .HasName("tbl_dendro_date_notes_pkey");

                entity.ToTable("tbl_dendro_date_notes");

                entity.Property(e => e.DendroDateNoteId).HasColumnName("dendro_date_note_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DendroDateId).HasColumnName("dendro_date_id");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.HasOne(d => d.DendroDate)
                    .WithMany(p => p.TblDendroDateNotes)
                    .HasForeignKey(d => d.DendroDateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dendro_date_notes_dendro_date_id");
            });

            modelBuilder.Entity<TblDendroDates>(entity =>
            {
                entity.HasKey(e => e.DendroDateId)
                    .HasName("tbl_dendro_dates_pkey");

                entity.ToTable("tbl_dendro_dates");

                entity.HasComment(@"20130722PIB: Added field dating_uncertainty_id to cater for >< etc.
        20130722pib: prefixed fieldnames age_younger and age_older with ""cal_"" to conform with equivalent names in other tables");

                entity.Property(e => e.DendroDateId).HasColumnName("dendro_date_id");

                entity.Property(e => e.AgeOlder).HasColumnName("age_older");

                entity.Property(e => e.AgeTypeId).HasColumnName("age_type_id");

                entity.Property(e => e.AgeYounger).HasColumnName("age_younger");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DatingUncertaintyId).HasColumnName("dating_uncertainty_id");

                entity.Property(e => e.DendroLookupId).HasColumnName("dendro_lookup_id");

                entity.Property(e => e.ErrorMinus).HasColumnName("error_minus");

                entity.Property(e => e.ErrorPlus).HasColumnName("error_plus");

                entity.Property(e => e.ErrorUncertaintyId).HasColumnName("error_uncertainty_id");

                entity.Property(e => e.SeasonOrQualifierId).HasColumnName("season_or_qualifier_id");

                entity.HasOne(d => d.AgeType)
                    .WithMany(p => p.TblDendroDates)
                    .HasForeignKey(d => d.AgeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_age_types_age_type_id");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblDendroDates)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dendro_dates_analysis_entity_id");

                entity.HasOne(d => d.DatingUncertainty)
                    .WithMany(p => p.TblDendroDates)
                    .HasForeignKey(d => d.DatingUncertaintyId)
                    .HasConstraintName("fk_dendro_dates_dating_uncertainty_id");

                entity.HasOne(d => d.DendroLookup)
                    .WithMany(p => p.TblDendroDates)
                    .HasForeignKey(d => d.DendroLookupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dendro_lookup_dendro_lookup_id");

                entity.HasOne(d => d.ErrorUncertainty)
                    .WithMany(p => p.TblDendroDates)
                    .HasForeignKey(d => d.ErrorUncertaintyId)
                    .HasConstraintName("fk_tbl_error_uncertainties_error_uncertainty_id");
            });

            modelBuilder.Entity<TblDendroLookup>(entity =>
            {
                entity.HasKey(e => e.DendroLookupId)
                    .HasName("tbl_dendro_lookup_pkey");

                entity.ToTable("tbl_dendro_lookup");

                entity.HasComment("type=lookup");

                entity.Property(e => e.DendroLookupId).HasColumnName("dendro_lookup_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblDendroLookup)
                    .HasForeignKey(d => d.MethodId)
                    .HasConstraintName("fk_dendro_lookup_method_id");
            });

            modelBuilder.Entity<TblDendroMeasurements>(entity =>
            {
                entity.HasKey(e => e.DendroMeasurementId)
                    .HasName("tbl_dendro_measurements_pkey");

                entity.ToTable("tbl_dendro_measurements");

                entity.HasComment("Type=lookup");

                entity.Property(e => e.DendroMeasurementId).HasColumnName("dendro_measurement_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblDendroMeasurements)
                    .HasForeignKey(d => d.MethodId)
                    .HasConstraintName("fk_dendro_measurements_method_id");
            });

            modelBuilder.Entity<TblDimensions>(entity =>
            {
                entity.HasKey(e => e.DimensionId)
                    .HasName("pk_dimensions");

                entity.ToTable("tbl_dimensions");

                entity.Property(e => e.DimensionId).HasColumnName("dimension_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DimensionAbbrev)
                    .HasColumnName("dimension_abbrev")
                    .HasMaxLength(16);

                entity.Property(e => e.DimensionDescription).HasColumnName("dimension_description");

                entity.Property(e => e.DimensionName)
                    .IsRequired()
                    .HasColumnName("dimension_name")
                    .HasMaxLength(50);

                entity.Property(e => e.MethodGroupId)
                    .HasColumnName("method_group_id")
                    .HasComment("Limits choice of dimension by method group (e.g. size measurements, coordinate systems)");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");

                entity.HasOne(d => d.MethodGroup)
                    .WithMany(p => p.TblDimensions)
                    .HasForeignKey(d => d.MethodGroupId)
                    .HasConstraintName("fk_dimensions_method_group_id");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.TblDimensions)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("fk_dimensions_unit_id");
            });

            modelBuilder.Entity<TblEcocodeDefinitions>(entity =>
            {
                entity.HasKey(e => e.EcocodeDefinitionId)
                    .HasName("pk_ecocode_definitions");

                entity.ToTable("tbl_ecocode_definitions");

                entity.Property(e => e.EcocodeDefinitionId).HasColumnName("ecocode_definition_id");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("abbreviation")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.EcocodeGroupId)
                    .HasColumnName("ecocode_group_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(150)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.EcocodeGroup)
                    .WithMany(p => p.TblEcocodeDefinitions)
                    .HasForeignKey(d => d.EcocodeGroupId)
                    .HasConstraintName("fk_ecocode_definitions_ecocode_group_id");
            });

            modelBuilder.Entity<TblEcocodeGroups>(entity =>
            {
                entity.HasKey(e => e.EcocodeGroupId)
                    .HasName("pk_ecocode_groups");

                entity.ToTable("tbl_ecocode_groups");

                entity.HasIndex(e => e.EcocodeSystemId)
                    .HasName("tbl_ecocode_groups_idx_ecocodesystemid");

                entity.HasIndex(e => e.Name)
                    .HasName("tbl_ecocode_groups_idx_name");

                entity.Property(e => e.EcocodeGroupId).HasColumnName("ecocode_group_id");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("abbreviation")
                    .HasMaxLength(255);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Definition)
                    .HasColumnName("definition")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.EcocodeSystemId)
                    .HasColumnName("ecocode_system_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.EcocodeSystem)
                    .WithMany(p => p.TblEcocodeGroups)
                    .HasForeignKey(d => d.EcocodeSystemId)
                    .HasConstraintName("fk_ecocode_groups_ecocode_system_id");
            });

            modelBuilder.Entity<TblEcocodeSystems>(entity =>
            {
                entity.HasKey(e => e.EcocodeSystemId)
                    .HasName("pk_ecocode_systems");

                entity.ToTable("tbl_ecocode_systems");

                entity.HasIndex(e => e.BiblioId)
                    .HasName("tbl_ecocode_systems_biblioid");

                entity.HasIndex(e => e.Name)
                    .HasName("tbl_ecocode_systems_ecocodegroupid");

                entity.Property(e => e.EcocodeSystemId).HasColumnName("ecocode_system_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Definition)
                    .HasColumnName("definition")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblEcocodeSystems)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("fk_ecocode_systems_biblio_id");
            });

            modelBuilder.Entity<TblEcocodes>(entity =>
            {
                entity.HasKey(e => e.EcocodeId)
                    .HasName("pk_ecocodes");

                entity.ToTable("tbl_ecocodes");

                entity.Property(e => e.EcocodeId).HasColumnName("ecocode_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.EcocodeDefinitionId)
                    .HasColumnName("ecocode_definition_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TaxonId)
                    .HasColumnName("taxon_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.EcocodeDefinition)
                    .WithMany(p => p.TblEcocodes)
                    .HasForeignKey(d => d.EcocodeDefinitionId)
                    .HasConstraintName("tbl_ecocodes_ecocode_definition_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblEcocodes)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_ecocodes_taxon_id_fkey");
            });

            modelBuilder.Entity<TblErrorUncertainties>(entity =>
            {
                entity.HasKey(e => e.ErrorUncertaintyId)
                    .HasName("tbl_error_uncertainties_pkey");

                entity.ToTable("tbl_error_uncertainties");

                entity.Property(e => e.ErrorUncertaintyId).HasColumnName("error_uncertainty_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ErrorUncertaintyType)
                    .IsRequired()
                    .HasColumnName("error_uncertainty_type")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<TblFeatureTypes>(entity =>
            {
                entity.HasKey(e => e.FeatureTypeId)
                    .HasName("pk_feature_type_id");

                entity.ToTable("tbl_feature_types");

                entity.Property(e => e.FeatureTypeId).HasColumnName("feature_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FeatureTypeDescription).HasColumnName("feature_type_description");

                entity.Property(e => e.FeatureTypeName)
                    .HasColumnName("feature_type_name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<TblFeatures>(entity =>
            {
                entity.HasKey(e => e.FeatureId)
                    .HasName("pk_features");

                entity.ToTable("tbl_features");

                entity.Property(e => e.FeatureId).HasColumnName("feature_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FeatureDescription)
                    .HasColumnName("feature_description")
                    .HasComment("description of the feature. may include any field notes, lab notes or interpretation information useful for interpreting the sample data.");

                entity.Property(e => e.FeatureName)
                    .HasColumnName("feature_name")
                    .HasColumnType("character varying")
                    .HasComment(@"estabilished reference name/number for the feature (note: not the sample). e.g. well 47, anl.3, c107.
remember that a sample can come from multiple features (e.g. c107 in well 47) but each feature should have a separate record.");

                entity.Property(e => e.FeatureTypeId).HasColumnName("feature_type_id");

                entity.HasOne(d => d.FeatureType)
                    .WithMany(p => p.TblFeatures)
                    .HasForeignKey(d => d.FeatureTypeId)
                    .HasConstraintName("fk_feature_type_id_feature_type_id");
            });

            modelBuilder.Entity<TblGeochronRefs>(entity =>
            {
                entity.HasKey(e => e.GeochronRefId)
                    .HasName("pk_geochron_refs");

                entity.ToTable("tbl_geochron_refs");

                entity.Property(e => e.GeochronRefId).HasColumnName("geochron_ref_id");

                entity.Property(e => e.BiblioId)
                    .HasColumnName("biblio_id")
                    .HasComment("reference for specific date");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.GeochronId).HasColumnName("geochron_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblGeochronRefs)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_geochron_refs_biblio_id");

                entity.HasOne(d => d.Geochron)
                    .WithMany(p => p.TblGeochronRefs)
                    .HasForeignKey(d => d.GeochronId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_geochron_refs_geochron_id");
            });

            modelBuilder.Entity<TblGeochronology>(entity =>
            {
                entity.HasKey(e => e.GeochronId)
                    .HasName("pk_geochronology");

                entity.ToTable("tbl_geochronology");

                entity.HasComment("20130722PIB: Altered field uncertainty (varchar) to dating_uncertainty_id and linked to tbl_dating_uncertainty to enable lookup of uncertainty modifiers for dates");

                entity.Property(e => e.GeochronId).HasColumnName("geochron_id");

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("radiocarbon (or other radiometric) age.");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DatingLabId).HasColumnName("dating_lab_id");

                entity.Property(e => e.DatingUncertaintyId).HasColumnName("dating_uncertainty_id");

                entity.Property(e => e.Delta13c)
                    .HasColumnName("delta_13c")
                    .HasColumnType("numeric(10,5)")
                    .HasComment("delta 13c where available for calibration correction.");

                entity.Property(e => e.ErrorOlder)
                    .HasColumnName("error_older")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("plus (+) side of the measured error (set same as error_younger if standard +/- error)");

                entity.Property(e => e.ErrorYounger)
                    .HasColumnName("error_younger")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("minus (-) side of the measured error (set same as error_younger if standard +/- error)");

                entity.Property(e => e.LabNumber)
                    .HasColumnName("lab_number")
                    .HasMaxLength(40);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasComment("notes specific to this date");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblGeochronology)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_geochronology_analysis_entity_id_fkey");

                entity.HasOne(d => d.DatingLab)
                    .WithMany(p => p.TblGeochronology)
                    .HasForeignKey(d => d.DatingLabId)
                    .HasConstraintName("tbl_geochronology_dating_lab_id_fkey");

                entity.HasOne(d => d.DatingUncertainty)
                    .WithMany(p => p.TblGeochronology)
                    .HasForeignKey(d => d.DatingUncertaintyId)
                    .HasConstraintName("tbl_geochronology_dating_uncertainty_id_fkey");
            });

            modelBuilder.Entity<TblHorizons>(entity =>
            {
                entity.HasKey(e => e.HorizonId)
                    .HasName("pk_horizons");

                entity.ToTable("tbl_horizons");

                entity.Property(e => e.HorizonId).HasColumnName("horizon_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.HorizonName)
                    .IsRequired()
                    .HasColumnName("horizon_name")
                    .HasMaxLength(15);

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblHorizons)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_horizons_method_id");
            });

            modelBuilder.Entity<TblIdentificationLevels>(entity =>
            {
                entity.HasKey(e => e.IdentificationLevelId)
                    .HasName("pk_identification_levels");

                entity.ToTable("tbl_identification_levels");

                entity.Property(e => e.IdentificationLevelId).HasColumnName("identification_level_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IdentificationLevelAbbrev)
                    .HasColumnName("identification_level_abbrev")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.IdentificationLevelName)
                    .HasColumnName("identification_level_name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Notes).HasColumnName("notes");
            });

            modelBuilder.Entity<TblImageTypes>(entity =>
            {
                entity.HasKey(e => e.ImageTypeId)
                    .HasName("pk_image_types");

                entity.ToTable("tbl_image_types");

                entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageType)
                    .IsRequired()
                    .HasColumnName("image_type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<TblImportedTaxaReplacements>(entity =>
            {
                entity.HasKey(e => e.ImportedTaxaReplacementId)
                    .HasName("pk_imported_taxa_replacements");

                entity.ToTable("tbl_imported_taxa_replacements");

                entity.Property(e => e.ImportedTaxaReplacementId).HasColumnName("imported_taxa_replacement_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ImportedNameReplaced)
                    .IsRequired()
                    .HasColumnName("imported_name_replaced")
                    .HasMaxLength(100);

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblImportedTaxaReplacements)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("fk_imported_taxa_replacements_taxon_id");
            });

            modelBuilder.Entity<TblIsotopeMeasurements>(entity =>
            {
                entity.HasKey(e => e.IsotopeMeasurementId)
                    .HasName("tbl_isotope_measurements_pkey");

                entity.ToTable("tbl_isotope_measurements");

                entity.Property(e => e.IsotopeMeasurementId).HasColumnName("isotope_measurement_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IsotopeStandardId).HasColumnName("isotope_standard_id");

                entity.Property(e => e.IsotopeTypeId).HasColumnName("isotope_type_id");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.HasOne(d => d.IsotopeStandard)
                    .WithMany(p => p.TblIsotopeMeasurements)
                    .HasForeignKey(d => d.IsotopeStandardId)
                    .HasConstraintName("fk_isotope_measurements_isotope_standard_id");

                entity.HasOne(d => d.IsotopeType)
                    .WithMany(p => p.TblIsotopeMeasurements)
                    .HasForeignKey(d => d.IsotopeTypeId)
                    .HasConstraintName("fk_isotope_isotope_type_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblIsotopeMeasurements)
                    .HasForeignKey(d => d.MethodId)
                    .HasConstraintName("fk_isotope_method_id");
            });

            modelBuilder.Entity<TblIsotopeStandards>(entity =>
            {
                entity.HasKey(e => e.IsotopeStandardId)
                    .HasName("tbl_isotope_standards_pkey");

                entity.ToTable("tbl_isotope_standards");

                entity.HasComment("Type=lookup");

                entity.Property(e => e.IsotopeStandardId).HasColumnName("isotope_standard_id");

                entity.Property(e => e.AcceptedRatioXe6)
                    .HasColumnName("accepted_ratio_xe6")
                    .HasColumnType("character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ErrorOfRatio)
                    .HasColumnName("error_of_ratio")
                    .HasColumnType("character varying");

                entity.Property(e => e.InternationalScale)
                    .HasColumnName("international_scale")
                    .HasColumnType("character varying");

                entity.Property(e => e.IsotopeRation)
                    .HasColumnName("isotope_ration")
                    .HasColumnType("character varying");

                entity.Property(e => e.Reference)
                    .HasColumnName("reference")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TblIsotopeTypes>(entity =>
            {
                entity.HasKey(e => e.IsotopeTypeId)
                    .HasName("tbl_isotope_types_pkey");

                entity.ToTable("tbl_isotope_types");

                entity.HasComment("Type=lookup");

                entity.Property(e => e.IsotopeTypeId).HasColumnName("isotope_type_id");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("abbreviation")
                    .HasColumnType("character varying");

                entity.Property(e => e.AlternativeDesignation)
                    .HasColumnName("alternative_designation")
                    .HasColumnType("character varying");

                entity.Property(e => e.AtomicNumber)
                    .HasColumnName("atomic_number")
                    .HasColumnType("numeric");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Designation)
                    .HasColumnName("designation")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TblIsotopeValueSpecifiers>(entity =>
            {
                entity.HasKey(e => e.IsotopeValueSpecifierId)
                    .HasName("tbl_isotope_value_specifiers_pkey");

                entity.ToTable("tbl_isotope_value_specifiers");

                entity.Property(e => e.IsotopeValueSpecifierId)
                    .HasColumnName("isotope_value_specifier_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TblIsotopes>(entity =>
            {
                entity.HasKey(e => e.IsotopeId)
                    .HasName("tbl_isotopes_pkey");

                entity.ToTable("tbl_isotopes");

                entity.Property(e => e.IsotopeId).HasColumnName("isotope_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.IsotopeMeasurementId).HasColumnName("isotope_measurement_id");

                entity.Property(e => e.IsotopeStandardId).HasColumnName("isotope_standard_id");

                entity.Property(e => e.IsotopeValueSpecifierId).HasColumnName("isotope_value_specifier_id");

                entity.Property(e => e.MeasurementValue).HasColumnName("measurement_value");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblIsotopes)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_isotopes_analysis_entity_id");

                entity.HasOne(d => d.IsotopeMeasurement)
                    .WithMany(p => p.TblIsotopes)
                    .HasForeignKey(d => d.IsotopeMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_isotopes_isotope_measurement_id");

                entity.HasOne(d => d.IsotopeStandard)
                    .WithMany(p => p.TblIsotopes)
                    .HasForeignKey(d => d.IsotopeStandardId)
                    .HasConstraintName("fk_isotopes_isotope_standard_id");

                entity.HasOne(d => d.IsotopeValueSpecifier)
                    .WithMany(p => p.TblIsotopes)
                    .HasForeignKey(d => d.IsotopeValueSpecifierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_isotopes_isotope_value_specifier_id");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.TblIsotopes)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_isotopes_unit_id");
            });

            modelBuilder.Entity<TblLanguages>(entity =>
            {
                entity.HasKey(e => e.LanguageId)
                    .HasName("pk_languages");

                entity.ToTable("tbl_languages");

                entity.HasIndex(e => e.LanguageId)
                    .HasName("tbl_languages_language_id");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LanguageNameEnglish)
                    .HasColumnName("language_name_english")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.LanguageNameNative)
                    .HasColumnName("language_name_native")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<TblLithology>(entity =>
            {
                entity.HasKey(e => e.LithologyId)
                    .HasName("pk_lithologies");

                entity.ToTable("tbl_lithology");

                entity.Property(e => e.LithologyId).HasColumnName("lithology_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DepthBottom)
                    .HasColumnName("depth_bottom")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.DepthTop)
                    .HasColumnName("depth_top")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.LowerBoundary)
                    .HasColumnName("lower_boundary")
                    .HasMaxLength(255);

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblLithology)
                    .HasForeignKey(d => d.SampleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_lithology_sample_group_id");
            });

            modelBuilder.Entity<TblLocationTypes>(entity =>
            {
                entity.HasKey(e => e.LocationTypeId)
                    .HasName("pk_location_types");

                entity.ToTable("tbl_location_types");

                entity.Property(e => e.LocationTypeId).HasColumnName("location_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.LocationType)
                    .HasColumnName("location_type")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<TblLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("pk_locations");

                entity.ToTable("tbl_locations");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DefaultLatDd)
                    .HasColumnName("default_lat_dd")
                    .HasColumnType("numeric(18,10)")
                    .HasComment("default latitude in decimal degrees for location, e.g. mid point of country. leave empty if not known.");

                entity.Property(e => e.DefaultLongDd)
                    .HasColumnName("default_long_dd")
                    .HasColumnType("numeric(18,10)")
                    .HasComment("default longitude in decimal degrees for location, e.g. mid point of country");

                entity.Property(e => e.LocationName)
                    .IsRequired()
                    .HasColumnName("location_name")
                    .HasMaxLength(255);

                entity.Property(e => e.LocationTypeId).HasColumnName("location_type_id");

                entity.HasOne(d => d.LocationType)
                    .WithMany(p => p.TblLocations)
                    .HasForeignKey(d => d.LocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_locations_location_type_id_fkey");
            });

            modelBuilder.Entity<TblMcrNames>(entity =>
            {
                entity.HasKey(e => e.TaxonId)
                    .HasName("pk_mcr_names");

                entity.ToTable("tbl_mcr_names");

                entity.Property(e => e.TaxonId)
                    .HasColumnName("taxon_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ComparisonNotes)
                    .HasColumnName("comparison_notes")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.McrNameTrim)
                    .HasColumnName("mcr_name_trim")
                    .HasMaxLength(80)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.McrNumber)
                    .HasColumnName("mcr_number")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.McrSpeciesName)
                    .HasColumnName("mcr_species_name")
                    .HasMaxLength(200)
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.Taxon)
                    .WithOne(p => p.TblMcrNames)
                    .HasForeignKey<TblMcrNames>(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_mcr_names_taxon_id");
            });

            modelBuilder.Entity<TblMcrSummaryData>(entity =>
            {
                entity.HasKey(e => e.McrSummaryDataId)
                    .HasName("pk_mcr_summary_data");

                entity.ToTable("tbl_mcr_summary_data");

                entity.HasIndex(e => e.TaxonId)
                    .HasName("key_mcr_summary_data_taxon_id")
                    .IsUnique();

                entity.Property(e => e.McrSummaryDataId).HasColumnName("mcr_summary_data_id");

                entity.Property(e => e.CogMidTmax)
                    .HasColumnName("cog_mid_tmax")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CogMidTrange)
                    .HasColumnName("cog_mid_trange")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.Property(e => e.TmaxHi)
                    .HasColumnName("tmax_hi")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TmaxLo)
                    .HasColumnName("tmax_lo")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TminHi)
                    .HasColumnName("tmin_hi")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TminLo)
                    .HasColumnName("tmin_lo")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TrangeHi)
                    .HasColumnName("trange_hi")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TrangeLo)
                    .HasColumnName("trange_lo")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Taxon)
                    .WithOne(p => p.TblMcrSummaryData)
                    .HasForeignKey<TblMcrSummaryData>(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_mcr_summary_data_taxon_id");
            });

            modelBuilder.Entity<TblMcrdataBirmbeetledat>(entity =>
            {
                entity.HasKey(e => e.McrdataBirmbeetledatId)
                    .HasName("pk_mcrdata_birmbeetledat");

                entity.ToTable("tbl_mcrdata_birmbeetledat");

                entity.Property(e => e.McrdataBirmbeetledatId).HasColumnName("mcrdata_birmbeetledat_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.McrData).HasColumnName("mcr_data");

                entity.Property(e => e.McrRow).HasColumnName("mcr_row");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblMcrdataBirmbeetledat)
                    .HasForeignKey(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_mcrdata_birmbeetledat_taxon_id");
            });

            modelBuilder.Entity<TblMeasuredValueDimensions>(entity =>
            {
                entity.HasKey(e => e.MeasuredValueDimensionId)
                    .HasName("pk_measured_weights");

                entity.ToTable("tbl_measured_value_dimensions");

                entity.Property(e => e.MeasuredValueDimensionId).HasColumnName("measured_value_dimension_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DimensionId).HasColumnName("dimension_id");

                entity.Property(e => e.DimensionValue)
                    .HasColumnName("dimension_value")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.MeasuredValueId).HasColumnName("measured_value_id");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.TblMeasuredValueDimensions)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_measured_value_dimensions_dimension_id");

                entity.HasOne(d => d.MeasuredValue)
                    .WithMany(p => p.TblMeasuredValueDimensions)
                    .HasForeignKey(d => d.MeasuredValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_measured_weights_value_id");
            });

            modelBuilder.Entity<TblMeasuredValues>(entity =>
            {
                entity.HasKey(e => e.MeasuredValueId)
                    .HasName("pk_measured_values");

                entity.ToTable("tbl_measured_values");

                entity.Property(e => e.MeasuredValueId).HasColumnName("measured_value_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MeasuredValue)
                    .HasColumnName("measured_value")
                    .HasColumnType("numeric(20,10)");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblMeasuredValues)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_measured_values_analysis_entity_id");
            });

            modelBuilder.Entity<TblMethodGroups>(entity =>
            {
                entity.HasKey(e => e.MethodGroupId)
                    .HasName("pk_method_groups");

                entity.ToTable("tbl_method_groups");

                entity.Property(e => e.MethodGroupId).HasColumnName("method_group_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnName("group_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TblMethods>(entity =>
            {
                entity.HasKey(e => e.MethodId)
                    .HasName("pk_methods");

                entity.ToTable("tbl_methods");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.MethodAbbrevOrAltName)
                    .HasColumnName("method_abbrev_or_alt_name")
                    .HasMaxLength(50);

                entity.Property(e => e.MethodGroupId).HasColumnName("method_group_id");

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasColumnName("method_name")
                    .HasMaxLength(50);

                entity.Property(e => e.RecordTypeId).HasColumnName("record_type_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblMethods)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("fk_methods_biblio_id");

                entity.HasOne(d => d.MethodGroup)
                    .WithMany(p => p.TblMethods)
                    .HasForeignKey(d => d.MethodGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_methods_method_group_id");

                entity.HasOne(d => d.RecordType)
                    .WithMany(p => p.TblMethods)
                    .HasForeignKey(d => d.RecordTypeId)
                    .HasConstraintName("fk_methods_record_type_id");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.TblMethods)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("fk_methods_unit_id");
            });

            modelBuilder.Entity<TblModificationTypes>(entity =>
            {
                entity.HasKey(e => e.ModificationTypeId)
                    .HasName("pk_modification_types");

                entity.ToTable("tbl_modification_types");

                entity.Property(e => e.ModificationTypeId).HasColumnName("modification_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ModificationTypeDescription)
                    .HasColumnName("modification_type_description")
                    .HasComment("clear explanation of modification so that name makes sense to non-domain scientists");

                entity.Property(e => e.ModificationTypeName)
                    .HasColumnName("modification_type_name")
                    .HasMaxLength(128)
                    .HasComment("short name of modification, e.g. carbonised");
            });

            modelBuilder.Entity<TblPhysicalSampleFeatures>(entity =>
            {
                entity.HasKey(e => e.PhysicalSampleFeatureId)
                    .HasName("tbl_physical_sample_features_pkey");

                entity.ToTable("tbl_physical_sample_features");

                entity.HasIndex(e => e.FeatureId)
                    .HasName("idx_tbl_physical_sample_features_feature_id");

                entity.Property(e => e.PhysicalSampleFeatureId).HasColumnName("physical_sample_feature_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FeatureId).HasColumnName("feature_id");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.TblPhysicalSampleFeatures)
                    .HasForeignKey(d => d.FeatureId)
                    .HasConstraintName("fk_physical_sample_features_feature_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblPhysicalSampleFeatures)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .HasConstraintName("fk_physical_sample_features_physical_sample_id");
            });

            modelBuilder.Entity<TblPhysicalSamples>(entity =>
            {
                entity.HasKey(e => e.PhysicalSampleId)
                    .HasName("pk_physical_samples");

                entity.ToTable("tbl_physical_samples");

                entity.HasComment(@"20120504PIB: deleted columns XYZ and created external tbl_sample_coodinates
20120506PIB: deleted columns depth_top & depth_bottom and moved to tbl_sample_dimensions
20130416PIB: changed to date_sampled from date to varchar format to increase flexibility");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.Property(e => e.AltRefTypeId)
                    .HasColumnName("alt_ref_type_id")
                    .HasComment("type of name represented by primary sample name, e.g. lab number, museum number etc.");

                entity.Property(e => e.DateSampled)
                    .HasColumnName("date_sampled")
                    .HasColumnType("character varying")
                    .HasComment("Date samples were taken. ");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.Property(e => e.SampleName)
                    .IsRequired()
                    .HasColumnName("sample_name")
                    .HasMaxLength(50)
                    .HasComment("reference number or name of sample. multiple references/names can be added as alternative references.");

                entity.Property(e => e.SampleTypeId)
                    .HasColumnName("sample_type_id")
                    .HasComment("physical form of sample, e.g. bulk sample, kubienta subsample, core subsample, dendro core, dendro slice...");

                entity.HasOne(d => d.AltRefType)
                    .WithMany(p => p.TblPhysicalSamples)
                    .HasForeignKey(d => d.AltRefTypeId)
                    .HasConstraintName("tbl_physical_samples_alt_ref_type_id_fkey");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblPhysicalSamples)
                    .HasForeignKey(d => d.SampleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_physical_samples_sample_group_id_fkey");

                entity.HasOne(d => d.SampleType)
                    .WithMany(p => p.TblPhysicalSamples)
                    .HasForeignKey(d => d.SampleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_physical_samples_sample_type_id_fkey");
            });

            modelBuilder.Entity<TblProjectStages>(entity =>
            {
                entity.HasKey(e => e.ProjectStageId)
                    .HasName("dataset_stage_id");

                entity.ToTable("tbl_project_stages");

                entity.Property(e => e.ProjectStageId)
                    .HasColumnName("project_stage_id")
                    .HasDefaultValueSql("nextval('tbl_project_stage_project_stage_id_seq'::regclass)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("explanation of stage name term, including details of purpose and general contents");

                entity.Property(e => e.StageName)
                    .HasColumnName("stage_name")
                    .HasColumnType("character varying")
                    .HasComment("stage of project in investigative cycle, e.g. desktop study, prospection, final excavation");
            });

            modelBuilder.Entity<TblProjectTypes>(entity =>
            {
                entity.HasKey(e => e.ProjectTypeId)
                    .HasName("pk_project_type_id");

                entity.ToTable("tbl_project_types");

                entity.Property(e => e.ProjectTypeId).HasColumnName("project_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("project type combinations can be used where appropriate, e.g. teaching/research");

                entity.Property(e => e.ProjectTypeName)
                    .HasColumnName("project_type_name")
                    .HasColumnType("character varying")
                    .HasComment("descriptive name for project type, e.g. consultancy, research, teaching; also combinations consultancy/teaching");
            });

            modelBuilder.Entity<TblProjects>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("pk_projects");

                entity.ToTable("tbl_projects");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("brief description of project and any useful information for finding out more.");

                entity.Property(e => e.ProjectAbbrevName)
                    .HasColumnName("project_abbrev_name")
                    .HasMaxLength(25)
                    .HasComment("optional. abbreviation of project name or acronym (e.g. vgv, swedab)");

                entity.Property(e => e.ProjectName)
                    .HasColumnName("project_name")
                    .HasMaxLength(150)
                    .HasComment("name of project (e.g. phil's phd thesis, malmö ringroad vägverket)");

                entity.Property(e => e.ProjectStageId).HasColumnName("project_stage_id");

                entity.Property(e => e.ProjectTypeId).HasColumnName("project_type_id");

                entity.HasOne(d => d.ProjectStage)
                    .WithMany(p => p.TblProjects)
                    .HasForeignKey(d => d.ProjectStageId)
                    .HasConstraintName("fk_projects_project_stage_id");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.TblProjects)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .HasConstraintName("fk_projects_project_type_id");
            });

            modelBuilder.Entity<TblRdb>(entity =>
            {
                entity.HasKey(e => e.RdbId)
                    .HasName("pk_rdb");

                entity.ToTable("tbl_rdb");

                entity.Property(e => e.RdbId).HasColumnName("rdb_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LocationId)
                    .HasColumnName("location_id")
                    .HasComment("geographical source/relevance of the specific code. e.g. the international iucn classification of species in the uk.");

                entity.Property(e => e.RdbCodeId).HasColumnName("rdb_code_id");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblRdb)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_rdb_location_id_fkey");

                entity.HasOne(d => d.RdbCode)
                    .WithMany(p => p.TblRdb)
                    .HasForeignKey(d => d.RdbCodeId)
                    .HasConstraintName("tbl_rdb_rdb_code_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblRdb)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_rdb_taxon_id_fkey");
            });

            modelBuilder.Entity<TblRdbCodes>(entity =>
            {
                entity.HasKey(e => e.RdbCodeId)
                    .HasName("pk_rdb_codes");

                entity.ToTable("tbl_rdb_codes");

                entity.Property(e => e.RdbCodeId).HasColumnName("rdb_code_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.RdbCategory)
                    .HasColumnName("rdb_category")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.RdbDefinition)
                    .HasColumnName("rdb_definition")
                    .HasMaxLength(200)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.RdbSystemId)
                    .HasColumnName("rdb_system_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.RdbSystem)
                    .WithMany(p => p.TblRdbCodes)
                    .HasForeignKey(d => d.RdbSystemId)
                    .HasConstraintName("fk_rdb_codes_rdb_system_id");
            });

            modelBuilder.Entity<TblRdbSystems>(entity =>
            {
                entity.HasKey(e => e.RdbSystemId)
                    .HasName("pk_rdb_systems");

                entity.ToTable("tbl_rdb_systems");

                entity.Property(e => e.RdbSystemId).HasColumnName("rdb_system_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LocationId)
                    .HasColumnName("location_id")
                    .HasComment("geaographical relevance of rdb code system, e.g. uk, international, new forest");

                entity.Property(e => e.RdbFirstPublished).HasColumnName("rdb_first_published");

                entity.Property(e => e.RdbSystem)
                    .HasColumnName("rdb_system")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.RdbSystemDate).HasColumnName("rdb_system_date");

                entity.Property(e => e.RdbVersion)
                    .HasColumnName("rdb_version")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblRdbSystems)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rdb_systems_biblio_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblRdbSystems)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_rdb_systems_location_id");
            });

            modelBuilder.Entity<TblRecordTypes>(entity =>
            {
                entity.HasKey(e => e.RecordTypeId)
                    .HasName("pk_record_types");

                entity.ToTable("tbl_record_types");

                entity.HasComment("may also use this to group methods - e.g. phosphate analyses (whereas tbl_method_groups would store the larger group \"palaeo chemical/physical\" methods)");

                entity.Property(e => e.RecordTypeId).HasColumnName("record_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.RecordTypeDescription)
                    .HasColumnName("record_type_description")
                    .HasComment("detailed description of group and explanation for grouping");

                entity.Property(e => e.RecordTypeName)
                    .HasColumnName("record_type_name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying")
                    .HasComment("short name of proxy/proxies in group");
            });

            modelBuilder.Entity<TblRelativeAgeRefs>(entity =>
            {
                entity.HasKey(e => e.RelativeAgeRefId)
                    .HasName("pk_relative_age_refs");

                entity.ToTable("tbl_relative_age_refs");

                entity.Property(e => e.RelativeAgeRefId).HasColumnName("relative_age_ref_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.RelativeAgeId).HasColumnName("relative_age_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblRelativeAgeRefs)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_relative_age_refs_biblio_id");

                entity.HasOne(d => d.RelativeAge)
                    .WithMany(p => p.TblRelativeAgeRefs)
                    .HasForeignKey(d => d.RelativeAgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_relative_age_refs_relative_age_id");
            });

            modelBuilder.Entity<TblRelativeAgeTypes>(entity =>
            {
                entity.HasKey(e => e.RelativeAgeTypeId)
                    .HasName("tbl_relative_age_types_pkey");

                entity.ToTable("tbl_relative_age_types");

                entity.HasComment(@"20130723PIB: replaced date_updated column with new one with same name but correct data type
20140226EE: replaced date_updated column with correct time data type");

                entity.Property(e => e.RelativeAgeTypeId).HasColumnName("relative_age_type_id");

                entity.Property(e => e.AgeType)
                    .HasColumnName("age_type")
                    .HasColumnType("character varying")
                    .HasComment("name of chronological age type, e.g. archaeological period, single calendar date, calendar age range, blytt-sernander");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("description of chronological age type, e.g. period defined by archaeological and or geological dates representing cultural activity period, climate period defined by palaeo-vegetation records");
            });

            modelBuilder.Entity<TblRelativeAges>(entity =>
            {
                entity.HasKey(e => e.RelativeAgeId)
                    .HasName("pk_relative_ages");

                entity.ToTable("tbl_relative_ages");

                entity.HasComment(@"20120504PIB: removed biblio_id as is replaced by tbl_relative_age_refs
20130722PIB: changed colour in model to AliceBlue to reflect degree of user addition possible (i.e. ages can be added for reference in tbl_relative_dates)");

                entity.Property(e => e.RelativeAgeId).HasColumnName("relative_age_id");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("abbreviation")
                    .HasColumnType("character varying")
                    .HasComment("Standard abbreviated form of name if available");

                entity.Property(e => e.C14AgeOlder)
                    .HasColumnName("c14_age_older")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("c14 age of younger boundary of period (where relevant).");

                entity.Property(e => e.C14AgeYounger)
                    .HasColumnName("c14_age_younger")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("c14 age of later boundary of period (where relevant). leave blank for calendar ages.");

                entity.Property(e => e.CalAgeOlder)
                    .HasColumnName("cal_age_older")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("(approximate) age before present (1950) of earliest boundary of period. or if calendar age then the calendar age converted to bp.");

                entity.Property(e => e.CalAgeYounger)
                    .HasColumnName("cal_age_younger")
                    .HasColumnType("numeric(20,5)")
                    .HasComment("(approximate) age before present (1950) of latest boundary of period. or if calendar age then the calendar age converted to bp.");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("a description of the (usually) period.");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasComment("any further notes not included in the description, such as reliability of definition or fuzzyness of boundaries.");

                entity.Property(e => e.RelativeAgeName)
                    .HasColumnName("relative_age_name")
                    .HasMaxLength(50)
                    .HasComment("name of the dating period, e.g. bronze age. calendar ages should be given appropriate names such as ad 1492, 74 bc");

                entity.Property(e => e.RelativeAgeTypeId).HasColumnName("relative_age_type_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblRelativeAges)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("tbl_relative_ages_location_id_fkey");

                entity.HasOne(d => d.RelativeAgeType)
                    .WithMany(p => p.TblRelativeAges)
                    .HasForeignKey(d => d.RelativeAgeTypeId)
                    .HasConstraintName("tbl_relative_ages_relative_age_type_id_fkey");
            });

            modelBuilder.Entity<TblRelativeDates>(entity =>
            {
                entity.HasKey(e => e.RelativeDateId)
                    .HasName("pk_relative_dates");

                entity.ToTable("tbl_relative_dates");

                entity.HasComment(@"20120504PIB: Added method_id to store dating method used to attribute sample to period or calendar date (e.g. strategraphic dating, typological)
20130722PIB: added field dating_uncertainty_id to cater for ""from"", ""to"" and ""ca."" etc. especially from import of BugsCEP
20170906PIB: removed fk physical_samples_id and replaced with analysis_entity_id");

                entity.Property(e => e.RelativeDateId).HasColumnName("relative_date_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DatingUncertaintyId).HasColumnName("dating_uncertainty_id");

                entity.Property(e => e.MethodId)
                    .HasColumnName("method_id")
                    .HasComment("dating method used to attribute sample to period or calendar date.");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasComment("any notes specific to the dating of this sample to this calendar or period based age");

                entity.Property(e => e.RelativeAgeId).HasColumnName("relative_age_id");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblRelativeDates)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_relative_dates_analysis_entity_id_fkey");

                entity.HasOne(d => d.DatingUncertainty)
                    .WithMany(p => p.TblRelativeDates)
                    .HasForeignKey(d => d.DatingUncertaintyId)
                    .HasConstraintName("tbl_relative_dates_dating_uncertainty_id_fkey");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblRelativeDates)
                    .HasForeignKey(d => d.MethodId)
                    .HasConstraintName("tbl_relative_dates_method_id_fkey");

                entity.HasOne(d => d.RelativeAge)
                    .WithMany(p => p.TblRelativeDates)
                    .HasForeignKey(d => d.RelativeAgeId)
                    .HasConstraintName("tbl_relative_dates_relative_age_id_fkey");
            });

            modelBuilder.Entity<TblSampleAltRefs>(entity =>
            {
                entity.HasKey(e => e.SampleAltRefId)
                    .HasName("pk_sample_alt_refs");

                entity.ToTable("tbl_sample_alt_refs");

                entity.Property(e => e.SampleAltRefId).HasColumnName("sample_alt_ref_id");

                entity.Property(e => e.AltRef)
                    .IsRequired()
                    .HasColumnName("alt_ref")
                    .HasMaxLength(60);

                entity.Property(e => e.AltRefTypeId).HasColumnName("alt_ref_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.AltRefType)
                    .WithMany(p => p.TblSampleAltRefs)
                    .HasForeignKey(d => d.AltRefTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_alt_refs_alt_ref_type_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleAltRefs)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_alt_refs_physical_sample_id");
            });

            modelBuilder.Entity<TblSampleColours>(entity =>
            {
                entity.HasKey(e => e.SampleColourId)
                    .HasName("pk_sample_colours");

                entity.ToTable("tbl_sample_colours");

                entity.Property(e => e.SampleColourId).HasColumnName("sample_colour_id");

                entity.Property(e => e.ColourId).HasColumnName("colour_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.Colour)
                    .WithMany(p => p.TblSampleColours)
                    .HasForeignKey(d => d.ColourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_colours_colour_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleColours)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_colours_physical_sample_id");
            });

            modelBuilder.Entity<TblSampleCoordinates>(entity =>
            {
                entity.HasKey(e => e.SampleCoordinateId)
                    .HasName("tbl_sample_coordinates_pkey");

                entity.ToTable("tbl_sample_coordinates");

                entity.Property(e => e.SampleCoordinateId)
                    .HasColumnName("sample_coordinate_id")
                    .HasDefaultValueSql("nextval('tbl_sample_coordinates_sample_coordinates_id_seq'::regclass)");

                entity.Property(e => e.Accuracy)
                    .HasColumnName("accuracy")
                    .HasColumnType("numeric(20,10)")
                    .HasComment("GPS type accuracy, e.g. 5m 10m 0.01m");

                entity.Property(e => e.CoordinateMethodDimensionId).HasColumnName("coordinate_method_dimension_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Measurement)
                    .HasColumnName("measurement")
                    .HasColumnType("numeric(20,10)");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.CoordinateMethodDimension)
                    .WithMany(p => p.TblSampleCoordinates)
                    .HasForeignKey(d => d.CoordinateMethodDimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_coordinates_coordinate_method_dimension_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleCoordinates)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_coordinates_physical_sample_id");
            });

            modelBuilder.Entity<TblSampleDescriptionSampleGroupContexts>(entity =>
            {
                entity.HasKey(e => e.SampleDescriptionSampleGroupContextId)
                    .HasName("tbl_sample_description_sample_group_contexts_pkey");

                entity.ToTable("tbl_sample_description_sample_group_contexts");

                entity.Property(e => e.SampleDescriptionSampleGroupContextId)
                    .HasColumnName("sample_description_sample_group_context_id")
                    .HasDefaultValueSql("nextval('tbl_sample_description_sample_sample_description_sample_gro_seq'::regclass)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SampleDescriptionTypeId).HasColumnName("sample_description_type_id");

                entity.Property(e => e.SamplingContextId).HasColumnName("sampling_context_id");

                entity.HasOne(d => d.SampleDescriptionType)
                    .WithMany(p => p.TblSampleDescriptionSampleGroupContexts)
                    .HasForeignKey(d => d.SampleDescriptionTypeId)
                    .HasConstraintName("fk_sample_description_types_sample_group_context_id");

                entity.HasOne(d => d.SamplingContext)
                    .WithMany(p => p.TblSampleDescriptionSampleGroupContexts)
                    .HasForeignKey(d => d.SamplingContextId)
                    .HasConstraintName("fk_sample_description_sample_group_contexts_sampling_context_id");
            });

            modelBuilder.Entity<TblSampleDescriptionTypes>(entity =>
            {
                entity.HasKey(e => e.SampleDescriptionTypeId)
                    .HasName("tbl_sample_description_types_pkey");

                entity.ToTable("tbl_sample_description_types");

                entity.Property(e => e.SampleDescriptionTypeId).HasColumnName("sample_description_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TypeDescription).HasColumnName("type_description");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TblSampleDescriptions>(entity =>
            {
                entity.HasKey(e => e.SampleDescriptionId)
                    .HasName("tbl_sample_descriptions_pkey");

                entity.ToTable("tbl_sample_descriptions");

                entity.Property(e => e.SampleDescriptionId).HasColumnName("sample_description_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.Property(e => e.SampleDescriptionTypeId).HasColumnName("sample_description_type_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleDescriptions)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_descriptions_physical_sample_id");

                entity.HasOne(d => d.SampleDescriptionType)
                    .WithMany(p => p.TblSampleDescriptions)
                    .HasForeignKey(d => d.SampleDescriptionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_descriptions_sample_description_type_id");
            });

            modelBuilder.Entity<TblSampleDimensions>(entity =>
            {
                entity.HasKey(e => e.SampleDimensionId)
                    .HasName("pk_sample_dimensions");

                entity.ToTable("tbl_sample_dimensions");

                entity.HasComment("20120506pib: depth measurements for samples moved here from tbl_physical_samples");

                entity.Property(e => e.SampleDimensionId).HasColumnName("sample_dimension_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DimensionId)
                    .HasColumnName("dimension_id")
                    .HasComment("details of the dimension measured");

                entity.Property(e => e.DimensionValue)
                    .HasColumnName("dimension_value")
                    .HasColumnType("numeric(20,10)")
                    .HasComment("numerical value of dimension, in the units indicated in the documentation and interface.");

                entity.Property(e => e.MethodId)
                    .HasColumnName("method_id")
                    .HasComment("method describing dimension measurement, with link to units used");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.TblSampleDimensions)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_sample_dimensions_dimension_id_fkey");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblSampleDimensions)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_sample_dimensions_method_id_fkey");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleDimensions)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_sample_dimensions_physical_sample_id_fkey");
            });

            modelBuilder.Entity<TblSampleGroupCoordinates>(entity =>
            {
                entity.HasKey(e => e.SampleGroupPositionId)
                    .HasName("tbl_sample_group_coordinates_pkey");

                entity.ToTable("tbl_sample_group_coordinates");

                entity.Property(e => e.SampleGroupPositionId).HasColumnName("sample_group_position_id");

                entity.Property(e => e.CoordinateMethodDimensionId).HasColumnName("coordinate_method_dimension_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.PositionAccuracy)
                    .HasColumnName("position_accuracy")
                    .HasMaxLength(128);

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.Property(e => e.SampleGroupPosition)
                    .HasColumnName("sample_group_position")
                    .HasColumnType("numeric(20,10)");

                entity.HasOne(d => d.CoordinateMethodDimension)
                    .WithMany(p => p.TblSampleGroupCoordinates)
                    .HasForeignKey(d => d.CoordinateMethodDimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_positions_coordinate_method_dimension_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblSampleGroupCoordinates)
                    .HasForeignKey(d => d.SampleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_positions_sample_group_id");
            });

            modelBuilder.Entity<TblSampleGroupDescriptionTypeSamplingContexts>(entity =>
            {
                entity.HasKey(e => e.SampleGroupDescriptionTypeSamplingContextId)
                    .HasName("tbl_sample_group_description_type_sample_contexts_pkey");

                entity.ToTable("tbl_sample_group_description_type_sampling_contexts");

                entity.Property(e => e.SampleGroupDescriptionTypeSamplingContextId)
                    .HasColumnName("sample_group_description_type_sampling_context_id")
                    .HasDefaultValueSql("nextval('tbl_sample_group_description__sample_group_desciption_sampl_seq'::regclass)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SampleGroupDescriptionTypeId).HasColumnName("sample_group_description_type_id");

                entity.Property(e => e.SamplingContextId).HasColumnName("sampling_context_id");

                entity.HasOne(d => d.SampleGroupDescriptionType)
                    .WithMany(p => p.TblSampleGroupDescriptionTypeSamplingContexts)
                    .HasForeignKey(d => d.SampleGroupDescriptionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_description_type_sampling_context_id");

                entity.HasOne(d => d.SamplingContext)
                    .WithMany(p => p.TblSampleGroupDescriptionTypeSamplingContexts)
                    .HasForeignKey(d => d.SamplingContextId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_sampling_context_id0");
            });

            modelBuilder.Entity<TblSampleGroupDescriptionTypes>(entity =>
            {
                entity.HasKey(e => e.SampleGroupDescriptionTypeId)
                    .HasName("tbl_sample_group_description_types_pkey");

                entity.ToTable("tbl_sample_group_description_types");

                entity.Property(e => e.SampleGroupDescriptionTypeId)
                    .HasColumnName("sample_group_description_type_id")
                    .HasDefaultValueSql("nextval('tbl_sample_group_description__sample_group_description_type_seq'::regclass)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TypeDescription)
                    .HasColumnName("type_description")
                    .HasColumnType("character varying");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TblSampleGroupDescriptions>(entity =>
            {
                entity.HasKey(e => e.SampleGroupDescriptionId)
                    .HasName("pk_sample_group_description_id");

                entity.ToTable("tbl_sample_group_descriptions");

                entity.Property(e => e.SampleGroupDescriptionId).HasColumnName("sample_group_description_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.GroupDescription)
                    .HasColumnName("group_description")
                    .HasColumnType("character varying");

                entity.Property(e => e.SampleGroupDescriptionTypeId).HasColumnName("sample_group_description_type_id");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.HasOne(d => d.SampleGroupDescriptionType)
                    .WithMany(p => p.TblSampleGroupDescriptions)
                    .HasForeignKey(d => d.SampleGroupDescriptionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_descriptions_sample_group_description_type_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblSampleGroupDescriptions)
                    .HasForeignKey(d => d.SampleGroupId)
                    .HasConstraintName("fk_sample_groups_sample_group_descriptions_id");
            });

            modelBuilder.Entity<TblSampleGroupDimensions>(entity =>
            {
                entity.HasKey(e => e.SampleGroupDimensionId)
                    .HasName("pk_sample_group_dimensions");

                entity.ToTable("tbl_sample_group_dimensions");

                entity.Property(e => e.SampleGroupDimensionId).HasColumnName("sample_group_dimension_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DimensionId).HasColumnName("dimension_id");

                entity.Property(e => e.DimensionValue)
                    .HasColumnName("dimension_value")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.HasOne(d => d.Dimension)
                    .WithMany(p => p.TblSampleGroupDimensions)
                    .HasForeignKey(d => d.DimensionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_dimensions_dimension_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblSampleGroupDimensions)
                    .HasForeignKey(d => d.SampleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_dimensions_sample_group_id");
            });

            modelBuilder.Entity<TblSampleGroupImages>(entity =>
            {
                entity.HasKey(e => e.SampleGroupImageId)
                    .HasName("pk_sample_group_images");

                entity.ToTable("tbl_sample_group_images");

                entity.Property(e => e.SampleGroupImageId).HasColumnName("sample_group_image_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageLocation)
                    .IsRequired()
                    .HasColumnName("image_location");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasMaxLength(80);

                entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.TblSampleGroupImages)
                    .HasForeignKey(d => d.ImageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_images_image_type_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblSampleGroupImages)
                    .HasForeignKey(d => d.SampleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_images_sample_group_id");
            });

            modelBuilder.Entity<TblSampleGroupNotes>(entity =>
            {
                entity.HasKey(e => e.SampleGroupNoteId)
                    .HasName("pk_sample_group_note_id");

                entity.ToTable("tbl_sample_group_notes");

                entity.Property(e => e.SampleGroupNoteId).HasColumnName("sample_group_note_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("character varying");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblSampleGroupNotes)
                    .HasForeignKey(d => d.SampleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_sample_group_notes_sample_groups");
            });

            modelBuilder.Entity<TblSampleGroupReferences>(entity =>
            {
                entity.HasKey(e => e.SampleGroupReferenceId)
                    .HasName("pk_sample_group_references");

                entity.ToTable("tbl_sample_group_references");

                entity.HasIndex(e => e.BiblioId)
                    .HasName("idx_biblio_id");

                entity.HasIndex(e => e.SampleGroupId)
                    .HasName("idx_sample_group_id");

                entity.Property(e => e.SampleGroupReferenceId).HasColumnName("sample_group_reference_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SampleGroupId)
                    .HasColumnName("sample_group_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblSampleGroupReferences)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_group_references_biblio_id");

                entity.HasOne(d => d.SampleGroup)
                    .WithMany(p => p.TblSampleGroupReferences)
                    .HasForeignKey(d => d.SampleGroupId)
                    .HasConstraintName("fk_sample_group_references_sample_group_id");
            });

            modelBuilder.Entity<TblSampleGroupSamplingContexts>(entity =>
            {
                entity.HasKey(e => e.SamplingContextId)
                    .HasName("pk_sample_group_sampling_contexts");

                entity.ToTable("tbl_sample_group_sampling_contexts");

                entity.HasComment("Type=lookup");

                entity.Property(e => e.SamplingContextId).HasColumnName("sampling_context_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("full explanation of the grouping term");

                entity.Property(e => e.SamplingContext)
                    .IsRequired()
                    .HasColumnName("sampling_context")
                    .HasMaxLength(80)
                    .HasComment("short but meaningful name defining sample group context, e.g. stratigraphic sequence, archaeological excavation");

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasComment("allows lists to group similar or associated group context close to each other, e.g. modern investigations together, palaeo investigations together");
            });

            modelBuilder.Entity<TblSampleGroups>(entity =>
            {
                entity.HasKey(e => e.SampleGroupId)
                    .HasName("pk_sample_groups");

                entity.ToTable("tbl_sample_groups");

                entity.Property(e => e.SampleGroupId).HasColumnName("sample_group_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId)
                    .HasColumnName("method_id")
                    .HasComment("sampling method, e.g. russian auger core, pitfall traps. note different from context in that it is specific to method of sample retrieval and not type of investigation.");

                entity.Property(e => e.SampleGroupDescription).HasColumnName("sample_group_description");

                entity.Property(e => e.SampleGroupName)
                    .HasColumnName("sample_group_name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying")
                    .HasComment("Name which identifies the collection of samples. For ceramics, use vessel number.");

                entity.Property(e => e.SamplingContextId).HasColumnName("sampling_context_id");

                entity.Property(e => e.SiteId)
                    .HasColumnName("site_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblSampleGroups)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_sample_groups_method_id_fkey");

                entity.HasOne(d => d.SamplingContext)
                    .WithMany(p => p.TblSampleGroups)
                    .HasForeignKey(d => d.SamplingContextId)
                    .HasConstraintName("tbl_sample_groups_sampling_context_id_fkey");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSampleGroups)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("tbl_sample_groups_site_id_fkey");
            });

            modelBuilder.Entity<TblSampleHorizons>(entity =>
            {
                entity.HasKey(e => e.SampleHorizonId)
                    .HasName("pk_sample_horizons");

                entity.ToTable("tbl_sample_horizons");

                entity.Property(e => e.SampleHorizonId).HasColumnName("sample_horizon_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.HorizonId).HasColumnName("horizon_id");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.Horizon)
                    .WithMany(p => p.TblSampleHorizons)
                    .HasForeignKey(d => d.HorizonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_horizons_horizon_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleHorizons)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_horizons_physical_sample_id");
            });

            modelBuilder.Entity<TblSampleImages>(entity =>
            {
                entity.HasKey(e => e.SampleImageId)
                    .HasName("pk_sample_images");

                entity.ToTable("tbl_sample_images");

                entity.Property(e => e.SampleImageId).HasColumnName("sample_image_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageLocation)
                    .IsRequired()
                    .HasColumnName("image_location");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasMaxLength(80);

                entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.TblSampleImages)
                    .HasForeignKey(d => d.ImageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_images_image_type_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleImages)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_images_physical_sample_id");
            });

            modelBuilder.Entity<TblSampleLocationTypeSamplingContexts>(entity =>
            {
                entity.HasKey(e => e.SampleLocationTypeSamplingContextId)
                    .HasName("tbl_sample_location_sampling_contexts_pkey");

                entity.ToTable("tbl_sample_location_type_sampling_contexts");

                entity.Property(e => e.SampleLocationTypeSamplingContextId)
                    .HasColumnName("sample_location_type_sampling_context_id")
                    .HasDefaultValueSql("nextval('tbl_sample_location_sampling__sample_location_type_sampling_seq'::regclass)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SampleLocationTypeId)
                    .HasColumnName("sample_location_type_id")
                    .HasDefaultValueSql("nextval('tbl_sample_location_sampling_contex_sample_location_type_id_seq'::regclass)");

                entity.Property(e => e.SamplingContextId).HasColumnName("sampling_context_id");

                entity.HasOne(d => d.SampleLocationType)
                    .WithMany(p => p.TblSampleLocationTypeSamplingContexts)
                    .HasForeignKey(d => d.SampleLocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_location_sampling_contexts_sampling_context_id");

                entity.HasOne(d => d.SamplingContext)
                    .WithMany(p => p.TblSampleLocationTypeSamplingContexts)
                    .HasForeignKey(d => d.SamplingContextId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_location_type_sampling_context_id");
            });

            modelBuilder.Entity<TblSampleLocationTypes>(entity =>
            {
                entity.HasKey(e => e.SampleLocationTypeId)
                    .HasName("tbl_sample_location_types_pkey");

                entity.ToTable("tbl_sample_location_types");

                entity.Property(e => e.SampleLocationTypeId).HasColumnName("sample_location_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LocationType)
                    .HasColumnName("location_type")
                    .HasMaxLength(255);

                entity.Property(e => e.LocationTypeDescription).HasColumnName("location_type_description");
            });

            modelBuilder.Entity<TblSampleLocations>(entity =>
            {
                entity.HasKey(e => e.SampleLocationId)
                    .HasName("tbl_sample_locations_pkey");

                entity.ToTable("tbl_sample_locations");

                entity.Property(e => e.SampleLocationId).HasColumnName("sample_location_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasMaxLength(255);

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.Property(e => e.SampleLocationTypeId).HasColumnName("sample_location_type_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleLocations)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_locations_physical_sample_id");

                entity.HasOne(d => d.SampleLocationType)
                    .WithMany(p => p.TblSampleLocations)
                    .HasForeignKey(d => d.SampleLocationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_locations_sample_location_type_id");
            });

            modelBuilder.Entity<TblSampleNotes>(entity =>
            {
                entity.HasKey(e => e.SampleNoteId)
                    .HasName("pk_sample_notes");

                entity.ToTable("tbl_sample_notes");

                entity.Property(e => e.SampleNoteId).HasColumnName("sample_note_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasComment("note contents");

                entity.Property(e => e.NoteType)
                    .HasColumnName("note_type")
                    .HasColumnType("character varying")
                    .HasComment("origin of the note, e.g. field note, lab note");

                entity.Property(e => e.PhysicalSampleId).HasColumnName("physical_sample_id");

                entity.HasOne(d => d.PhysicalSample)
                    .WithMany(p => p.TblSampleNotes)
                    .HasForeignKey(d => d.PhysicalSampleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sample_notes_physical_sample_id");
            });

            modelBuilder.Entity<TblSampleTypes>(entity =>
            {
                entity.HasKey(e => e.SampleTypeId)
                    .HasName("pk_sample_types");

                entity.ToTable("tbl_sample_types");

                entity.Property(e => e.SampleTypeId).HasColumnName("sample_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("type_name")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<TblSeasonOrQualifier>(entity =>
            {
                entity.HasKey(e => e.SeasonOrQualifierId)
                    .HasName("tbl_season_or_qualifier_pkey");

                entity.ToTable("tbl_season_or_qualifier");

                entity.Property(e => e.SeasonOrQualifierId).HasColumnName("season_or_qualifier_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.SeasonOrQualifierType)
                    .IsRequired()
                    .HasColumnName("season_or_qualifier_type")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<TblSeasonTypes>(entity =>
            {
                entity.HasKey(e => e.SeasonTypeId)
                    .HasName("pk_season_types");

                entity.ToTable("tbl_season_types");

                entity.Property(e => e.SeasonTypeId).HasColumnName("season_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.SeasonType)
                    .HasColumnName("season_type")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<TblSeasons>(entity =>
            {
                entity.HasKey(e => e.SeasonId)
                    .HasName("pk_seasons");

                entity.ToTable("tbl_seasons");

                entity.Property(e => e.SeasonId).HasColumnName("season_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SeasonName)
                    .HasColumnName("season_name")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.SeasonType)
                    .HasColumnName("season_type")
                    .HasMaxLength(30)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.SeasonTypeId).HasColumnName("season_type_id");

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.SeasonTypeNavigation)
                    .WithMany(p => p.TblSeasons)
                    .HasForeignKey(d => d.SeasonTypeId)
                    .HasConstraintName("fk_seasons_season_type_id");
            });

            modelBuilder.Entity<TblSiteImages>(entity =>
            {
                entity.HasKey(e => e.SiteImageId)
                    .HasName("pk_site_images");

                entity.ToTable("tbl_site_images");

                entity.Property(e => e.SiteImageId).HasColumnName("site_image_id");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.Credit)
                    .HasColumnName("credit")
                    .HasMaxLength(100);

                entity.Property(e => e.DateTaken)
                    .HasColumnName("date_taken")
                    .HasColumnType("date");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageLocation)
                    .IsRequired()
                    .HasColumnName("image_location");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasMaxLength(80);

                entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");

                entity.Property(e => e.SiteId).HasColumnName("site_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblSiteImages)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("fk_site_images_contact_id");

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.TblSiteImages)
                    .HasForeignKey(d => d.ImageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_site_images_image_type_id");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSiteImages)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_site_images_site_id");
            });

            modelBuilder.Entity<TblSiteLocations>(entity =>
            {
                entity.HasKey(e => e.SiteLocationId)
                    .HasName("pk_site_location");

                entity.ToTable("tbl_site_locations");

                entity.Property(e => e.SiteLocationId).HasColumnName("site_location_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.SiteId).HasColumnName("site_id");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblSiteLocations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_site_locations_location_id_fkey");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSiteLocations)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_site_locations_site_id_fkey");
            });

            modelBuilder.Entity<TblSiteNatgridrefs>(entity =>
            {
                entity.HasKey(e => e.SiteNatgridrefId)
                    .HasName("pk_sitenatgridrefs");

                entity.ToTable("tbl_site_natgridrefs");

                entity.HasComment("20120507pib: removed tbl_national_grids and trasfered storage of coordinate systems to tbl_methods");

                entity.Property(e => e.SiteNatgridrefId).HasColumnName("site_natgridref_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.MethodId)
                    .HasColumnName("method_id")
                    .HasComment("points to coordinate system.");

                entity.Property(e => e.Natgridref)
                    .IsRequired()
                    .HasColumnName("natgridref")
                    .HasColumnType("character varying");

                entity.Property(e => e.SiteId).HasColumnName("site_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.TblSiteNatgridrefs)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_site_natgridrefs_method_id");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSiteNatgridrefs)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_site_natgridrefs_sites_id");
            });

            modelBuilder.Entity<TblSiteOtherRecords>(entity =>
            {
                entity.HasKey(e => e.SiteOtherRecordsId)
                    .HasName("pk_site_other_records");

                entity.ToTable("tbl_site_other_records");

                entity.Property(e => e.SiteOtherRecordsId).HasColumnName("site_other_records_id");

                entity.Property(e => e.BiblioId)
                    .HasColumnName("biblio_id")
                    .HasComment("reference to publication containing data");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.RecordTypeId)
                    .HasColumnName("record_type_id")
                    .HasComment("reference to type of data (proxy)");

                entity.Property(e => e.SiteId).HasColumnName("site_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblSiteOtherRecords)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("tbl_site_other_records_biblio_id_fkey");

                entity.HasOne(d => d.RecordType)
                    .WithMany(p => p.TblSiteOtherRecords)
                    .HasForeignKey(d => d.RecordTypeId)
                    .HasConstraintName("tbl_site_other_records_record_type_id_fkey");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSiteOtherRecords)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("tbl_site_other_records_site_id_fkey");
            });

            modelBuilder.Entity<TblSitePreservationStatus>(entity =>
            {
                entity.HasKey(e => e.SitePreservationStatusId)
                    .HasName("pk_site_preservation_status");

                entity.ToTable("tbl_site_preservation_status");

                entity.Property(e => e.SitePreservationStatusId).HasColumnName("site_preservation_status_id");

                entity.Property(e => e.AssessmentAuthorContactId)
                    .HasColumnName("assessment_author_contact_id")
                    .HasComment("person or authority in tbl_contacts responsible for the assessment of preservation status and threat");

                entity.Property(e => e.AssessmentType)
                    .HasColumnName("assessment_type")
                    .HasColumnType("character varying")
                    .HasComment("type of assessment giving information on preservation status and threat, e.g. unesco report, archaeological survey");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasComment("brief description of site preservation status or threat to site preservation. include data here that does not fit in the other fields (for now - we may expand these features later if demand exists)");

                entity.Property(e => e.EvaluationDate)
                    .HasColumnName("Evaluation_date")
                    .HasColumnType("date")
                    .HasComment("Date of assessment, either formal or informal");

                entity.Property(e => e.PreservationStatusOrThreat)
                    .HasColumnName("preservation_status_or_threat")
                    .HasColumnType("character varying")
                    .HasComment(@"descriptive name for:
preservation status, e.g. (e.g. lost, damaged, threatened) or
main reason for potential or real risk to site (e.g. hydroelectric, oil exploitation, mining, forestry, climate change, erosion)");

                entity.Property(e => e.SiteId)
                    .HasColumnName("site_id")
                    .HasComment("allows multiple preservation/threat records per site");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSitePreservationStatus)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("fk_site_preservation_status_site_id ");
            });

            modelBuilder.Entity<TblSiteReferences>(entity =>
            {
                entity.HasKey(e => e.SiteReferenceId)
                    .HasName("pk_site_references");

                entity.ToTable("tbl_site_references");

                entity.Property(e => e.SiteReferenceId).HasColumnName("site_reference_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SiteId)
                    .HasColumnName("site_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblSiteReferences)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_site_references_biblio_id_fkey");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.TblSiteReferences)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("tbl_site_references_site_id_fkey");
            });

            modelBuilder.Entity<TblSites>(entity =>
            {
                entity.HasKey(e => e.SiteId)
                    .HasName("pk_sites");

                entity.ToTable("tbl_sites");

                entity.HasComment("Accuracy of highest location resolution level. E.g. Nearest settlement, lake, bog, ancient monument, approximate");

                entity.Property(e => e.SiteId).HasColumnName("site_id");

                entity.Property(e => e.Altitude)
                    .HasColumnName("altitude")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LatitudeDd)
                    .HasColumnName("latitude_dd")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.LongitudeDd)
                    .HasColumnName("longitude_dd")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.NationalSiteIdentifier)
                    .HasColumnName("national_site_identifier")
                    .HasMaxLength(255);

                entity.Property(e => e.SiteDescription)
                    .HasColumnName("site_description")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.SiteLocationAccuracy)
                    .HasColumnName("site_location_accuracy")
                    .HasColumnType("character varying")
                    .HasComment("Accuracy of highest location resolution level. E.g. Nearest settlement, lake, bog, ancient monument, approximate");

                entity.Property(e => e.SiteName)
                    .HasColumnName("site_name")
                    .HasMaxLength(60)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.SitePreservationStatusId).HasColumnName("site_preservation_status_id");
            });

            modelBuilder.Entity<TblSpeciesAssociationTypes>(entity =>
            {
                entity.HasKey(e => e.AssociationTypeId)
                    .HasName("tbl_association_types_pkey");

                entity.ToTable("tbl_species_association_types");

                entity.Property(e => e.AssociationTypeId)
                    .HasColumnName("association_type_id")
                    .HasDefaultValueSql("nextval('tbl_association_types_association_type_id_seq'::regclass)");

                entity.Property(e => e.AssociationDescription).HasColumnName("association_description");

                entity.Property(e => e.AssociationTypeName)
                    .HasColumnName("association_type_name")
                    .HasMaxLength(255);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<TblSpeciesAssociations>(entity =>
            {
                entity.HasKey(e => e.SpeciesAssociationId)
                    .HasName("pk_species_associations");

                entity.ToTable("tbl_species_associations");

                entity.Property(e => e.SpeciesAssociationId).HasColumnName("species_association_id");

                entity.Property(e => e.AssociatedTaxonId)
                    .HasColumnName("associated_taxon_id")
                    .HasComment("Taxon with which the primary taxon (taxon_id) is associated. ");

                entity.Property(e => e.AssociationTypeId)
                    .HasColumnName("association_type_id")
                    .HasComment("Type of association between primary taxon (taxon_id) and associated taxon. Note that the direction of the association is important in most cases (e.g. x predates on y)");

                entity.Property(e => e.BiblioId)
                    .HasColumnName("biblio_id")
                    .HasComment("Reference where relationship between taxa is described or mentioned");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ReferencingType).HasColumnName("referencing_type");

                entity.Property(e => e.TaxonId)
                    .HasColumnName("taxon_id")
                    .HasComment("Primary taxon in relationship, i.e. this taxon has x relationship with the associated taxon");

                entity.HasOne(d => d.AssociationType)
                    .WithMany(p => p.TblSpeciesAssociations)
                    .HasForeignKey(d => d.AssociationTypeId)
                    .HasConstraintName("tbl_species_associations_association_type_id_fkey");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblSpeciesAssociations)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("tbl_species_associations_biblio_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblSpeciesAssociations)
                    .HasForeignKey(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_species_associations_taxon_id_fkey");
            });

            modelBuilder.Entity<TblTaxaCommonNames>(entity =>
            {
                entity.HasKey(e => e.TaxonCommonNameId)
                    .HasName("pk_taxa_common_names");

                entity.ToTable("tbl_taxa_common_names");

                entity.Property(e => e.TaxonCommonNameId).HasColumnName("taxon_common_name_id");

                entity.Property(e => e.CommonName)
                    .HasColumnName("common_name")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TaxonId)
                    .HasColumnName("taxon_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.TblTaxaCommonNames)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("fk_taxa_common_names_language_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxaCommonNames)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("fk_taxa_common_names_taxon_id");
            });

            modelBuilder.Entity<TblTaxaImages>(entity =>
            {
                entity.HasKey(e => e.TaxaImagesId)
                    .HasName("tbl_taxa_images_pkey");

                entity.ToTable("tbl_taxa_images");

                entity.HasComment("20140226EE: changed the data type for date_updated");

                entity.Property(e => e.TaxaImagesId).HasColumnName("taxa_images_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ImageLocation).HasColumnName("image_location");

                entity.Property(e => e.ImageName)
                    .HasColumnName("image_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.TblTaxaImages)
                    .HasForeignKey(d => d.ImageTypeId)
                    .HasConstraintName("fk_taxa_images_image_type_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxaImages)
                    .HasForeignKey(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_taxa_images_taxa_tree_master_id");
            });

            modelBuilder.Entity<TblTaxaMeasuredAttributes>(entity =>
            {
                entity.HasKey(e => e.MeasuredAttributeId)
                    .HasName("pk_taxa_measured_attributes");

                entity.ToTable("tbl_taxa_measured_attributes");

                entity.Property(e => e.MeasuredAttributeId).HasColumnName("measured_attribute_id");

                entity.Property(e => e.AttributeMeasure)
                    .HasColumnName("attribute_measure")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.AttributeType)
                    .HasColumnName("attribute_type")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.AttributeUnits)
                    .HasColumnName("attribute_units")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("numeric(18,10)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxaMeasuredAttributes)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_taxa_measured_attributes_taxon_id_fkey");
            });

            modelBuilder.Entity<TblTaxaReferenceSpecimens>(entity =>
            {
                entity.HasKey(e => e.TaxaReferenceSpecimenId)
                    .HasName("tbl_taxa_reference_specimens_pkey");

                entity.ToTable("tbl_taxa_reference_specimens");

                entity.HasComment("20140226EE: changed date_updated to correct data type");

                entity.Property(e => e.TaxaReferenceSpecimenId).HasColumnName("taxa_reference_specimen_id");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.TblTaxaReferenceSpecimens)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_taxa_reference_specimens_contact_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxaReferenceSpecimens)
                    .HasForeignKey(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_taxa_reference_specimens_taxon_id");
            });

            modelBuilder.Entity<TblTaxaSeasonality>(entity =>
            {
                entity.HasKey(e => e.SeasonalityId)
                    .HasName("pk_taxa_seasonality");

                entity.ToTable("tbl_taxa_seasonality");

                entity.Property(e => e.SeasonalityId).HasColumnName("seasonality_id");

                entity.Property(e => e.ActivityTypeId).HasColumnName("activity_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LocationId)
                    .HasColumnName("location_id")
                    .HasComment("geographical relevance of seasonality data");

                entity.Property(e => e.SeasonId)
                    .HasColumnName("season_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.ActivityType)
                    .WithMany(p => p.TblTaxaSeasonality)
                    .HasForeignKey(d => d.ActivityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_taxa_seasonality_activity_type_id_fkey");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.TblTaxaSeasonality)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_taxa_seasonality_location_id_fkey");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.TblTaxaSeasonality)
                    .HasForeignKey(d => d.SeasonId)
                    .HasConstraintName("tbl_taxa_seasonality_season_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxaSeasonality)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_taxa_seasonality_taxon_id_fkey");
            });

            modelBuilder.Entity<TblTaxaSynonyms>(entity =>
            {
                entity.HasKey(e => e.SynonymId)
                    .HasName("pk_taxa_synonyms");

                entity.ToTable("tbl_taxa_synonyms");

                entity.Property(e => e.SynonymId).HasColumnName("synonym_id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FamilyId).HasColumnName("family_id");

                entity.Property(e => e.GenusId).HasColumnName("genus_id");

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasDefaultValueSql("NULL::character varying")
                    .HasComment("Any information useful to the history or usage of the synonym.");

                entity.Property(e => e.ReferenceType)
                    .HasColumnName("reference_type")
                    .HasColumnType("character varying")
                    .HasComment("Form of information relating to the synonym in the given bibliographic link, e.g. by use, definition, incorrect usage.");

                entity.Property(e => e.Synonym)
                    .HasColumnName("synonym")
                    .HasMaxLength(255)
                    .HasComment("Synonym at level defined by id level. I.e. if synonym is at genus level, then only the genus synonym is included here. Another synonym record is used for the species level synonym for the same taxon only if the name is different to that used in the master list.");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.TblTaxaSynonyms)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("fk_taxa_synonyms_taxa_tree_author_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTaxaSynonyms)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_taxa_synonyms_biblio_id");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.TblTaxaSynonyms)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("fk_taxa_synonyms_family_id");

                entity.HasOne(d => d.Genus)
                    .WithMany(p => p.TblTaxaSynonyms)
                    .HasForeignKey(d => d.GenusId)
                    .HasConstraintName("fk_taxa_synonyms_genus_id");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxaSynonyms)
                    .HasForeignKey(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_taxa_synonyms_taxon_id");
            });

            modelBuilder.Entity<TblTaxaTreeAuthors>(entity =>
            {
                entity.HasKey(e => e.AuthorId)
                    .HasName("pk_taxa_tree_authors");

                entity.ToTable("tbl_taxa_tree_authors");

                entity.HasIndex(e => e.AuthorName)
                    .HasName("tbl_taxa_tree_authors_name");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.AuthorName)
                    .HasColumnName("author_name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<TblTaxaTreeFamilies>(entity =>
            {
                entity.HasKey(e => e.FamilyId)
                    .HasName("pk_taxa_tree_families");

                entity.ToTable("tbl_taxa_tree_families");

                entity.HasIndex(e => e.FamilyName)
                    .HasName("tbl_taxa_tree_families_name");

                entity.HasIndex(e => e.OrderId)
                    .HasName("tbl_taxa_tree_families_order_id");

                entity.Property(e => e.FamilyId).HasColumnName("family_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FamilyName)
                    .HasColumnName("family_name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TblTaxaTreeFamilies)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_taxa_tree_families_order_id");
            });

            modelBuilder.Entity<TblTaxaTreeGenera>(entity =>
            {
                entity.HasKey(e => e.GenusId)
                    .HasName("pk_taxa_tree_genera");

                entity.ToTable("tbl_taxa_tree_genera");

                entity.HasIndex(e => e.FamilyId)
                    .HasName("tbl_taxa_tree_genera_family_id");

                entity.HasIndex(e => e.GenusName)
                    .HasName("tbl_taxa_tree_genera_name");

                entity.Property(e => e.GenusId).HasColumnName("genus_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FamilyId).HasColumnName("family_id");

                entity.Property(e => e.GenusName)
                    .HasColumnName("genus_name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.Family)
                    .WithMany(p => p.TblTaxaTreeGenera)
                    .HasForeignKey(d => d.FamilyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tbl_taxa_tree_genera_family_id_fkey");
            });

            modelBuilder.Entity<TblTaxaTreeMaster>(entity =>
            {
                entity.HasKey(e => e.TaxonId)
                    .HasName("pk_taxa_tree_master");

                entity.ToTable("tbl_taxa_tree_master");

                entity.HasComment("20130416PIB: removed default=0 for author_id and genus_id as was incorrect");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.GenusId).HasColumnName("genus_id");

                entity.Property(e => e.Species)
                    .HasColumnName("species")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.TblTaxaTreeMaster)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("tbl_taxa_tree_master_author_id_fkey");

                entity.HasOne(d => d.Genus)
                    .WithMany(p => p.TblTaxaTreeMaster)
                    .HasForeignKey(d => d.GenusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tbl_taxa_tree_master_genus_id_fkey");
            });

            modelBuilder.Entity<TblTaxaTreeOrders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("pk_taxa_tree_orders");

                entity.ToTable("tbl_taxa_tree_orders");

                entity.HasIndex(e => e.OrderId)
                    .HasName("tbl_taxa_tree_orders_order_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.OrderName)
                    .HasColumnName("order_name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.RecordTypeId).HasColumnName("record_type_id");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.HasOne(d => d.RecordType)
                    .WithMany(p => p.TblTaxaTreeOrders)
                    .HasForeignKey(d => d.RecordTypeId)
                    .HasConstraintName("fk_taxa_tree_orders_record_type_id");
            });

            modelBuilder.Entity<TblTaxonomicOrder>(entity =>
            {
                entity.HasKey(e => e.TaxonomicOrderId)
                    .HasName("pk_taxonomic_order");

                entity.ToTable("tbl_taxonomic_order");

                entity.HasIndex(e => e.TaxonId)
                    .HasName("tbl_taxonomic_order_taxon_id");

                entity.HasIndex(e => e.TaxonomicCode)
                    .HasName("tbl_taxonomic_order_taxonomic_code");

                entity.HasIndex(e => e.TaxonomicOrderId)
                    .HasName("tbl_taxonomic_order_taxonomic_order_id");

                entity.HasIndex(e => e.TaxonomicOrderSystemId)
                    .HasName("tbl_taxonomic_order_taxonomic_system_id");

                entity.Property(e => e.TaxonomicOrderId).HasColumnName("taxonomic_order_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonId)
                    .HasColumnName("taxon_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TaxonomicCode)
                    .HasColumnName("taxonomic_code")
                    .HasColumnType("numeric(18,10)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TaxonomicOrderSystemId)
                    .HasColumnName("taxonomic_order_system_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxonomicOrder)
                    .HasForeignKey(d => d.TaxonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tbl_taxonomic_order_taxon_id_fkey");

                entity.HasOne(d => d.TaxonomicOrderSystem)
                    .WithMany(p => p.TblTaxonomicOrder)
                    .HasForeignKey(d => d.TaxonomicOrderSystemId)
                    .HasConstraintName("tbl_taxonomic_order_taxonomic_order_system_id_fkey");
            });

            modelBuilder.Entity<TblTaxonomicOrderBiblio>(entity =>
            {
                entity.HasKey(e => e.TaxonomicOrderBiblioId)
                    .HasName("pk_taxonomic_order_biblio");

                entity.ToTable("tbl_taxonomic_order_biblio");

                entity.HasIndex(e => e.BiblioId)
                    .HasName("tbl_taxonomic_order_biblio_biblio_id");

                entity.HasIndex(e => e.TaxonomicOrderBiblioId)
                    .HasName("tbl_taxonomic_order_biblio_taxonomic_order_biblio_id");

                entity.HasIndex(e => e.TaxonomicOrderSystemId)
                    .HasName("tbl_taxonomic_order_biblio_taxonomic_order_system_id");

                entity.Property(e => e.TaxonomicOrderBiblioId).HasColumnName("taxonomic_order_biblio_id");

                entity.Property(e => e.BiblioId)
                    .HasColumnName("biblio_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonomicOrderSystemId)
                    .HasColumnName("taxonomic_order_system_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTaxonomicOrderBiblio)
                    .HasForeignKey(d => d.BiblioId)
                    .HasConstraintName("fk_taxonomic_order_biblio_biblio_id");

                entity.HasOne(d => d.TaxonomicOrderSystem)
                    .WithMany(p => p.TblTaxonomicOrderBiblio)
                    .HasForeignKey(d => d.TaxonomicOrderSystemId)
                    .HasConstraintName("fk_taxonomic_order_biblio_taxonomic_order_system_id");
            });

            modelBuilder.Entity<TblTaxonomicOrderSystems>(entity =>
            {
                entity.HasKey(e => e.TaxonomicOrderSystemId)
                    .HasName("pk_taxonomic_order_systems");

                entity.ToTable("tbl_taxonomic_order_systems");

                entity.HasIndex(e => e.TaxonomicOrderSystemId)
                    .HasName("tbl_taxonomic_order_systems_taxonomic_system_id");

                entity.Property(e => e.TaxonomicOrderSystemId).HasColumnName("taxonomic_order_system_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.SystemDescription).HasColumnName("system_description");

                entity.Property(e => e.SystemName)
                    .HasColumnName("system_name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<TblTaxonomyNotes>(entity =>
            {
                entity.HasKey(e => e.TaxonomyNotesId)
                    .HasName("pk_taxonomy_notes");

                entity.ToTable("tbl_taxonomy_notes");

                entity.Property(e => e.TaxonomyNotesId).HasColumnName("taxonomy_notes_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.Property(e => e.TaxonomyNotes).HasColumnName("taxonomy_notes");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTaxonomyNotes)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_taxonomy_notes_biblio_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTaxonomyNotes)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_taxonomy_notes_taxon_id_fkey");
            });

            modelBuilder.Entity<TblTemperatures>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("tbl_temperatures_pkey");

                entity.ToTable("tbl_temperatures");

                entity.Property(e => e.RecordId).HasColumnName("record_id");

                entity.Property(e => e.D180Gisp2)
                    .HasColumnName("d180_gisp2")
                    .HasColumnType("numeric");

                entity.Property(e => e.YearsBp).HasColumnName("years_bp");
            });

            modelBuilder.Entity<TblTephraDates>(entity =>
            {
                entity.HasKey(e => e.TephraDateId)
                    .HasName("pk_tephra_dates");

                entity.ToTable("tbl_tephra_dates");

                entity.HasComment("20130722PIB: Added field dating_uncertainty_id to cater for >< etc.");

                entity.Property(e => e.TephraDateId).HasColumnName("tephra_date_id");

                entity.Property(e => e.AnalysisEntityId).HasColumnName("analysis_entity_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DatingUncertaintyId).HasColumnName("dating_uncertainty_id");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.TephraId).HasColumnName("tephra_id");

                entity.HasOne(d => d.AnalysisEntity)
                    .WithMany(p => p.TblTephraDates)
                    .HasForeignKey(d => d.AnalysisEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tephra_dates_analysis_entity_id");

                entity.HasOne(d => d.DatingUncertainty)
                    .WithMany(p => p.TblTephraDates)
                    .HasForeignKey(d => d.DatingUncertaintyId)
                    .HasConstraintName("fk_tephra_dates_dating_uncertainty_id");

                entity.HasOne(d => d.Tephra)
                    .WithMany(p => p.TblTephraDates)
                    .HasForeignKey(d => d.TephraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tephra_dates_tephra_id");
            });

            modelBuilder.Entity<TblTephraRefs>(entity =>
            {
                entity.HasKey(e => e.TephraRefId)
                    .HasName("pk_tephra_refs");

                entity.ToTable("tbl_tephra_refs");

                entity.Property(e => e.TephraRefId).HasColumnName("tephra_ref_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TephraId).HasColumnName("tephra_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTephraRefs)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tephra_refs_biblio_id");

                entity.HasOne(d => d.Tephra)
                    .WithMany(p => p.TblTephraRefs)
                    .HasForeignKey(d => d.TephraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tephra_refs_tephra_id");
            });

            modelBuilder.Entity<TblTephras>(entity =>
            {
                entity.HasKey(e => e.TephraId)
                    .HasName("pk_tephras");

                entity.ToTable("tbl_tephras");

                entity.Property(e => e.TephraId).HasColumnName("tephra_id");

                entity.Property(e => e.C14Age)
                    .HasColumnName("c14_age")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.C14AgeOlder)
                    .HasColumnName("c14_age_older")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.C14AgeYounger)
                    .HasColumnName("c14_age_younger")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.CalAge)
                    .HasColumnName("cal_age")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.CalAgeOlder)
                    .HasColumnName("cal_age_older")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.CalAgeYounger)
                    .HasColumnName("cal_age_younger")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.TephraName)
                    .HasColumnName("tephra_name")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<TblTextBiology>(entity =>
            {
                entity.HasKey(e => e.BiologyId)
                    .HasName("pk_text_biology");

                entity.ToTable("tbl_text_biology");

                entity.Property(e => e.BiologyId).HasColumnName("biology_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.BiologyText).HasColumnName("biology_text");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTextBiology)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_text_biology_biblio_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTextBiology)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_text_biology_taxon_id_fkey");
            });

            modelBuilder.Entity<TblTextDistribution>(entity =>
            {
                entity.HasKey(e => e.DistributionId)
                    .HasName("pk_text_distribution");

                entity.ToTable("tbl_text_distribution");

                entity.Property(e => e.DistributionId).HasColumnName("distribution_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.DistributionText).HasColumnName("distribution_text");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTextDistribution)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_text_distribution_biblio_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTextDistribution)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_text_distribution_taxon_id_fkey");
            });

            modelBuilder.Entity<TblTextIdentificationKeys>(entity =>
            {
                entity.HasKey(e => e.KeyId)
                    .HasName("pk_text_identification_keys");

                entity.ToTable("tbl_text_identification_keys");

                entity.Property(e => e.KeyId).HasColumnName("key_id");

                entity.Property(e => e.BiblioId).HasColumnName("biblio_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.KeyText).HasColumnName("key_text");

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");

                entity.HasOne(d => d.Biblio)
                    .WithMany(p => p.TblTextIdentificationKeys)
                    .HasForeignKey(d => d.BiblioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_text_identification_keys_biblio_id_fkey");

                entity.HasOne(d => d.Taxon)
                    .WithMany(p => p.TblTextIdentificationKeys)
                    .HasForeignKey(d => d.TaxonId)
                    .HasConstraintName("tbl_text_identification_keys_taxon_id_fkey");
            });

            modelBuilder.Entity<TblUnits>(entity =>
            {
                entity.HasKey(e => e.UnitId)
                    .HasName("pk_units");

                entity.ToTable("tbl_units");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.UnitAbbrev)
                    .HasColumnName("unit_abbrev")
                    .HasMaxLength(15);

                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasColumnName("unit_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblUpdatesLog>(entity =>
            {
                entity.HasKey(e => e.UpdatesLogId)
                    .HasName("pk_updates_log");

                entity.ToTable("tbl_updates_log");

                entity.Property(e => e.UpdatesLogId)
                    .HasColumnName("updates_log_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("last_updated")
                    .HasColumnType("date");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnName("table_name")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<TblYearsTypes>(entity =>
            {
                entity.HasKey(e => e.YearsTypeId)
                    .HasName("tbl_years_types_pkey");

                entity.ToTable("tbl_years_types");

                entity.Property(e => e.YearsTypeId).HasColumnName("years_type_id");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ViewTaxaAlphabetically>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("view_taxa_alphabetically");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(100);

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.Family)
                    .HasColumnName("family")
                    .HasMaxLength(100);

                entity.Property(e => e.FamilyId).HasColumnName("family_id");

                entity.Property(e => e.Genus)
                    .HasColumnName("genus")
                    .HasMaxLength(100);

                entity.Property(e => e.GenusId).HasColumnName("genus_id");

                entity.Property(e => e.Order)
                    .HasColumnName("order")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Species)
                    .HasColumnName("species")
                    .HasMaxLength(255);

                entity.Property(e => e.TaxonId).HasColumnName("taxon_id");
            });

            modelBuilder.HasSequence("tbl_sample_geometry_sample_geometry_id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
