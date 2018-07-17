using KarryKart.API.Helpers;
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
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}