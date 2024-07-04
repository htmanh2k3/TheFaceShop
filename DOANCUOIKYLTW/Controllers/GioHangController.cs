using DOANCUOIKYLTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOANCUOIKYLTW.Controllers
{
    public class GioHangController : Controller
    {
        QLMyPhamDataContext db = new QLMyPhamDataContext();
        static List<GioHang> gioHangs = new List<GioHang>();
        static List<GIOHANG> gioHangDB = new List<GIOHANG>();

        //
        // GET: /GioHang/
        public ActionResult Index()
        {
            if (Session["TENTK"] != null)
            {
                string tenTK = Session["TENTK"].ToString();
                return View("Index", db.GIOHANGs.Where(t => t.TENTK == tenTK).ToList());
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult ThemGioHang(string maSP, int soLuong)
        {
            SANPHAM sanPham = db.SANPHAMs.Where(t => t.MASP == maSP).FirstOrDefault();
            if (Session["TENTK"] != null)
            {
                string tenTK = Session["TENTK"].ToString();

                // Nếu sản phẩm đã có trong giỏ hàng của tài khoản
                if (db.GIOHANGs.Where(t => t.MASP == maSP && t.TENTK == tenTK).Any())
                {
                    GIOHANG gh = db.GIOHANGs.Where(t => t.MASP == maSP && t.TENTK == tenTK).FirstOrDefault();
                    gh.SOLUONG = gh.SOLUONG + soLuong;
                    return View("Index", db.GIOHANGs.Where(t => t.TENTK == tenTK).ToList());
                }

                // Thêm sản phẩm vào giỏ hàng
                GIOHANG gioHang = new GIOHANG()
                {
                    MASP = sanPham.MASP,
                    TENTK = tenTK,
                    SOLUONG = soLuong
                };

                db.GIOHANGs.InsertOnSubmit(gioHang);
                db.SubmitChanges();
                return View("Index", db.GIOHANGs.Where(t => t.TENTK == tenTK).ToList());
            }
            else
            {
                List<GioHang> gioHangHienTai = Session["GioHang"] as List<GioHang>;

                if (gioHangHienTai != null && gioHangHienTai.Any(t => t.sMaSP == sanPham.MASP))
                {
                    foreach (var i in gioHangs)
                    {
                        if (i.sMaSP == sanPham.MASP)
                        {
                            i.iSoLuong += soLuong;
                        }
                    }
                    return View("Index");
                }

                GioHang gioHang = new GioHang()
                {
                    sMaSP = sanPham.MASP,
                    iSoLuong = soLuong,
                    sAnh = sanPham.ANHDAIDIEN,
                    sTenSP = sanPham.TENSP,
                    dDonGia = (double)sanPham.GIABAN
                };
                gioHangs.Add(gioHang);
                Session["GioHang"] = gioHangs;
            }
            return View("Index");
        }

        public ActionResult XoaGioHang(string maSP)
        {
            if (Session["TENTK"] != null)
            {
                string tenTK = Session["TENTK"].ToString();
                GIOHANG gh = db.GIOHANGs.Where(t => t.MASP == maSP && t.TENTK == tenTK).FirstOrDefault();
                if (gh != null)
                {
                    db.GIOHANGs.DeleteOnSubmit(gh);
                    db.SubmitChanges();
                }
                return RedirectToAction("Index", "GioHang");
            }
            else
            {
                List<GioHang> gioHangHienTai = Session["GioHang"] as List<GioHang>;
                GioHang sp = gioHangHienTai.FirstOrDefault(x => x.sMaSP == maSP);
                if (sp != null)
                {
                    gioHangHienTai.RemoveAll(s => s.sMaSP == maSP);
                    Session["GioHang"] = gioHangHienTai;
                }
                return RedirectToAction("Index", "GioHang");
            }
        }

        public ActionResult XoaAllGioHang()
        {
            if (Session["TENTK"] != null)
            {
                string tenTK = Session["TENTK"].ToString();
                List<GIOHANG> lstGH = db.GIOHANGs.Where(t => t.TENTK == tenTK).ToList();
                if (lstGH != null)
                {
                    db.GIOHANGs.DeleteAllOnSubmit(lstGH);
                    db.SubmitChanges();
                }
                return RedirectToAction("Index", "GioHang");
            }
            else
            {
                Session["GioHang"] = null;
                return RedirectToAction("Index", "GioHang");
            }
        }

        private float TinhTongTien(List<GIOHANG> g)
        {
            float kq = 0;
            foreach (var i in g)
            {
                SANPHAM s = db.SANPHAMs.Where(row => string.Compare(row.MASP, i.MASP) == 0).FirstOrDefault();
                kq += ((int)i.SOLUONG * (float)s.GIABAN);
            }
            return kq;
        }

        private CTDX ChuyenGioHangThanhDonXuat(GIOHANG gIOHANG, string mADX)
        {
            CTDX c = new CTDX();
            SANPHAM s = db.SANPHAMs.Where(row => string.Compare(row.MASP, gIOHANG.MASP) == 0).FirstOrDefault();
            c.MADX = mADX;
            c.MASP = gIOHANG.MASP;
            c.SOLUONG = gIOHANG.SOLUONG;
            c.THANHTIEN = ((decimal?)((int)gIOHANG.SOLUONG * (float)s.GIABAN));
            return c;
        }

        public ActionResult ThanhToan()
        {
            string tenTK = Session["TENTK"].ToString();
            DONXUAT t = new DONXUAT();
            List<GIOHANG> g = db.GIOHANGs.Where(row => string.Compare(row.TENTK, tenTK) == 0).ToList();
            t.MADX = (string)db.MaDHXTiepTheo();
            t.NGAYLAP = DateTime.Now;
            t.TRIGIA = (decimal?)TinhTongTien(g);
            t.TRANGTHAI = "Đang lập đơn";

            t.DIACHIGIAO = db.DONXUATs.Where(e => e.TENTK == tenTK).OrderByDescending(e => e.NGAYLAP).FirstOrDefault().DIACHIGIAO;
            t.SDT = db.DONXUATs.Where(e => e.TENTK == tenTK).OrderByDescending(e => e.NGAYLAP).FirstOrDefault().SDT;
            ViewBag.CTDX = g;
            return View(t);
        }

        [HttpPost]
        public ActionResult ThanhToan(DONXUAT t)
        {
            string tenTK = Session["TENTK"].ToString();
            List<GIOHANG> g = db.GIOHANGs.Where(row => string.Compare(row.TENTK, tenTK) == 0).ToList();
            List<CTDX> ctdx = new List<CTDX>();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ThanhToan");
            }
            t.MADX = "DX000";
            t.NGAYLAP = DateTime.Now;
            t.TENTK = tenTK;
            t.TRANGTHAI = "Đang chuẩn bị";
            t.TRIGIA = 0;
            db.DONXUATs.InsertOnSubmit(t);
            db.SubmitChanges();
            for (int i = 0; i < g.Count; i++)
            {
                CTDX x = new CTDX();
                x.MADX = (string)db.MaDHXVuaTao();
                x.MASP = g[i].MASP;
                x.SOLUONG = g[i].SOLUONG;
                ctdx.Add(x);
            }
            db.CTDXes.InsertAllOnSubmit(ctdx);
            db.SubmitChanges();
            db.GIOHANGs.DeleteAllOnSubmit(g);
            db.SubmitChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ThanhToanChuaDangNhap()
        {

            gioHangDB = new List<GIOHANG>();
            string tenTK = Session["TENTK"].ToString();

            foreach (GioHang gioHang in gioHangs)
            {
                GIOHANG GH = new GIOHANG()
                {
                    MASP = gioHang.sMaSP,
                    SOLUONG = gioHang.iSoLuong,
                    TENTK = tenTK
                };
                gioHangDB.Add(GH);

                if (!db.GIOHANGs.Any(t => t.MASP == gioHang.sMaSP && t.TENTK == tenTK))
                {
                    db.GIOHANGs.InsertOnSubmit(GH);
                }

            }
            db.SubmitChanges();

            //return RedirectToAction("ThanhToan", gioHangDB);
            DONXUAT dx = LayDonXuat(gioHangDB);

            gioHangDB.Clear();
            foreach (GioHang gioHang in gioHangs)
            {
                gioHangDB.Add(db.GIOHANGs.First(t => t.MASP == gioHang.sMaSP && t.TENTK == tenTK));
            }
            ViewBag.CTDX = gioHangDB;

            return View("ThanhToan", dx);
        }

        [HttpPost]
        public ActionResult ThanhToanChuaDangNhap(DONXUAT t)
        {
            string tenTK = Session["TENTK"].ToString();
            List<CTDX> ctdx = new List<CTDX>();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ThanhToan");
            }
            t.MADX = "DX000";
            t.NGAYLAP = DateTime.Now;
            t.TENTK = tenTK;
            t.TRANGTHAI = "Đang chuẩn bị";
            t.TRIGIA = 0;
            db.DONXUATs.InsertOnSubmit(t);
            db.SubmitChanges();
            for (int i = 0; i < gioHangDB.Count; i++)
            {
                CTDX x = new CTDX();
                x.MADX = (string)db.MaDHXVuaTao();
                x.MASP = gioHangDB[i].MASP;
                x.SOLUONG = gioHangDB[i].SOLUONG;
                ctdx.Add(x);
            }
            db.CTDXes.InsertAllOnSubmit(ctdx);
            db.SubmitChanges();
            foreach (GIOHANG gh in gioHangDB)
            {
                db.GIOHANGs.DeleteOnSubmit(db.GIOHANGs.First(e => e.MASP == gh.MASP && e.TENTK == tenTK));
            }
            db.SubmitChanges();

            return RedirectToAction("Index", "Home");
        }

        public DONXUAT LayDonXuat(List<GIOHANG> g)
        {
            string tenTK = Session["TENTK"].ToString();
            DONXUAT t = new DONXUAT();
            t.MADX = (string)db.MaDHXTiepTheo();
            t.NGAYLAP = DateTime.Now;
            t.TRIGIA = (decimal?)TinhTongTien(g);
            t.TRANGTHAI = "Đang lập đơn";

            t.DIACHIGIAO = db.DONXUATs.Where(e => e.TENTK == tenTK).OrderByDescending(e => e.NGAYLAP).FirstOrDefault().DIACHIGIAO;
            t.SDT = db.DONXUATs.Where(e => e.TENTK == tenTK).OrderByDescending(e => e.NGAYLAP).FirstOrDefault().SDT;

            return (t);
        }

        [HttpGet]
        public ActionResult ThayDoiSoLuong(string maSP, int soLuong)
        {
            try
            {
                string tenTK = Session["TENTK"].ToString();
                // Đã đăng nhập
                GIOHANG GH = db.GIOHANGs.FirstOrDefault(t => t.MASP == maSP && t.TENTK == tenTK);

                GH.SOLUONG = soLuong;
                db.SubmitChanges();

                return PartialView("ThongTinGioHang", db.GIOHANGs.Where(t=>t.TENTK == tenTK).ToList());
            }
            catch
            {
                // Chưa đăng nhập
                gioHangs.FirstOrDefault(t => t.sMaSP == maSP).iSoLuong = soLuong;
                return PartialView("ThongTinGioHang");
            }

        }

        public ActionResult ThongTinGioHang()
        {
            if (Session["TENTK"] != null)
            {
                string tenTK = Session["TENTK"].ToString();

                return PartialView(db.GIOHANGs.Where(t => t.TENTK == tenTK).ToList());
            }

            return PartialView();
        }
    }
}