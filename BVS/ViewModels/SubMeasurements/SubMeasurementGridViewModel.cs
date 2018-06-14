using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BVS.ViewModels.SubMeasurements
{
    public class SubMeasurementGridViewModel
    {
        public decimal? CalculatedVolume { get; set; }

        public int ID { get; set; }

        public bool IsVoided { get; set; }

        public int MeasurementID { get; set; }

        public DateTime MeasurementOn { get; set; }
    }
}