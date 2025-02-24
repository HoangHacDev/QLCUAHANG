using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANHANG.Models
{
    public class HangHoaModel
    {
        public string ID_HangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public string TenNhomHang { get; set; }
        public string MauSac { get; set; }
        public string KichThuoc { get; set; }
        public string DacTinhKyThuat { get; set; }
        public string DonViTinh { get; set; }
        public int? SoLuong { get; set; }
        public byte[] Anh { get; set; }
        public int? GiaBan { get; set; }
    }
}
