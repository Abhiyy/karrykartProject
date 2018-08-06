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
    public class UserDetailsController : ApiController
    {
        // GET api/<controller>
        LoginHelper _loginHelper = null;
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public UserDetails Get(Guid id)
        {
            try
            {
                _loginHelper = new LoginHelper();

               return _loginHelper.GetUser(id);
            }
            catch (Exception ex)
            {
                return null;
            }
         
        }

        // POST api/<controller>
        public UserDetails Post(AddUserAddressModel user)
        {
            try
            {
                _loginHelper = new LoginHelper();

                return _loginHelper.AddUserAddress(user);

            }
            catch (Exception ex)
            {
                return null;
            }
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