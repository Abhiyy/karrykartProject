using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        UserHelper _userHelper = null;
        ProductHelper _productHelper = null;
        string _adminEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
        public enum PaymentMethod { 
        CashOnDelivery=1
        }
        public OrderModel CreateOrder(OrderInputModel OrderToCheckout)
        {
            using (_context = new karrykartEntities())
            {
                var cart = _context.Carts.Find(OrderToCheckout.CartID);
                if (cart != null)
                {
                    _cartHelper = new CartHelper();
                    var cartDetails = String.IsNullOrEmpty(OrderToCheckout.Coupon)?_cartHelper.GetCart(OrderToCheckout.CartID):_cartHelper.ApplyCouponCode(OrderToCheckout.CartID,OrderToCheckout.Coupon);
                    var payment = new Payment() { ID = OrderToCheckout.GuestCheckout?Guid.NewGuid():OrderToCheckout.TransactionID==Guid.Empty?Guid.NewGuid():OrderToCheckout.TransactionID, Amount = Convert.ToDecimal(cartDetails.GrandTotal), Type = OrderToCheckout.PaymentType, IsSuccessful = OrderToCheckout.GuestCheckout ? false : OrderToCheckout.TransactionStatus=="success"? true:false };
                    _context.Payments.Add(payment);
                    _context.SaveChanges();
                    var order = new Order() { ID=Guid.NewGuid(),
                                              CartID=cart.ID,
                                              UserID=OrderToCheckout.UserID,
                                              PaymentID=payment.ID,
                                              Status= (int)APICommonHelper.OrderStatus.Placed,
                                              PlaceOn=DateTime.Now,
                                              GuestCheckout=OrderToCheckout.GuestCheckout,
                                              DeliveryAddressID=OrderToCheckout.GuestCheckout?0:OrderToCheckout.AddressID};
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var p in cart.CartProducts)
                    {
                        var prod = new OrderProduct() { OrderID = order.ID, ProductID = p.ProductID, Quantity = p.Quantity  };
                        _context.OrderProducts.Add(prod);
                        
                    }
                    _context.SaveChanges();

                    _emailHelper = new APIEmailHelper();
                    _loginHelper = new LoginHelper();
                    UserDetails userDetails = null;
                    if (!OrderToCheckout.GuestCheckout)
                    {
                        var user = _context.Users.Find(OrderToCheckout.UserID);
                        userDetails = _loginHelper.GetUser(user.UserID, OrderToCheckout.AddressID);
                    }
                    else {
                        userDetails = _loginHelper.GetGuestUserDetails(OrderToCheckout.UserID);
                    }
                    var orderHtml = BuildOrderHtml(cartDetails, order, cart, userDetails,GetOrderStatus(order,_context),OrderToCheckout.Coupon);
                    _emailHelper.SendOrderPlacedEmail(userDetails.FirstName+" " + userDetails.LastName, userDetails.Email, orderHtml);
                    _emailHelper.SendOrderPlacedEmail("Admin", _adminEmail, orderHtml);
                    AddOrderJourneyElement(order.ID, order.Status.Value, APICommonHelper.OrderStatus.Placed.ToString(), userDetails.Email);
                    APICommonHelper.AddUserAlert(userDetails.UserID, "Order has been placed succefully. Please go to my orders page for more details.");
                    var orderDetail = new OrderModel() { OrderID = order.ID, OrderPlaced = true };
                    return orderDetail;
                }
            }
            return null;

           
        }

        public List<OrderDetailModel> GetOrders() {
            List<OrderDetailModel> lstOrders = null;
            using (_context = new karrykartEntities())
            {
                var lstOrder = _context.Orders.OrderByDescending(o => o.PlaceOn).ToList();
                if (lstOrder.Count > 0)
                {
                    lstOrders = new List<OrderDetailModel>();
                    foreach (var order in lstOrder)
                    {
                        lstOrders.Add(GetOrder(order.ID));
                    }
                }
            }
            return lstOrders;
        }

        public  List<OrderDetailModel>  GetOrderByCartID(Guid UserID, Guid CartID)
        { 
            using(_context = new karrykartEntities())
            {
                var order=_context.Orders.Where(x=>x.CartID==CartID && x.UserID==UserID).FirstOrDefault();
                if (order != null)
                {
                    List<OrderDetailModel> lstOrders = new List<OrderDetailModel>();
                    lstOrders.Add(GetOrder(order.ID));
                    return lstOrders;
                }
            }
            return null;
        
        }

        public List<OrderDetailModel> GetUserOrders(Guid ID)
        {
            List<OrderDetailModel> lstOrders = null;
            using (_context = new karrykartEntities())
            {
                var lstOrder = _context.Orders.Where(o => o.UserID == ID).OrderByDescending(o=>o.PlaceOn).ToList();
                if (lstOrder.Count > 0) { 
                    lstOrders = new List<OrderDetailModel>();
                    foreach (var order in lstOrder) {
                        lstOrders.Add(GetOrder(order.ID));
                    }
                }
            }
            return lstOrders;
        }

        public OrderDetailModel GetOrder(Guid OrderID)
        {
            OrderDetailModel order = null;
            using (_context = new karrykartEntities()) {

                var oDetail = _context.Orders.Find(OrderID);

                if (oDetail != null)
                {
                    order = new OrderDetailModel(); 
                    order.OrderID = oDetail.ID;
                    order.OrderDeliveredOn = oDetail.DeliveredOn.HasValue ? oDetail.DeliveredOn.Value.ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                    order.OrderPlacedOn = oDetail.PlaceOn.Value;
                    order.OrderPlacedDateTime = oDetail.PlaceOn.Value.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    order.OrderStatusID = oDetail.Status.Value;
                    order.OrderStatus = _context.ImportantValues.Where(i => i.Type == "OrderStatus" && i.Value == order.OrderStatusID).FirstOrDefault().Description;
                    order.PaymentID = oDetail.PaymentID.Value;
                    order.PaymentType = (oDetail.Payment.Type.Value==(int)PaymentMethod.CashOnDelivery)?"Cash on delivery":"Online Payment";
                    order.PaymentTypeID = oDetail.Payment.Type.Value;
                    order.isPaymentSuccessful = oDetail.Payment.IsSuccessful.Value;
                    order.TotalAmount = oDetail.Payment.Amount.Value;
                    if (oDetail.GuestCheckout.Value)
                        order.User = new LoginHelper().GetGuestUserDetails(oDetail.UserID.Value);
                    else
                        order.User = new LoginHelper().GetUser(oDetail.UserID.Value);

                    order.GuestCheckout = oDetail.GuestCheckout.Value;
                    order.DeliveryAddress = order.User.AddressList.Where(x => x.AddressID == oDetail.DeliveryAddressID).FirstOrDefault();
                    _productHelper = new ProductHelper();

                    order.OrderProducts = new List<KeyValuePair<ProductModel, int>>();
                    
                    foreach (var p in oDetail.OrderProducts)
                    {
                        order.OrderProducts.Add(new KeyValuePair<ProductModel, int>(_productHelper.GetProductDetail(p.ProductID.Value),p.Quantity.Value));
                    }
                    
                    
                }

            }
            return order;
        }

        private string BuildOrderHtml(CartDetailsModel cartDetails, Order order, Cart cart, UserDetails address, string OrderStatus,string Coupon="")
        {
            StringBuilder orderHtml = new StringBuilder();
            var productHelper= new ProductHelper();
            orderHtml.Append("<p>Your Order has been placed successfully. Please find your order details below: </p>");
            orderHtml.Append("<div class='well'>");
            orderHtml.Append("<p>");
            orderHtml.Append("<span><strong>OrderID </strong>: " + order.ID + "</span> <br>");
            orderHtml.Append("<span><strong>PaymentMode </strong>: " + ((order.Payment.Type.Value == 1) ? "Cash On Delivery" : string.Empty) + "</span> <br>");
            orderHtml.Append("<span><strong>Order Status </strong>: " + OrderStatus + "</span> <br>");
            orderHtml.Append("</p>");
            orderHtml.Append("<p>");
            orderHtml.Append("Delivery Details:<br>");
            orderHtml.Append(address.AddressList.FirstOrDefault().AddressLine1+"<br>");
            orderHtml.Append(address.AddressList.FirstOrDefault().AddressLine2 + "<br>");
            orderHtml.Append(address.AddressList.FirstOrDefault().City + "," + address.AddressList.FirstOrDefault().State + "," + address.AddressList.FirstOrDefault().Country+"<br>");
            orderHtml.Append("Pincode - "+address.AddressList.FirstOrDefault().PinCode + "<br>");
            orderHtml.Append("Phone - " + address.Phone + "<br>");
            
            if(!string.IsNullOrEmpty(address.AddressList.FirstOrDefault().LandMark))
               orderHtml.Append("LandMark - " + address.AddressList.FirstOrDefault().LandMark + "<br>");
            
            orderHtml.Append("</p>");
            orderHtml.Append("<table style='border-collapse: collapse;border:1px solid #fcfcfc'><thead style='background-color:#06B4F0;color:#fff;font-weight:bold'>");
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
                orderHtml.Append("<tr style='padding:5px'>");
                orderHtml.Append("<td>" + productDetails.Name + "</td>");
                orderHtml.Append("<td>" + product.Quantity + "</td>");
                if (String.IsNullOrEmpty(Coupon))
                {
                    orderHtml.Append("<td>" + productDetails.Prices.FirstOrDefault().Price.ToString() + "</td>");
                    orderHtml.Append("<td>" + (product.Quantity * productDetails.Prices.FirstOrDefault().Price).ToString() + "</td>");
                }
                else
                {

                    using (_context = new karrykartEntities())
                    {
                        var coupon = _context.Coupons.Where(x => x.DisplayName.ToUpper() == Coupon.ToUpper()).FirstOrDefault();
                        if (coupon != null)
                        {
                            var couponValue = _context.CouponValues.Where(x => x.CouponID == coupon.CouponID).FirstOrDefault();
                            orderHtml.Append("<td>" + (productDetails.Prices.FirstOrDefault().Price - (productDetails.Prices.FirstOrDefault().Price * (couponValue.CouponValue1.Value / 100))).ToString() + "</td>");
                            orderHtml.Append("<td>" + (product.Quantity * (productDetails.Prices.FirstOrDefault().Price - (productDetails.Prices.FirstOrDefault().Price * (couponValue.CouponValue1.Value / 100)))).ToString() + "</td>");
                        }
                    }
                }
            }


                orderHtml.Append("</tr>");
                orderHtml.Append("<tr style='padding:5px'>");
                orderHtml.Append("<td>Tax(" + cartDetails.TaxPercentage + "%) </td>");
                orderHtml.Append("<td></td>");
                orderHtml.Append("<td></td>");
                orderHtml.Append("<td>" + cartDetails.SubTotal + "</td>");
                orderHtml.Append("</tr>");


                orderHtml.Append("<tr style='padding:5px'>");
                orderHtml.Append("<td><b>Grand Total</b></td>");
                orderHtml.Append("<td></td>");
                orderHtml.Append("<td></td>");
                orderHtml.Append("<td><b>" + cartDetails.GrandTotal + "</b></td>");
                orderHtml.Append("</tr>");
                orderHtml.Append("</tbody>");
                orderHtml.Append("</table>");
            
            return orderHtml.ToString();
        }

        private string GetOrderStatus(Order order, karrykartEntities context)
        {
            return context.ImportantValues.Where(i => i.Value == order.Status.Value && i.Type == "OrderStatus").FirstOrDefault().Description;
        }

        public void AddOrderJourneyElement(Guid OrderID, int StatusID, string OrderStatus, string user)
        {
            var OrderJourney = new OrderJourney() {
                OrderID = OrderID,
                OrderStatusID = StatusID,
                OrderStatus = OrderStatus,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            };

            _context.OrderJourneys.Add(OrderJourney);
            _context.SaveChanges();

        }

        
    }
}