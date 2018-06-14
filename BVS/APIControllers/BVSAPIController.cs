using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BVS.BusinessServices;
using System.Web;
using Microsoft.Owin;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BVS.APIControllers
{
    public abstract class BVSAPIController : ApiController
    {
        private BVSBusinessServiceManager _BVSBusinessServices;

        public BVSBusinessServiceManager BVSBusinessServices
        {
            get
            {
                if ( _BVSBusinessServices == null )
                {
                    _BVSBusinessServices = new BVSBusinessServiceManager();
                }
                return _BVSBusinessServices;
            }
        }

        internal new void Dispose()
        {
            Dispose( true );
        }

        protected override void Dispose(bool disposing)
        {
            if ( disposing )
            {
                if ( _BVSBusinessServices != null )
                {
                    _BVSBusinessServices.Dispose();
                }
            }
        }
    }
}