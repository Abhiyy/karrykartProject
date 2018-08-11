using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class OrderHelper
    {
        karrykartEntities _context = null;
        APIEmailHelper _emailHelper = null;
        SmsHelper _smsHelper = null;
        CartHelper _cartHelper = null;
        LoginHelper _loginHelper = null;
        public enum PaymentMethod { 
        CashOnDelivery
        }
        public OrderModel CreateOrder(OrderInputModel OrderToCheckout)
        {
            using (_context = new karrykartEntities())
            {
                var cart = _context.Carts.Find(OrderToCheckout.CartID);
                if (cart != null)
                {
                    _cartHelper = new CartHelper();
                    var cartDetails = _cartHelper.GetCart(OrderToCheckout.CartID);
                    var payment = new Payment() { ID = Guid.NewGuid(), Amount = Convert.ToDecimal(cartDetails.GrandTotal), Type = OrderToCheckout.PaymentType, IsSuccessful = OrderToCheckout.GuestCheckout ? false : true };
                    _context.Payments.Add(payment);
                    _context.SaveChanges();
                    var order = new Order() { ID=Guid.NewGuid(),CartID=cart.ID,UserID=cart.CartUserID,PaymentID=payment.ID};
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var p in cart.CartProducts)
                    {
                        var prod = new OrderProduct() { OrderID = order.ID, ProductID = p.ProductID, Quantity = p.Quantity };
                        _context.OrderProducts.Add(prod);
                        
                    }
                    _context.SaveChanges();

                    _emailHelper = new APIEmailHelper();
                    _loginHelper = new LoginHelper();
                    var user = _context.Users.Find(OrderToCheckout.UserID);
                    var userDetails = _loginHelper.GetUser(user.UserID, OrderToCheckout.AddressID);
                    var orderHtml = BuildOrderHtml(cartDetails, order, cart, userDetails);
                    _emailHelper.SendOrderPlacedEmail(userDetails.FirstName + userDetails.LastName, user.EmailAddress, orderHtml);
                   // string smsMsg = "Hi " + user.UserDetails.FirstOrDefault().FirstName + user.UserDetails.FirstOrDefault().LastName + ", thank you for placing order with us. Your order will be confirmed shortly.";
           //         _smsHelper = new SmsHelper();
           //         _smsHelper.SendOrderConfirmationToUser(new SmsModel() { Message =smsMsg , Number = contact });
                   // _smsHelper = null;
                    var orderDetail = new OrderModel() { OrderID = order.ID, OrderPlaced = true };
                    return orderDetail;
                }
            }
            return null;

           
        }
        
        private string BuildOrderHtml(CartDetailsModel cartDetails, Order order, Cart cart, UserDetails address)
        {
            StringBuilder orderHtml = new StringBuilder();
            var productHelper= new ProductHelper();
            orderHtml.Append("<p>Your Order has been placed successfully. Please find your order details below: </p>");
            orderHtml.Append("<div class='well'>");
            orderHtml.Append("<p>");
            orderHtml.Append("<span>OrderID: "+order.ID+"</span> <br>");
            orderHtml.Append("<span>PaymentMode: " + ((order.Payment.Type.Value == 1)?"Cash On Delivery":string.Empty) + "</span>");
            orderHtml.Append("</p>");
            orderHtml.Append("<p>");
            orderHtml.Append("Delivery Details:<br>");
            orderHtml.Append(address.AddressList.FirstOrDefault().AddressLine1+"<br>");
            orderHtml.Append(address.AddressList.FirstOrDefault().AddressLine2 + "<br>");
            orderHtml.Append(address.AddressList.FirstOrDefault().City + "," + address.AddressList.FirstOrDefault().State + "," + address.AddressList.FirstOrDefault().Country+"<br>");
            orderHtml.Append("Pincode - "+address.AddressList.FirstOrDefault().PinCode + "<br>");
            orderHtml.Append("Phone - " + address.Phone + "<br>");
            orderHtml.Append("</p>");
            orderHtml.Append("<table class='table' border='1'><thead class='thead-dark'>");
            orderHtml.Append("<td>Product</td>");
            orderHtml.Append("<td>Quantity</td>");
            orderHtml.Append("<td>Price (INR)</td>");
            orderHtml.Append("<td>Total Price (INR)</td>");
            orderHtml.Append("</th>");
            orderHtml.Append("</thead>");
            orderHtml.Append("<tbody>");

            foreach (var product in cart.CartProducts)
            {
                var productDetails = productHelper.GetProductDetail(product.ProductID.Value);
                orderHtml .Append( "<tr>");
                orderHtml .Append( "<td>"+productDetails.Name+"</td>");
                orderHtml .Append( "<td>" +product.Quantity + "</td>");
                orderHtml .Append( "<td>" + productDetails.Prices.FirstOrDefault().Price.ToString() + "</td>");
                orderHtml .Append( "<td>" + (product.Quantity * productDetails.Prices.FirstOrDefault().Price).ToString() + "</td>");
                orderHtml .Append( "</tr>");
            }
            orderHtml.Append("<tr>");
            orderHtml.Append("<td>Tax("+cartDetails.TaxPercentage+"%) </td>");
            orderHtml.Append("<td></td>");
            orderHtml.Append("<td></td>");
            orderHtml.Append("<td>" + cartDetails.SubTotal + "</td>");
            orderHtml.Append("</tr>");


            orderHtml.Append("<tr>");
            orderHtml.Append("<td>Grand Total</td>");
            orderHtml.Append("<td></td>");
            orderHtml.Append("<td></td>");
            orderHtml.Append("<td>" +cartDetails.GrandTotal+ "</td>");
            orderHtml.Append("</tr>");
            orderHtml.Append("</tbody>");
            orderHtml.Append("</table>");

            return orderHtml.ToString();
        }
    }
}