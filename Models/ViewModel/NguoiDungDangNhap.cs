using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elearningAPI.Models.ViewModel
{
    public class NguoiDungDangNhap
    {
        public string TaiKhoan { get; set; }

        public string Email { get; set; }
        public string SoDT { get; set; }
        public string MaNhom { get; set;
        }

        public string MaLoaiNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string accessToken { get; set; }
    }
    public class ThongTinDangNhap {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }
}