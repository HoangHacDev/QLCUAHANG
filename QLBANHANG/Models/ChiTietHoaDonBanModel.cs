using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANHANG.Models
{
    public class ChiTietHoaDonBanModel
    {
        public string ID_HoaDonBan { get; set; }
        public string ID_HangHoa { get; set; }
        public string TenHangHoa { get; set; }
        public int SoLuong { get; set; }
        public string QuiCach { get; set; }    
        public int GiaBan { get; set; }
        public string BaoHanh { get; set; }
    }
}
