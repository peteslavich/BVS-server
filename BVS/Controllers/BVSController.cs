using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVS.BusinessServices;

namespace BVS.Controllers
{
    public class BVSController : Controller
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

        public new void Dispose()
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