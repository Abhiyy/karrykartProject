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
    
    public class LoginController : ApiController
    {
        // GET api/<controller>
        LoginHelper _login = null;
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public UserInformation Get(string key, string value)
        {
            try
            {
                _login = new LoginHelper();
                return _login.LoginUser(new UserModel() { user=key,pwd=value});
            }
            catch (Exception ex){
                return new UserInformation() { Error = ex.Message };
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