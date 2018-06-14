using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BVS.DataModels;

namespace BVS.APIControllers
{
    public class SubMeasurementController : BVSAPIController
    {
        [HttpGet]
        public IEnumerable<SubMeasurement> GetByMeasurementID(int ID)
        {
            return BVSBusinessServices.SubMeasurements.GetByMeasurementID( ID );
        }
    }
}