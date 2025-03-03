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
    public class HoaDonHandler
    {
        private readonly HoaDonServices _hoaDonServices;

        //private readonly ConnectionString _connectionString;

        public HoaDonHandler(HoaDonServices hoaDonService)
        {
            _hoaDonServices = hoaDonService ?? throw new ArgumentNullException(nameof(hoaDonService));
        }

        public void HandleInsert(ComboBox id_Nhanvien, ComboBox id_KhachHang, DateTime ngayBan, Action<string> onSuccess)
        {
            string idNhaVien = id_Nhanvien.SelectedValue?.ToString();
            string idKH = id_KhachHang.SelectedValue?.ToString();
            DateTime ngayBann = ngayBan;
            bool daThuTien = false;

            if (ngayBann > DateTime.Now)
            {
                MessageBox.Show("Ngày bán không được lớn hơn ngày hiện tại!");
                return;
            }

            string newId = _hoaDonServices.InsertHoaDon(idNhaVien, idKH, ngayBann, daThuTien);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm hoá đơn: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id_Nhanvien.SelectedValue = -1;
                id_KhachHang.SelectedValue = -1;
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }

        public void HandleLoadData(DataGridView dgvHoaDon)
        {
            List<HoaDonModel> hoaDons = _hoaDonServices.GetHoaDon();

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã hóa đơn", typeof(string));
            dt.Columns.Add("Ngày bán", typeof(string));
            dt.Columns.Add("Tổng tiền", typeof(string));
            dt.Columns.Add("Đã thu tiền", typeof(string));

            for (int i = 0; i < hoaDons.Count; i++)
            {
                dt.Rows.Add(i + 1, hoaDons[i].ID_HoaDonBan, hoaDons[i].NgayBan, hoaDons[i].TongTien, hoaDons[i].DaThuTien);
            }

            dgvHoaDon.DataSource = dt;
            if (dgvHoaDon.Columns["ID_NhanVien"] != null)
            {
                dgvHoaDon.Columns["ID_NhanVien"].Visible = false;
            }
            if (dgvHoaDon.Columns["ID_KhachHang"] != null)
            {
                dgvHoaDon.Columns["ID_KhachHang"].Visible = false;
            }
            // Tùy chỉnh giao diện cột
            dgvHoaDon.Columns["STT"].Width = 50;
            dgvHoaDon.Columns["Mã hóa đơn"].Width = 100;
            dgvHoaDon.Columns["Ngày bán"].Width = 150;
            dgvHoaDon.Columns["Tổng tiền"].Width = 100;
            dgvHoaDon.Columns["Đã thu tiền"].Width = 150;
        }

        public void HandleUpdate(string idHoaDon, ComboBox id_Nhanvien, ComboBox id_KhachHang, DateTime ngayBan, string tongTien, TextBox daThuTien, Action onSuccess)
        {
            string idNhaVien = id_Nhanvien.SelectedValue?.ToString();
            string idNhaKH = id_KhachHang.SelectedValue?.ToString();
            DateTime ngayBann = ngayBan;
            int TongTienHD = int.Parse(tongTien);
            string DaThuTien = daThuTien.Text.Trim();

            if (ngayBann > DateTime.Now)
            {
                MessageBox.Show("Ngày bán không được lớn hơn ngày hiện tại!");
                return;
            }

            bool updated = _hoaDonServices.UpdateHoaDonBan(idHoaDon, idNhaVien, idNhaKH, ngayBann, TongTienHD, DaThuTien);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật hoá đơn !");
                onSuccess?.Invoke();
            }
        }

        public void HandleDelete(string idHoaDon, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idHoaDon))
            {
                MessageBox.Show("Vui lòng chọn hoá đơn nhập để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _hoaDonServices.DeleteHoaDon(idHoaDon);
                if (!deleted)
                {
                    MessageBox.Show("Đã xóa hoá đơn !");
                    onSuccess?.Invoke();
                }
            }
        }

    }
}
