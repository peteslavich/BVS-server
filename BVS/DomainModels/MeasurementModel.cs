using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;

namespace BVS.DomainModels
{
    public class MeasurementModel
    {
        public decimal? CalculatedVolume { get; set; }
        public Guid ClientGUID { get; set; }
        public int ID { get; set; }
        public bool? IsPatientRatingThumbsDown { get; set; }
        public bool? IsPatientRatingThumbsUp { get; set; }
        public DateTime MeasurementOn { get; set; }
        public string PatientFeedback { get; set; }
        public int PatientID { get; set; }
        public int? PatientRating { get; set; }
        public List<SubMeasurement> SubMeasurements { get; set; }
    }
}