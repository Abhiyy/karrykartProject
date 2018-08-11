using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class OrderModel
    {
        public Guid OrderID { get; set; }
        public bool OrderPlaced { get; set; }
    }

    public class OrderInputModel {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public int AddressID { get; set; }
        public bool GuestCheckout { get; set; }
        public int PaymentType { get; set; }
    }
}