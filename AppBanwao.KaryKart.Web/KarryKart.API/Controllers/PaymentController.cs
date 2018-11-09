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
    public class PaymentController : ApiController
    {
        // GET api/<controller>
        PaymentHelper _payment = null;
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

        public PaymentRedirectModel Post(PaymentModel data)
        {
            try
            {
                _payment = new PaymentHelper();
                var payment = _payment.GetPaymentURL(data,0);
                
                return payment;
            }
            catch (Exception ex) {
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