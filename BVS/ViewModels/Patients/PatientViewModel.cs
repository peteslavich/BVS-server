using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;
using BVS.ViewModels.Measurements;

namespace BVS.ViewModels.Patients
{
    public class PatientViewModel
    {
        public Patient Patient { get; set; }
        public List<MeasurementGridViewModel> PatientMeasurements { get; set; }
    }
}