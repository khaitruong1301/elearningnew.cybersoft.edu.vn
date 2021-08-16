using System;
using System.Collections.Generic;

namespace elearningAPI.Models
{
    public partial class BaiHoc
    {
        public int MaBaiHoc { get; set; }
        public string TenBaiHoc { get; set; }
        public string NoiDung { get; set; }
        public string LinkVideo { get; set; }
        public string MaKhoaHoc { get; set; }
        public string MoTaNgan { get; set; }

        public virtual KhoaHoc MaKhoaHocNavigation { get; set; }
    }
}
