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
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Name { get; set; }
    }
}