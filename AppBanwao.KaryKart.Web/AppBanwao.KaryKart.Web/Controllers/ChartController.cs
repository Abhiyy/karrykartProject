using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppBanwao.KaryKart.Web.Models;
using AppBanwao.KaryKart.Web.Helpers;
namespace AppBanwao.KaryKart.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ChartController : BaseController
    {
        //
        // GET: /Chart/
        [HttpGet]
        public JsonResult GetSalesRevenueData(string type)
        {
            return Json(ChartHelper.GetSalesRevenueChart(type), JsonRequestBehavior.AllowGet);
        }

    }
}
