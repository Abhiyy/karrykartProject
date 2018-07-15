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

        public ActionResult Index(Guid id)
        {

            return View();
        }

    }
}
