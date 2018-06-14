using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;

namespace BVS.BusinessServices
{
    public class PatientService : BVSBusinessService
    {
        public BusinessServiceOperationResult Delete(Patient patient)
        {
            if ( patient.ID > 0 )
            {
                DB.Entry( patient ).State = System.Data.Entity.EntityState.Modified;
                patient.UpdatedByID = 1;
                patient.UpdatedOn = DateTime.Now;
                patient.IsVoided = true;
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

        public IQueryable<Patient> GetAll()
        {
            IQueryable<Patient> query = DB.Patients.Where( q => !q.IsVoided );
            return query;
        }

        public Patient GetByID(int ID)
        {
            Patient patient = DB.Patients.Where( q => q.ID == ID ).FirstOrDefault();
            return patient;
        }

        public BusinessServiceOperationResult Save(Patient patient)
        {
            if ( patient.ID > 0 )
            {
                DB.Entry( patient ).State = System.Data.Entity.EntityState.Modified;
                patient.UpdatedByID = 1;
                patient.UpdatedOn = DateTime.Now;
            }
            else
            {
                DB.Patients.Add( patient );
                patient.CreatedByID = 1;
                patient.UpdatedByID = 1;
                patient.CreatedOn = DateTime.Now;
                patient.UpdatedOn = DateTime.Now;
                patient.IsVoided = false;
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