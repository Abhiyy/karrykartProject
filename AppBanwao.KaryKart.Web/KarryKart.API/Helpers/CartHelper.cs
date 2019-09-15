using AppBanwao.KarryKart.Model;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class CartHelper
    {
        karrykartEntities _context = null;
        public CartModel CreateCart (CartModel cartModel)
        {
            _context = new karrykartEntities();

            
            Cart userCart = new Cart() { ID = Guid.NewGuid(), CartUserID = cartModel.User==Guid.Empty?Guid.NewGuid():cartModel.User };
            _context.Carts.Add(userCart);
            _context.SaveChanges();

            _context.CartProducts.Add(new CartProduct(){CartID=userCart.ID, ProductID= cartModel.ProductID,Quantity=cartModel.Quantity});
            _context.SaveChanges();

            cartModel.CartID=userCart.ID;
            cartModel.ProductCount = userCart.CartProducts.Sum(x=>x.Quantity).Value;
            cartModel.User = userCart.CartUserID.Value;
            cartModel.UserName = string.IsNullOrEmpty(cartModel.UserName)?"Guest":cartModel.UserName;
            _context = null;
            return cartModel;
            
        }

        public CartModel UpdateCart(CartModel cartModel)
        {
            _context = new karrykartEntities();

            var cart = _context.Carts.Find(cartModel.CartID);
            if (cart != null) {
                var product = cart.CartProducts.Where(x => x.ProductID == cartModel.ProductID).FirstOrDefault();
                if (product!=null)
                {
                    if (cartModel.IsQuantityUpdate)
                    {
                       product.Quantity = cartModel.Quantity;
                    }
                    else
                    {
                       product.Quantity = product.Quantity + 1;
                    }
                    _context.Entry(product).State = System.Data.Entity.EntityState.Modified;

                }
                else
                {
                    _context.CartProducts.Add(new CartProduct() { CartID = cart.ID, ProductID = cartModel.ProductID, Quantity = cartModel.Quantity });
                }
                    _context.SaveChanges();
                cartModel.ProductCount = cart.CartProducts.Sum(x=>x.Quantity).Value;
            }
            return cartModel;
        }

        public CartModel DeleteCart(Guid CartID)
        {
            _context = new karrykartEntities();

            var cart = _context.Carts.Find(CartID);
            if (cart != null)
            {
               // var products = cart.CartProducts.Where(x => x.CartID == CartID);
               // if (products != null)
               // {
               ////     foreach (var p in products)
               // //    {
               //         _context.Entry(products).State = System.Data.Entity.EntityState.Deleted;
               ////     }

               // }
                _context.Entry(cart).State = System.Data.Entity.EntityState.Deleted;
                _context.SaveChanges();
                return new CartModel();
            }
            return null;
        }

        public CartModel DeleteCart(Guid CartID, Guid ProductID)
        {
            _context = new karrykartEntities();

            var cart = _context.Carts.Find(CartID);
            if (cart != null)
            {
                var products = cart.CartProducts.Where(x => x.ProductID == ProductID).FirstOrDefault();
                if (products != null)
                {
                    _context.Entry(products).State = System.Data.Entity.EntityState.Deleted;
                }
                _context.SaveChanges();
                if (cart.CartProducts.Count == 0) {
                    _context.Entry(cart).State = System.Data.Entity.EntityState.Deleted;
                    _context.SaveChanges();

                    return null;
                }
                return new CartModel() { CartID = CartID };
            }
            return null;
        }


        public CartDetailsModel GetCart(Guid CartID)
        { 
         
         using(_context =new karrykartEntities()){
             var cartModel = new CartDetailsModel();
             var cart = _context.Carts.Find(CartID);
             var productHelper = new ProductHelper();
             if(cart!=null)
             {
                 cartModel.CartID = cart.ID;
                 cartModel.CartCount = cart.CartProducts.Sum(x => x.Quantity).Value;
                 
                 foreach (var prod in cart.CartProducts)
                 { 
                     var product = productHelper.GetProductDetail(prod.ProductID.Value);
                     if(product!=null)
                     {
                        cartModel.Products.Add( new CartProductModel(product,prod.Quantity.Value));
                        cartModel.CartTotal += Convert.ToDouble( product.Prices.FirstOrDefault().Price * prod.Quantity.Value);
                     }
                 }
                 cartModel.TaxPercentage  = 5;
                 cartModel.SubTotal = ((double)(cartModel.TaxPercentage) / 100) * cartModel.CartTotal;
                 cartModel.GrandTotal = cartModel.CartTotal + cartModel.SubTotal;
                 cartModel.User = cart.CartUserID.Value;
                 cartModel.Username = (_context.Users.Any(x=>x.UserID == cart.CartUserID))?"Registered User":"Guest";
                 return cartModel;
             }
         }
         return null;
        }

        public CartDetailsModel ApplyCouponCode(Guid CartID, string CouponCode)
        {
            using (_context = new karrykartEntities())
            {
                var coupon = _context.Coupons.Where(x => x.DisplayName.ToUpper() == CouponCode.ToUpper()).FirstOrDefault();
                if (coupon != null)
                {
                    var couponValue = _context.CouponValues.Where(x => x.CouponID == coupon.CouponID).FirstOrDefault();
                    var cart = GetCart(CartID);
                    cart.CartTotal = (double)(cart.CartTotal - (((double)couponValue.CouponValue1.Value / 100) * cart.CartTotal));
                    cart.SubTotal = ((double)(cart.TaxPercentage) / 100) * cart.CartTotal;
                    cart.GrandTotal = cart.CartTotal + cart.SubTotal;
                    return cart;
                }
                else {
                    return null;
                }
               
            }
        }

    }
}