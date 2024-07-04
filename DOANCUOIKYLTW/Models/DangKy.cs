using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOANCUOIKYLTW.Models
{
    public class DangKy
    {
        [Required]
        public string TenTK { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmMatKhau { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool QuanTri { get; set; }
    }
}