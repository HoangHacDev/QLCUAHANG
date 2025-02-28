using QLBANHANG.Handlers;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBANHANG
{
    public partial class QLDanhMucKHForm: Form
    {
        private readonly KhachHangServices _khachHangServices;
        
        private readonly KhachHangHandler _khachHangHandler;
        public QLDanhMucKHForm()
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();
            _khachHangServices = new KhachHangServices(connectionString);

            _khachHangHandler = new KhachHangHandler(_khachHangServices);
        }

        private void QLDanhMucKHForm_Load(object sender, EventArgs e)
        {
            TxtMaKH.ReadOnly = true;
            TxtMaKH.TabStop = false;
            TxtMaKH.Enabled = false;
        }

        private void BtnReloadKHBtn_Click(object sender, EventArgs e)
        {
            _khachHangHandler.HandleLoadData(DgvKhachHang); // Load dữ liệu khi form mở
        }

        private void BtnThemKH_Click(object sender, EventArgs e)
        {
            _khachHangHandler.HandleInsert(TxtTenKH, TxtDiaChiKH, TxtSDTKH, TxtEmailKH, newId =>
            {
                _khachHangHandler.HandleLoadData(DgvKhachHang);
            });
        }

        private void BtnSuaKH_Click(object sender, EventArgs e)
        {
            if (DgvKhachHang.CurrentRow != null)
            {
                string id = TxtMaKH.Text;
                _khachHangHandler.HandleUpdate(id, TxtTenKH, TxtDiaChiKH, TxtSDTKH, TxtEmailKH, () =>
                {
                    _khachHangHandler.HandleLoadData(DgvKhachHang);
                });
            }
        }

        private void BtnXoaKH_Click(object sender, EventArgs e)
        {
            if (DgvKhachHang.CurrentRow != null)
            {
                string id = TxtMaKH.Text;
                _khachHangHandler.HandleDelete(id, () =>
                {
                    _khachHangHandler.HandleLoadData(DgvKhachHang);
                });
            }
        }

        private void BtnHuyONhapLieu_Click(object sender, EventArgs e)
        {
            TxtMaKH.Clear();
            TxtTenKH.Clear();
            TxtDiaChiKH.Clear();
            TxtSDTKH.Clear();
            TxtEmailKH.Clear();
        }

        private void BtnThoatFormQLKH_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
           "Bạn có chắc chắn muốn thoát Quản lý Khách Hàng không?",
           "Xác nhận thoát",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void DgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có click vào dòng hợp lệ không (tránh header hoặc ngoài dữ liệu)
            if (e.RowIndex >= 0 && e.RowIndex < DgvKhachHang.Rows.Count)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DgvKhachHang.Rows[e.RowIndex]; 
                TxtMaKH.Text = _khachHangServices.GetKhachHang()[e.RowIndex].ID_KhachHang;
                TxtTenKH.Text = row.Cells["Tên khách hàng"].Value?.ToString() ?? "";
                TxtDiaChiKH.Text = row.Cells["Địa chỉ"].Value?.ToString() ?? "";
                TxtSDTKH.Text = row.Cells["Số điện thoại"].Value?.ToString() ?? "";
                TxtEmailKH.Text = row.Cells["Email"].Value?.ToString() ?? "";
            }
        }
    }
}
