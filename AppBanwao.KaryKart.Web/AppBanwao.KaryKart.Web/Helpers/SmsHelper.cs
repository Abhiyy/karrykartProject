﻿using AppBanwao.KaryKart.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class SmsHelper
    {
        string _ApiUrl = ConfigurationManager.AppSettings["SMSAPIUrl"].ToString();
        string _SmsKey = ConfigurationManager.AppSettings["SMSAPIKey"].ToString();
        public static bool SendRegisterMessage(string userNumber)
        {

            return false;
        }

        public static bool SendVerificationMessage(string userNumber)
        {

            return false;
        }

        public static bool SendOtpMessage(string userNumber,string otp)
        {
            return false;
        }

        public static bool SendChangePasswordMessage(string userNumber)
        {
            return false;
        }

        public string SendOrderConfirmationToUser(SmsModel sms)
        {
            SendSMS(sms.Message, sms.Number);
            return null;
        }

        public string SendSMS(string message, string number, string brand="HUNGCL")
        {
            String msg = HttpUtility.UrlEncode(message);
            number += "91";
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues(_ApiUrl, new NameValueCollection()
                {
                {"apikey" , _SmsKey},
                {"numbers" , number},
                {"message" , msg},
                {"sender" , brand}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }
    }
}