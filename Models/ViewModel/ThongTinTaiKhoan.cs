using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elearningAPI.Models.ViewModel
{
    public class ThongTinTaiKhoan
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string SoDT { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string MaNhom { get; set; }
        public string Email { get; set; }
        public List<ChiTietKhoaHoc> ChiTietKhoaHocGhiDanh = new List<ChiTietKhoaHoc>();
    }
    public class ChiTietKhoaHoc {
        public string MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }
    }

    

}