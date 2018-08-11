using DA.EmailEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class APIEmailHelper
    {
        string _senderEmail = ConfigurationManager.AppSettings["SenderEmail"].ToString();
        string _adminEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
        string _companyLogo = ConfigurationManager.AppSettings["Logo"].ToString();
        string _supportPhone = ConfigurationManager.AppSettings["ContactNumber"].ToString();

        public bool SendRegisterEmail(string toEmail, string Brand="KarryKart.com")
        {
            string body;
            //Read template file from the App_Data folder
            try
            {
                using (var sr = new StreamReader(HttpContext.Current.Server.MapPath(@"\\App_Data\\EmailTemplates\RegisterConfirmation.txt")))
                {
                    body = sr.ReadToEnd();
                }
                string otp = toEmail.Substring(0,4)+CommonHelper.GenerateOTP();
                CommonHelper.SaveOTP(otp,toEmail);
                string messagebody = string.Format(body, _companyLogo, "", toEmail,otp ,  _supportPhone);
                return SendEmail(string.Format("Welcome to {0}", Brand), messagebody, toEmail);
                
            }
            catch (Exception ex) {
                return false;
            }
            return false;
        }

        public bool SendVerificationEmail(string toEmail, string Brand = "KarryKart.com")
        {
            string body;
            //Read template file from the App_Data folder
            try
            {
                using (var sr = new StreamReader(HttpContext.Current.Server.MapPath(@"\\App_Data\\EmailTemplates\OtpVerificationEmail.txt")))
                {
                    body = sr.ReadToEnd();
                }
               
                string messagebody = string.Format(body, _companyLogo, "", toEmail, _supportPhone);
                
                return SendEmail(string.Format("{0} - Email Verification Complete",Brand), messagebody, toEmail);
                
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public bool SendOtpEmail(string toEmail, string otp, string Brand = "KarryKart.com")
        {

            string body;
            //Read template file from the App_Data folder
            try
            {
                using (var sr = new StreamReader(HttpContext.Current.Server.MapPath(@"\\App_Data\\EmailTemplates\OtpEmail.txt")))
                {
                    body = sr.ReadToEnd();
                }

                string messagebody = string.Format(body, _companyLogo,  toEmail,otp, _supportPhone);

                return SendEmail(string.Format("{0} - OTP",Brand), messagebody, toEmail);

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public bool SendOrderPlacedEmail(string name, string toEmail, string orderHtml,string Brand = "KarryKart.com")
        {

            string body;
            //Read template file from the App_Data folder
            try
            {
                using (var sr = new StreamReader(HttpContext.Current.Server.MapPath(@"\\App_Data\\EmailTemplates\OrderPlaceEmail.txt")))
                {
                    body = sr.ReadToEnd();
                }

                string messagebody = string.Format(body, _companyLogo, name, orderHtml, _supportPhone);

                return SendEmail(string.Format("{0}- Order Placed: ",Brand), messagebody, toEmail);

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        bool SendEmail(string Subject, string Body, string EmailAddress)
        {
            //StringBuilder
            Mailer objEmail = new Mailer();
            objEmail.Recipient = EmailAddress;
            objEmail.Body = Body;
            objEmail.Sender = _senderEmail;
            objEmail.Subject = Subject;
            return objEmail.Send();
           
        }
    }
}