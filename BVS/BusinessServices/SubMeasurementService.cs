using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;

namespace BVS.BusinessServices
{
    public class SubMeasurementService : BVSBusinessService
    {
        public IQueryable<SubMeasurement> GetAll()
        {
            IQueryable<SubMeasurement> query = DB.SubMeasurements;
            return query;
        }

        public SubMeasurement GetByID(int ID)
        {
            SubMeasurement measurement = DB.SubMeasurements.Where( q => q.ID == ID ).FirstOrDefault();
            return measurement;
        }

        public IQueryable<SubMeasurement> GetByMeasurementID(int measurementID)
        {
            IQueryable<SubMeasurement> query = DB.SubMeasurements.Where( q => q.MeasurementID == measurementID );
            return query;
        }

        public BusinessServiceOperationResult Save(SubMeasurement measurement)
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
                DB.SubMeasurements.Add( measurement );
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