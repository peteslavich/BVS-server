using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVS.BusinessServices
{
    public class BVSBusinessServiceManager : IDisposable

    {
        private BVSContext _DBContext;
        private MeasurementService _MeasurementService;
        private PatientService _PatientService;
        private SubMeasurementService _SubMeasurementService;

        public BVSContext DBContext
        {
            get
            {
                if ( _DBContext == null )
                {
                    _DBContext = new BVSContext();
                }

                return _DBContext;
            }
        }

        public MeasurementService Measurements
        {
            get
            {
                if ( _MeasurementService == null )
                {
                    _MeasurementService = new MeasurementService
                    {
                        DB = DBContext,
                        ServiceManager = this
                    };
                }
                return _MeasurementService;
            }
        }

        public PatientService Patients
        {
            get
            {
                if ( _PatientService == null )
                {
                    _PatientService = new PatientService
                    {
                        DB = DBContext,
                        ServiceManager = this
                    };
                }
                return _PatientService;
            }
        }

        public SubMeasurementService SubMeasurements
        {
            get
            {
                if ( _SubMeasurementService == null )
                {
                    _SubMeasurementService = new SubMeasurementService
                    {
                        DB = DBContext,
                        ServiceManager = this
                    };
                }
                return _SubMeasurementService;
            }
        }

        public void Dispose()
        {
            Dispose( true );
        }

        protected void Dispose(bool disposing)
        {
            if ( disposing )
            {
                if ( _DBContext != null )
                {
                    _DBContext.Dispose();
                }
            }
        }
    }
}