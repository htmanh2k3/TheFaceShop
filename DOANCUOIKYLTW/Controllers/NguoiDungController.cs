using DOANCUOIKYLTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DOANCUOIKYLTW.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        QLMyPhamDataContext db=new QLMyPhamDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(TAIKHOAN model)
        {
            if (ModelState.IsValid)
            {
                var user = new DangKy
                {
                    TenTK = model.TENTK,
                    MatKhau = model.MATKHAU,
                    Email = model.EMAIL,
                    QuanTri=false,
                };
                if (model.QUANTRI == null)
                    model.QUANTRI = false;
                db.TAIKHOANs.InsertOnSubmit(model);
                db.SubmitChanges();
                // Process registration (e.g., save to the database)
                // Redirect to a success page or login page
                return RedirectToAction("DangNhap");
            }
            // Model is not valid; redisplay the registration view with validation errors
            return View(model);
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(TAIKHOAN model)
        {
            var user=db.TAIKHOANs.FirstOrDefault(u=>u.TENTK==model.TENTK && u.MATKHAU==model.MATKHAU);
            if(user!=null)
            {
                Session["TENTK"] = user.TENTK;
                Session.Remove("GioHang");

                ViewBag.Message = "Xin chào" + user.TENTK;
                if (user.QUANTRI==false) 
                {
                    return RedirectToAction("showProduct", "SanPham");
                }
                else
                {
                    return RedirectToAction("Index", "Admin_SanPham");
                }
            }
            ViewBag.Message = "Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin đăng nhập.";
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }
        public ActionResult DangXuat()
        {
            Session["TENTK"] = null;
            return RedirectToAction("DangNhap", "NguoiDung");
        }

        [HttpGet]
        public ActionResult DangNhap_GioHang()
        {
            ViewBag.thanhToanGioHang = true;
            return View("DangNhap");
        }

        [HttpPost]
        public ActionResult DangNhap_GioHang(TAIKHOAN model)
        {
            var user=db.TAIKHOANs.FirstOrDefault(u=>u.TENTK==model.TENTK && u.MATKHAU==model.MATKHAU);
            if(user!=null)
            {
                Session["TENTK"] = user.TENTK;
                Session.Remove("GioHang");

                ViewBag.Message = "Xin chào" + user.TENTK;
                if (user.QUANTRI==false) 
                {
                    return RedirectToAction("ThanhToanChuaDangNhap", "GioHang");
                }
                else
                {
                    return RedirectToAction("Index", "Admin_SanPham");
                }
            }
            ViewBag.Message = "Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin đăng nhập.";
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View("DangNhap", model);
        }
    }
    
}