using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    public class CategoryProductsController : Controller
    {
        //
        // GET: /CategoryProducts/

        public ActionResult Index(int id=-1)
        {
            return View();
        }

    }
}
