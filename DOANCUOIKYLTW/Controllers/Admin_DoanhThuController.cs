using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{
    public class Admin_DoanhThuController : Controller
    {
        // GET: Admin_DoanhThu
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        public ActionResult Index(int page = 1)
        {
            ViewBag.ThongKeTongChi = db.ThongKeTongChi().ToList();
            ViewBag.ThongKeTongThu = db.ThongKeTongThu().ToList();
            ViewBag.TongTriGiaDHNTrong6Thang = (double)db.TongTriGiaDHNTrong6Thang();
            ViewBag.TongTriGiaDHX = (long)db.TongTriGiaDHX();
            ViewBag.SoLuongDHN = (int)db.SoLuongDHN();
            ViewBag.SoLuongDHX = (int)db.SoLuongDHX();
            ViewBag.thtc = db.TinhHinhThuChi().ToList();
            return View();
        }
    }
}