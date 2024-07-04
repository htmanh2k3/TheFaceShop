using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{
    public class Admin_DonXuatController : Controller
    {
        // GET: Admin_DonXuat
        QLMyPhamDataContext db = new QLMyPhamDataContext();

        public ActionResult Index(string search = "", string SortColumn = "MADX", string IconClass = "bxs-up-arrow", int page = 1)
        {
            List<DONXUAT> donhangs = db.DONXUATs.Where(row => row.MADX.Contains(search)).ToList();
            ViewBag.Search = search;

            ViewBag.SoLuongDHX = (int)db.SoLuongDHX();
            ViewBag.SoLuongDHXChuaGiao = (int)db.SoLuongDHXChuaGiao();
            ViewBag.SoLuongDHXDaGiao = (int)db.SoLuongDHXDaGiao();
            ViewBag.TongTriGiaDHX = (long)db.TongTriGiaDHX();

            //Sort
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;

            if (SortColumn == "MADX")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    donhangs = donhangs.OrderBy(row => row.MADX).ToList();
                }
                else
                {
                    donhangs = donhangs.OrderByDescending(row => row.MADX).ToList();
                }
            }
            else if (SortColumn == "NGAYLAP")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    donhangs = donhangs.OrderBy(row => row.NGAYLAP).ToList();
                }
                else
                {
                    donhangs = donhangs.OrderByDescending(row => row.NGAYLAP).ToList();
                }
            }
            else if (SortColumn == "TRIGIA")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    donhangs = donhangs.OrderBy(row => row.TRIGIA).ToList();
                }
                else
                {
                    donhangs = donhangs.OrderByDescending(row => row.TRIGIA).ToList();
                }
            }

            //Paging
            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(donhangs.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            donhangs = donhangs.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(donhangs);
        }

        public ActionResult Detail(string id)
        {
            DONXUAT t = db.DONXUATs.Where(row => string.Compare(row.MADX, id) == 0).FirstOrDefault();
            ViewBag.CTDX = db.CTDXes.Where(row => string.Compare(row.MADX, id) == 0).ToList();
            return View(t);
        }

        public ActionResult Edit(string id)
        {
            DONXUAT t = db.DONXUATs.Where(row => string.Compare(row.MADX, id) == 0).FirstOrDefault();
            ViewBag.CTDX = db.CTDXes.Where(row => string.Compare(row.MADX, id) == 0).ToList();
            return View(t);
        }

        [HttpPost]

        public ActionResult Edit(string id, DONXUAT t)
        {
            DONXUAT x = db.DONXUATs.Where(row => string.Compare(row.MADX, id) == 0).FirstOrDefault();
            //update
            x.DIACHIGIAO = t.DIACHIGIAO;
            x.SDT = t.SDT;
            x.TRANGTHAI = t.TRANGTHAI;
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            DONXUAT t = db.DONXUATs.Where(row => string.Compare(row.MADX, id) == 0).FirstOrDefault();
            ViewBag.CTDX = db.CTDXes.Where(row => string.Compare(row.MADX, id) == 0).ToList();

            return View(t);
        }

        [HttpPost]

        public ActionResult Delete(string id, DONXUAT t)
        {
            List<CTDX> ct = db.CTDXes.Where(row => string.Compare(row.MADX, id) == 0).ToList();
            db.CTDXes.DeleteAllOnSubmit(ct);
            db.SubmitChanges();
            DONXUAT x = db.DONXUATs.Where(row => string.Compare(row.MADX, id) == 0).FirstOrDefault();
            db.DONXUATs.DeleteOnSubmit(x);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}