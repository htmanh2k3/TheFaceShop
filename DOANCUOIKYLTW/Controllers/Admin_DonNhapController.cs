using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANCUOIKYLTW.Models;

namespace DOANCUOIKYLTW.Controllers
{
    public class Admin_DonNhapController : Controller
    {
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        // GET: Admin_DonNhap
        public ActionResult Index(string search = "", string SortColumn = "MADN", string IconClass = "bxs-up-arrow", int page = 1)
        {
            List<DONNHAP> donhangs = db.DONNHAPs.Where(row => row.MADN.Contains(search)).ToList();
            ViewBag.Search = search;

            ViewBag.SoLuongDHN = (int)db.SoLuongDHN();
            ViewBag.SoLuongDHNChuaNhan = (int)db.SoLuongDHNChuaNhan();
            ViewBag.SoLuongDHNDaNhan = (int)db.SoLuongDHNDaNhan();
            ViewBag.TongTriGiaDHNTrong6Thang = (double)db.TongTriGiaDHNTrong6Thang();

            //Sort
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;

            if (SortColumn == "MADN")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    donhangs = donhangs.OrderBy(row => row.MADN).ToList();
                }
                else
                {
                    donhangs = donhangs.OrderByDescending(row => row.MADN).ToList();
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
            else if (SortColumn == "TONGTIEN")
            {
                if (IconClass == "bxs-up-arrow")
                {
                    donhangs = donhangs.OrderBy(row => row.TONGTIEN).ToList();
                }
                else
                {
                    donhangs = donhangs.OrderByDescending(row => row.TONGTIEN).ToList();
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
            DONNHAP t = db.DONNHAPs.Where(row => string.Compare(row.MADN, id) == 0).FirstOrDefault();
            ViewBag.CTDN = db.CTDNs.Where(row => string.Compare(row.MADN, id) == 0).ToList();
            return View(t);
        }

        public static List<CTDN> listCTDN = new List<CTDN>();

        public int tinhThanhTien(CTDN c)
        {
            SANPHAM t = db.SANPHAMs.Where(row => string.Compare(row.MASP, c.MASP) == 0).FirstOrDefault();
            int kq = (int)t.GIANHAP * (int)c.SOLUONG;
            return kq;
        }

        public ActionResult CreateCTDN()
        {
            ViewBag.SanPham = db.SANPHAMs.ToList();
            return View();
        }

        [HttpPost]

        public ActionResult CreateCTDN(CTDN t)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CreateCTDN");
            }
            foreach(var i in listCTDN)
            {
                if (string.Compare(i.MASP, t.MASP) == 0)
                {
                    i.SOLUONG += t.SOLUONG;
                    i.THANHTIEN = tinhThanhTien(i);
                    return RedirectToAction("Create");
                }        
            }
            t.THANHTIEN = tinhThanhTien(t);
            listCTDN.Add(t);
            return RedirectToAction("Create");
        }

        public ActionResult DeleteCTDN(string id)
        {
            CTDN t = listCTDN.Where(row => string.Compare(row.MASP, id) == 0).FirstOrDefault();
            listCTDN.Remove(t);
            return RedirectToAction("Create");
        }    

        public int tinhTongTien()
        {
            int kq = 0;
            foreach(var i in listCTDN)
            {
                kq += (int)i.THANHTIEN;
            }
            return kq;
        }

        public ActionResult Create()
        {
            DONNHAP t = new DONNHAP();
            ViewBag.CTDN = listCTDN;
            t.MADN = (string)db.MaDHNTiepTheo();
            t.NGAYLAP = DateTime.Now;
            t.TONGTIEN = tinhTongTien();
            t.TRANGTHAI = "Đã đặt";
            return View(t);
        }

        [HttpPost]

        public ActionResult Create(DONNHAP t)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            t.MADN = "DN000";
            t.NGAYLAP = DateTime.Now;
            t.TONGTIEN = 0;
            t.TRANGTHAI = "Đã đặt";
            db.DONNHAPs.InsertOnSubmit(t);
            db.SubmitChanges();
            foreach (var i in listCTDN)
            {
                i.MADN = (string)db.MaDHNVuaTao();
            }

            db.CTDNs.InsertAllOnSubmit(listCTDN);
            db.SubmitChanges();
            listCTDN.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            DONNHAP t = db.DONNHAPs.Where(row => string.Compare(row.MADN, id) == 0).FirstOrDefault();
            ViewBag.CTDN = db.CTDNs.Where(row => string.Compare(row.MADN, id) == 0).ToList();
            return View(t);
        }

        [HttpPost]

        public ActionResult Edit(string id, DONNHAP t)
        {
            DONNHAP x = db.DONNHAPs.Where(row => string.Compare(row.MADN, id) == 0).FirstOrDefault();
            //update
            x.TRANGTHAI = t.TRANGTHAI;
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            DONNHAP t = db.DONNHAPs.Where(row => string.Compare(row.MADN, id) == 0).FirstOrDefault();
            ViewBag.CTDN = db.CTDNs.Where(row => string.Compare(row.MADN, id) == 0).ToList();
            return View(t);
        }

        [HttpPost]

        public ActionResult Delete(string id, DONNHAP t)
        {
            List<CTDN> ct = db.CTDNs.Where(row => string.Compare(row.MADN, id) == 0).ToList();
            db.CTDNs.DeleteAllOnSubmit(ct);
            db.SubmitChanges();
            DONNHAP x = db.DONNHAPs.Where(row => string.Compare(row.MADN, id) == 0).FirstOrDefault();
            db.DONNHAPs.DeleteOnSubmit(x);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}