using DOANCUOIKYLTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANCUOIKYLTW.Controllers
{
    [Authorize]
    public class DanhGiaController : Controller
    {
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        // GET: DanhGia
        public ActionResult ShowDanhGia(string id)
        {
            return View(db.DANHGIAs.Where(x=>x.MASP==id).ToList());
        }
        public ActionResult ThemDanhGia()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemDanhGia(DANHGIA model)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Lấy thông tin người dùng hiện tại từ CSDL
                var user = db.TAIKHOANs.FirstOrDefault(u => u.TENTK == User.Identity.Name);

                // Kiểm tra xem người dùng đã đánh giá sản phẩm này chưa
                var daDanhGia = db.DANHGIAs.Any(dg => dg.MASP == model.MASP && dg.TENTK == user.TENTK);

                if (!daDanhGia)
                {
                    // Thêm đánh giá vào CSDL
                    var danhGia = new DANHGIA
                    {
                        TENTK = user.TENTK,
                        MASP = model.MASP,
                        BINHLUAN = model.BINHLUAN,
                        MUCDODG = model.MUCDODG,
                        NGAYBL = DateTime.Now
                    };

                    db.DANHGIAs.InsertOnSubmit(danhGia);
                    db.SubmitChanges();

                    return RedirectToAction("DetailSanPham", new { id = model.MASP });
                }
                else
                {
                    TempData["Message"] = "Bạn đã đánh giá sản phẩm này rồi.";
                    return RedirectToAction("DetailSanPham", new { id = model.MASP });
                }
            }
            else
            {
                TempData["Message"] = "Bạn cần đăng nhập để có thể đánh giá.";
                return RedirectToAction("DangNhap", "NguoiDung");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}