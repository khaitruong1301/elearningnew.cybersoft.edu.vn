using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class DanhMucKhoaHoc
    {
        public DanhMucKhoaHoc()
        {
            KhoaHoc = new HashSet<KhoaHoc>();
        }

        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string BiDanh { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<KhoaHoc> KhoaHoc { get; set; }
    }
}
