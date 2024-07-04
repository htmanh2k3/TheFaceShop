using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{
    public class Admin_SanPhamController : Controller
    {
        // GET: Admin_SanPham
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        public ActionResult Index(string search = "", string SortColumn = "MASP", string IconClass = "bxs-up-arrow", int page = 1)
        {
            ViewBag.SoLuongSPDangBan = (int)db.SoLuongSPDangBan();
            ViewBag.SoLuongSPTamNgung = (int)db.SoLuongSPTamNgung();
            ViewBag.SanPhamBanChayNhat = (string)db.SanPhamBanChayNhat();
            ViewBag.DanhGiaTrungBinh = (float)db.DanhGiaTrungBinh();

            List<SANPHAM> sanphams = db.SANPHAMs.Where(row => row.TENSP.Contains(search)).ToList();
            ViewBag.Search = search;

            //Sort
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;

            if (SortColumn == "MASP")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    sanphams = sanphams.OrderBy(row => row.MASP).ToList();
                }
                else
                {
                    sanphams = sanphams.OrderByDescending(row => row.MASP).ToList();
                }
            }
            else if (SortColumn == "TENSP")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    sanphams = sanphams.OrderBy(row => row.TENSP).ToList();
                }
                else
                {
                    sanphams = sanphams.OrderByDescending(row => row.TENSP).ToList();
                }
            }
            else if (SortColumn == "GIANHAP")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    sanphams = sanphams.OrderBy(row => row.GIANHAP).ToList();
                }
                else
                {
                    sanphams = sanphams.OrderByDescending(row => row.GIANHAP).ToList();
                }
            }

            //Paging
            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(sanphams.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            sanphams = sanphams.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(sanphams);
        }

        public ActionResult Detail(string ID)
        {
            SANPHAM t = db.SANPHAMs.Where(row => string.Compare(row.MASP, ID) == 0).FirstOrDefault();
            ViewBag.TongSoBanRaCuaSPX = (int)db.TongSoBanRaCuaSPX(ID);
            ViewBag.TongSoNhapVeCuaSPX = (int)db.TongSoNhapVeCuaSPX(ID);
            var LanCuoiBanRaCuaSPX = (DateTime)db.LanCuoiBanRaCuaSPX(ID);
            ViewBag.LanCuoiBanRaCuaSPX = LanCuoiBanRaCuaSPX.ToString("dd-MM-yyyy");
            if (ViewBag.LanCuoiBanRaCuaSPX == "01-01-2000")
            {
                ViewBag.LanCuoiBanRaCuaSPX = "Chưa xuất";
            }
            ViewBag.hdnx = db.HoatDongSPX(ID).ToList();

            return View(t);
        }

        public ActionResult Create()
        {
            ViewBag.Loai = db.LOAIs.ToList();
            return View();
        }

        [HttpPost]

        public ActionResult Create(SANPHAM t)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            t.MASP = "SP000";
            t.TONKHO = 0;
            t.TONGDANHGIA = 0;
            db.SANPHAMs.InsertOnSubmit(t);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(string id)
        {
            SANPHAM t = db.SANPHAMs.Where(row => string.Compare(row.MASP, id) == 0).FirstOrDefault();
            ViewBag.Loai = db.LOAIs.ToList();
            return View(t);
        }

        [HttpPost]

        public ActionResult Edit(string id, SANPHAM t)
        {
            SANPHAM sp = db.SANPHAMs.Where(row => string.Compare(row.MASP, id) == 0).FirstOrDefault();
            //update
            sp.TENSP = t.TENSP;
            sp.MALOAI = t.MALOAI;
            sp.GIABAN = t.GIABAN;
            sp.GIANHAP = t.GIANHAP;
            sp.DANGBAOCHE = t.DANGBAOCHE;
            sp.QCDONGGOI = t.QCDONGGOI;
            sp.ANHDAIDIEN = t.ANHDAIDIEN;
            sp.TRANGTHAI = t.TRANGTHAI;
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            SANPHAM t = db.SANPHAMs.Where(row => string.Compare(row.MASP, id) == 0).FirstOrDefault();
            return View(t);
        }

        [HttpPost]

        public ActionResult Delete(string id, SANPHAM t)
        {
            QLMyPhamDataContext db = new QLMyPhamDataContext();
            SANPHAM s = db.SANPHAMs.Where(row => string.Compare(row.MASP, id) == 0).FirstOrDefault();
            db.SANPHAMs.DeleteOnSubmit(s);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}