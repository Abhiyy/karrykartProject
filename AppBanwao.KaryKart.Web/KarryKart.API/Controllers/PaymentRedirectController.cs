using KarryKart.API.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarryKart.API.Controllers
{
    public class PaymentRedirectController : Controller
    {
        //
        // GET: /PaymentRedirect/
        PaymentHelper _payment = null;
        public ActionResult Index(Guid UserID, Guid CartID, bool GuestCheckout, int AddressID)
        {
            try
            {
                _payment = new PaymentHelper();
                var objPayment = _payment.GetPaymentGatewayDetails(UserID, CartID, GuestCheckout, AddressID);
                objPayment.Post();
               
            }
            catch (Exception ex)
            {
                return null;
            }
            return View();
        }

        public ActionResult Success(FormCollection form) {

            string[] merc_hash_vars_seq;
                    string merc_hash_string = string.Empty;
                    string merc_hash = string.Empty;
                    string order_id = string.Empty;
                    string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                    // if PaymentSuccess Then Order Placed
                    if (form["status"].ToString() == "success")
                    {

                        merc_hash_vars_seq = hash_seq.Split('|');
                        Array.Reverse(merc_hash_vars_seq);
                        merc_hash_string = ConfigurationManager.AppSettings["MerchantSalt"] + "|" + form["status"].ToString();


                        foreach (string merc_hash_var in merc_hash_vars_seq)
                        {
                            merc_hash_string += "|";
                            merc_hash_string = merc_hash_string + (form[merc_hash_var] != null ? form[merc_hash_var] : "");

                        }
                        // Response.Write(merc_hash_string);
                        merc_hash = PaymentHelper.Generatehash512(merc_hash_string).ToLower();



                        if (merc_hash != form["hash"])
                        {
                            Response.Write("Hash value did not matched");

                        }
                        else
                        {
                            //Place order
                            OrderHelper orderHelper = new OrderHelper();

                            var transactionID = Request.Form["txnid"].ToString();

                            var userTran = PaymentHelper.GetTransactionDetail(Guid.Parse(transactionID));

                            var order= orderHelper.CreateOrder(new Models.OrderInputModel() { 
                                                                                     AddressID=userTran.AddressID.Value,
                                                                                     CartID = userTran.CartID.Value,
                                                                                     GuestCheckout = userTran.GuestCheckout.Value,
                                                                                     PaymentType = 2,
                                                                                     TransactionID=userTran.TxnID.Value,
                                                                                     UserID=userTran.UserID.Value
                                                                                   });
                                //var userId = context.UserNames.Where(x => x.EmailAddress == User.Identity.Name).FirstOrDefault().UserId;
                                //var customer = context.CutomerDetails.Where(x => x.UserId == userId).FirstOrDefault();

                                //var cart = (CartDetailsModel)TempData["CartDetails"];
                                //TempData.Keep("CartDetails");
                                //order_id = Request.Form["txnid"];
                                //int orderId = PlaceOrder(context, order_id, cart, customer, false);
                                //if (orderId > 0)
                                //{
                                //    // Send Acknowledgement Email to customer

                                //    EmailHelper.SendAcknowledgementEmail(customer.FirstName + " " + customer.LastName, orderId, cart, User.Identity.Name);
                                //    //Send Email To administrator about order

                                //    EmailHelper.SendOrderEmailToAdmin(customer.FirstName + " " + customer.LastName, orderId, cart);
                                //    //Delete Cart Details 
                                //    if (DeleteCartDetails(cart, context))
                                //    {
                                //        Response.Cookies["Cart"].Expires = DateTime.Now.AddDays(-1);
                                //    }
                                //    //Get Order Details
                                //    var orderDetail = context.Orders.Find(orderId);
                                //    return View(orderDetail);
                                //}
                            


                            //  ViewData["Message"] = "Status is successful. Hash value is matched";
                            //  Response.Write("<br/>Hash value matched");


                            //Hash value did not matched
                        }
                    }

            return View();
        }

        public ActionResult Failure() {

            return View();
        }

    }
}
