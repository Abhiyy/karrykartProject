//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppBanwao.KarryKart.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CartID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<System.Guid> PaymentID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> PlaceOn { get; set; }
        public Nullable<System.DateTime> DeliveredOn { get; set; }
        public Nullable<bool> GuestCheckout { get; set; }
        public Nullable<int> DeliveryAddressID { get; set; }
    
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
