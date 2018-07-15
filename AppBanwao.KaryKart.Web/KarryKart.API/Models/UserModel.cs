using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class UserModel
    {
        public string user { get; set; }

        public string pwd { get; set; }
    }

    public class UserInformation
    {
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Error { get; set; }
    }

    public class UserSignUpModel:UserModel {
        public string Name { get; set; }
        public Guid UserID { get; set; }
        public string Message { get; set; }
        public string Otp { get; set; }
    }
}