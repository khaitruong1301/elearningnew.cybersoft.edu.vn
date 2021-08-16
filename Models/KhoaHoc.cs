using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class KhoaHoc
    {
        public KhoaHoc()
        {
            BaiHoc = new HashSet<BaiHoc>();
            HocVienKhoaHoc = new HashSet<HocVienKhoaHoc>();
        }

        public string MaKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string TenKhoaHoc { get; set; }
        public string MoTa { get; set; }
        public int? LuotXem { get; set; }
        public string NguoiTao { get; set; }
        public string HinhAnh { get; set; }
        public string MaNhom { get; set; }
        public DateTime? NgayTao { get; set; }
        public string MaDanhMucKhoaHoc { get; set; }
        public int? DanhGia { get; set; }

        public virtual DanhMucKhoaHoc MaDanhMucKhoaHocNavigation { get; set; }
        public virtual Nhom MaNhomNavigation { get; set; }
        public virtual NguoiDung NguoiTaoNavigation { get; set; }
        public virtual ICollection<BaiHoc> BaiHoc { get; set; }
        public virtual ICollection<HocVienKhoaHoc> HocVienKhoaHoc { get; set; }
    }
}
