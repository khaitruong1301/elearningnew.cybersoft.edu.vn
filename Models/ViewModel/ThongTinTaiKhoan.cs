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
        public List<ChiTietKhoaHocVM> ChiTietKhoaHocGhiDanh = new List<ChiTietKhoaHocVM>();
    }
    public class ChiTietKhoaHoc {
        public string MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }
    }
    public class ChiTietKhoaHocVM
    {
        public string MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string MoTa { get; set; }
        public int? LuotXem { get; set; }
        public string HinhAnh { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? DanhGia { get; set; }
    }



}