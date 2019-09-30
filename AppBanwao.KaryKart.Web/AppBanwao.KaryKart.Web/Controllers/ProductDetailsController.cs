using AppBanwao.KaryKart.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    public class ProductDetailsController : Controller
    {
        //
        // GET: /ProductDetails/
        ApiHelper _apiHelper = null;

        public ActionResult Index(Guid id)
        {

            return View();
        }

        public ActionResult AddToCart(Guid ProductID, int Quantity)
        {
            _apiHelper = new ApiHelper();
            _apiHelper.SendRequest("cart");
            return View();
        }

    }
}
