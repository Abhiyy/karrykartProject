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
    
    public partial class ProductFeature
    {
        public int FeatureID { get; set; }
        public Nullable<System.Guid> ProductID { get; set; }
        public string Feature { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Product Product1 { get; set; }
    }
}
