using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BVS.DataModels
{
    [Table( "DoctorPatient" )]
    public partial class DoctorPatient
    {
        public int CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int DoctorUserID { get; set; }
        public int ID { get; set; }
        public bool IsVoided { get; set; }
        public int PatientUserID { get; set; }
        public int UpdatedByID { get; set; }
        public DateTime UpdatedOn { get; set; }
        public virtual Patient User { get; set; }

        public virtual Patient User1 { get; set; }
    }
}