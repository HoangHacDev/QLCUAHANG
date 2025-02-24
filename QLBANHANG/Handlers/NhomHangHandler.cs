using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QLBANHANG.Handlers
{
    public class NhomHangHandler
    {
        // Gọi services
        private readonly NhomHangServices _nhomHangServices;

        // Nhúng vào contructor
        public NhomHangHandler(NhomHangServices nhomHangServices)
        {
            _nhomHangServices = nhomHangServices ?? throw new ArgumentNullException(nameof(nhomHangServices));
        }

        // Thêm 
        public void HandleInsert(TextBox txtTenNhomHang, Action<string> onSuccess)
        {
            string tenNhomHang = txtTenNhomHang.Text.Trim();
            if (string.IsNullOrEmpty(tenNhomHang))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm hàng!");
                return;
            }

            string newId = _nhomHangServices.InsertNhomHang(tenNhomHang);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm nhóm hàng: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNhomHang.Clear(); // Xóa TextBox ngay trong handler
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }

        // Lấy danh sách
        public void HandleLoadData(DataGridView dgvNhomHang)
        {
            // Lấy dữ liệu từ service
            List<NhomHangModel> nhomHangs = _nhomHangServices.GetNhomHang();

            // Tạo DataTable để tùy chỉnh cột
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));           // Thêm cột STT
            dt.Columns.Add("Tên nhóm hàng", typeof(string)); // Đổi tên cột TenNhomHang

            // Đổ dữ liệu vào DataTable và thêm STT
            for (int i = 0; i < nhomHangs.Count; i++)
            {
                dt.Rows.Add(i + 1, nhomHangs[i].TenNhomHang);
            }

            // Gán DataTable vào DataGridView
            dgvNhomHang.DataSource = dt;

            // Ẩn cột ID_NhomHang (nếu cần giữ trong dữ liệu gốc, ta không thêm vào DataTable)
            // Nếu vẫn muốn giữ ID_NhomHang để xử lý logic, có thể thêm vào DataTable nhưng ẩn nó
            if (dgvNhomHang.Columns["ID_NhomHang"] != null)
            {
                dgvNhomHang.Columns["ID_NhomHang"].Visible = false;
            }

            // Tùy chỉnh giao diện cột (nếu cần)
            dgvNhomHang.Columns["STT"].Width = 50;
            dgvNhomHang.Columns["Tên nhóm hàng"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // Cập nhật
        public void HandleUpdate(string idNhomHang, string tenNhomHang, Action onSuccess)
        {
            if (string.IsNullOrEmpty(tenNhomHang))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm hàng!");
                return;
            }

            bool updated = _nhomHangServices.UpdateNhomHang(idNhomHang, tenNhomHang);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật nhóm hàng!");
                onSuccess?.Invoke();
            }
        }

        // Xóa
        public void HandleDelete(string idNhomHang, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idNhomHang))
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _nhomHangServices.DeleteNhomHang(idNhomHang);
                if (deleted)
                {
                    MessageBox.Show("Đã xóa nhóm hàng!");
                    onSuccess?.Invoke();
                }
            }
        }

    }
}
