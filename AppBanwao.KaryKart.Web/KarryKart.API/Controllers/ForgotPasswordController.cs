using KarryKart.API.Helpers;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KarryKart.API.Controllers
{
    public class ForgotPasswordController : ApiController
    {
        // GET api/<controller>
        LoginHelper _loginHelper = null;
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public bool Get(string key)
        {
            try
            {
                _loginHelper = new LoginHelper();
                return _loginHelper.ForgotPassword(key);
            }
            catch (Exception ex) {
                return false;
            }
        }

        // POST api/<controller>
        public bool Post(UserSignUpModel user)
        {
            try
            {
                _loginHelper = new LoginHelper();
                return _loginHelper.VerfiyOtpForgotPassword(user);
            }
            catch (Exception ex) {
                return false;
            }
        }

        // PUT api/<controller>/5
        public bool Put(UserSignUpModel user)
        {
            try
            {
                _loginHelper = new LoginHelper();
                return _loginHelper.ChangePassword(user);
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