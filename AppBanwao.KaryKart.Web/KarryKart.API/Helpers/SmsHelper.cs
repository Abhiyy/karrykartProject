using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class SmsHelper
    {
        string _ApiUrl = ConfigurationManager.AppSettings["SMSAPIUrl"].ToString();
        string _SmsKey = ConfigurationManager.AppSettings["SMSAPIKey"].ToString();
        public string SendOrderConfirmationToUser(SmsModel sms)
        {
            var result = SendSMS(sms.Message, sms.Number);
            return null;
        }

        public string SendSMS(string message, string number, string brand = "TXTLCL")
        {
            String msg = HttpUtility.UrlEncode(message);
           
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues(_ApiUrl, new NameValueCollection()
                {
                {"apikey" , _SmsKey},
                {"numbers" , "91"+number},
                {"message" , msg},
                {"sender" , brand}
                });
                return System.Text.Encoding.UTF8.GetString(response);
                
            }
        }
    }
}