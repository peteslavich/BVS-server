using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVS.ViewModels.Patients
{
    public class PatientChartViewModel : PatientViewModel
    {
        public int ChartWidth { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
    }
}