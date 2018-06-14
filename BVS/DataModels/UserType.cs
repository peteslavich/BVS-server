using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BVS.DataModels
{
    [Table( "UserType" )]
    public partial class UserType
    {
        public int CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ID { get; set; }

        public bool IsVoided { get; set; }

        [Required]
        [StringLength( 100 )]
        [Display( Name = "User Type" )]
        public string Name { get; set; }

        public int UpdatedByID { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}