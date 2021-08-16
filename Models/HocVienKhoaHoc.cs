using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class HocVienKhoaHoc
    {
        public string MaKhoaHoc { get; set; }
        public string TaiKhoan { get; set; }
        public DateTime? NgayGhiDanh { get; set; }
        public bool? KichHoat { get; set; }

        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
        public virtual NguoiDung TaiKhoanNavigation { get; set; }
    }
}
