using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class PaymentModel
    {
        public Guid UserID { get; set; }

        public Guid CartID { get; set; }
    }

    public class PaymentRedirectModel {
        public string URL { get; set; }
        public PaymentDetail options { get; set; }
    }

    public class PaymentDetail{
        public string key { get; set; }
        public string txnid { get; set; }
        public double amount { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string surl { get; set; }
        public string furl { get; set; }
        public string hash { get; set; }
        public string service_provider { get; set; }
    }
}