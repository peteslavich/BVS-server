using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BVS.DataModels;

namespace BVS.BusinessServices
{
    public partial class BVSContext : DbContext
    {
        public BVSContext()
            : base( "name=BVSContext" )
        {
        }

        public virtual DbSet<DoctorPatient> DoctorPatients { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<SubMeasurement> SubMeasurements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Measurement>()
                .Property( e => e.PatientFeedback )
                .IsUnicode( false );

            modelBuilder.Entity<Measurement>()
                .HasMany( e => e.SubMeasurements )
                .WithRequired( e => e.Measurement )
                .WillCascadeOnDelete( false );

            modelBuilder.Entity<Patient>()
                .Property( e => e.FirstName )
                .IsUnicode( false );

            modelBuilder.Entity<Patient>()
                .Property( e => e.LastName )
                .IsUnicode( false );

            modelBuilder.Entity<Patient>()
                .HasMany( e => e.DoctorPatients )
                .WithRequired( e => e.User )
                .HasForeignKey( e => e.DoctorUserID )
                .WillCascadeOnDelete( false );

            modelBuilder.Entity<Patient>()
                .HasMany( e => e.DoctorPatients1 )
                .WithRequired( e => e.User1 )
                .HasForeignKey( e => e.PatientUserID )
                .WillCascadeOnDelete( false );

            modelBuilder.Entity<Patient>()
                .HasMany( e => e.Measurements )
                .WithRequired( e => e.Patient )
                .WillCascadeOnDelete( false );
        }
    }
}