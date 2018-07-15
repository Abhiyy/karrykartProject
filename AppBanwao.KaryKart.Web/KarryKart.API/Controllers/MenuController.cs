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
    public class MenuController : ApiController
    {
        // GET api/<controller>
        MenuHelper _menuHelper = null;
        public List<KeyValuePair<MenuModel, List<MenuModel>>> Get()
        {
            try
            {
                _menuHelper = new MenuHelper();

                return _menuHelper.GetMenu();
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
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