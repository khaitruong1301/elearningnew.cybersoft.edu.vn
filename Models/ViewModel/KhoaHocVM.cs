using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elearningAPI.Models.ViewModel
{
    public class KhoaHocVM
    {

        public string MaKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string TenKhoaHoc { get; set; }
        public string MoTa { get; set; }
        public Nullable<int> LuotXem { get; set; }

        public string HinhAnh { get; set; }
        public string MaNhom { get; set; }
        public string NgayTao { get; set; }

        public int SoLuongHocVien { get; set; }

        public NguoiTaoVM NguoiTao { get; set; }
        public DanhMucKHVM DanhMucKhoaHoc { get; set; }


    }
    public class KhoaHocDropDown {
        public string MaKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string TenKhoaHoc { get; set; }
    }
    public class DanhMucKHVM{
        public string MaDanhMucKhoahoc { get; set; }
        public string TenDanhMucKhoaHoc { get; set; }
    }

    public class KhoaHocModel {
        public string MaKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string TenKhoaHoc { get; set; }
        public string MoTa { get; set; }
        public Nullable<int> LuotXem { get; set; }
        public int DanhGia { get; set; }
        public string HinhAnh { get; set; }
        public string MaNhom { get; set; }
        public string NgayTao { get; set; }
        public string MaDanhMucKhoaHoc { get; set; }
        public string TaiKhoanNguoiTao { get; set; }
        
    }
    public class KhoaHocVMM: KhoaHocVM
    {
        public List<HocVienVM> lstHocVien = new List<HocVienVM>();
    }

    public class NguoiTaoVM {
        public string TaiKhoan;
        public string HoTen;
        public string MaLoaiNguoiDung;
        public string TenLoaiNguoiDung;
    }

    public class HocVienVMM {
        public string TaiKhoan;
        public string HoTen;
        public string BiDanh;
    }

   
   
}