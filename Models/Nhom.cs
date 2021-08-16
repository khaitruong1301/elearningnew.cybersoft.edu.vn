using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class Nhom
    {
        public Nhom()
        {
            KhoaHoc = new HashSet<KhoaHoc>();
        }

        public string MaNhom { get; set; }
        public string TenNhom { get; set; }

        public virtual ICollection<KhoaHoc> KhoaHoc { get; set; }
    }
}
