using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANHANG.Models
{
    public class ChiTietHoaDonNhapModel
    {
        public string ID_HoaDonNhap { get; set; }
        public string ID_HangHoa { get; set; }
        public string TenHanghoa { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
    }
}
