using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SQT.Infrastructure;
using SQT.Mocks;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class StudyDbContext : JsonSeededFacetContext
    {

        public StudyDbContext(DbContextOptions<StudyDbContext> options, JsonFacetContextFixture fixture)
            : base(options, fixture)
        {
        }

        public virtual DbSet<Cohort> Cohort { get; set; }
        public virtual DbSet<CohortDescription> CohortDescription { get; set; }
        public virtual DbSet<Experiment> Experiment { get; set; }
        public virtual DbSet<Method> Method { get; set; }
        public virtual DbSet<MethodType> MethodType { get; set; }
        public virtual DbSet<Observable> Observable { get; set; }
        public virtual DbSet<ObservedCount> ObservedCount { get; set; }
        public virtual DbSet<ObservedMagnitude> ObservedMagnitude { get; set; }
        public virtual DbSet<ObservedSpan> ObservedSpan { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectProperty> ProjectProperty { get; set; }
        public virtual DbSet<ProjectReport> ProjectReport { get; set; }
        public virtual DbSet<ProjectResidence> ProjectResidence { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Residence> Residence { get; set; }
        public virtual DbSet<ResidenceType> ResidenceType { get; set; }
        public virtual DbSet<Study> Study { get; set; }
        public virtual DbSet<StudyType> StudyType { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectNote> SubjectNote { get; set; }
        public virtual DbSet<SubjectType> SubjectType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cohort>(entity =>
            {
                entity.ToTable("cohort");

                entity.Property(e => e.CohortId).HasColumnName("cohort_id");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.Cohort)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cohort_method_id_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Cohort)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("cohort_project_id_fkey");
            });

            modelBuilder.Entity<CohortDescription>(entity =>
            {
                entity.ToTable("cohort_description");

                entity.Property(e => e.CohortDescriptionId).HasColumnName("cohort_description_id");

                entity.Property(e => e.CohortId).HasColumnName("cohort_id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Cohort)
                    .WithMany(p => p.CohortDescription)
                    .HasForeignKey(d => d.CohortId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cohort_description_cohort_id_fkey");
            });

            modelBuilder.Entity<Experiment>(entity =>
            {
                entity.ToTable("experiment");

                entity.Property(e => e.ExperimentId).HasColumnName("experiment_id");

                entity.Property(e => e.StudyId).HasColumnName("study_id");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.HasOne(d => d.Study)
                    .WithMany(p => p.Experiment)
                    .HasForeignKey(d => d.StudyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("experiment_study_id_fkey");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Experiment)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("experiment_subject_id_fkey");
            });

            modelBuilder.Entity<Method>(entity =>
            {
                entity.ToTable("method");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.MethodName)
                    .IsRequired()
                    .HasColumnName("method_name")
                    .HasMaxLength(50);

                entity.Property(e => e.MethodTypeId).HasColumnName("method_type_id");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.HasOne(d => d.MethodType)
                    .WithMany(p => p.Method)
                    .HasForeignKey(d => d.MethodTypeId)
                    .HasConstraintName("method_method_type_id_fkey");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.Method)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("method_report_id_fkey");
            });

            modelBuilder.Entity<MethodType>(entity =>
            {
                entity.ToTable("method_type");

                entity.Property(e => e.MethodTypeId)
                    .HasColumnName("method_type_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<Observable>(entity =>
            {
                entity.ToTable("observable");

                entity.Property(e => e.ObservableId).HasColumnName("observable_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("numeric(20,10)");
            });

            modelBuilder.Entity<ObservedCount>(entity =>
            {
                entity.ToTable("observed_count");

                entity.Property(e => e.ObservedCountId).HasColumnName("observed_count_id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.ExperimentId).HasColumnName("experiment_id");

                entity.Property(e => e.ObservableId).HasColumnName("observable_id");

                entity.HasOne(d => d.Experiment)
                    .WithMany(p => p.ObservedCount)
                    .HasForeignKey(d => d.ExperimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_count_experiment_id_fkey");

                entity.HasOne(d => d.Observable)
                    .WithMany(p => p.ObservedCount)
                    .HasForeignKey(d => d.ObservableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_count_observable_id_fkey");
            });

            modelBuilder.Entity<ObservedMagnitude>(entity =>
            {
                entity.ToTable("observed_magnitude");

                entity.Property(e => e.ObservedMagnitudeId).HasColumnName("observed_magnitude_id");

                entity.Property(e => e.ExperimentId).HasColumnName("experiment_id");

                entity.Property(e => e.ObservableId).HasColumnName("observable_id");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("numeric(20,10)");

                entity.HasOne(d => d.Experiment)
                    .WithMany(p => p.ObservedMagnitude)
                    .HasForeignKey(d => d.ExperimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_magnitude_experiment_id_fkey");

                entity.HasOne(d => d.Observable)
                    .WithMany(p => p.ObservedMagnitude)
                    .HasForeignKey(d => d.ObservableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_magnitude_observable_id_fkey");
            });

            modelBuilder.Entity<ObservedSpan>(entity =>
            {
                entity.ToTable("observed_span");

                entity.Property(e => e.ObservedSpanId).HasColumnName("observed_span_id");

                entity.Property(e => e.ExperimentId).HasColumnName("experiment_id");

                entity.Property(e => e.High)
                    .HasColumnName("high")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.Low)
                    .HasColumnName("low")
                    .HasColumnType("numeric(20,5)");

                entity.Property(e => e.ObservableId).HasColumnName("observable_id");

                entity.HasOne(d => d.Experiment)
                    .WithMany(p => p.ObservedSpan)
                    .HasForeignKey(d => d.ExperimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_span_experiment_id_fkey");

                entity.HasOne(d => d.Observable)
                    .WithMany(p => p.ObservedSpan)
                    .HasForeignKey(d => d.ObservableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_span_observable_id_fkey");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.LatitudeDd)
                    .HasColumnName("latitude_dd")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.LongitudeDd)
                    .HasColumnName("longitude_dd")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(60)
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ProjectTypeId).HasColumnName("project_type_id");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("project_project_type_id_fkey");
            });

            modelBuilder.Entity<ProjectProperty>(entity =>
            {
                entity.ToTable("project_property");

                entity.Property(e => e.ProjectPropertyId).HasColumnName("project_property_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PropertyName)
                    .HasColumnName("property_name")
                    .HasMaxLength(80);

                entity.Property(e => e.PropertyValue).HasColumnName("property_value");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectProperty)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("project_property_project_id_fkey");
            });

            modelBuilder.Entity<ProjectReport>(entity =>
            {
                entity.ToTable("project_report");

                entity.Property(e => e.ProjectReportId).HasColumnName("project_report_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReportId)
                    .HasColumnName("report_id")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectReport)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("project_report_project_id_fkey");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.ProjectReport)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("project_report_report_id_fkey");
            });

            modelBuilder.Entity<ProjectResidence>(entity =>
            {
                entity.ToTable("project_residence");

                entity.Property(e => e.ProjectResidenceId).HasColumnName("project_residence_id");

                entity.Property(e => e.LatitudeDd)
                    .HasColumnName("latitude_dd")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.LongitudeDd)
                    .HasColumnName("longitude_dd")
                    .HasColumnType("numeric(18,10)");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ResidenceId).HasColumnName("residence_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectResidence)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("project_residence_project_id_fkey");

                entity.HasOne(d => d.Residence)
                    .WithMany(p => p.ProjectResidence)
                    .HasForeignKey(d => d.ResidenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("project_residence_residence_id_fkey");
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("project_type");

                entity.Property(e => e.ProjectTypeId).HasColumnName("project_type_id");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasMaxLength(60)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasColumnType("character varying");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("character varying");

                entity.Property(e => e.Year)
                    .HasColumnName("year")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<Residence>(entity =>
            {
                entity.ToTable("residence");

                entity.Property(e => e.ResidenceId).HasColumnName("residence_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ResidenceTypeId).HasColumnName("residence_type_id");

                entity.HasOne(d => d.ResidenceType)
                    .WithMany(p => p.Residence)
                    .HasForeignKey(d => d.ResidenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("residence_residence_type_id_fkey");
            });

            modelBuilder.Entity<ResidenceType>(entity =>
            {
                entity.ToTable("residence_type");

                entity.Property(e => e.ResidenceTypeId).HasColumnName("residence_type_id");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<Study>(entity =>
            {
                entity.ToTable("study");

                entity.Property(e => e.StudyId).HasColumnName("study_id");

                entity.Property(e => e.MethodId).HasColumnName("method_id");

                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.StudyName)
                    .IsRequired()
                    .HasColumnName("study_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StudyTypeId).HasColumnName("study_type_id");

                entity.HasOne(d => d.Method)
                    .WithMany(p => p.Study)
                    .HasForeignKey(d => d.MethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("study_method_id_fkey");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.Study)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("study_report_id_fkey");

                entity.HasOne(d => d.StudyType)
                    .WithMany(p => p.Study)
                    .HasForeignKey(d => d.StudyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("study_study_type_id_fkey");
            });

            modelBuilder.Entity<StudyType>(entity =>
            {
                entity.ToTable("study_type");

                entity.Property(e => e.StudyTypeId).HasColumnName("study_type_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(25)
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subject");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.CohortId).HasColumnName("cohort_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.SubjectTypeId).HasColumnName("subject_type_id");

                entity.HasOne(d => d.Cohort)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.CohortId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subject_cohort_id_fkey");

                entity.HasOne(d => d.SubjectType)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.SubjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subject_subject_type_id_fkey");
            });

            modelBuilder.Entity<SubjectNote>(entity =>
            {
                entity.ToTable("subject_note");

                entity.Property(e => e.SubjectNoteId).HasColumnName("subject_note_id");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note");

                entity.Property(e => e.NoteType)
                    .HasColumnName("note_type")
                    .HasColumnType("character varying");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectNote)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subject_note_subject_id_fkey");
            });

            modelBuilder.Entity<SubjectType>(entity =>
            {
                entity.ToTable("subject_type");

                entity.Property(e => e.SubjectTypeId).HasColumnName("subject_type_id");

                entity.Property(e => e.Description).HasColumnName("description");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
