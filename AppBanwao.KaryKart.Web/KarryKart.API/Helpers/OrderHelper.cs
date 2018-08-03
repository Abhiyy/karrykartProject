using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class OrderHelper
    {
        karrykartEntities _context = null;
        APIEmailHelper _emailHelper = null;
        SmsHelper _smsHelper = null;
        public OrderModel CreateOrder(Guid CartID, string email, string contact, string name)
        {
            using (_context = new karrykartEntities())
            {
                var cart = _context.Carts.Find(CartID);
                if (cart != null)
                {
                    var order = new Order() { ID=Guid.NewGuid(),CartID=cart.ID,UserID=cart.CartUserID};
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var p in cart.CartProducts)
                    {
                        var prod = new OrderProduct() { OrderID = order.ID, ProductID = p.ProductID, Quantity = p.Quantity };
                        _context.OrderProducts.Add(prod);
                        
                    }
                    _context.SaveChanges();

                    _emailHelper = new APIEmailHelper();
                    _emailHelper.SendOrderPlacedEmail(email, order.ID,name);
                    string smsMsg ="Hi " + name + ", thank you for placing order with us. Your order will be confirmed shortly.";
                    _smsHelper = new SmsHelper();
                    _smsHelper.SendOrderConfirmationToUser(new SmsModel() { Message =smsMsg , Number = contact });
                    _smsHelper = null;
                    var orderDetail = new OrderModel() { OrderID = order.ID, OrderPlaced = true };
                    return orderDetail;
                }
            }
            return null;


        }
    }
}