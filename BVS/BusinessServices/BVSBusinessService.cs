using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;

namespace BVS.BusinessServices
{
    public class BVSBusinessService
    {
        public BVSContext DB
        {
            get; set;
        }

        public BVSBusinessServiceManager ServiceManager
        {
            get; set;
        }
    }
}