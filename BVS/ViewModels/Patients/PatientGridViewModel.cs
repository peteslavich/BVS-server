using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BVS.ViewModels.Patients
{
    public class PatientGridViewModel
    {
        [Display( Name = "Email Address" )]
        public string EmailAddress { get; set; }

        [Required]
        [Display( Name = "First Name" )]
        public string FirstName { get; set; }

        public int ID { get; set; }
        public bool IsVoided { get; set; }

        [Display( Name = "Last Name" )]
        public string LastName { get; set; }

        public int TypeID { get; set; }
    }
}