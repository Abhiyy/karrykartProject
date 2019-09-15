using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        OrderHelper _orderHelper = new OrderHelper();
        karrykartEntities _dbContext = new karrykartEntities();
        EmailHelper _emailHelper = new EmailHelper();

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetOrders()
        {
            var list = _orderHelper.GetOrders();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(Guid id)
        {
            if (id != Guid.Empty)
            { 
            
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetOrderStatus()
        {
            _dbContext = new karrykartEntities();
            var orderstatus = _dbContext.ImportantValues.Where(x=>x.Type=="OrderStatus").Select(x => new { x.Value, x.Description }).ToList();
            _dbContext = null;
            return Json(orderstatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePaymentStatus(Guid PaymentID, bool update)
        {
            _dbContext = new karrykartEntities();

            var payment = _dbContext.Payments.Find(PaymentID);
            if (payment != null)
            {
                payment.IsSuccessful = update;
                _dbContext.Entry(payment).State = System.Data.Entity.EntityState.Modified;
                int i = _dbContext.SaveChanges();
                if (i > 0)
                {
                    _dbContext = null;
                    _logger.WriteLog(CommonHelper.MessageType.Success, "Order's payment updated successfully with id=" + payment.ID, "UpdatePaymentStatus", "Order", User.Identity.Name);
                    return Json(new { messagetype = ApplicationMessages.OrderAdmin.SUCCESS, message = "Payment for order updated successfully." });
                }
                else
                {
                    _dbContext = null;
                    _logger.WriteLog(CommonHelper.MessageType.Error, "Order's payment cannot be updated successfully with id=" + payment.ID, "UpdatePaymentStatus", "Order", User.Identity.Name);
                    return Json(new { messagetype = ApplicationMessages.OrderAdmin.SUCCESS, message = "Payment for order can not be updated." });
                }

                
            }
            return View();
        }

        [HttpPost]
        public ActionResult AdjustOrderAmount(Guid PaymentID, decimal Amount)
        {
            _dbContext = new karrykartEntities();

            var payment = _dbContext.Payments.Find(PaymentID);
            if (payment != null)
            {
                payment.Amount = Amount;
                _dbContext.Entry(payment).State = System.Data.Entity.EntityState.Modified;
                int i = _dbContext.SaveChanges();

                if (i > 0)
                {
                    _dbContext = null;
                    _logger.WriteLog(CommonHelper.MessageType.Success, "Order's payment amount updated successfully with id=" + payment.ID, "AdjustOrderAmount", "Order", User.Identity.Name);
                    return Json(new { messagetype = ApplicationMessages.OrderAdmin.SUCCESS, message = "Payment amount for order updated successfully." });
                }
                else {
                    _dbContext = null;
                    _logger.WriteLog(CommonHelper.MessageType.Success, "Order's payment amount can not be updated with id=" + payment.ID, "AdjustOrderAmount", "Order", User.Identity.Name);
                    return Json(new { messagetype = ApplicationMessages.OrderAdmin.SUCCESS, message = "Payment amount for order updated successfully." });
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateOrderStatus(Guid OrderID, int Status, string UserEmail)
        {
            _dbContext = new karrykartEntities();

            var order = _dbContext.Orders.Find(OrderID);

            if (order != null)
            {
                
                order.Status = Status;
                
                if (Status == (int)CommonHelper.OrderStatus.Delievered)
                    order.DeliveredOn = DateTime.Now;

                _dbContext.Entry(order).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                var statusDesc = _dbContext.ImportantValues.Where(v => v.Value == Status && v.Type == "OrderStatus").FirstOrDefault().Description;
                _orderHelper.AddOrderJourneyElement(order.ID, Status,statusDesc , "Karrykart Team");
                
                _emailHelper.SendOrderUpdateChangeEmail(UserEmail, statusDesc);
                _dbContext = null;
                _logger.WriteLog(CommonHelper.MessageType.Success, "Order's status updated successfully with id=" + OrderID, "UpdateOrderStatus", "Order", User.Identity.Name);
                return Json(new { messagetype = ApplicationMessages.OrderAdmin.SUCCESS, message = "Order status updated successfully." });
            }
            return View();
        }
    }
}
