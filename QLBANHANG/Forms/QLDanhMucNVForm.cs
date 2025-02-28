using QLBANHANG.Handlers;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace QLBANHANG
{
    public partial class QLDanhMucNVForm: Form
    {
        private readonly NhanVienServices _nhanVienServices;

        private readonly NhanVienHandler _nhanVienHandler;

        public QLDanhMucNVForm()
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();
            _nhanVienServices = new NhanVienServices(connectionString);

            _nhanVienHandler = new NhanVienHandler(_nhanVienServices);
        }

        private void QLDanhMucNVForm_Load(object sender, EventArgs e)
        {
            TxtMaNV.ReadOnly = true;
            TxtMaNV.TabStop = false;
            TxtMaNV.Enabled = false;
        }

        private void BtnLoadDSNV_Click(object sender, EventArgs e)
        {
            _nhanVienHandler.HandleLoadData(DgvNhanVien); // Load dữ liệu khi form mở
        }

        private void BtnThoatFormNV_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
          "Bạn có chắc chắn muốn thoát Quản lý Nhân viên không?",
          "Xác nhận thoát",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnThemNV_Click(object sender, EventArgs e)
        {
            _nhanVienHandler.HandleInsert(TxtTenNV, DTPNgaySinh.Value, TxtGioiTinhNV, TxtDiaChiNV, TxtSDTNV, TxtEmailNV, newId =>
            {
                _nhanVienHandler.HandleLoadData(DgvNhanVien);
            });
        }

        private void BtnXoaNV_Click(object sender, EventArgs e)
        {
            if (DgvNhanVien.CurrentRow != null)
            {
                string id = TxtMaNV.Text;
                _nhanVienHandler.HandleDelete(id, () =>
                {
                    _nhanVienHandler.HandleLoadData(DgvNhanVien);
                });
            }
        }

        private void BtnSuaNV_Click(object sender, EventArgs e)
        {
            if (DgvNhanVien.CurrentRow != null)
            {
                string id = TxtMaNV.Text;
                _nhanVienHandler.HandleUpdate(id, TxtTenNV, DTPNgaySinh.Value, TxtGioiTinhNV, TxtDiaChiNV, TxtSDTNV, TxtEmailNV, () =>
                {
                    _nhanVienHandler.HandleLoadData(DgvNhanVien);
                });
            }
        }

        private void BtnHuyDuLieu_Click(object sender, EventArgs e)
        {
            TxtMaNV.Clear();
            TxtTenNV.Clear();
            DTPNgaySinh.Value = DateTime.Now;
            TxtGioiTinhNV.Clear();
            TxtDiaChiNV.Clear();
            TxtSDTNV.Clear();
            TxtEmailNV.Clear();
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có click vào dòng hợp lệ không (tránh header hoặc ngoài dữ liệu)
            if (e.RowIndex >= 0 && e.RowIndex < DgvNhanVien.Rows.Count)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DgvNhanVien.Rows[e.RowIndex];
                TxtMaNV.Text = _nhanVienServices.GetNhanVien()[e.RowIndex].ID_NhanVien;
                TxtTenNV.Text = row.Cells["Tên nhân viên"].Value?.ToString() ?? "";
                DTPNgaySinh.Text = row.Cells["Ngày sinh"].Value?.ToString() ?? "";
                TxtGioiTinhNV.Text = row.Cells["Giới tính"].Value?.ToString() ?? "";
                TxtDiaChiNV.Text = row.Cells["Địa chỉ"].Value?.ToString() ?? "";
                TxtSDTNV.Text = row.Cells["Số điện thoại"].Value?.ToString() ?? "";
                TxtEmailNV.Text = row.Cells["Email"].Value?.ToString() ?? "";
            }
        }
    }
}
