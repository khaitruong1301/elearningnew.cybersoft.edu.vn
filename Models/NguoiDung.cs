using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            HocVienKhoaHoc = new HashSet<HocVienKhoaHoc>();
            KhoaHoc = new HashSet<KhoaHoc>();
        }

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string SoDt { get; set; }
        public string MaLoaiNguoiDung { get; set; }
        public string MaNhom { get; set; }
        public string Email { get; set; }
        public string BiDanh { get; set; }

        public virtual LoaiNguoiDung MaLoaiNguoiDungNavigation { get; set; }
        public virtual ICollection<HocVienKhoaHoc> HocVienKhoaHoc { get; set; }
        public virtual ICollection<KhoaHoc> KhoaHoc { get; set; }
    }
}
