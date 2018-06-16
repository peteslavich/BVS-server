using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Transactions;

using BVS.BusinessServices;
using BVS.DataModels;
using BVS.DomainModels;
using BVS.Attributes;

namespace BVS.APIControllers
{
    public class MeasurementController : BVSAPIController
    {
        // GET api/Measurement/5
        [HttpGet]
        [BasicAuthorize]
        public Measurement Get(int id)
        {
            return BVSBusinessServices.Measurements.GetByID( id );
        }

        // GET api/<controller>
        //[HttpGet]
        //public IEnumerable<Measurement> GetByPatientID(int ID)
        //{
        //    return BVSBusinessServices.Measurements.GetByPatientID( ID );
        //}

        // POST api/<controller>
        [HttpPost]
        [BasicAuthorize]
        public HttpResponseMessage Post([FromBody]MeasurementModel value)
        {
            //IEnumerable<string> headerValues;
            //var headerValue = String.Empty;

            //if ( Request.Headers.TryGetValues( "Authorization", out headerValues ) )
            //{
            //    headerValue = headerValues.FirstOrDefault();
            //}

            //var credentials = "";
            //if ( headerValue != String.Empty )
            //{
            //    credentials = Encoding.UTF8.GetString( Convert.FromBase64String( headerValue.Substring( 6 ).Trim() ) );
            //}

            bool isUpdate = false;

            Measurement measurement;

            if ( value.ID > 0 )
            {
                measurement = BVSBusinessServices.Measurements.GetByID( value.ID );
                isUpdate = true;
            }
            else
            {
                measurement = new Measurement
                {
                    MeasurementOn = value.MeasurementOn,
                    CalculatedVolume = value.CalculatedVolume,
                    PatientID = value.PatientID,
                    ClientGUID = value.ClientGUID
                };
            }

            measurement.PatientFeedback = value.PatientFeedback;
            measurement.PatientRating = value.PatientRating;
            measurement.IsPatientRatingThumbsUp = value.IsPatientRatingThumbsUp;
            measurement.IsPatientRatingThumbsDown = value.IsPatientRatingThumbsDown;

            BusinessServiceOperationResult result;

            result = BVSBusinessServices.Measurements.Save( measurement );

            if ( !result.HasErrors && !isUpdate )
            {
                if ( value.SubMeasurements != null )
                {
                    foreach ( var subMeasurement in value.SubMeasurements )
                    {
                        subMeasurement.MeasurementID = measurement.ID;
                        result = BVSBusinessServices.SubMeasurements.Save( subMeasurement );
                        if ( result.HasErrors )
                        {
                            break;
                        }
                    }
                }
            }

            if ( !result.HasErrors )
            {
                if ( isUpdate )
                {
                    return Request.CreateResponse( HttpStatusCode.OK, new { id = measurement.ID, clientGUID = measurement.ClientGUID, updatedOn = measurement.UpdatedOn } );
                }
                else
                {
                    return Request.CreateResponse( HttpStatusCode.OK, new { id = measurement.ID, clientGUID = measurement.ClientGUID, createdOn = measurement.CreatedOn } );
                }
            }
            else
            {
                HttpResponseMessage response = Request.CreateResponse( HttpStatusCode.BadRequest, "value" );
                response.Content = new StringContent( String.Join( "; ", result.Errors ), Encoding.Unicode );

                return response;
            }
        }
    }
}