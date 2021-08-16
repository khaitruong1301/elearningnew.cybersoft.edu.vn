using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using bookingticketAPI.Filter;
using elearningAPI.common;
using elearningAPI.Models;
using elearningAPI.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace elearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterTokenCyber]
    public class QuanLyNguoiDungController : ControllerBase
    {
        elearningContext db = new elearningContext();
        ThongBaoLoi tbl = new ThongBaoLoi();
        private const string SECRET_KEY = "0123456789123456";//Khóa bí mật
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));


        [HttpGet("LayDanhSachLoaiNguoiDung")]
        public async Task<ActionResult> LayDanhSachLoaiNguoiDung()
        {
            var lstModel = db.LoaiNguoiDung.Select(n => new { n.MaLoaiNguoiDung, n.TenLoaiNguoiDung });
            return Ok(lstModel);
        }

        [HttpPost("DangNhap")]
        public async Task<ActionResult> DangNhap(ThongTinDangNhap ndDN)
        {
            NguoiDung nguoiDungCapNhat = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == ndDN.TaiKhoan && n.MatKhau == ndDN.MatKhau);
            if (nguoiDungCapNhat != null)
            {

                NguoiDungDangNhap nd = new NguoiDungDangNhap { TaiKhoan = nguoiDungCapNhat.TaiKhoan, HoTen = nguoiDungCapNhat.HoTen, Email = nguoiDungCapNhat.Email, SoDT = nguoiDungCapNhat.SoDt, MaNhom = nguoiDungCapNhat.MaNhom, MaLoaiNguoiDung = nguoiDungCapNhat.MaLoaiNguoiDung };

                string accessToken = GenerateToken(nd);
                nd.accessToken = accessToken;


                return Ok(nd);
            }
            var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản hoặc mật khẩu không đúng!");
            return response;
        }
        [HttpPost("DangKy")]
        public async Task<ActionResult> DangKy(NguoiDungVMM nd)
        {
            bool ckNhom = db.Nhom.Any(n => n.MaNhom == nd.MaNhom);
            if (!ckNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Nhóm người dùng không hợp lệ!");
                return response;
            }
            bool ckEmail = db.NguoiDung.Any(n => n.Email == nd.Email);
            if (ckEmail)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Email đã tồn tại!");
                return response;
            }
            var nguoiDung = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == nd.TaiKhoan);
            if (nguoiDung != null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản đã tồn tại!");
                return response;
            }
            try
            {
                NguoiDung ndInsert = Mapper.Map<NguoiDungVMM, NguoiDung>(nd);
                ndInsert.BiDanh = LoaiBoKyTu.bestLower(ndInsert.HoTen);
                ndInsert.MaLoaiNguoiDung = "HV";
                db.NguoiDung.Add(ndInsert);
                db.SaveChanges();
                return Ok(nd);
            }
            catch (Exception ex)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Dữ liệu không hợp lệ!");
                return response;
            }
        }

        [Authorize]
        [HttpPost("ThongTinNguoiDung")]
        public async Task<ActionResult> ThongTinNguoiDung()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            string taiKhoan = parseJWTToTaiKhoan(accessToken);

            NguoiDung tt = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == taiKhoan);
            if (tt == null)
            {
                // I wish to return an error response how can i do that?
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Kiểm tra lại token!");
                return response;

            }
            IEnumerable<ChiTietKhoaHoc> lstKhoaHocGhiDanh = db.HocVienKhoaHoc.Where(n => n.TaiKhoan == taiKhoan).Select(n => new ChiTietKhoaHoc { MaKhoaHoc = n.MaKhoaHoc, TenKhoaHoc = n.MaKhoaHocNavigation.TenKhoaHoc });

            ThongTinTaiKhoan ttTK = Mapper.Map<NguoiDung, ThongTinTaiKhoan>(tt);
            if (lstKhoaHocGhiDanh.Count() > 0)
            {
                ttTK.ChiTietKhoaHocGhiDanh = lstKhoaHocGhiDanh.ToList();
            }

            return Ok(ttTK);
        }
        private string parseJWTToTaiKhoan(string tokenstring)
        {

            tokenstring = tokenstring.Replace("Bearer ", "");
            //var stream = Encoding.ASCII.GetBytes("CYBERSOFT2020_SECRET");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenstring);
            var taiKhoan = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            return taiKhoan;
        }

        private string GenerateToken(NguoiDungDangNhap ndDN)
        {


            var token = new JwtSecurityToken(
                    claims: new Claim[] {
                        new Claim(ClaimTypes.Name,ndDN.TaiKhoan),
                        new Claim(ClaimTypes.Role,ndDN.MaLoaiNguoiDung),

                    },
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                    signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)


                );

            //string token1 = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("LayDanhSachNguoiDung")]
        public async Task<ActionResult> LayDanhSachNguoiDung(string MaNhom = "GP01", string tuKhoa = "")
        {
            var ktNhom = db.Nhom.Any(n => n.MaNhom == MaNhom);
            if (!ktNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Mã nhóm không hợp lệ!");
                return response;
            }
            var lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.BiDanh.Contains(tuKhoa)).Select(n => new { n.TaiKhoan, n.HoTen, n.Email, n.SoDt,n.MaLoaiNguoiDung });
            if (lstResult.Count() == 0)
            {
                lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.TaiKhoan == tuKhoa).Select(n => new { n.TaiKhoan, n.HoTen, n.Email, n.SoDt, n.MaLoaiNguoiDung });
            }
            if (lstResult.Count() == 0)
            {
                lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.SoDt.Contains(tuKhoa)).Select(n => new { n.TaiKhoan, n.HoTen, n.Email, n.SoDt, n.MaLoaiNguoiDung });
            }

            return Ok(lstResult);
        }

        [HttpGet("LayDanhSachNguoiDung_PhanTrang")]
        public async Task<ActionResult> LayDanhSachNguoiDung_PhanTrang(string MaNhom = "GP01", string tuKhoa = "",int page=1, int pageSize = 10)
        {
            var ktNhom = db.Nhom.Any(n => n.MaNhom == MaNhom);
            if (!ktNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Mã nhóm không hợp lệ!");
                return response;
            }
            IEnumerable<NguoiDungResult> lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.BiDanh.Contains(tuKhoa)).Select(n => new NguoiDungResult {TaiKhoan= n.TaiKhoan, HoTen= n.HoTen,Email= n.Email, SoDT= n.SoDt ,MaLoaiNguoiDung=n.MaLoaiNguoiDung,TenLoaiNguoiDung=n.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung});
            if (lstResult.Count() == 0)
            {
                lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.TaiKhoan == tuKhoa).Select(n => new NguoiDungResult { TaiKhoan = n.TaiKhoan, HoTen = n.HoTen, Email = n.Email, SoDT = n.SoDt, MaLoaiNguoiDung = n.MaLoaiNguoiDung, TenLoaiNguoiDung = n.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung });
            }
            if (lstResult.Count() == 0)
            {
                lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.SoDt.Contains(tuKhoa)).Select(n => new NguoiDungResult { TaiKhoan = n.TaiKhoan, HoTen = n.HoTen, Email = n.Email, SoDT = n.SoDt, MaLoaiNguoiDung = n.MaLoaiNguoiDung, TenLoaiNguoiDung = n.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung });
            }

            PaginationSet<NguoiDungResult> result = new PaginationSet<NguoiDungResult>();
            result.CurrentPage = page;
            result.TotalPages = (lstResult.Count() / pageSize) + 1;
            result.Items = lstResult.Skip((page - 1) * pageSize).Take(pageSize);
            result.TotalCount = lstResult.Count();

            return Ok(result);
        }

        [HttpGet("TimKiemNguoiDung")]
        public async Task<ActionResult> TimKiemNguoiDung(string MaNhom = "GP01", string tuKhoa = "")
        {
            var ktNhom = db.Nhom.Any(n => n.MaNhom == MaNhom);
            if (!ktNhom)
            {
                // I wish to return an error response how can i do that?
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Mã nhóm không hợp lệ!");
                return response;

            }
            var lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.BiDanh.Contains(tuKhoa)).Select(n => new { n.TaiKhoan, n.HoTen, n.Email, n.SoDt, n.MatKhau, MaLoaiNguoiDung = n.MaLoaiNguoiDung, TenLoaiNguoiDung = n.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung });
            if (lstResult.Count() == 0)
            {
                lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.TaiKhoan == tuKhoa).Select(n => new { n.TaiKhoan, n.HoTen, n.Email, n.SoDt, n.MatKhau, MaLoaiNguoiDung = n.MaLoaiNguoiDung, TenLoaiNguoiDung = n.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung });
            }
            if (lstResult.Count() == 0)
            {
                lstResult = db.NguoiDung.Where(n => n.MaNhom == MaNhom && n.SoDt.Contains(tuKhoa)).Select(n => new { n.TaiKhoan, n.HoTen, n.Email, n.SoDt, n.MatKhau , MaLoaiNguoiDung = n.MaLoaiNguoiDung, TenLoaiNguoiDung = n.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung });
            }
            return Ok(lstResult);

        }
        [Authorize]
        [HttpPost("ThongTinTaiKhoan")]
        public async Task<ActionResult> ThongTinTaiKhoan(ThongTinDangNhap tttk)
        {

            NguoiDung tt = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == tttk.TaiKhoan);
            if (tt == null)
            {
                // I wish to return an error response how can i do that?
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Tài khoản không hợp lệ!");
                return response;

            }
            IEnumerable<ChiTietKhoaHoc> lstKhoaHocGhiDanh = db.HocVienKhoaHoc.Where(n => n.TaiKhoan == tttk.TaiKhoan).Select(n => new ChiTietKhoaHoc { MaKhoaHoc = n.MaKhoaHoc, TenKhoaHoc = n.MaKhoaHocNavigation.TenKhoaHoc });

            ThongTinTaiKhoan ttTK = Mapper.Map<NguoiDung, ThongTinTaiKhoan>(tt);
            if (lstKhoaHocGhiDanh.Count() > 0)
            {
                ttTK.ChiTietKhoaHocGhiDanh = lstKhoaHocGhiDanh.ToList();
            }

            return Ok(ttTK);
        }
        [Authorize(Roles = "GV" )]
        [HttpPost("ThemNguoiDung")]
        public async Task<ActionResult> ThemNguoiDung(NguoiDungVM nd)
        {
            bool ckbLoaiND = db.LoaiNguoiDung.Any(n => n.MaLoaiNguoiDung == nd.MaLoaiNguoiDung);
            if (!ckbLoaiND)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Loại người dùng không hợp lệ!");
                return response;
            }
            bool ckNhom = db.Nhom.Any(n => n.MaNhom == nd.MaNhom);
            if (!ckNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Nhóm người dùng không hợp lệ!");
                return response;
            }
            bool ckEmail = db.NguoiDung.Any(n => n.Email == nd.Email);
            if (ckEmail)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Email đã tồn tại!");
                return response;
            }
            var nguoiDung = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == nd.TaiKhoan);
            if (nguoiDung != null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản đã tồn tại!");
                return response;
            }
            try
            {
                NguoiDung ndInsert = Mapper.Map<NguoiDungVM, NguoiDung>(nd);
                ndInsert.BiDanh = LoaiBoKyTu.bestLower(ndInsert.HoTen);
                db.NguoiDung.Add(ndInsert);
                db.SaveChanges();
                return Ok(nd);
            }
            catch (Exception ex)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Dữ liệu không hợp lệ!");
                return response;
            }
        }
        [Authorize]
        [HttpPut("CapNhatThongTinNguoiDung")]
        public async Task<ActionResult> CapNhatThongTinNguoiDung(NguoiDungVM nd)
        {
            bool ckbLoaiND = db.LoaiNguoiDung.Any(n => n.MaLoaiNguoiDung == nd.MaLoaiNguoiDung);
            if (!ckbLoaiND)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Loại người dùng không hợp lệ!");
                return response;
            }
            bool ckNhom = db.Nhom.Any(n => n.MaNhom == nd.MaNhom);
            if (!ckNhom)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Nhóm người dùng không hợp lệ!");
                return response;
            }
            bool ckEmail = db.NguoiDung.Any(n => n.Email == nd.Email && n.TaiKhoan != nd.TaiKhoan);
            if (ckEmail)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Email đã tồn tại!");
                return response;
            }
            NguoiDung nguoiDungCapNhat = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == nd.TaiKhoan);
            if (nguoiDungCapNhat == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
                return response;
            }
            try
            {


                if (nd.MatKhau == "")
                {
                    nd.MatKhau = nguoiDungCapNhat.MatKhau;
                }

                //nguoiDungCapNhat.TaiKhoan = ndUpdate.TaiKhoan;
                nguoiDungCapNhat.HoTen = nd.HoTen;
                nguoiDungCapNhat.MatKhau = nd.MatKhau;
                nguoiDungCapNhat.BiDanh = LoaiBoKyTu.bestLower(nd.HoTen);
                nguoiDungCapNhat.SoDt = nd.SoDT;
                nguoiDungCapNhat.MaLoaiNguoiDung = nd.MaLoaiNguoiDung;
                nguoiDungCapNhat.Email = nd.Email;
                //nguoiDungCapNhat.MaNhom = ndUpdate.MaNhom;

                db.SaveChanges();
                return Ok(new NguoiDung { TaiKhoan = nguoiDungCapNhat.TaiKhoan, MatKhau = nguoiDungCapNhat.MatKhau, HoTen = nguoiDungCapNhat.HoTen, Email = nguoiDungCapNhat.Email, SoDt = nguoiDungCapNhat.SoDt, MaNhom = nguoiDungCapNhat.MaNhom, MaLoaiNguoiDung = nguoiDungCapNhat.MaLoaiNguoiDung });
            }
            catch (Exception ex)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Dữ liệu không hợp lệ!");
                return response;
            }
        }

        [Authorize(Roles = "GV")]
        [HttpDelete("XoaNguoiDung")]
        public async Task<ActionResult> XoaNguoiDung(string TaiKhoan)
        {
            var ckND = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == TaiKhoan);
            if (ckND == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
                return response;
            }
            var ckTaoKhoaHoc = db.KhoaHoc.Where(n => n.NguoiTao == TaiKhoan);
            if (ckTaoKhoaHoc.Count() > 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Người dùng này đã tạo khóa học không thể xóa!");
                return response;
            }
            var ckHocKhoaHoc = db.HocVienKhoaHoc.Where(n => n.TaiKhoan == TaiKhoan);
            if (ckHocKhoaHoc.Count() > 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Người dùng đã được ghi danh không thể xóa!");
                return response;
            }
            db.NguoiDung.Remove(ckND);
            db.SaveChanges();

            return Ok("Xóa thành công!");
        }


        [Authorize(Roles ="GV")]
        [HttpPost("LayDanhSachKhoaHocChuaGhiDanh")]
        public async Task<IActionResult> LayDanhSachKhoaHocChuaGhiDanh(string TaiKhoan = "khai")
        {
            NguoiDung nd = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == TaiKhoan);
            if (nd == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
            }


            var lstKhoaHoc = db.KhoaHoc.Where( n=> n.MaNhom == nd.MaNhom);
            var lstKhoaHocHVGhiDanh = db.HocVienKhoaHoc.Where(n => n.TaiKhoan == TaiKhoan);

            List<KhoaHocDropDown> lstKQ = new List<KhoaHocDropDown>();
            foreach (var kh in lstKhoaHoc)
            {
                bool b = false;
                foreach (var khGhiDanh in lstKhoaHocHVGhiDanh)
                {
                    if (kh.MaKhoaHoc == khGhiDanh.MaKhoaHoc)
                    {
                        b = true;
                    }
                }
                if (!b)
                {
                    lstKQ.Add(new KhoaHocDropDown { MaKhoaHoc = kh.MaKhoaHoc, TenKhoaHoc = kh.TenKhoaHoc, BiDanh = kh.BiDanh });
                }
            }


            return Ok(lstKQ);
        }

        [Authorize(Roles = "GV")]
        [HttpPost("LayDanhSachKhoaHocChoXetDuyet")]
        public async Task<IActionResult> LayDanhSachKhoaHocChoXetDuyet(TaiKhoanVM taiKhoan)
        {
            NguoiDung nd = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == taiKhoan.taiKhoan);
            if (nd == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
            }
            var lstKhoaHocHVGhiDanh = db.HocVienKhoaHoc.Where(n => n.TaiKhoan == taiKhoan.taiKhoan && n.KichHoat != true );
            List<ChiTietKhoaHoc> lstChiTietKhoaHoc = new List<ChiTietKhoaHoc>();
            foreach (var item in lstKhoaHocHVGhiDanh)
            {
                ChiTietKhoaHoc ct = new ChiTietKhoaHoc();
                ct.MaKhoaHoc = item.MaKhoaHoc;
                ct.TenKhoaHoc = item.MaKhoaHocNavigation.TenKhoaHoc;
                lstChiTietKhoaHoc.Add(ct);
            }

            return Ok(lstChiTietKhoaHoc);
        }

        [Authorize(Roles = "GV")]
        [HttpPost("LayDanhSachKhoaHocDaXetDuyet")]
        public async Task<IActionResult> LayDanhSachKhoaHocDaXetDuyet(TaiKhoanVM TaiKhoan)
        {
            NguoiDung nd = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == TaiKhoan.taiKhoan);
            if (nd == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
            }
            var lstKhoaHocHVGhiDanh = db.HocVienKhoaHoc.Where(n => n.TaiKhoan == TaiKhoan.taiKhoan && n.KichHoat == true);
            List<ChiTietKhoaHoc> lstChiTietKhoaHoc = new List<ChiTietKhoaHoc>();
            foreach (var item in lstKhoaHocHVGhiDanh)
            {
                ChiTietKhoaHoc ct = new ChiTietKhoaHoc();
                ct.MaKhoaHoc = item.MaKhoaHoc;
                ct.TenKhoaHoc = item.MaKhoaHocNavigation.TenKhoaHoc;
                lstChiTietKhoaHoc.Add(ct);
            }

            return Ok(lstChiTietKhoaHoc);

        }


        [Authorize(Roles = "GV")]
        [HttpPost("LayDanhSachNguoiDungChuaGhiDanh")]
        public async Task<IActionResult> LayDanhSachNguoiDungChuaGhiDanh(MaKhoaHocVM MaKhoaHoc)
        {
            KhoaHoc nd = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == MaKhoaHoc.maKhoaHoc);
            if (nd == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không tồn tại!");
            }


            var lstHocVien = db.NguoiDung.Where(n => n.MaNhom == nd.MaNhom);
            var lstKhoaHocHVGhiDanh = db.HocVienKhoaHoc.Where(n => n.MaKhoaHoc == MaKhoaHoc.maKhoaHoc);

            List<HocVienVMM> lstKQ = new List<HocVienVMM>();
            foreach (var kh in lstHocVien)
            {
                bool b = false;
                foreach (var khGhiDanh in lstKhoaHocHVGhiDanh)
                {
                    if (kh.TaiKhoan == khGhiDanh.TaiKhoan)
                    {
                        b = true;
                    }
                }
                if (!b)
                {
                    lstKQ.Add(new HocVienVMM { TaiKhoan = kh.TaiKhoan, HoTen = kh.HoTen, BiDanh = kh.BiDanh });
                }
            }


            return Ok(lstKQ);
        }

        [Authorize(Roles = "GV")]
        [HttpPost("LayDanhSachHocVienChoXetDuyet")]
        public async Task<IActionResult> LayDanhSachHocVienChoXetDuyet(MaKhoaHocVM MaKhoaHoc)
        {
            KhoaHoc nd = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == MaKhoaHoc.maKhoaHoc);
            if (nd == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không tồn tại!");
            }
            var lstKhoaHocHVGhiDanh = db.HocVienKhoaHoc.Where(n => n.MaKhoaHoc == MaKhoaHoc.maKhoaHoc && n.KichHoat != true );
            List<HocVienVM> lstChiTietKhoaHoc = new List<HocVienVM>();
            foreach (var item in lstKhoaHocHVGhiDanh)
            {
                HocVienVM ct = new HocVienVM();
                ct.TaiKhoan = item.TaiKhoan;
                ct.HoTen = item.TaiKhoanNavigation.HoTen;
                ct.BiDanh = LoaiBoKyTu.bestLower(ct.HoTen);
                lstChiTietKhoaHoc.Add(ct);
            }
            return Ok(lstChiTietKhoaHoc);
        }

        [Authorize(Roles = "GV")]
        [HttpPost("LayDanhSachHocVienKhoaHoc")]
        public async Task<IActionResult> LayDanhSachHocVienKhoaHoc(MaKhoaHocVM MaKhoaHoc)
        {
            KhoaHoc kh = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == MaKhoaHoc.maKhoaHoc);
            if (kh == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không tồn tại!");
            }
            var lstKhoaHocHVGhiDanh = db.HocVienKhoaHoc.Where(n => n.MaKhoaHoc == MaKhoaHoc.maKhoaHoc && n.KichHoat == true );
            List<HocVienVM> lstChiTietKhoaHoc = new List<HocVienVM>();
            foreach (var item in lstKhoaHocHVGhiDanh)
            {
                HocVienVM ct = new HocVienVM();
                ct.TaiKhoan = item.TaiKhoan;
                ct.HoTen = item.TaiKhoanNavigation.HoTen;
                ct.BiDanh = LoaiBoKyTu.bestLower(ct.HoTen);
                lstChiTietKhoaHoc.Add(ct);
            }

            return Ok(lstChiTietKhoaHoc);

        }
    }
}