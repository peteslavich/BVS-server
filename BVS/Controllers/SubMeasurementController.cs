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
            var viewModel = new SubMeasurementViewModel
            {
                SubMeasurement = BVSBusinessServices.SubMeasurements.GetByID( id )
            };

            return View( viewModel );
        }
    }
}