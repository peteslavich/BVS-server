using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity.Owin;

namespace BVS.Attributes
{
    public class BasicAuthorize : AuthorizeAttribute
    {
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>(); ;
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var headerValue = String.Empty;

            if ( actionContext.Request.Headers.TryGetValues( "Authorization", out IEnumerable<string> headerValues ) )
            {
                headerValue = headerValues.FirstOrDefault();
            }

            var credentials = "";
            if ( headerValue != String.Empty )
            {
                credentials = Encoding.UTF8.GetString( Convert.FromBase64String( headerValue.Substring( 6 ).Trim() ) );
            }

            bool isAuthenticated = false;

            int index = credentials.IndexOf( ':' );
            if ( index > -1 )
            {
                string username = credentials.Substring( 0, index );
                string password = credentials.Substring( index + 1, credentials.Length - (index + 1) );

                var result = SignInManager.PasswordSignInAsync( username, password, false, shouldLockout: false ).Result;

                if ( result == SignInStatus.Success )
                {
                    isAuthenticated = true;
                }
            }

            return isAuthenticated;
        }
    }
}