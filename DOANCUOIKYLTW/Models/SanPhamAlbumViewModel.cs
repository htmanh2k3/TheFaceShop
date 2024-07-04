using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANCUOIKYLTW.Models
{
    public class SanPhamAlbumViewModel
    {
        public DOANCUOIKYLTW.Models.SANPHAM SanPham { get; set; }
        public DOANCUOIKYLTW.Models.ALBUM Album { get; set; }
        public List<DANHGIA> DanhGiaList { get; set; } // Change to a list of DANHGIA
        public DOANCUOIKYLTW.Models.DANHGIA DanhGia { get; set; }
        public DOANCUOIKYLTW.Models.LOAI Loai { get; set; }
    }
}