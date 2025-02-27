using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBANHANG.Handlers
{
    public class KhachHangHandler
    {
        private readonly KhachHangServices _khachHangServices;
        //private readonly ConnectionString _connectionString;

        public KhachHangHandler(KhachHangServices khachHangServices)
        {
            _khachHangServices = khachHangServices ?? throw new ArgumentNullException(nameof(khachHangServices));
        }

        public void HandleInsert(TextBox txtTenKhachHang, TextBox txtDiaChi, TextBox txtSoDienThoai, TextBox txtEmail, Action<string> onSuccess)
        {
            string tenKhachHang = txtTenKhachHang.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                return;
            }

            string newId = _khachHangServices.InsertkhachHang(tenKhachHang, diaChi, soDienThoai, email);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm khách hàng: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenKhachHang.Clear();
                txtDiaChi.Clear();
                txtSoDienThoai.Clear();
                txtEmail.Clear();
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }

        // Lấy danh sách khách hàng
        public void HandleLoadData(DataGridView dgvKhachHang)
        {
            List<KhachHangModel> khachHangs = _khachHangServices.GetKhachHang();

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Tên khách hàng", typeof(string));
            dt.Columns.Add("Địa chỉ", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            for (int i = 0; i < khachHangs.Count; i++)
            {
                dt.Rows.Add(i + 1, khachHangs[i].TenKhachHang, khachHangs[i].DiaChi, khachHangs[i].SoDienThoai, khachHangs[i].Email);
            }

            dgvKhachHang.DataSource = dt;
            if (dgvKhachHang.Columns["ID_KhachHang"] != null)
            {
                dgvKhachHang.Columns["ID_KhachHang"].Visible = false;
            }
            // Tùy chỉnh giao diện cột
            dgvKhachHang.Columns["STT"].Width = 50;
            dgvKhachHang.Columns["Tên khách hàng"].Width = 150;
            dgvKhachHang.Columns["Địa chỉ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvKhachHang.Columns["Số điện thoại"].Width = 100;
            dgvKhachHang.Columns["Email"].Width = 150;
        }

        // Cập nhật khách hàng
        public void HandleUpdate(string idKhachHang, TextBox txtTenKhachHang, TextBox txtDiaChi, TextBox txtSoDienThoai, TextBox txtEmail, Action onSuccess)
        {
            string tenKhachHang = txtTenKhachHang.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!");
                return;
            }

            bool updated = _khachHangServices.UpdateKhachHang(idKhachHang, tenKhachHang, diaChi, soDienThoai, email);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật khách hàng!");
                onSuccess?.Invoke();
            }
        }

        // Xóa khách hàng
        public void HandleDelete(string idKhachHang, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idKhachHang))
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _khachHangServices.DeleteKhachHang(idKhachHang);
                if (deleted)
                {
                    MessageBox.Show("Đã xóa khách hàng!");
                    onSuccess?.Invoke();
                }
            }
        }
    }
}
