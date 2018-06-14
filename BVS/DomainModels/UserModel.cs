using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVS.DataModels;

namespace BVS.DomainModels
{
    public class UserModel
    {
        public string Email { get; set; }
        public string ID { get; set; }
        public UserType UserType { get; set; }
    }
}