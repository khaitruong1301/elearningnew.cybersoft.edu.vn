using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class LoaiNguoiDung
    {
        public LoaiNguoiDung()
        {
            NguoiDung = new HashSet<NguoiDung>();
        }

        public virtual string MaLoaiNguoiDung { get; set; }
        public virtual string TenLoaiNguoiDung { get; set; }

        public virtual ICollection<NguoiDung> NguoiDung { get; set; }
    }
}
