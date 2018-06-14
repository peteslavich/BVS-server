using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;
using BVS.ViewModels.SubMeasurements;

namespace BVS.ViewModels.Measurements
{
    public class MeasurementViewModel
    {
        public BVS.DataModels.Measurement Measurement { get; set; }
        public List<SubMeasurementGridViewModel> SubMeasurements { get; set; }
    }
}