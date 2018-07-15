using AppBanwao.KaryKart.Web.Helpers;
using KarryKart.API.Helpers;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace KarryKart.API.Controllers
{
    public class SignUpController : ApiController
    {
        // GET api/<controller>
        LoginHelper _loginHelper = null;
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public UserSignUpModel Post(UserSignUpModel user)
        {
            _loginHelper = new LoginHelper();
            var createdUser = _loginHelper.SignUpUser(user);
            var userRegType = _loginHelper.SendRegisterUserConfirmation(createdUser);
            switch (userRegType)
            {
                case ApplicationMessages.UserRegisterationType.EMAILWITHERROR:
                   // return Json(new { messagetype = ApplicationMessages.UserRegisterationType.EMAILWITHERROR, message = "Unable to deliver the email to your mailbox. Please contact administrator (call helpline) to know your OTP." }, JsonRequestBehavior.AllowGet);

                case ApplicationMessages.UserRegisterationType.EMAIL:
                    return new UserSignUpModel() { Name = user.Name, UserID = createdUser.UserID, Message = "user_created", user=user.user };
                    

                case ApplicationMessages.UserRegisterationType.MOBILE:
                    //return Json(new { messagetype = ApplicationMessages.UserRegisterationType.MOBILE, message = "success" }, JsonRequestBehavior.AllowGet);

                case ApplicationMessages.UserRegisterationType.MOBILEWITHERROR:
                    //return Json(new { messagetype = ApplicationMessages.UserRegisterationType.MOBILEWITHERROR, message = "success" }, JsonRequestBehavior.AllowGet);

                case ApplicationMessages.UserRegisterationType.USEREXIST:
                    //return Json(new { messagetype = ApplicationMessages.UserRegisterationType.USEREXIST, message = "The user already exists. Please try with different email." }, JsonRequestBehavior.AllowGet);
                    break;
            }
            return null;
        }

        // PUT api/<controller>/5
        public bool Put(UserSignUpModel user)
        {
            _loginHelper = new LoginHelper();
            try
            {
                return _loginHelper.VerifyOtp(user);
            }
            catch (Exception ex) {
                return false;
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}