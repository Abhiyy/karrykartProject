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
        public Guid TransactionID { get; set; }
        public string TransactionStatus { get; set; }
    }

   
    public class OrderDetailModel {
        
        public UserDetails User { get; set; }

        public Guid OrderID { get; set; }

        public List<KeyValuePair<ProductModel,int>> OrderProducts { get; set; }

        public DateTime OrderPlacedOn { get; set; }

        public string OrderPlacedDateTime { get; set; }

        public string OrderDeliveredOn { get; set; }

        public int OrderStatusID { get; set; }

        public string OrderStatus { get; set; }

        public Guid PaymentID { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentType { get; set; }

        public int PaymentTypeID { get; set; }

        public bool isPaymentSuccessful { get; set; }

        public bool GuestCheckout { get; set; }

        public UserAddress DeliveryAddress { get; set; }
    }

    


}