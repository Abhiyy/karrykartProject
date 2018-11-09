using AppBanwao.KarryKart.Model;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KarryKart.API.Helpers
{
    

    public class PaymentHelper
    {
        LoginHelper _loginHelper = null;
        CartHelper _cartHelper = null;
        karrykartEntities _context = null;

        public class RemotePost
    {
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();


        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();

            System.Web.HttpContext.Current.Response.Write("<html><head>");

            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");

            System.Web.HttpContext.Current.Response.End();
        }
    }

        public PaymentRedirectModel GetPaymentURL(PaymentModel model,int AddressID, bool GuestCheckout=true) {


            if (model.CartID != Guid.Empty && model.UserID != Guid.Empty) {
                _loginHelper = new LoginHelper();
                _cartHelper = new CartHelper();
                var user = _loginHelper.GetUser(model.UserID);
                var cart = _cartHelper.GetCart(model.CartID);
                var txnID = SaveTranasactionDetail(model.UserID, model.CartID,GuestCheckout,AddressID);
                var productInfo = "food order";
                string hashString = ConfigurationManager.AppSettings["MerchantKey"].ToString() + "|" + txnID + "|" + cart.CartTotal + "|" + productInfo + "|" + user.FirstName + "|" + user.Email + "|||||||||||" + ConfigurationManager.AppSettings["MerchantSalt"].ToString();
                var dataOptions = new PaymentDetail() { amount =cart.CartTotal,
                                                        email = user.Email,
                                                        firstname=user.FirstName,
                                                        furl = ConfigurationManager.AppSettings["FailureURL"].ToString(),
                                                        hash = Generatehash512(hashString),
                                                        key = ConfigurationManager.AppSettings["MerchantKey"].ToString(),
                                                        phone = user.Phone,
                                                        productinfo = productInfo,
                                                        service_provider = ConfigurationManager.AppSettings["PaymentProvider"].ToString(),
                                                        surl = ConfigurationManager.AppSettings["SuccessURL"].ToString(),
                                                        txnid = txnID.ToString()
                
                };

                var responseObject = new PaymentRedirectModel() { URL = ConfigurationManager.AppSettings["PaymentURL"].ToString(),options=dataOptions };

                return responseObject;
            }

            return null;
        }

        public static Guid SaveTranasactionDetail(Guid UserID, Guid CartID,bool GuestCheckout, int AddressID) {

            using (karrykartEntities dbContext = new karrykartEntities()) {

                var txnID = Guid.NewGuid();

                dbContext.PaymentTransactions.Add(new PaymentTransaction(){
                                                                           CartID = CartID, 
                                                                           TxnID=txnID,
                                                                           UserID = UserID,
                                                                           GuestCheckout=GuestCheckout,
                                                                           AddressID= AddressID
                                                                           });
                dbContext.SaveChanges();
                return txnID;
            }
        }

        public static PaymentTransaction GetTransactionDetail(Guid TxnID)
        {
            using (karrykartEntities dbContext = new karrykartEntities())
            {
                var paymentTransaction = dbContext.PaymentTransactions.Where(x => x.TxnID==TxnID).FirstOrDefault();

                return paymentTransaction;
            }

        }

        public static string UpdateTransactionDetail(Guid TxnID, string Status)
        {
            using (karrykartEntities dbContext = new karrykartEntities())
            {
                var transaction = dbContext.PaymentTransactions.Where(x => x.TxnID == TxnID).FirstOrDefault();
                if (transaction != null)
                {
                    transaction.TransactionStatus = Status;
                    dbContext.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                    transaction.TxnID.Value.ToString();
                }
                return null;
            }
        }

        public static string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        public RemotePost GetPaymentGatewayDetails(Guid UserID, Guid CartID, bool GuestCheckout, int AddressID)
        {
            _cartHelper = new CartHelper();
            _loginHelper = new LoginHelper();
            var user = _loginHelper.GetUser(UserID);
            var cart = _cartHelper.GetCart(CartID);
            var txnID = PaymentHelper.SaveTranasactionDetail(UserID, CartID,GuestCheckout,AddressID).ToString();
            var productInfo = "order";
            RemotePost objPayment = new RemotePost();

            objPayment.Url = ConfigurationManager.AppSettings["PaymentURL"].ToString();

            objPayment.Add("key", ConfigurationManager.AppSettings["MerchantKey"].ToString());

            string amount = Math.Floor(cart.CartTotal).ToString();
            //string txnid = Guid.NewGuid().ToString();
            objPayment.Add("txnid", txnID);
            objPayment.Add("amount", amount);
            objPayment.Add("productinfo", productInfo);
            objPayment.Add("firstname", user.FirstName);
            objPayment.Add("phone", user.Phone);
            objPayment.Add("email", user.Email);
            objPayment.Add("surl", ConfigurationManager.AppSettings["SuccessURL"].ToString());//Change the success url here depending upon the port number of your local system.
            objPayment.Add("furl", ConfigurationManager.AppSettings["FailureURL"].ToString()); //Change the failure url here depending upon the port number of your local system.
            objPayment.Add("service_provider", "payu_paisa");
            string hashString = ConfigurationManager.AppSettings["MerchantKey"].ToString() + "|" + txnID + "|" + amount + "|" + productInfo + "|" + user.FirstName + "|" + user.Email + "|||||||||||" + ConfigurationManager.AppSettings["MerchantSalt"].ToString();
            //string hashString = "3Q5c3q|2590640|3053.00|OnlineBooking|vimallad|ladvimal@gmail.com|||||||||||mE2RxRwx";
            //  string hash = Generatehash512(hashString);
            objPayment.Add("hash",Generatehash512(hashString));
            return objPayment;
        }
    }

    
}