using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Linq;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{
    public class SanPhamController : Controller
    {
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        public ActionResult showProduct()
        {
            var LstSanPham_Album = (from sp in db.SANPHAMs 
                                 join alb in db.ALBUMs on sp.MASP equals alb.MASP 
                                 select new SanPhamAlbumViewModel
                                 {
                                    SanPham = sp, Album = alb
                                 }
                                 ).ToList();
            return View(LstSanPham_Album);
        }
        //
        public ActionResult SearchSanPham(string keyword)
        {
            var LstSanPham_Album = (from sp in db.SANPHAMs
                                    join alb in db.ALBUMs on sp.MASP equals alb.MASP
                                    select new SanPhamAlbumViewModel
                                    {
                                        SanPham = sp,
                                        Album = alb
                                    }
                         ).ToList();

            var filteredProducts = LstSanPham_Album
                .Where(x => x.SanPham.TENSP.ToLower().Contains(keyword.ToLower()))
                .ToList();

            if (filteredProducts.Count <= 0)
            {
                ViewBag.TB = "Không tìm thấy " + keyword + " bạn đang tìm";
            }

            return View(filteredProducts);
        }

        public ActionResult DetailSanPham(string id)
        {
            var review = db.DANHGIAs.Where(r => r.MASP == id).ToList();
            var SanPham_Album_DanhGia = (from sp in db.SANPHAMs
                                         join alb in db.ALBUMs on sp.MASP equals alb.MASP
                                         join dg in db.DANHGIAs on sp.MASP equals dg.MASP into danhGiaGroup
                                         from danhGia in danhGiaGroup.DefaultIfEmpty() // Sử dụng left join để lấy cả sản phẩm không có đánh giá
                                         where sp.MASP == id
                                         select new SanPhamAlbumViewModel
                                         {
                                             SanPham = sp,
                                             Album = alb,
                                             DanhGiaList = danhGiaGroup.ToList(), // Convert the group to a list
      
                                         }).ToList();

            return View(SanPham_Album_DanhGia);
            //return View(db.SANPHAMs.FirstOrDefault(x => x.MASP == id));
        }
        public ActionResult ChiTietSP(string id)
        {
            return View(db.SANPHAMs.FirstOrDefault(x => x.MASP == id));
        }
        public ActionResult SanPhamTheoLoai(string loai)
        {
            var LstSanPham_Album = (from sp in db.SANPHAMs
                                    join alb in db.ALBUMs on sp.MASP equals alb.MASP
                                    select new SanPhamAlbumViewModel
                                    {
                                        SanPham = sp,
                                        Album = alb
                                    })
                                    .ToList();

            var sanPhamTheoLoai = LstSanPham_Album
                .Where(x => x.SanPham.MALOAI == loai)
                .ToList();

            return View("showProduct", sanPhamTheoLoai);
        }

        public PartialViewResult LoaiPartiview()
        {
            var loaiSanPham = (from sp in db.SANPHAMs
                               join loai in db.LOAIs on sp.MALOAI equals loai.MALOAI
                               select new SanPhamAlbumViewModel
                               {
                                   SanPham = sp,
                                   Album = null // Set to null or populate as needed
                               })
                               .GroupBy(x => x.SanPham.MALOAI)
                               .Select(group => group.First())
                               .ToList();

            return PartialView(loaiSanPham);
        }

        public ActionResult LocSanPhamTheoGia(string priceRange)
        {
            var LstSanPham_Album = (from sp in db.SANPHAMs
                                    join alb in db.ALBUMs on sp.MASP equals alb.MASP
                                    select new SanPhamAlbumViewModel
                                    {
                                        SanPham = sp,
                                        Album = alb
                                    });

            IQueryable<SanPhamAlbumViewModel> filteredProducts = LstSanPham_Album;
            switch (priceRange)
            {
                case "0-100000":
                    filteredProducts = filteredProducts.Where(p => p.SanPham.GIABAN >= 0 && p.SanPham.GIABAN <= 100000);
                    break;
                case "100000-200000":
                    filteredProducts = filteredProducts.Where(p => p.SanPham.GIABAN > 100000 && p.SanPham.GIABAN <= 200000);
                    break;
                case "above200000":
                    filteredProducts = filteredProducts.Where(p => p.SanPham.GIABAN > 200000);
                    break;
            }
            return View("showProduct", filteredProducts.ToList());
        }

        [HttpPost]
        public ActionResult AddReview(string masp, float mucdodg, string Review)
        {
            string tenTK = Session["TENTK"].ToString();
            var danhgia = new DANHGIA
            {
                MASP = masp,
                TENTK = tenTK,
                MUCDODG = mucdodg,
                NGAYBL = DateTime.Now,
                BINHLUAN = Review
            };
            db.DANHGIAs.InsertOnSubmit(danhgia);
            db.SubmitChanges();

            return RedirectToAction("DetailSanPham", "SanPham");
        }

        // GET: /SanPham/
        public ActionResult Index()
        {
            return View();
        }
        
	}
}