using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{

    public class HomeController : Controller
    {
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            ViewBag.Top10SPBanChay = db.Top10SPBanChay().ToList();
            return View();
        }
    }
}
