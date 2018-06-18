using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;

namespace BVS.DataModels
{
    [Table( "Measurement" )]
    public partial class Measurement
    {
        [Display( Name = "Calculated Volume" )]
        public int? CalculatedVolume { get; set; }

        public Guid ClientGUID { get; set; }
        public int CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ID { get; set; }
        public bool? IsPatientRatingThumbsDown { get; set; }
        public bool? IsPatientRatingThumbsUp { get; set; }
        public bool IsVoided { get; set; }

        [Display( Name = "Measurement DateTime" )]
        public DateTime MeasurementOn { get; set; }

        [JsonIgnore]
        public virtual Patient Patient { get; set; }

        [Display( Name = "Patient Feedback" )]
        public string PatientFeedback { get; set; }

        public int PatientID { get; set; }

        [Display( Name = "Patient Rating" )]
        public int? PatientRating { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        public virtual ICollection<SubMeasurement> SubMeasurements { get; set; }

        public int UpdatedByID { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}