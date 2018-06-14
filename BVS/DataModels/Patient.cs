using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BVS.DataModels
{
    [Table( "Patient" )]
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors" )]
        public Patient()
        {
            DoctorPatients = new HashSet<DoctorPatient>();
            DoctorPatients1 = new HashSet<DoctorPatient>();
            Measurements = new HashSet<Measurement>();
        }

        public int CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        [JsonIgnore]
        public virtual ICollection<DoctorPatient> DoctorPatients { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        [JsonIgnore]
        public virtual ICollection<DoctorPatient> DoctorPatients1 { get; set; }

        [Required]
        [StringLength( 100 )]
        [Display( Name = "Email Address" )]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength( 100 )]
        [Display( Name = "First Name" )]
        public string FirstName { get; set; }

        public int ID { get; set; }
        public bool IsVoided { get; set; }

        [Required]
        [StringLength( 100 )]
        [Display( Name = "Last Name" )]
        public string LastName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        [JsonIgnore]
        public virtual ICollection<Measurement> Measurements { get; set; }

        public string MembershipProviderUserID { get; set; }

        [Display( Name = "Notes" )]
        public string Notes { get; set; }

        public int UpdatedByID { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}