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
    public class SearchController : ApiController
    {
        ProductHelper _productHelper = null;
        public List<ProductModel> Get(string ProductName)
        {
            try
            {
                _productHelper = new ProductHelper();

                return _productHelper.GetSearchedProduct(ProductName);
            }
            catch (Exception ex)
            {

            }
            finally
            {

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