using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using BVS.Attributes;

namespace BVS.APIControllers
{
    public class PatientController : BVSAPIController
    {
        [HttpGet]
        [BasicAuthorize]
        public HttpResponseMessage Login()
        {
            var headerValue = String.Empty;

            if ( Request.Headers.TryGetValues( "Authorization", out IEnumerable<string> headerValues ) )
            {
                headerValue = headerValues.FirstOrDefault();
            }

            var credentials = "";
            if ( headerValue != String.Empty )
            {
                credentials = Encoding.UTF8.GetString( Convert.FromBase64String( headerValue.Substring( 6 ).Trim() ) );
            }

            //bool isAuthenticated = false;

            int index = credentials.IndexOf( ':' );
            if ( index > -1 )
            {
                string username = credentials.Substring( 0, index );
                string password = credentials.Substring( index + 1, credentials.Length - (index + 1) );

                var userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
                var user = userManager.FindByName( username );
                var patient = BVSBusinessServices.Patients.GetAll().Where( x => x.MembershipProviderUserID == user.Id ).FirstOrDefault();

                if ( patient != null )
                {
                    return Request.CreateResponse( HttpStatusCode.OK, new { patientID = patient.ID, firstName = patient.FirstName, lastName = patient.LastName, emailAddress = patient.EmailAddress } );
                }
            }

            return Request.CreateErrorResponse( HttpStatusCode.Unauthorized, "No patient record found." );
        }
    }
}