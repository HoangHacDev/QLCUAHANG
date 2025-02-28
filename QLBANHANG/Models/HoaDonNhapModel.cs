using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANHANG.Models
{
    public class HoaDonNhapModel
    {
        public string ID_HoaDonNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string TongTien { get; set; }
        public string GhiChu { get; set; }
        public string ID_NhanVien { get; set; }
        public string ID_NhaCungCap { get; set; }
        public string TenNhanVien { get; set; }
        public string TenNhaCungCap { get; set; }
    }
}
