using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVS.ViewModels.Measurements;
using BVS.ViewModels.SubMeasurements;
using BVS.DataModels;

namespace BVS.Controllers
{
    [Authorize]
    public class MeasurementController : BVSController
    {
        public ActionResult Details(int id)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;

            var measurementViewModel = new MeasurementViewModel
            {
                Measurement = BVSBusinessServices.Measurements.GetByID( id ),
                SubMeasurements = new List<SubMeasurementGridViewModel>()
            };

            measurementViewModel.Measurement.MeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( measurementViewModel.Measurement.MeasurementOn, timeZoneInfo );
            var SubMeasurements = BVSBusinessServices.SubMeasurements.GetByMeasurementID( id );

            foreach ( SubMeasurement measurement in SubMeasurements )
            {
                var u = new SubMeasurementGridViewModel()
                {
                    ID = measurement.ID,
                    CalculatedVolume = measurement.CalculatedVolume,
                    MeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( measurement.MeasurementOn, timeZoneInfo ),
                    MeasurementID = measurement.MeasurementID,
                    IsVoided = measurement.IsVoided
                };

                measurementViewModel.SubMeasurements.Add( u );
            }

            return View( measurementViewModel );
        }
    }
}