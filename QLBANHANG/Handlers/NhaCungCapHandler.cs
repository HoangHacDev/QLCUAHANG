using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QLBANHANG.Handlers
{
    public class NhaCungCapHandler
    {
        private readonly NhaCungCapServices _nhaCungCapServices;
        //private readonly ConnectionString _connectionString;

        public NhaCungCapHandler(NhaCungCapServices nhaCungCapServices)
        {
            _nhaCungCapServices = nhaCungCapServices ?? throw new ArgumentNullException(nameof(nhaCungCapServices));
        }

        public void HandleInsert(TextBox txtTenNCC, TextBox txtDiaChi, TextBox txtSoDienThoai, TextBox txtEmail, Action<string> onSuccess)
        {
            //DateTime dt = DateTime.Now;
            string hoTen = txtTenNCC.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!");
                return;
            }

            string newId = _nhaCungCapServices.InsertNhaCungCap(hoTen, diaChi, soDienThoai, email);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm nhà cung cấp: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNCC.Clear();
                txtDiaChi.Clear();
                txtSoDienThoai.Clear();
                txtEmail.Clear();
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }

        // Lấy danh sách khách hàng
        public void HandleLoadData(DataGridView dgvKhachHang)
        {
            List<NhaCungCapModel> nhaCungCaps = _nhaCungCapServices.GetNhaCungCap();

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Tên nhà cung cấp", typeof(string));
            dt.Columns.Add("Địa chỉ", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            for (int i = 0; i < nhaCungCaps.Count; i++)
            {
                dt.Rows.Add(i + 1, nhaCungCaps[i].TenNhaCungCap, nhaCungCaps[i].DiaChi, nhaCungCaps[i].SoDienThoai, nhaCungCaps[i].Email);
            }

            dgvKhachHang.DataSource = dt;
            if (dgvKhachHang.Columns["ID_NhaCungCap"] != null)
            {
                dgvKhachHang.Columns["ID_NhaCungCap"].Visible = false;
            }
            // Tùy chỉnh giao diện cột
            dgvKhachHang.Columns["STT"].Width = 50;
            dgvKhachHang.Columns["Tên nhà cung cấp"].Width = 150;
            dgvKhachHang.Columns["Địa chỉ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvKhachHang.Columns["Số điện thoại"].Width = 100;
            dgvKhachHang.Columns["Email"].Width = 150;
        }

        // Cập nhật khách hàng
        public void HandleUpdate(string idNhaCC, TextBox txtTenNCC, TextBox txtDiaChi, TextBox txtSoDienThoai, TextBox txtEmail, Action onSuccess)
        {
            string hoTen = txtTenNCC.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!");
                return;
            }

            bool updated = _nhaCungCapServices.UpdateNhaCungCap(idNhaCC, hoTen, diaChi, soDienThoai, email);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật nhà cung cấp!");
                onSuccess?.Invoke();
            }
        }

        // Xóa khách hàng
        public void HandleDelete(string idNhaCC, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idNhaCC))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _nhaCungCapServices.DeleteNhaCungCap(idNhaCC);
                if (deleted)
                {
                    MessageBox.Show("Đã xóa nhà cung cấp!");
                    onSuccess?.Invoke();
                }
            }
        }
    }
}
