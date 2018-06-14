using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BVS.ViewModels.Measurements
{
    public class MeasurementGridViewModel
    {
        public decimal? CalculatedVolume { get; set; }
        public int ID { get; set; }
        public bool IsVoided { get; set; }
        public DateTime MeasurementOn { get; set; }
        public string PatientFeedback { get; set; }
        public int PatientID { get; set; }
        public int? PatientRating { get; set; }
    }
}