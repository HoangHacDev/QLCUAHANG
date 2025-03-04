using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANHANG.Models
{
    public class LoginModel
    {
        public bool IsSuccess { get; set; }
        public string Role { get; set; }
        public string ID_NhanVien { get; set; }
        public string Message { get; set; }
    }
}
