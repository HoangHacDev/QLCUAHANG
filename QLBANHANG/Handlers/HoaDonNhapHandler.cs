using Google.Protobuf.WellKnownTypes;
using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace QLBANHANG.Handlers
{
    public class HoaDonNhapHandler
    {
        private readonly HoaDonNhapService _hoaDonNhapService;

        //private readonly ConnectionString _connectionString;

        public HoaDonNhapHandler(HoaDonNhapService hoaDonNhapService)
        {
            _hoaDonNhapService = hoaDonNhapService ?? throw new ArgumentNullException(nameof(hoaDonNhapService));
        }

        public void HandleInsert(DateTime ngayBan, TextBox txtghiChu, ComboBox id_Nhanvien, ComboBox id_NhaCC, Action<string> onSuccess)
        {
            DateTime ngayBann = ngayBan;
            //int TongTien = int.Parse(txtongTien.Text.Trim());
            string GhiChu = txtghiChu.Text.Trim();
            string idNhaVien = id_Nhanvien.SelectedValue?.ToString();
            string idNhacc = id_NhaCC.SelectedValue?.ToString();

            if (ngayBann > DateTime.Now)
            {
                MessageBox.Show("Ngày bán không được lớn hơn ngày hiện tại!");
                return;
            }

            string newId = _hoaDonNhapService.InsertHoaDonNhap(ngayBann, GhiChu, idNhaVien, idNhacc);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm hoá đơn: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //ngayBan.Value = DateTime.Now;
                //txtongTien.Clear();
                txtghiChu.Clear();
                id_Nhanvien.SelectedValue = -1;
                id_NhaCC.SelectedValue = -1;
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }

        public void HandleLoadData(DataGridView dgvHoaDonNhap)
        {
            List<HoaDonNhapModel> hoaDonNhaps = _hoaDonNhapService.GetHoaDon();

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Ngày nhập", typeof(string));
            dt.Columns.Add("Tổng tiền", typeof(string));
            dt.Columns.Add("Ghi chú", typeof(string));
            dt.Columns.Add("Tên nhân viên", typeof(string));
            dt.Columns.Add("Tên nhà cung cấp", typeof(string));

            for (int i = 0; i < hoaDonNhaps.Count; i++)
            {
                dt.Rows.Add(i + 1, hoaDonNhaps[i].NgayNhap, hoaDonNhaps[i].TongTien, hoaDonNhaps[i].GhiChu, hoaDonNhaps[i].TenNhanVien, hoaDonNhaps[i].TenNhaCungCap);
            }

            dgvHoaDonNhap.DataSource = dt;
            if (dgvHoaDonNhap.Columns["ID_HoaDonNhap"] != null)
            {
                dgvHoaDonNhap.Columns["ID_HoaDonNhap"].Visible = false;
            }
            // Tùy chỉnh giao diện cột
            dgvHoaDonNhap.Columns["STT"].Width = 50;
            dgvHoaDonNhap.Columns["Ngày nhập"].Width = 150;
            dgvHoaDonNhap.Columns["Tổng tiền"].Width = 100;
            dgvHoaDonNhap.Columns["Ghi chú"].Width = 200;
            dgvHoaDonNhap.Columns["Tên nhân viên"].Width = 150;
            dgvHoaDonNhap.Columns["Tên nhà cung cấp"].Width = 150;
        }

        public void HandleLoadDataCT(DataGridView dgvHoaDonNhap)
        {
            List<ChiTietHoaDonNhapModel> ChiTietHoaDonNhaps = _hoaDonNhapService.GetCTHoaDon();

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Tên Hàng Hoá", typeof(string));
            dt.Columns.Add("Số lượng", typeof(string));
            dt.Columns.Add("Đơn giá", typeof(string));
            dt.Columns.Add("Thành Tiền", typeof(string));

            for (int i = 0; i < ChiTietHoaDonNhaps.Count; i++)
            {
                dt.Rows.Add(i + 1, ChiTietHoaDonNhaps[i].TenHanghoa, ChiTietHoaDonNhaps[i].SoLuong, ChiTietHoaDonNhaps[i].DonGia, ChiTietHoaDonNhaps[i].ThanhTien);
            }

            dgvHoaDonNhap.DataSource = dt;
            if (dgvHoaDonNhap.Columns["ID_HoaDonNhap"] != null)
            {
                dgvHoaDonNhap.Columns["ID_HoaDonNhap"].Visible = false;
            }
            if (dgvHoaDonNhap.Columns["ID_HangHoa"] != null)
            {
                dgvHoaDonNhap.Columns["ID_HangHoa"].Visible = false;
            }
            // Tùy chỉnh giao diện cột
            dgvHoaDonNhap.Columns["STT"].Width = 50;
            dgvHoaDonNhap.Columns["Tên Hàng Hoá"].Width = 150;
            dgvHoaDonNhap.Columns["Số lượng"].Width = 100;
            dgvHoaDonNhap.Columns["Đơn giá"].Width = 150;
            dgvHoaDonNhap.Columns["Thành Tiền"].Width = 200;
        }

        public void HandleUpdate(string idHoaDonNhap, DateTime ngayBan, string tongtien, TextBox txtghiChu, ComboBox id_Nhanvien, ComboBox id_NhaCC, Action onSuccess)
        {
            DateTime ngayBann = ngayBan;
            //int TongTien = int.Parse(txtongTien.Text.Trim());
            string GhiChu = txtghiChu.Text.Trim();
            string idNhaVien = id_Nhanvien.SelectedValue?.ToString();
            string idNhacc = id_NhaCC.SelectedValue?.ToString();
            int TongTienHD = int.Parse(tongtien);

            if (ngayBann > DateTime.Now)
            {
                MessageBox.Show("Ngày bán không được lớn hơn ngày hiện tại!");
                return;
            }

            bool updated = _hoaDonNhapService.UpdateHoaDonNhap(idHoaDonNhap, ngayBann, TongTienHD, GhiChu, idNhaVien, idNhacc);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật hoá đơn !");
                onSuccess?.Invoke();
            }
        }

        public void HandleDelete(string idHoaDonNhap, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idHoaDonNhap))
            {
                MessageBox.Show("Vui lòng chọn hoá đơn nhập để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _hoaDonNhapService.DeleteHoaDonNhap(idHoaDonNhap);
                if (!deleted)
                {
                    MessageBox.Show("Đã xóa hoá đơn !");
                    onSuccess?.Invoke();
                }
            }
        }

        public void HandleInsertCTHD(TextBox idHoaDonNhap, TextBox idHangHoa, TextBox soLuong, TextBox DonGia, Action<string> onSuccess)
        {
            //int TongTien = int.Parse(txtongTien.Text.Trim());
            string Idhoadonnhap = idHoaDonNhap.Text.Trim();
            string Idhanghoa = idHangHoa.Text.Trim();
            int SoLuong = int.Parse(soLuong.Text.Trim());
            int donGia = int.Parse(DonGia.Text.Trim().Replace(",", "").Replace("٬", "").Replace(".", ""));

            string newId = _hoaDonNhapService.InsertCTHoaDonNhap(Idhoadonnhap, Idhanghoa, SoLuong, donGia);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm hoá đơn: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                idHoaDonNhap.Clear();
                idHangHoa.Clear();
                soLuong.Clear();
                DonGia.Clear();
                onSuccess?.Invoke(newId); // Gọi callback để cập nhật UI
            }
        }
        public void HandleUpdateCTHD(string idHoaDonNhap, string idHangHoa, TextBox soLuong, TextBox DonGia, Action onSuccess)
        {
            int SoLuong = int.Parse(soLuong.Text.Trim());
            int donGia = int.Parse(DonGia.Text.Trim());

            bool updated = _hoaDonNhapService.UpdateCTHoaDonNhap(idHoaDonNhap, idHangHoa, SoLuong, donGia);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật hoá đơn !");
                onSuccess?.Invoke();
            }
        }
    }
}
