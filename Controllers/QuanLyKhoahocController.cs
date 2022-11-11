using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using elearningAPI.Models;
using elearningAPI.Models.ViewModel;
using elearningAPI.common;
using elearningAPI.Controllers;
using WebApi.Pagination;
using Pagination;
using ReflectionIT.Mvc.Paging;
using static elearningAPI.common.Common;
using Microsoft.AspNetCore.Authorization;

using System.IO;
using System.Reflection;
using bookingticketAPI.Filter;
using Microsoft.Extensions.FileProviders;
using static System.Net.WebRequestMethods;

namespace elearningAPI.Controllers
{
    [Route("api/[controller]")]
    [FilterTokenCyber]
    [ApiController]
    public class QuanLyKhoaHocController : ControllerBase, IDisposable
    {
        elearningContext db = new elearningContext();
        ThongBaoLoi tbl = new ThongBaoLoi();


        [HttpGet("LayDanhSachKhoaHoc")]
        public async Task<IActionResult> LayDanhSachKhoaHoc(string tenKhoaHoc = "", string MaNhom = "GP01")
        {
            tenKhoaHoc = tenKhoaHoc == null ? "" : tenKhoaHoc;
            tenKhoaHoc = LoaiBoKyTu.bestLower(tenKhoaHoc);
            var lstModel = db.KhoaHoc.Where(n => n.BiDanh.Contains(tenKhoaHoc) && n.MaNhom == MaNhom);
            if (lstModel.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi404, "Không tìm thấy khóa học!");
                return response;
            }
            return Ok(lstModel.Select(n => new KhoaHocVM { MaKhoaHoc = n.MaKhoaHoc, TenKhoaHoc = n.TenKhoaHoc, BiDanh = n.BiDanh, MaNhom = n.MaNhom, DanhMucKhoaHoc = new DanhMucKHVM { MaDanhMucKhoahoc = n.MaDanhMucKhoaHoc, TenDanhMucKhoaHoc = n.MaDanhMucKhoaHocNavigation.TenDanhMuc }, NgayTao = n.NgayTao != null ? n.NgayTao.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"), MoTa = n.MoTa, LuotXem = n.LuotXem, NguoiTao = new NguoiTaoVM { TaiKhoan = n.NguoiTao, HoTen = n.NguoiTaoNavigation.HoTen, MaLoaiNguoiDung = n.NguoiTaoNavigation.MaLoaiNguoiDung, TenLoaiNguoiDung = n.NguoiTaoNavigation.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung }, HinhAnh = DomainImage + n.HinhAnh }).ToList());
        }

        [HttpGet("LayDanhMucKhoaHoc")]
        public async Task<IActionResult> LayDanhMucKhoaHoc(string tenDanhMuc = "")
        {
            tenDanhMuc = tenDanhMuc == null ? "" : tenDanhMuc;
            tenDanhMuc = LoaiBoKyTu.bestLower(tenDanhMuc);
            var lstModel = db.DanhMucKhoaHoc.Where(n => n.BiDanh.Contains(tenDanhMuc));
            if (lstModel.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi404, "Không tìm thấy danh mục khóa học!");
                return response;
            }
            return Ok(lstModel.ToList().Select(n => new { MaDanhMuc = n.MaDanhMuc, tenDanhMuc = n.TenDanhMuc }));
        }

        [HttpGet("LayKhoaHocTheoDanhMuc")]
        public async Task<IActionResult> LayKhoaHocTheoDanhMuc(string maDanhMuc = "TuDuy", string MaNhom = "GP01")
        {

            var lstModel = db.KhoaHoc.Where(n => n.MaDanhMucKhoaHoc == maDanhMuc && n.MaNhom == MaNhom);
            if (lstModel.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi404, "Không tìm thấy danh mục khóa học!");
                return response;
            }
            return Ok(lstModel.Select(n => new KhoaHocVM { MaKhoaHoc = n.MaKhoaHoc, TenKhoaHoc = n.TenKhoaHoc, BiDanh = n.BiDanh, MaNhom = n.MaNhom, DanhMucKhoaHoc = new DanhMucKHVM { MaDanhMucKhoahoc = n.MaDanhMucKhoaHoc, TenDanhMucKhoaHoc = n.MaDanhMucKhoaHocNavigation.TenDanhMuc }, NgayTao = n.NgayTao != null ? n.NgayTao.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"), MoTa = n.MoTa, LuotXem = n.LuotXem, NguoiTao = new NguoiTaoVM { TaiKhoan = n.NguoiTao, HoTen = n.NguoiTaoNavigation.HoTen, MaLoaiNguoiDung = n.NguoiTaoNavigation.MaLoaiNguoiDung, TenLoaiNguoiDung = n.NguoiTaoNavigation.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung }, HinhAnh = DomainImage + n.HinhAnh }).ToList());
        }
        [HttpGet("LayDanhSachKhoaHoc_PhanTrang")]
        public async Task<IActionResult> LayDanhSachKhoaHoc_PhanTrang(string tenKhoaHoc = "", int page = 1, int pageSize = 10, string MaNhom = "GP01")
        {
            tenKhoaHoc = tenKhoaHoc == null ? "" : tenKhoaHoc;
            tenKhoaHoc = LoaiBoKyTu.bestLower(tenKhoaHoc);
            var lstModel = db.KhoaHoc.Where(n => n.BiDanh.Contains(tenKhoaHoc) && n.MaNhom == MaNhom);
            if (lstModel.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi404, "Không tìm thấy khóa học!");
                return response;
            }
            IEnumerable<KhoaHocVM> query = lstModel.Select(n => new KhoaHocVM { MaKhoaHoc = n.MaKhoaHoc, TenKhoaHoc = n.TenKhoaHoc, BiDanh = n.BiDanh, MaNhom = n.MaNhom, DanhMucKhoaHoc = new DanhMucKHVM { MaDanhMucKhoahoc = n.MaDanhMucKhoaHoc, TenDanhMucKhoaHoc = n.MaDanhMucKhoaHocNavigation.TenDanhMuc }, NgayTao = n.NgayTao != null ? n.NgayTao.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"), MoTa = n.MoTa, LuotXem = n.LuotXem, NguoiTao = new NguoiTaoVM { TaiKhoan = n.NguoiTao, HoTen = n.NguoiTaoNavigation.HoTen, MaLoaiNguoiDung = n.NguoiTaoNavigation.MaLoaiNguoiDung, TenLoaiNguoiDung = n.NguoiTaoNavigation.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung }, HinhAnh = DomainImage + n.HinhAnh });
            //var model = PagingList.Create(query, pageSize, page);

            PaginationSet<KhoaHocVM> result = new PaginationSet<KhoaHocVM>();
            result.CurrentPage = page;
            result.TotalPages = (query.Count() / pageSize) + 1;
            result.Items = query.Skip((page - 1) * pageSize).Take(pageSize);
            result.TotalCount = query.Count();

            return Ok(result);
        }



        [HttpGet("LayThongTinKhoaHoc")]
        public async Task<IActionResult> LayThongTinKhoaHoc(string maKhoaHoc = "LTC_GP01")
        {

            KhoaHoc khoaHoc = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == maKhoaHoc);
            if (khoaHoc == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Mã khóa học không hợp lệ!");
                return response;
            }
            KhoaHocVM khVM = new KhoaHocVM();
            khVM.MaKhoaHoc = khoaHoc.MaKhoaHoc;
            khVM.TenKhoaHoc = khoaHoc.TenKhoaHoc;
            khVM.MoTa = khoaHoc.MoTa;
            khVM.BiDanh = khoaHoc.BiDanh;
            khVM.LuotXem = khoaHoc.LuotXem;
            khVM.MoTa = khoaHoc.MoTa;
            khVM.NguoiTao = new NguoiTaoVM { TaiKhoan = khoaHoc.NguoiTao, HoTen = khoaHoc.NguoiTaoNavigation.HoTen, MaLoaiNguoiDung = khoaHoc.NguoiTaoNavigation.MaLoaiNguoiDung, TenLoaiNguoiDung = khoaHoc.NguoiTaoNavigation.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung };
            khVM.HinhAnh = DomainImage + khoaHoc.HinhAnh;
            khVM.MaNhom = khoaHoc.MaNhom;
            khVM.NgayTao = khoaHoc.NgayTao != null ? khoaHoc.NgayTao.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
            khVM.DanhMucKhoaHoc = new DanhMucKHVM { MaDanhMucKhoahoc = khoaHoc.MaDanhMucKhoaHoc, TenDanhMucKhoaHoc = khoaHoc.MaDanhMucKhoaHocNavigation.TenDanhMuc };
            return Ok(khVM);
        }

        [Authorize]
        [HttpGet("LayThongTinHocVienKhoaHoc")]
        public async Task<IActionResult> LayThongTinHocVienKhoaHoc(string maKhoaHoc = "LTC_GP01")
        {
            KhoaHoc khoaHoc = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == maKhoaHoc);
            if (khoaHoc == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Mã khóa học không hợp lệ!");
                return response;
            }
            KhoaHocVMM khVM = new KhoaHocVMM();
            khVM.MaKhoaHoc = khoaHoc.MaKhoaHoc;
            khVM.TenKhoaHoc = khoaHoc.TenKhoaHoc;
            khVM.MoTa = khoaHoc.MoTa;
            khVM.BiDanh = khoaHoc.BiDanh;
            khVM.MoTa = khoaHoc.MoTa;
            khVM.LuotXem = khoaHoc.LuotXem;
            khVM.NguoiTao = new NguoiTaoVM { TaiKhoan = khoaHoc.NguoiTao, HoTen = khoaHoc.NguoiTaoNavigation.HoTen, MaLoaiNguoiDung = khoaHoc.NguoiTaoNavigation.MaLoaiNguoiDung, TenLoaiNguoiDung = khoaHoc.NguoiTaoNavigation.MaLoaiNguoiDungNavigation.TenLoaiNguoiDung };
            khVM.HinhAnh = DomainImage + khVM.HinhAnh;
            khVM.MaNhom = khoaHoc.MaNhom;
            khVM.NgayTao = khoaHoc.NgayTao != null ? khoaHoc.NgayTao.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
            khVM.lstHocVien = db.HocVienKhoaHoc.Where(n => n.MaKhoaHoc == khVM.MaKhoaHoc).Select(n => new HocVienVM { TaiKhoan = n.TaiKhoan, HoTen = n.TaiKhoanNavigation.HoTen }).ToList();
            return Ok(khVM);
        }
        [Authorize(Roles = "GV")]
        [HttpPost("ThemKhoaHoc")]
        public async Task<IActionResult> ThemKhoaHoc(KhoaHocModel kh)
        {

            var ckKhoaHoc = db.KhoaHoc.Any(n => n.MaKhoaHoc == kh.MaKhoaHoc);
            if (ckKhoaHoc)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học đã tồn tại!");
                return response;
            }
            string biDanh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc);
            bool ckTenKhoaHoc = db.KhoaHoc.Any(n => n.BiDanh.Trim() == biDanh.Trim());
            if (ckTenKhoaHoc)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên khóa học đã tồn tại!");
                return response;
            }
            var ckbGiangVien = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == kh.TaiKhoanNguoiTao);
            if (ckbGiangVien != null)
            {
                if (ckbGiangVien.MaLoaiNguoiDung != "GV")
                {
                    var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Người tạo phải là tài khoản giảng viên!");
                    return response;
                }

            }
            else
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Người tạo phải là tài khoản giảng viên!");
                return response;
            }
            var ckbDanhMucKhoaHoc = db.DanhMucKhoaHoc.Any(n => n.MaDanhMuc == kh.MaDanhMucKhoaHoc);
            if (!ckbDanhMucKhoaHoc)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Danh mục khóa học không hợp lệ!");
                return response;
            }


            try
            {
                KhoaHoc khoaHocInsert = new KhoaHoc();
                khoaHocInsert.MaKhoaHoc = kh.MaKhoaHoc;
                khoaHocInsert.TenKhoaHoc = kh.TenKhoaHoc;
                khoaHocInsert.BiDanh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc);
                khoaHocInsert.HinhAnh = khoaHocInsert.BiDanh + "." + kh.HinhAnh.Split(".")[kh.HinhAnh.Split(".").Length - 1];
                khoaHocInsert.MoTa = kh.MoTa;
                khoaHocInsert.LuotXem = kh.LuotXem;



                khoaHocInsert.NguoiTao = kh.TaiKhoanNguoiTao;
                if (kh.DanhGia != 0)
                {
                    khoaHocInsert.DanhGia = kh.DanhGia;

                }
                else
                {
                    khoaHocInsert.DanhGia = 5;
                }
                if (kh.HinhAnh.Split('.').Count() > 1)
                {
                    khoaHocInsert.HinhAnh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc) + "." + kh.HinhAnh.Split('.')[kh.HinhAnh.Split('.').Length - 1];
                }
                else
                {
                    var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Hình ảnh không đúng định dạng!");
                    return response;
                }

                khoaHocInsert.MaNhom = kh.MaNhom;
                khoaHocInsert.NgayTao = string.IsNullOrEmpty(kh.NgayTao) ? DateTimes.ConvertDate(kh.NgayTao) : DateTime.Now;
                khoaHocInsert.MaDanhMucKhoaHoc = kh.MaDanhMucKhoaHoc;

                db.KhoaHoc.Add(khoaHocInsert);
                db.SaveChanges();
                return Ok(kh);
            }
            catch (Exception ex)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Thông tin khóa học không hợp lệ !");
                return response;
            }

        }

        //[Authorize(Roles = "GV")]
        [HttpPut("CapNhatKhoaHoc")]
        public async Task<IActionResult> CapNhatKhoaHoc(KhoaHocModel kh)
        {

            var khoaHocUpdate = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == kh.MaKhoaHoc);
            if (khoaHocUpdate == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Không tìm thấy khóa học!");
                return response;
            }
            string biDanh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc);
            int ckTenKhoaHoc = db.KhoaHoc.Where(n => n.BiDanh.Trim() == biDanh.Trim()).Count();
            if (ckTenKhoaHoc > 1)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên khóa học đã tồn tại!");
                return response;
            }
            var ckbGiangVien = db.NguoiDung.SingleOrDefault(n => n.TaiKhoan == kh.TaiKhoanNguoiTao);
            if (ckbGiangVien != null)
            {
                if (ckbGiangVien.MaLoaiNguoiDung != "GV")
                {
                    var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Người tạo phải là tài khoản giảng viên!");
                    return response;
                }
            }
            var ckbDanhMucKhoaHoc = db.DanhMucKhoaHoc.Any(n => n.MaDanhMuc == kh.MaDanhMucKhoaHoc);
            if (!ckbDanhMucKhoaHoc)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Danh mục khóa học không hợp lệ!");
                return response;
            }
            try
            {
                //KhoaHoc khoaHocInsert = new KhoaHoc();
                //khoaHocInsert.MaKhoaHoc = kh.MaKhoaHoc;
                khoaHocUpdate.MaKhoaHoc = kh.MaKhoaHoc;
                khoaHocUpdate.TenKhoaHoc = kh.TenKhoaHoc;
                khoaHocUpdate.BiDanh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc);
                if (!string.IsNullOrEmpty(kh.HinhAnh) && kh.HinhAnh != khoaHocUpdate.HinhAnh)
                {
                    khoaHocUpdate.HinhAnh = kh.BiDanh + "." + kh.HinhAnh.Split(".")[kh.HinhAnh.Split(".").Length - 1];
                }
                khoaHocUpdate.MoTa = kh.MoTa;
                khoaHocUpdate.LuotXem = kh.LuotXem;
                khoaHocUpdate.NguoiTao = kh.TaiKhoanNguoiTao;
                if (kh.DanhGia != 0)
                {
                    khoaHocUpdate.DanhGia = kh.DanhGia;

                }
                else
                {
                    khoaHocUpdate.DanhGia = 5;
                }
                if (!string.IsNullOrEmpty(kh.HinhAnh))
                {
                    khoaHocUpdate.HinhAnh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc) + "." + kh.HinhAnh.Split('.')[kh.HinhAnh.Split('.').Length - 1];
                }
                else
                {
                    if (kh.HinhAnh.Split('.').Count() > 1)
                    {
                        khoaHocUpdate.HinhAnh = LoaiBoKyTu.bestLower(kh.TenKhoaHoc) + "_" + LoaiBoKyTu.bestLower(kh.MaNhom) + "." + kh.HinhAnh.Split('.')[kh.HinhAnh.Split('.').Length - 1];
                    }
                    else
                    {
                        var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Hình ảnh không đúng định dạng!");
                        return response;
                    }
                }
                khoaHocUpdate.MaNhom = kh.MaNhom;
                khoaHocUpdate.NgayTao = string.IsNullOrEmpty(kh.NgayTao) ? DateTimes.ConvertDate(kh.NgayTao) : DateTime.Now;
                khoaHocUpdate.MaDanhMucKhoaHoc = kh.MaDanhMucKhoaHoc;

                db.SaveChanges();
                return Ok(kh);
            }
            catch (Exception ex)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Thông tin khóa học không hợp lệ !");
                return response;
            }

        }


        [Authorize(Roles = "GV")]
        [HttpDelete("XoaKhoaHoc")]

        public async Task<IActionResult> XoaKhoaHoc(string MaKhoaHoc)
        {
            var khoaHocDelete = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == MaKhoaHoc);
            if (khoaHocDelete == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi400, "Không tìm thấy khóa học!");
                return response;
            }
            var ckb = db.HocVienKhoaHoc.Where(n => n.MaKhoaHoc == MaKhoaHoc);
            if (ckb.Count() > 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Khóa học đã ghi danh học viên không thể xóa!");
                return response;
            }
            try
            {


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", khoaHocDelete.HinhAnh);
                IFileProvider physicalFileProvider = new PhysicalFileProvider(path);
                DeleteFiles(physicalFileProvider);


            }
            catch (Exception ex)
            {
            }
            db.KhoaHoc.Remove(khoaHocDelete);
            db.SaveChanges();


            return Ok("Xóa thành công");
        }

        private void DeleteFiles(IFileProvider physicalFileProvider)
        {
            if (physicalFileProvider is PhysicalFileProvider)
            {
                var directory = physicalFileProvider.GetDirectoryContents(string.Empty);
                foreach (var file in directory)
                {
                    if (!file.IsDirectory)
                    {
                        var fileInfo = new System.IO.FileInfo(file.PhysicalPath);
                        fileInfo.Delete();

                    }
                }
            }
        }
        [Authorize(Roles = "GV")]
        [HttpPost("GhiDanhKhoaHoc")]

        public async Task<IActionResult> GhiDanhKhoaHoc(ThongTinDangKy ttdk)
        {
            var ckTaiKhoan = db.NguoiDung.Where(n => n.TaiKhoan == ttdk.TaiKhoan);
            if (ckTaiKhoan.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không hợp lệ!");
                return response;
            }
            var ckKhoaHoc = db.KhoaHoc.Where(n => n.MaKhoaHoc == ttdk.MaKhoaHoc);
            if (ckKhoaHoc.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không hợp lệ!");
                return response;
            }
            var ckKhoaHocHocVien = db.HocVienKhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == ttdk.MaKhoaHoc && n.TaiKhoan == ttdk.TaiKhoan);
            if (ckKhoaHocHocVien != null)
            {
                //var response = tbl.TBLoi(ThongBaoLoi.Loi500, "Học viên đã đăng ký khóa học này rồi không thể đăng ký!");
                ckKhoaHocHocVien.KichHoat = true;
                db.SaveChanges();
                return Ok("Ghi danh thành công!");
            }
            HocVienKhoaHoc hvKhoaHoc = new HocVienKhoaHoc();
            hvKhoaHoc.MaKhoaHoc = ttdk.MaKhoaHoc;
            hvKhoaHoc.TaiKhoan = ttdk.TaiKhoan;
            hvKhoaHoc.NgayGhiDanh = DateTimes.Now();
            hvKhoaHoc.KichHoat = true;
            db.HocVienKhoaHoc.Add(hvKhoaHoc);
            db.SaveChanges();
            return Ok("Ghi danh thành công!");

        }
        [Authorize]
        [HttpPost("DangKyKhoaHoc")]

        public async Task<IActionResult> DangKyKhoaHoc(ThongTinDangKy ttdk)
        {
            var ckTaiKhoan = db.NguoiDung.Where(n => n.TaiKhoan == ttdk.TaiKhoan);
            if (ckTaiKhoan.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không hợp lệ!");
                return response;
            }
            var ckKhoaHoc = db.KhoaHoc.Where(n => n.MaKhoaHoc == ttdk.MaKhoaHoc);
            if (ckKhoaHoc.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không hợp lệ!");
                return response;
            }
            var ckKhoaHocHocVien = db.HocVienKhoaHoc.Where(n => n.MaKhoaHoc == ttdk.MaKhoaHoc && n.TaiKhoan == ttdk.TaiKhoan);
            if (ckKhoaHocHocVien.Count() > 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Đã đăng ký khóa học này rồi!");
                return response;
            }
            HocVienKhoaHoc hvKhoaHoc = new HocVienKhoaHoc();
            hvKhoaHoc.MaKhoaHoc = ttdk.MaKhoaHoc;
            hvKhoaHoc.TaiKhoan = ttdk.TaiKhoan;
            hvKhoaHoc.NgayGhiDanh = DateTimes.Now();
            hvKhoaHoc.KichHoat = false;
            db.HocVienKhoaHoc.Add(hvKhoaHoc);
            db.SaveChanges();
            return Ok("Ghi danh thành công!");

        }

        [Authorize]
        [HttpPost("HuyGhiDanh")]
        public async Task<IActionResult> HuyGhiDanh(ThongTinDangKy ttdk)
        {
            var ckTaiKhoan = db.NguoiDung.Where(n => n.TaiKhoan == ttdk.TaiKhoan);
            if (ckTaiKhoan.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Tài khoản không hợp lệ!");
                return response;
            }
            var ckKhoaHoc = db.KhoaHoc.Where(n => n.MaKhoaHoc == ttdk.MaKhoaHoc);
            if (ckKhoaHoc.Count() == 0)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không hợp lệ!");
                return response;
            }
            var ckKhoaHocHocVien = db.HocVienKhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == ttdk.MaKhoaHoc && n.TaiKhoan == ttdk.TaiKhoan);//ngu hủy thì ráng chịu
            if (ckKhoaHocHocVien == null)
            {
                var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Khóa học đã được hủy!");
                return response;
            }
            db.HocVienKhoaHoc.Remove(ckKhoaHocHocVien);
            db.SaveChanges();
            return Ok("Hủy ghi danh thành công!");
        }

        private const int TenMegaBytes = 1024 * 1024;
        //[Authorize(Roles = "GV")]
        [HttpPost("UploadHinhAnhKhoaHoc")]
        public async Task<IActionResult> UploadHinhAnhKhoaHoc()
        {
            //try
            //{

            IFormFile file = Request.Form.Files[0];
            string tenKhoaHoc = LoaiBoKyTu.bestLower(Request.Form["tenKhoaHoc"]);
            string type = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            if (file.Length > TenMegaBytes)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dung lượng file vượt quá 1 MB!");

            }
            if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/gif")
            {
                try
                {
                    //tenKhoaHoc = LoaiBoKyTu.bestLower(file.FileName);
                    //Check khoa học
                    var kh = db.KhoaHoc.Where(n => n.BiDanh.Contains(tenKhoaHoc));
                    if (kh.Count() == 0)
                    {
                        return await tbl.TBLoi(ThongBaoLoi.Loi500, "Khóa học không tồn tại không thể upload file");
                    }

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", tenKhoaHoc + "." + type);
                    var stream = new FileStream(path, FileMode.Create);
                    file.CopyTo(stream);
                    return Ok("Upload file thành công !");
                }
                catch
                {
                    var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Upload file không thành công!");
                    return response;
                }
            }
            else
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Định dạng file không hợp lệ!");
            }


        }
        [HttpPut("demo")]
        //[Authorize(Roles = "QuanTri")]
        public object Convert([FromForm] IFormCollection form, object model)
        {
            Type type = typeof(KhoaHocUpload);
            FieldInfo[] propertyInfos = type.GetFields();

            foreach (FieldInfo propertyInfo in propertyInfos)
            {
                var mykey = propertyInfo.Name;
                if (!string.IsNullOrEmpty(form[mykey]))
                {
                    try
                    {
                        string value = form[mykey];
                        propertyInfo.SetValue(model, value);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            return model;


        }

        //[Authorize(Roles = "GV")]
        [HttpPost("CapNhatKhoaHocUpload")]
        public async Task<ActionResult> CapNhatKhoaHocUpload([FromForm] IFormCollection frm)
        {
            KhoaHocUpload model = new KhoaHocUpload();
            model = (KhoaHocUpload)Convert(frm, model);
            model.MaKhoaHoc = frm["MaKhoaHoc"];
            model.MaNhom = model.MaNhom.ToUpper();
            model.HinhAnh = Request.Form.Files[0];

            if (string.IsNullOrEmpty(model.NgayTao))
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày tạo không hợp lệ, Ngày tạo phải có định dạng dd/MM/yyyy !");
            }
            model.BiDanh = LoaiBoKyTu.bestLower(model.TenKhoaHoc);
            try
            {

                KhoaHoc khUpdate = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == model.MaKhoaHoc);
                if (khUpdate == null)
                {
                    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học không tồn tại!");
                }
                model.MaNhom = model.MaNhom.ToUpper();
                bool ckb = db.Nhom.Any(n => n.MaNhom == model.MaNhom);
                if (!ckb)
                {
                    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ!");
                }
                string tenKhoaHoc = LoaiBoKyTu.bestLower(model.TenKhoaHoc);
                if (string.IsNullOrEmpty(tenKhoaHoc))
                {
                    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên khóa học không hợp lệ!");
                }
                var p = db.KhoaHoc.Where(n => n.BiDanh == tenKhoaHoc);
                int length = p.Count();
                //if (p.Count() > 2)
                //{

                //    //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim đã tồn tại!");
                //}


                khUpdate.TenKhoaHoc = model.TenKhoaHoc;
                khUpdate.BiDanh = LoaiBoKyTu.bestLower(model.TenKhoaHoc);

                khUpdate.MoTa = model.MoTa;
                if (model.HinhAnh != null)
                {

                    //phimUpdate.HinhAnh = model.HinhAnh;
                    khUpdate.HinhAnh = LoaiBoKyTu.bestLower(model.TenKhoaHoc) + "_" + LoaiBoKyTu.bestLower(model.MaNhom) + "." + model.HinhAnh.FileName.Split('.')[model.HinhAnh.FileName.Split('.').Length - 1];
                    string kq = UploadHinhAnh(model.HinhAnh, model.TenKhoaHoc, model.MaNhom);
                    if (kq.Trim() != "")
                    {
                        return await tbl.TBLoi(ThongBaoLoi.Loi500, kq);
                    }
                }
                khUpdate.DanhGia = model.DanhGia;
                khUpdate.MaNhom = LoaiBoKyTu.bestLower(model.MaNhom);
                khUpdate.MoTa = model.MoTa;
                khUpdate.TenKhoaHoc = model.TenKhoaHoc;
                khUpdate.LuotXem = model.LuotXem;
                khUpdate.NguoiTao = model.TaiKhoanNguoiTao;
                khUpdate.MaDanhMucKhoaHoc = model.MaDanhMucKhoaHoc;
                DateTime temp;
                try
                {
                    try
                    {
                        khUpdate.NgayTao = DateTimes.ConvertDate(model.NgayTao);
                    }
                    catch (Exception ex)
                    {
                        khUpdate.NgayTao = DateTime.Now;
                        //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                    }
                }
                catch (Exception ex)
                {
                    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                }

               
                db.SaveChanges();
                return Ok(model);
            }
            catch (Exception ex)
            {

                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dữ liệu không hợp lệ!");
            }
        }


        [HttpPost("ThemKhoaHocUploadHinh")]
        public async Task<ActionResult> ThemKhoaHocUploadHinh([FromForm] IFormCollection frm)
        {

            KhoaHocUpload model = new KhoaHocUpload();
            model = (KhoaHocUpload)Convert(frm, model);
            model.MaNhom = model.MaNhom.ToUpper();

            if (Request.Form.Files[0] == null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Chưa chọn hình ảnh !");

            }


            model.HinhAnh = Request.Form.Files[0];
            string request = Request.Form["HinhAnh"]; ;
            bool ckb = db.Nhom.Any(n => n.MaNhom == model.MaNhom);
            if (!ckb)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ!");
            }
            string tenKhoaHoc = LoaiBoKyTu.bestLower(model.TenKhoaHoc);
            if (string.IsNullOrEmpty(tenKhoaHoc))
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên khóa học không hợp lệ!");
            }
            KhoaHoc checkMa = db.KhoaHoc.SingleOrDefault(n => n.MaKhoaHoc == model.MaKhoaHoc);
            if (checkMa != null)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã khóa học đã tồn tại!");

            }
            var p = db.KhoaHoc.Where(n => n.BiDanh == model.BiDanh);
            if (p.Count() > 1)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên khóa học đã tồn tại!");
            }


            //PhimInsertNew phimNew = Mapper.Map<PhimInsert, PhimInsertNew>(model)
            KhoaHoc modelInsert = new KhoaHoc();
            modelInsert.MaKhoaHoc = model.MaKhoaHoc;
            modelInsert.BiDanh = LoaiBoKyTu.bestLower(model.TenKhoaHoc);
            modelInsert.DanhGia = model.DanhGia;
            //modelInsert.DaXoa = false;
            modelInsert.HinhAnh = LoaiBoKyTu.bestLower(model.TenKhoaHoc) + "_" + LoaiBoKyTu.bestLower(model.MaNhom) + "." + model.HinhAnh.FileName.Split('.')[model.HinhAnh.FileName.Split('.').Length - 1];
            modelInsert.MaNhom = LoaiBoKyTu.bestLower(model.MaNhom);
            modelInsert.MoTa = model.MoTa;
            modelInsert.TenKhoaHoc = model.TenKhoaHoc;
            modelInsert.LuotXem = model.LuotXem;
            modelInsert.NguoiTao = model.TaiKhoanNguoiTao;
            modelInsert.MaDanhMucKhoaHoc = model.MaDanhMucKhoaHoc;

            //modelInsert.NgayTao = model.NgayTao.;
            //modelInsert.Trailer = model.trailer;
            DateTime temp;
            try
            {
                try
                {
                    modelInsert.NgayTao = DateTimes.ConvertDate(model.NgayTao);
                }
                catch (Exception ex)
                {
                    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày tạo không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                }
            }
            catch (Exception ex)
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, "Ngày tạo không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
            }



            db.KhoaHoc.Add(modelInsert);
            string kq = UploadHinhAnh(model.HinhAnh, modelInsert.TenKhoaHoc, modelInsert.MaNhom);
            if (kq != "")
            {
                return await tbl.TBLoi(ThongBaoLoi.Loi500, kq);


            }
            modelInsert.MaNhom = model.MaNhom.ToUpper();

            db.SaveChanges();

            return Ok(modelInsert);

        }


        [HttpPost]
        public string UploadHinhAnh(IFormFile file, string tenKhoaHoc, string maNhom)
        {
            file = Request.Form.Files[0];
          
            //maNhom = maNhom.ToUpper();
            tenKhoaHoc = LoaiBoKyTu.bestLower(tenKhoaHoc);

            if (string.IsNullOrEmpty(tenKhoaHoc))
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ");
                return "Tên khóa học không hợp lệ";

            }
            if (string.IsNullOrEmpty(maNhom) || !db.Nhom.Any(n => n.MaNhom == maNhom))
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Mã nhóm không hợp lệ");
                return "Mã nhóm không hợp lệ";
            }


            if (file.Length > TenMegaBytes)
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Dung lượng file vượt quá 1 MB!");
                return "Dung lượng file vượt quá 1 MB!";
            }
            if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/gif")
            {
                try
                {
                    //tenPhim = LoaiBoKyTu.bestLower(file.FileName);
                    //Check khoa học
                    //var kh = db.Phim.Where(n => n.BiDanh.Contains(tenPhim));
                    //if (kh.Count() == 0)
                    //{
                    //    return await tbl.TBLoi(ThongBaoLoi.Loi500, "Phim không tồn tại không thể upload file");
                    //}

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", tenKhoaHoc + "_" + LoaiBoKyTu.bestLower(maNhom) + "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1]);
                    var stream = new FileStream(path, FileMode.Create);
                    file.CopyTo(stream);
                    return "";
                }
                catch
                {
                    //var response = await tbl.TBLoi(ThongBaoLoi.Loi500, "Upload file không thành công!");
                    return "Upload file không thành công!";
                }
            }
            else
            {
                //return await tbl.TBLoi(ThongBaoLoi.Loi500, "Định dạng file không hợp lệ!");
                return "Định dạng file không hợp lệ!";
            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                db.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}