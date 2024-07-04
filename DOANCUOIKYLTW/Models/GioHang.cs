using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANCUOIKYLTW.Models
{
    public class GioHang
    {
        QLMyPhamDataContext db = new QLMyPhamDataContext();

        public string sMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang()
        {

        }

        public GioHang(int MaSP)
        {

        }
    }
}