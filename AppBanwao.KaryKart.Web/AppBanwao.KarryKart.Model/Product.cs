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
    
    public partial class Product
    {
        public Product()
        {
            this.ProductFeatures = new HashSet<ProductFeature>();
            this.ProductFeatures1 = new HashSet<ProductFeature>();
            this.ProductImages = new HashSet<ProductImage>();
            this.ProductImages1 = new HashSet<ProductImage>();
            this.ProductPrices = new HashSet<ProductPrice>();
            this.ProductPrices1 = new HashSet<ProductPrice>();
            this.ProductShippings = new HashSet<ProductShipping>();
            this.ProductShippings1 = new HashSet<ProductShipping>();
            this.ProductSizeMappings = new HashSet<ProductSizeMapping>();
            this.ProductSizeMappings1 = new HashSet<ProductSizeMapping>();
            this.CartProducts = new HashSet<CartProduct>();
            this.OrderProducts = new HashSet<OrderProduct>();
        }
    
        public System.Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> SubCategoryID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> Active { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual Brand Brand1 { get; set; }
        public virtual Category Category { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        public virtual Subcategory Subcategory1 { get; set; }
        public virtual ICollection<ProductFeature> ProductFeatures { get; set; }
        public virtual ICollection<ProductFeature> ProductFeatures1 { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductImage> ProductImages1 { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices1 { get; set; }
        public virtual ICollection<ProductShipping> ProductShippings { get; set; }
        public virtual ICollection<ProductShipping> ProductShippings1 { get; set; }
        public virtual ICollection<ProductSizeMapping> ProductSizeMappings { get; set; }
        public virtual ICollection<ProductSizeMapping> ProductSizeMappings1 { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
