using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class OrderHelper
    { 
        public ApiHelper _apiHelper = null;
        karrykartEntities _dbContext = null;

        public OrderHelper()
        {
            _apiHelper = new ApiHelper();
            _dbContext = new karrykartEntities(); 
        }

        public IList<OrderList> GetOrders() 
        {
            using (_dbContext = new karrykartEntities())
            {
                var list = _dbContext.GetAllOrders().Select(x => new OrderList { IsGuest = x.IsGuest, OrderID = x.OrderID, PlaceOn = x.PlaceOn.Value.ToString("dd/MM/yyyy HH:mm:ss"), Status = x.OrderStatus, UserEmailAddress = x.EmailAddress }).ToList();
                return list;
            }
        }

        public void AddOrderJourneyElement(Guid OrderID, int StatusID, string OrderStatus, string user)
        {
            var OrderJourney = new OrderJourney()
            {
                OrderID = OrderID,
                OrderStatusID = StatusID,
                OrderStatus = OrderStatus,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            };

            _dbContext.OrderJourneys.Add(OrderJourney);
            _dbContext.SaveChanges();

        }

        
    }
}