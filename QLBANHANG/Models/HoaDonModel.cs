using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANHANG.Models
{
    public class HoaDonModel
    {
        public string ID_HoaDonBan { get; set; }
        public string ID_NhanVien { get; set; }
        public string ID_KhachHang { get; set; }
        public string TenNhanVien { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; } 
        public DateTime NgayBan { get; set; }
        public string TongTien { get; set; }
        public bool DaThuTien { get; set; }
    }
}
