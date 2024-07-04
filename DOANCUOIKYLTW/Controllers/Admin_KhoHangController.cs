using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{
    public class Admin_KhoHangController : Controller
    {
        // GET: Admin_KhoHang
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        public ActionResult Index(string search = "", string SortColumn = "MASP", string IconClass = "bxs-up-arrow", int page = 1)
        {
            ViewBag.ThongKeLoaiBanRa = db.ThongKeLoaiBanRa().ToList();
            ViewBag.ThongKeLoaiNhapVao = db.ThongKeLoaiNhapVao().ToList();
            ViewBag.SLSanPhamHetHang = (int)db.SoLuongSPTonKhoBeHonX(0);
            ViewBag.SLSanPhamDuoiDMT = (int)db.SoLuongSPTonKhoBeHonX(150);
            ViewBag.SLSanPhamConHang = (int)db.SoLuongSPTonKhoLonHonX(0);
            ViewBag.SLSanPhamVuotDMT = (int)db.SoLuongSPTonKhoLonHonX(150);

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
            else if (SortColumn == "TONKHO")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    sanphams = sanphams.OrderBy(row => row.TONKHO).ToList();
                }
                else
                {
                    sanphams = sanphams.OrderByDescending(row => row.TONKHO).ToList();
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
    }
}