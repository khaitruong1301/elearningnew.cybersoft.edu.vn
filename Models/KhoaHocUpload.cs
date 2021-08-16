using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elearningAPI.Models
{
    public class KhoaHocUpload
    {
        public string MaKhoaHoc = "";
        public string BiDanh = "";
        public string TenKhoaHoc = "";
        public string MoTa = "";
        public Nullable<int> LuotXem = 100;
        public int DanhGia = 10;
        //public string HinhAnh { get; set; }
        public string MaNhom = "GP01";
        public string NgayTao = "10/10/2020";
        public string MaDanhMucKhoaHoc = "BackEnd";
        public string TaiKhoanNguoiTao = "adminkhai";
        public IFormFile HinhAnh { get; set; }
    }
}
