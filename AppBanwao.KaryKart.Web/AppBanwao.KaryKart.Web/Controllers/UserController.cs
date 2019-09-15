using AppBanwao.KaryKart.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppBanwao.KaryKart.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        //
        // GET: /User/
        UserHelper _userHelper = new UserHelper();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetData()
        {

            return Json(_userHelper.GetAllUsers(), JsonRequestBehavior.AllowGet);
        }
    }
}
