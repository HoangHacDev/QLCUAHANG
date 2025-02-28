using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QLBANHANG.Handlers
{
    public class NhanVienHandler
    {
        private readonly NhanVienServices _nhanVienServices;
        //private readonly ConnectionString _connectionString;

        public NhanVienHandler(NhanVienServices nhanVienServices)
        {
            _nhanVienServices = nhanVienServices ?? throw new ArgumentNullException(nameof(nhanVienServices));
        }

        public void HandleInsert(TextBox txtHoTen, DateTime dateTimeNgaySinh, TextBox txtgioiTinh, TextBox txtDiaChi, TextBox txtSoDienThoai, TextBox txtEmail, Action<string> onSuccess)
        {
            //DateTime dt = DateTime.Now;
            string hoTen = txtHoTen.Text.Trim();
            DateTime ngaysinh = dateTimeNgaySinh;
            string gioiTinh = txtgioiTinh.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!");
                return;
            }

            if (ngaysinh > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại!");
                return;
            }

            string newId = _nhanVienServices.InsertNhanVien(hoTen, ngaysinh, gioiTinh, diaChi, soDienThoai, email);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm nhân viên: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHoTen.Clear();
                txtgioiTinh.Clear();
                txtDiaChi.Clear();
                txtSoDienThoai.Clear();
                txtEmail.Clear();
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }

        // Lấy danh sách khách hàng
        public void HandleLoadData(DataGridView dgvKhachHang)
        {
            List<NhanVienModel> nhanViens = _nhanVienServices.GetNhanVien();

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Tên nhân viên", typeof(string));
            dt.Columns.Add("Ngày sinh", typeof(DateTime));
            dt.Columns.Add("Giới tính", typeof(string));
            dt.Columns.Add("Địa chỉ", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            for (int i = 0; i < nhanViens.Count; i++)
            {
                dt.Rows.Add(i + 1, nhanViens[i].Hoten, nhanViens[i].NgaySinh, nhanViens[i].GioiTinh, nhanViens[i].DiaChi, nhanViens[i].SoDienThoai, nhanViens[i].Email);
            }

            dgvKhachHang.DataSource = dt;
            if (dgvKhachHang.Columns["ID_NhanVien"] != null)
            {
                dgvKhachHang.Columns["ID_NhanVien"].Visible = false;
            }
            // Tùy chỉnh giao diện cột
            dgvKhachHang.Columns["STT"].Width = 50;
            dgvKhachHang.Columns["Tên nhân viên"].Width = 150;
            dgvKhachHang.Columns["Ngày sinh"].Width = 100;
            dgvKhachHang.Columns["Giới tính"].Width = 100;
            dgvKhachHang.Columns["Địa chỉ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvKhachHang.Columns["Số điện thoại"].Width = 100;
            dgvKhachHang.Columns["Email"].Width = 150;
        }

        // Cập nhật khách hàng
        public void HandleUpdate(string idNhanVien, TextBox txtHoTen, DateTime dateTimeNgaySinh, TextBox txtgioiTinh, TextBox txtDiaChi, TextBox txtSoDienThoai, TextBox txtEmail, Action onSuccess)
        {
            string hoTen = txtHoTen.Text.Trim();
            DateTime ngaysinh = dateTimeNgaySinh;
            string gioiTinh = txtgioiTinh.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!");
                return;
            }

            if (ngaysinh > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh không được lớn hơn ngày hiện tại!");
                return;
            }

            bool updated = _nhanVienServices.UpdateNhanVien(idNhanVien, hoTen, ngaysinh, gioiTinh, diaChi, soDienThoai, email);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật nhân viên!");
                onSuccess?.Invoke();
            }
        }

        // Xóa khách hàng
        public void HandleDelete(string idNhanVien, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idNhanVien))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _nhanVienServices.DeleteNhanVien(idNhanVien);
                if (deleted)
                {
                    MessageBox.Show("Đã xóa nhân viên!");
                    onSuccess?.Invoke();
                }
            }
        }
    }
}
