using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PMSAPI.Models
{
    public partial class PMSWEBContext : DbContext
    {
        public PMSWEBContext()
        {
        }

        public PMSWEBContext(DbContextOptions<PMSWEBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<BloodGroups> BloodGroups { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<HospitalEmployee> HospitalEmployee { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=ABHIS;Database=PMSWEB;
Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ConsultationFees)
                    .HasColumnName("Consultation_Fees")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DoctorId)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.PatientId)
                    .HasColumnName("PatientID")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Appointme__Depar__17036CC0");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK__Appointme__Docto__17F790F9");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK__Appointme__Patie__160F4887");
            });

            modelBuilder.Entity<BloodGroups>(entity =>
            {
                entity.HasKey(e => e.BloodGroupId)
                    .HasName("PK__BloodGro__4398C68FF586E1DA");

                entity.Property(e => e.BloodGroup)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DepartmentId)
                    .HasName("PK__Departme__B2079BED873EC95B");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.DoctorId)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ConsultationFees)
                    .HasColumnName("Consultation_Fees")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Qualification)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Specializations)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Doctor)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Doctor__Departme__1920BF5C");
            });

            modelBuilder.Entity<HospitalEmployee>(entity =>
            {
                entity.HasKey(e => e.HemployeeId)
                    .HasName("PK__Hospital__50ADCB043FF5124D");

                entity.ToTable("Hospital_Employee");

                entity.Property(e => e.HemployeeId)
                    .HasColumnName("HEmployeeId")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.HemployeeName)
                    .IsRequired()
                    .HasColumnName("HEmployeeName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId)
                    .HasColumnName("PatientID")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Gender)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.BloodGroup)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.BloodGroupId)
                    .HasConstraintName("FK__Patient__BloodGr__656C112C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
