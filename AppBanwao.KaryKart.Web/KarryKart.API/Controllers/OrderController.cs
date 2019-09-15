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
    public class OrderController : ApiController
    {
        OrderHelper _orderHelper = null;
         //GET api/<controller>
        public List<OrderDetailModel> Get()
        {
            try
            {
                _orderHelper = new OrderHelper();
                return _orderHelper.GetOrders();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET api/<controller>/5
        public List<OrderDetailModel> Get(string OrderID, string UserID, string CartID, bool ForAdmin=false)
        {
            try
            {
                _orderHelper = new OrderHelper();
                
                if (!string.IsNullOrEmpty(OrderID) && ForAdmin)
                { 
                //return _orderHelper.GetOrder(Guid.Parse(OrderID));
                }
                if (!string.IsNullOrEmpty(OrderID))
                {
                    return _orderHelper.GetOrderByCartID(Guid.Parse(UserID), Guid.Parse(CartID));
                }
                else {
                    return _orderHelper.GetUserOrders( Guid.Parse(UserID));
                }
            }
            catch (Exception ex) {
                return null;
            }

            return null;
        }

        // POST api/<controller>
        public OrderModel Post(OrderInputModel model)
        {
            try {
                _orderHelper = new OrderHelper();

                return _orderHelper.CreateOrder(model);
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