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
    
    public partial class Payment
    {
        public Payment()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public System.Guid ID { get; set; }
        public string Reference { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> IsSuccessful { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
    }
}