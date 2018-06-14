using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;

namespace BVS.BusinessServices
{
    public class MeasurementService : BVSBusinessService
    {
        public IQueryable<Measurement> GetAll()
        {
            IQueryable<Measurement> query = DB.Measurements;
            return query;
        }

        public Measurement GetByID(int ID)
        {
            Measurement measurement = DB.Measurements.Where( q => q.ID == ID ).FirstOrDefault();
            return measurement;
        }

        public IQueryable<Measurement> GetByPatientID(int patientID)
        {
            IQueryable<Measurement> query = DB.Measurements.Where( q => q.PatientID == patientID );
            return query;
        }

        public BusinessServiceOperationResult Save(Measurement measurement)
        {
            if ( measurement.ID > 0 )
            {
                DB.Entry( measurement ).State = System.Data.Entity.EntityState.Modified;
                measurement.UpdatedByID = 1;
                measurement.UpdatedOn = DateTime.Now;
                measurement.IsVoided = false;
            }
            else
            {
                DB.Measurements.Add( measurement );
                measurement.CreatedByID = 1;
                measurement.UpdatedByID = 1;
                measurement.CreatedOn = DateTime.Now;
                measurement.UpdatedOn = DateTime.Now;
                measurement.IsVoided = false;
            }

            var result = new BusinessServiceOperationResult();
            try
            {
                DB.SaveChanges();
            }
            catch ( Exception ex )
            {
                result.AddError( ex.Message );
            }

            return result;
        }
    }
}