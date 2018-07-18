using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BVS.ViewModels.Patients;
using BVS.ViewModels.Measurements;
using BVS.BusinessServices;
using BVS.DataModels;
using BVS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BVS.Controllers
{
    [Authorize]
    public class PatientController : BVSController
    {
        public ActionResult Create()
        {
            var viewModel = new PatientPasswordViewModel
            {
                Patient = new Patient()
            };

            return View( viewModel );
        }

        [HttpPost]
        public ActionResult Create(PatientPasswordViewModel viewModel)
        {
            BusinessServiceOperationResult result;
            using ( var transaction = new TransactionScope() )
            {
                result = BVSBusinessServices.Patients.Save( viewModel.Patient );
                if ( !result.HasErrors )
                {
                    ApplicationDbContext context = new ApplicationDbContext();

                    var roleManager = new RoleManager<IdentityRole>( new RoleStore<IdentityRole>( context ) );
                    var userManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>( context ) );

                    var user = new ApplicationUser
                    {
                        UserName = viewModel.Patient.EmailAddress,
                        Email = viewModel.Patient.EmailAddress
                    };

                    var identityResult = userManager.Create( user, viewModel.NewPassword );
                    if ( !identityResult.Succeeded )
                    {
                        result.AddError( String.Format( "Could not create application user account: {0}", String.Join( ", ", identityResult.Errors ) ) );
                    }
                    else
                    {
                        identityResult = userManager.AddToRole( user.Id, "Patient" );
                        if ( !identityResult.Succeeded )
                        {
                            result.AddError( "Could not assign user to patient role" );
                        }
                    }

                    if ( !result.HasErrors )
                    {
                        viewModel.Patient.MembershipProviderUserID = user.Id;
                        result = BVSBusinessServices.Patients.Save( viewModel.Patient );
                    }

                    if ( !result.HasErrors )
                    {
                        transaction.Complete();
                    }
                }
            }
            if ( !result.HasErrors )
            {
                return RedirectToAction( "Details", "Patient", new { id = viewModel.Patient.ID } );
            }
            else
            {
                foreach ( var error in result.Errors )
                {
                    ModelState.AddModelError( "key", error );
                }
                return View( viewModel );
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var patient = BVSBusinessServices.Patients.GetByID( id );

            var result = BVSBusinessServices.Patients.Delete( patient );
            if ( !result.HasErrors )
            {
                return RedirectToAction( "Index" );
            }
            else
            {
                return RedirectToAction( "Details", new { id = id } );
            }
        }

        // GET: Patient
        public ActionResult Details(int id)
        {
            var patientViewModel = new PatientViewModel
            {
                Patient = BVSBusinessServices.Patients.GetByID( id )
            };

            var patientMeasurements = BVSBusinessServices.Measurements.GetByPatientID( id ).OrderByDescending( m => m.MeasurementOn );
            patientViewModel.PatientMeasurements = new List<MeasurementGridViewModel>();

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;

            foreach ( Measurement measurement in patientMeasurements )
            {
                var u = new MeasurementGridViewModel()
                {
                    ID = measurement.ID,
                    CalculatedVolume = measurement.CalculatedVolume,
                    MeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( measurement.MeasurementOn, timeZoneInfo ),
                    PatientFeedback = measurement.PatientFeedback,
                    PatientRating = (measurement.PatientRating.HasValue ? (measurement.PatientRating.ToString() + " ") : "") + ((measurement.IsPatientRatingThumbsUp.HasValue && measurement.IsPatientRatingThumbsUp.Value) ? "(Thumbs Up)" : "") + ((measurement.IsPatientRatingThumbsDown.HasValue && measurement.IsPatientRatingThumbsDown.Value) ? "(Thumbs Down)" : ""),
                    PatientID = measurement.PatientID,
                    IsVoided = measurement.IsVoided
                };

                patientViewModel.PatientMeasurements.Add( u );
            }

            return View( patientViewModel );
        }

        public ActionResult Edit(int id)
        {
            var viewModel = new PatientViewModel
            {
                Patient = BVSBusinessServices.Patients.GetByID( id )
            };

            return View( viewModel );
        }

        [HttpPost]
        public ActionResult Edit(int id, PatientViewModel viewModel)
        {
            viewModel.Patient = BVSBusinessServices.Patients.GetByID( id );
            if ( TryUpdateModel( viewModel.Patient, "Patient" ) )
            {
                var result = BVSBusinessServices.Patients.Save( viewModel.Patient );
                if ( !result.HasErrors )
                {
                    return RedirectToAction( "Details", "Patient", new { id = viewModel.Patient.ID } );
                }
            }

            return View( viewModel );
        }

        public ActionResult Index()
        {
            var query = BVSBusinessServices.Patients.GetAll();
            var viewModel = new PatientSearchViewModel();
            viewModel.Patients = new List<PatientGridViewModel>();

            foreach ( Patient patient in query )
            {
                var u = new PatientGridViewModel()
                {
                    ID = patient.ID,
                    EmailAddress = patient.EmailAddress,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    IsVoided = patient.IsVoided
                };
                viewModel.Patients.Add( u );
            }
            return View( viewModel );
        }

        public ActionResult MeasurementChart(int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;

            var patientViewModel = new PatientChartViewModel
            {
                Patient = BVSBusinessServices.Patients.GetByID( id ),
                StartDate = startDate,
                EndDate = endDate,
                PatientMeasurements = new List<MeasurementGridViewModel>()
            };

            IEnumerable<Measurement> patientMeasurements = BVSBusinessServices.Measurements.GetByPatientID( id ).ToList();

            if ( startDate.HasValue )
            {
                startDate = startDate.Value.Date;
                patientMeasurements = patientMeasurements.Where( x => TimeZoneInfo.ConvertTimeFromUtc( x.MeasurementOn, timeZoneInfo ) >= startDate );
            }
            if ( endDate.HasValue )
            {
                endDate = endDate.Value.Date;
                var nextDay = endDate.Value.AddDays( 1 );
                patientMeasurements = patientMeasurements.Where( x => TimeZoneInfo.ConvertTimeFromUtc( x.MeasurementOn, timeZoneInfo ) < nextDay );
            }
            patientMeasurements = patientMeasurements.OrderBy( m => m.MeasurementOn );

            foreach ( Measurement measurement in patientMeasurements )
            {
                var u = new MeasurementGridViewModel()
                {
                    ID = measurement.ID,
                    CalculatedVolume = measurement.CalculatedVolume,
                    MeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( measurement.MeasurementOn, timeZoneInfo ),
                    PatientFeedback = measurement.PatientFeedback,
                    PatientRating = measurement.PatientRating.HasValue ? measurement.PatientRating.ToString() : "",
                    PatientID = measurement.PatientID,
                    IsVoided = measurement.IsVoided
                };
                patientViewModel.PatientMeasurements.Add( u );
            }

            var width = 800;
            if ( patientViewModel.PatientMeasurements.Count() > 1 )
            {
                var timeDifff = patientViewModel.PatientMeasurements.Last().MeasurementOn.Subtract( patientViewModel.PatientMeasurements.First().MeasurementOn ).TotalMinutes;
                int timeDiff = (int)timeDifff;
                var newWidth = timeDiff * 8;
                if ( newWidth > width )
                {
                    width = newWidth;
                }
            }
            patientViewModel.ChartWidth = width;

            return View( patientViewModel );
        }

        public ActionResult MeasurementDownload(int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;

            var patient = BVSBusinessServices.Patients.GetByID( id );
            var measurements = patient.Measurements.Where( x => !x.IsVoided );
            if ( startDate != null )
            {
                measurements = measurements.Where( x => x.MeasurementOn >= startDate );
            }
            if ( endDate != null )
            {
                var nextDay = endDate.Value.Date.AddDays( 1 );
                measurements = measurements.Where( x => x.MeasurementOn < nextDay );
            }

            StringBuilder s = new StringBuilder();
            s.Append( "ClientGuid,PatientID,MeasurementOn,CalculatedVolume,IsPatientRatingThumbsUp,IsPatientRatingThumbsDown,PatientRating,PatientFeedback,SubmeasurementClientGuid,SubmeasurementOn,SubmeasurementCalculatedVolume,Sensor1LED1,Sensor2LED1,Sensor3LED1,Sensor4LED1,Sensor5LED1,Sensor6LED1,Sensor7LED1,Sensor8LED1,Sensor1LED2,Sensor2LED2,Sensor3LED2,Sensor4LED2,Sensor5LED2,Sensor6LED2,Sensor7LED2,Sensor8LED2,Sensor1LED3,Sensor2LED3,Sensor3LED3,Sensor4LED3,Sensor5LED3,Sensor6LED3,Sensor7LED3,Sensor8LED3,Sensor1LED4,Sensor2LED4,Sensor3LED4,Sensor4LED4,Sensor5LED4,Sensor6LED4,Sensor7LED4,Sensor8LED4,Sensor1LED5,Sensor2LED5,Sensor3LED5,Sensor4LED5,Sensor5LED5,Sensor6LED5,Sensor7LED5,Sensor8LED5,Sensor1LED6,Sensor2LED6,Sensor3LED6,Sensor4LED6,Sensor5LED6,Sensor6LED6,Sensor7LED6,Sensor8LED6,Sensor1LED7,Sensor2LED7,Sensor3LED7,Sensor4LED7,Sensor5LED7,Sensor6LED7,Sensor7LED7,Sensor8LED7,Sensor1LED8,Sensor2LED8,Sensor3LED8,Sensor4LED8,Sensor5LED8,Sensor6LED8,Sensor7LED8,Sensor8LED8," );
            s.AppendLine();

            foreach ( var measurement in measurements.OrderBy( x => x.MeasurementOn ) )
            {
                var clientGuid = measurement.ClientGUID.ToString();
                var patientID = measurement.PatientID.ToString();
                var measurementOn = TimeZoneInfo.ConvertTimeFromUtc( measurement.MeasurementOn, timeZoneInfo ).ToString( "yyyy-MM-dd H:mm:ss" );
                var calculatedVolume = measurement.CalculatedVolume.ToString();
                var isPatientRatingThumbsUp = measurement.IsPatientRatingThumbsUp.HasValue && measurement.IsPatientRatingThumbsUp.Value ? "TRUE" : "FALSE";
                var isPatientRatingThumbsDown = measurement.IsPatientRatingThumbsDown.HasValue && measurement.IsPatientRatingThumbsDown.Value ? "TRUE" : "FALSE";
                var patientRating = measurement.PatientRating.HasValue ? measurement.PatientRating.ToString() : "";
                var patientFeedback = measurement.PatientFeedback != null ? measurement.PatientFeedback.Replace( "\"", "\"\"" ) : "";
                patientFeedback = String.Format( "\"{0}\"", patientFeedback );

                var submeasurements = measurement.SubMeasurements;
                if ( submeasurements.Count > 0 )
                {
                    foreach ( var submeasurement in submeasurements )
                    {
                        var subClientGuid = submeasurement.ClientGUID.ToString();
                        var subMeasurementOn = TimeZoneInfo.ConvertTimeFromUtc( submeasurement.MeasurementOn, timeZoneInfo ).ToString( "yyyy-MM-dd H:mm:ss" );
                        var subCalculatedVolume = submeasurement.CalculatedVolume.ToString();

                        var sensor1LED1 = submeasurement.Sensor1LED1.ToString();
                        var sensor2LED1 = submeasurement.Sensor2LED1.ToString();
                        var sensor3LED1 = submeasurement.Sensor3LED1.ToString();
                        var sensor4LED1 = submeasurement.Sensor4LED1.ToString();
                        var sensor5LED1 = submeasurement.Sensor5LED1.ToString();
                        var sensor6LED1 = submeasurement.Sensor6LED1.ToString();
                        var sensor7LED1 = submeasurement.Sensor7LED1.ToString();
                        var sensor8LED1 = submeasurement.Sensor8LED1.ToString();

                        var sensor1LED2 = submeasurement.Sensor1LED2.ToString();
                        var sensor2LED2 = submeasurement.Sensor2LED2.ToString();
                        var sensor3LED2 = submeasurement.Sensor3LED2.ToString();
                        var sensor4LED2 = submeasurement.Sensor4LED2.ToString();
                        var sensor5LED2 = submeasurement.Sensor5LED2.ToString();
                        var sensor6LED2 = submeasurement.Sensor6LED2.ToString();
                        var sensor7LED2 = submeasurement.Sensor7LED2.ToString();
                        var sensor8LED2 = submeasurement.Sensor8LED2.ToString();

                        var sensor1LED3 = submeasurement.Sensor1LED3.ToString();
                        var sensor2LED3 = submeasurement.Sensor2LED3.ToString();
                        var sensor3LED3 = submeasurement.Sensor3LED3.ToString();
                        var sensor4LED3 = submeasurement.Sensor4LED3.ToString();
                        var sensor5LED3 = submeasurement.Sensor5LED3.ToString();
                        var sensor6LED3 = submeasurement.Sensor6LED3.ToString();
                        var sensor7LED3 = submeasurement.Sensor7LED3.ToString();
                        var sensor8LED3 = submeasurement.Sensor8LED3.ToString();

                        var sensor1LED4 = submeasurement.Sensor1LED4.ToString();
                        var sensor2LED4 = submeasurement.Sensor2LED4.ToString();
                        var sensor3LED4 = submeasurement.Sensor3LED4.ToString();
                        var sensor4LED4 = submeasurement.Sensor4LED4.ToString();
                        var sensor5LED4 = submeasurement.Sensor5LED4.ToString();
                        var sensor6LED4 = submeasurement.Sensor6LED4.ToString();
                        var sensor7LED4 = submeasurement.Sensor7LED4.ToString();
                        var sensor8LED4 = submeasurement.Sensor8LED4.ToString();

                        var sensor1LED5 = submeasurement.Sensor1LED5.ToString();
                        var sensor2LED5 = submeasurement.Sensor2LED5.ToString();
                        var sensor3LED5 = submeasurement.Sensor3LED5.ToString();
                        var sensor4LED5 = submeasurement.Sensor4LED5.ToString();
                        var sensor5LED5 = submeasurement.Sensor5LED5.ToString();
                        var sensor6LED5 = submeasurement.Sensor6LED5.ToString();
                        var sensor7LED5 = submeasurement.Sensor7LED5.ToString();
                        var sensor8LED5 = submeasurement.Sensor8LED5.ToString();

                        var sensor1LED6 = submeasurement.Sensor1LED6.ToString();
                        var sensor2LED6 = submeasurement.Sensor2LED6.ToString();
                        var sensor3LED6 = submeasurement.Sensor3LED6.ToString();
                        var sensor4LED6 = submeasurement.Sensor4LED6.ToString();
                        var sensor5LED6 = submeasurement.Sensor5LED6.ToString();
                        var sensor6LED6 = submeasurement.Sensor6LED6.ToString();
                        var sensor7LED6 = submeasurement.Sensor7LED6.ToString();
                        var sensor8LED6 = submeasurement.Sensor8LED6.ToString();

                        var sensor1LED7 = submeasurement.Sensor1LED7.ToString();
                        var sensor2LED7 = submeasurement.Sensor2LED7.ToString();
                        var sensor3LED7 = submeasurement.Sensor3LED7.ToString();
                        var sensor4LED7 = submeasurement.Sensor4LED7.ToString();
                        var sensor5LED7 = submeasurement.Sensor5LED7.ToString();
                        var sensor6LED7 = submeasurement.Sensor6LED7.ToString();
                        var sensor7LED7 = submeasurement.Sensor7LED7.ToString();
                        var sensor8LED7 = submeasurement.Sensor8LED7.ToString();

                        var sensor1LED8 = submeasurement.Sensor1LED8.ToString();
                        var sensor2LED8 = submeasurement.Sensor2LED8.ToString();
                        var sensor3LED8 = submeasurement.Sensor3LED8.ToString();
                        var sensor4LED8 = submeasurement.Sensor4LED8.ToString();
                        var sensor5LED8 = submeasurement.Sensor5LED8.ToString();
                        var sensor6LED8 = submeasurement.Sensor6LED8.ToString();
                        var sensor7LED8 = submeasurement.Sensor7LED8.ToString();
                        var sensor8LED8 = submeasurement.Sensor8LED8.ToString();

                        s.AppendFormat( "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58},{59},{60},{61},{62},{63},{64},{65},{66},{67},{68},{69},{70},{71},{72},{73},{74}", clientGuid, patientID, measurementOn, calculatedVolume, isPatientRatingThumbsUp, isPatientRatingThumbsDown, patientRating, patientFeedback, subClientGuid, subMeasurementOn, subCalculatedVolume, sensor1LED1, sensor2LED1, sensor3LED1, sensor4LED1, sensor5LED1, sensor6LED1, sensor7LED1, sensor8LED1, sensor1LED2, sensor2LED2, sensor3LED2, sensor4LED2, sensor5LED2, sensor6LED2, sensor7LED2, sensor8LED2, sensor1LED3, sensor2LED3, sensor3LED3, sensor4LED3, sensor5LED3, sensor6LED3, sensor7LED3, sensor8LED3, sensor1LED4, sensor2LED4, sensor3LED4, sensor4LED4, sensor5LED4, sensor6LED4, sensor7LED4, sensor8LED4, sensor1LED5, sensor2LED5, sensor3LED5, sensor4LED5, sensor5LED5, sensor6LED5, sensor7LED5, sensor8LED5, sensor1LED6, sensor2LED6, sensor3LED6, sensor4LED6, sensor5LED6, sensor6LED6, sensor7LED6, sensor8LED6, sensor1LED7, sensor2LED7, sensor3LED7, sensor4LED7, sensor5LED7, sensor6LED7, sensor7LED7, sensor8LED7, sensor1LED8, sensor2LED8, sensor3LED8, sensor4LED8, sensor5LED8, sensor6LED8, sensor7LED8, sensor8LED8 );
                        s.AppendLine();
                    }
                }
                else
                {
                    s.AppendFormat( "{0},{1},{2},{3},{4},{5},{6},{7}", clientGuid, patientID, TimeZoneInfo.ConvertTimeFromUtc( measurement.MeasurementOn, timeZoneInfo ), calculatedVolume, isPatientRatingThumbsUp, isPatientRatingThumbsDown, patientRating, patientFeedback );
                    s.AppendLine();
                }
            }

            Response.ContentType = "text/plain";
            Response.AddHeader( "content-disposition", "attachment;filename=" + string.Format( "measurementpatient-{0}.csv", patient.ID ) );
            Response.Clear();

            using ( StreamWriter writer = new StreamWriter( Response.OutputStream, Encoding.UTF8 ) )
            {
                writer.Write( s.ToString() );
            }

            Response.End();

            return null;
        }

        public ActionResult ResetPassword(int id)
        {
            var viewModel = new PatientPasswordViewModel
            {
                Patient = BVSBusinessServices.Patients.GetByID( id )
            };

            return View( viewModel );
        }

        [HttpPost]
        public ActionResult ResetPassword(int id, PatientPasswordViewModel viewModel)
        {
            var patient = BVSBusinessServices.Patients.GetByID( id );

            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>( new RoleStore<IdentityRole>( context ) );
            var userManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>( context ) );

            var user = userManager.FindById( patient.MembershipProviderUserID );
            var result = userManager.RemovePassword( user.Id );
            if ( result.Succeeded )
            {
                result = userManager.AddPassword( user.Id, viewModel.NewPassword );
            }

            if ( result.Succeeded )
            {
                return RedirectToAction( "Details", new { id = id } );
            }
            else
            {
                ModelState.AddModelError( "Password", String.Format( "Unable to change password. {0}", String.Join( ", ", result.Errors ) ) );
                return View();
            }
        }
    }
}