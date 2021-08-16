using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace elearningAPI.Models
{
    public partial class elearningContext : DbContext
    {
        public elearningContext()
        {

        }

        public elearningContext(DbContextOptions<elearningContext> options)
            : base(options)
        {
            

        }

        public virtual DbSet<BaiHoc> BaiHoc { get; set; }
        public virtual DbSet<DanhMucKhoaHoc> DanhMucKhoaHoc { get; set; }
        public virtual DbSet<HocVienKhoaHoc> HocVienKhoaHoc { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHoc { get; set; }
        public virtual DbSet<LoaiNguoiDung> LoaiNguoiDung { get; set; }
        public virtual DbSet<NguoiDung> NguoiDung { get; set; }
        public virtual DbSet<Nhom> Nhom { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            
            if (!optionsBuilder.IsConfigured)
            {


                //To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //b. Install the Microsoft.EntityFrameworkCore.Proxies package

               
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("server=103.97.125.205,1433;database=elearning;user id=khaicybersoft;password=khaicybersoft321@;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<BaiHoc>(entity =>
            {
                entity.HasKey(e => e.MaBaiHoc);

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(50);

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.BaiHoc)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .HasConstraintName("FK_BaiHoc_KhoaHoc");
            });

            modelBuilder.Entity<DanhMucKhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc);

                entity.Property(e => e.MaDanhMuc)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BiDanh).HasMaxLength(50);

                entity.Property(e => e.Logo).HasMaxLength(50);

                entity.Property(e => e.TenDanhMuc).HasMaxLength(50);
            });

            modelBuilder.Entity<HocVienKhoaHoc>(entity =>
            {
                entity.HasKey(e => new { e.MaKhoaHoc, e.TaiKhoan });

                entity.Property(e => e.MaKhoaHoc).HasMaxLength(50);

                entity.Property(e => e.TaiKhoan).HasMaxLength(255);

                entity.Property(e => e.NgayGhiDanh).HasColumnType("datetime");

                entity.HasOne(d => d.MaKhoaHocNavigation)
                    .WithMany(p => p.HocVienKhoaHoc)
                    .HasForeignKey(d => d.MaKhoaHoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HocVienKhoaHoc_KhoaHoc");

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.HocVienKhoaHoc)
                    .HasForeignKey(d => d.TaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HocVienKhoaHoc_NguoiDung");
            });

            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaKhoaHoc);

                entity.Property(e => e.MaKhoaHoc)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BiDanh).HasMaxLength(255);

                entity.Property(e => e.MaDanhMucKhoaHoc).HasMaxLength(50);

                entity.Property(e => e.MaNhom).HasMaxLength(50);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NguoiTao).HasMaxLength(255);

                entity.Property(e => e.TenKhoaHoc).HasMaxLength(255);

                entity.HasOne(d => d.MaDanhMucKhoaHocNavigation)
                    .WithMany(p => p.KhoaHoc)
                    .HasForeignKey(d => d.MaDanhMucKhoaHoc)
                    .HasConstraintName("FK_KhoaHoc_DanhMucKhoaHoc");

                entity.HasOne(d => d.MaNhomNavigation)
                    .WithMany(p => p.KhoaHoc)
                    .HasForeignKey(d => d.MaNhom)
                    .HasConstraintName("FK_KhoaHoc_Nhom");

                entity.HasOne(d => d.NguoiTaoNavigation)
                    .WithMany(p => p.KhoaHoc)
                    .HasForeignKey(d => d.NguoiTao)
                    .HasConstraintName("FK_KhoaHoc_NguoiDung");
            });

            modelBuilder.Entity<LoaiNguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaLoaiNguoiDung);

                entity.Property(e => e.MaLoaiNguoiDung)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.TenLoaiNguoiDung).HasMaxLength(50);
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan);

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.BiDanh).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.HoTen).HasMaxLength(255);

                entity.Property(e => e.MaLoaiNguoiDung).HasMaxLength(50);

                entity.Property(e => e.MaNhom).HasMaxLength(50);

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.SoDt)
                    .HasColumnName("SoDT")
                    .HasMaxLength(255);

                entity.HasOne(d => d.MaLoaiNguoiDungNavigation)
                    .WithMany(p => p.NguoiDung)
                    .HasForeignKey(d => d.MaLoaiNguoiDung)
                    .HasConstraintName("FK_NguoiDung_LoaiNguoiDung");
            });

            modelBuilder.Entity<Nhom>(entity =>
            {
                entity.HasKey(e => e.MaNhom);

                entity.Property(e => e.MaNhom)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.TenNhom).HasMaxLength(50);
            });
        }
    }
}
