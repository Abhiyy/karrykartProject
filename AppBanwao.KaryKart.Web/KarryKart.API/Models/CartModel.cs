using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class CartModel
    {
        public Guid CartID { get; set; }
        public Guid ProductID { get; set; }
        public int  Quantity { get; set; }
        public bool CreateCart { get; set; }
        public Guid User { get; set; }
        public int  ProductCount { get; set; }
        public string UserName { get; set; }
        public bool IsQuantityUpdate { get; set; }
        
    }

    public class CartDetailsModel
    {
        public CartDetailsModel() {
            this.Products = new List<CartProductModel>();
        }
        public Guid CartID { get; set; }
        public Guid User { get; set; }
        public List<CartProductModel> Products { get; set; }
        public double CartTotal { get; set; }
        public double SubTotal { get; set; }
        public double GrandTotal { get; set; }
        public int TaxPercentage { get; set; }
        public int CartCount {get;set;}
        public string Username { get; set; }
    }

    public class CartProductModel : ProductModel {
        public CartProductModel() { }
        public CartProductModel(ProductModel product, int quan) {
            this.Active = product.Active;
            this.BrandID = product.BrandID;
            this.BrandName = product.BrandName;
            this.CategoryID = product.CategoryID;
            this.CategoryName = product.CategoryName;
            this.Description = product.Description;
            this.Features = product.Features;
            this.Images = product.Images;
            this.Name = product.Name;
            this.Prices = product.Prices;
            this.ProductID = product.ProductID;
            this.SubCategoryID = product.SubCategoryID;
            this.ShippingDetails = product.ShippingDetails;
            this.SubCategoryName = product.SubCategoryName;
            this.QuantitySelected = quan;
        }
        public int QuantitySelected { get; set; }
    }
   
}