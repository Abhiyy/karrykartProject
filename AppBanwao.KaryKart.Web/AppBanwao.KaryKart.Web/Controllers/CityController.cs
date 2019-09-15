using AppBanwao.KaryKart.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    public class CityController : BaseController
    {
        //
        // GET: /City/
        LocationHelper _locationHelper = new LocationHelper();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetData()
        {
            var cities = _locationHelper.GetCity();
            return Json(cities,JsonRequestBehavior.AllowGet);
        }

    }
}
