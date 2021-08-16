using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elearningAPI.Models.ViewModel
{
    public class NguoiDungVM
    {

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string SoDT { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string MaNhom { get; set; }
        public string Email { get; set; }
    }


    public class NguoiDungVMM
    {

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string SoDT { get; set; }

        public string MaNhom { get; set; }
        public string Email { get; set; }
    }
    public class NguoiDungResult
    {

        public string TaiKhoan { get; set; }

        public string HoTen { get; set; }
        public string SoDT { get; set; }

        public string MaNhom { get; set; }
        public string Email { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string TenLoaiNguoiDung { get; set; }
    }

    public class HocVienVM
    {

        public string TaiKhoan { get; set; }
        public string BiDanh { get; set; }
      
        public string HoTen { get; set; }
    }
}