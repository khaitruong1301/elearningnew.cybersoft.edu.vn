using elearningAPI.Models;
using elearningAPI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace elearningAPI
{
    public class Automapperweb : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a => a.AddProfile<Automapperweb>());
        }
        public Automapperweb()
        {
            CreateMap<NguoiDungVM, NguoiDung>();
            CreateMap<NguoiDungVMM, NguoiDung>();
            CreateMap<NguoiDung, ThongTinTaiKhoan>();
            CreateMap<KhoaHoc, KhoaHocVM>();
        }
    }
}
