using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVS.DataModels;
using BVS.ViewModels.SubMeasurements;
using BVS.BusinessServices;

namespace BVS.Controllers
{
    [Authorize]
    public class SubMeasurementController : BVSController
    {
        public ActionResult Details(int id)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;

            var viewModel = new SubMeasurementViewModel
            {
                SubMeasurement = BVSBusinessServices.SubMeasurements.GetByID( id )
            };
            viewModel.SubMeasurement.MeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( viewModel.SubMeasurement.MeasurementOn, timeZoneInfo );

            viewModel.SubMeasurement.Measurement.MeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( viewModel.SubMeasurement.Measurement.MeasurementOn, timeZoneInfo );
            return View( viewModel );
        }
    }
}