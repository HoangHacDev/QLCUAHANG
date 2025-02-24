using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBANHANG.Handlers
{
    public class HangHoaHandler
    {
        private readonly HangHoaServices _hangHoaServices;
        //private readonly ConnectionString _connectionString;

        public HangHoaHandler(HangHoaServices hangHoaServices)
        {
            _hangHoaServices = hangHoaServices ?? throw new ArgumentNullException(nameof(hangHoaServices));
        }

        public void HandleInsert(TextBox txtTenHangHoa, ComboBox cboNhomHang, TextBox txtMauSac, TextBox txtKichThuoc,
                         TextBox txtDacTinhKyThuat, TextBox txtDonViTinh, NumericUpDown txtSoLuong,
                         TextBox txtGiaBan, PictureBox pictureBox, Action<string> onSuccess)
        {
            string tenHangHoa = txtTenHangHoa.Text.Trim();
            string idNhomHang = cboNhomHang.SelectedValue?.ToString();
            string mauSac = txtMauSac.Text.Trim();
            string kichThuoc = txtKichThuoc.Text.Trim();
            string dacTinhKyThuat = txtDacTinhKyThuat.Text.Trim();
            string donViTinh = txtDonViTinh.Text.Trim();
            string soLuongText = txtSoLuong.Text.Trim();
            string giaBanText = txtGiaBan.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(tenHangHoa))
            {
                MessageBox.Show("Vui lòng nhập tên hàng hóa!");
                return;
            }
            if (string.IsNullOrEmpty(idNhomHang))
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng!");
                return;
            }

            if (txtSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int? soLuong = null;
            if (!string.IsNullOrEmpty(soLuongText) && !int.TryParse(soLuongText, out _))
            {
                MessageBox.Show("Số lượng phải là số nguyên!");
                return;
            }
            else if (!string.IsNullOrEmpty(soLuongText))
            {
                soLuong = int.Parse(soLuongText);
            }

            if (!int.TryParse(txtGiaBan.Text.Replace(".", ""), out int giaBan) || giaBan <= 0)
            {
                MessageBox.Show("Giá bán phải là số nguyên lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //int? giaBan = null;
            //if (!string.IsNullOrEmpty(giaBanText) && !int.TryParse(giaBanText.Replace(".", ""), out _))
            //{
            //    MessageBox.Show("Giá bán phải là số nguyên!");
            //    return;
            //}
            //else if (!string.IsNullOrEmpty(giaBanText))
            //{
            //    giaBan = int.Parse(giaBanText.Replace(".", ""));
            //}

            // Xử lý ảnh từ PictureBox
            byte[] anh = null;
            if (pictureBox.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                    anh = ms.ToArray();
                }
            }else
            {
                MessageBox.Show("Vui lòng chọn hình ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                string newId = _hangHoaServices.InsertHangHoa(tenHangHoa, idNhomHang, mauSac, kichThuoc, dacTinhKyThuat,
                                                              donViTinh, soLuong, giaBan, anh);
            if (newId != null)
            {
                MessageBox.Show($"Đã thêm hàng hóa: {newId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenHangHoa.Clear();
                cboNhomHang.SelectedIndex = -1;
                txtMauSac.Clear();
                txtKichThuoc.Clear();
                txtDacTinhKyThuat.Clear();
                txtDonViTinh.Clear();
                txtSoLuong.Value = 0;
                txtGiaBan.Clear();
                pictureBox.Image = null;
                onSuccess?.Invoke(newId);
            }
        }

        public void HandleLoadData(DataGridView dgvHangHoa, ComboBox cboNhomHang = null)
        {
            List<HangHoaModel> hangHoas = _hangHoaServices.GetHangHoa();
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID_HangHoa", typeof(string));
            dt.Columns.Add("Tên hàng hóa", typeof(string));
            dt.Columns.Add("Nhóm hàng", typeof(string));
            dt.Columns.Add("Màu sắc", typeof(string));
            dt.Columns.Add("Kích thước", typeof(string));
            dt.Columns.Add("Đặc tính kỹ thuật", typeof(string));
            dt.Columns.Add("Đơn vị tính", typeof(string));
            dt.Columns.Add("Số lượng", typeof(string));
            dt.Columns.Add("Giá bán", typeof(string));
            dt.Columns.Add("Ảnh", typeof(byte[]));

            for (int i = 0; i < hangHoas.Count; i++)
            {
                string giaBanFormatted = hangHoas[i].GiaBan.HasValue ? hangHoas[i].GiaBan.Value.ToString("N0") : "";
                dt.Rows.Add(i + 1, hangHoas[i].ID_HangHoa, hangHoas[i].TenHangHoa, hangHoas[i].TenNhomHang,
                            hangHoas[i].MauSac, hangHoas[i].KichThuoc, hangHoas[i].DacTinhKyThuat, hangHoas[i].DonViTinh,
                            hangHoas[i].SoLuong?.ToString(), giaBanFormatted, hangHoas[i].Anh);
            }

            dgvHangHoa.DataSource = dt;
            dgvHangHoa.Columns["ID_HangHoa"].Visible = false; // Ẩn cột ID
            dgvHangHoa.Columns["Ảnh"].Visible = false; // Ẩn cột ảnh
            dgvHangHoa.Columns["STT"].Width = 50;
            dgvHangHoa.Columns["Tên hàng hóa"].Width = 150;
            dgvHangHoa.Columns["Nhóm hàng"].Width = 100;
            dgvHangHoa.Columns["Màu sắc"].Width = 50;
            dgvHangHoa.Columns["Kích thước"].Width = 50;
            dgvHangHoa.Columns["Đặc tính kỹ thuật"].Width = 150;
            dgvHangHoa.Columns["Đơn vị tính"].Width = 50;
            dgvHangHoa.Columns["Số lượng"].Width = 80;
        }

        public void HandleUpdate(string idHangHoa, TextBox txtTenHangHoa, ComboBox cboNhomHang, TextBox txtMauSac,
                             TextBox txtKichThuoc, TextBox txtDacTinhKyThuat, TextBox txtDonViTinh, NumericUpDown txtSoLuong,
                             TextBox txtGiaBan, PictureBox pictureBox, Action onSuccess)
        {
            string tenHangHoa = txtTenHangHoa.Text.Trim();
            string idNhomHang = cboNhomHang.SelectedValue?.ToString();
            string mauSac = txtMauSac.Text.Trim();
            string kichThuoc = txtKichThuoc.Text.Trim();
            string dacTinhKyThuat = txtDacTinhKyThuat.Text.Trim();
            string donViTinh = txtDonViTinh.Text.Trim();
            string soLuongText = txtSoLuong.Text.Trim();
            string giaBanText = txtGiaBan.Text.Trim();

            if (string.IsNullOrEmpty(tenHangHoa))
            {
                MessageBox.Show("Vui lòng nhập tên hàng hóa!");
                return;
            }

            if (string.IsNullOrEmpty(idNhomHang))
            {
                MessageBox.Show("Vui lòng chọn nhóm hàng!");
                return;
            }

            int? soLuong = null;
            if (!string.IsNullOrEmpty(soLuongText) && !int.TryParse(soLuongText, out _))
            {
                MessageBox.Show("Số lượng phải là số nguyên!");
                return;
            }
            else if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!string.IsNullOrEmpty(soLuongText))
            {
                soLuong = int.Parse(soLuongText);
            }

            int? giaBan = null;
            if (!string.IsNullOrEmpty(giaBanText) && !int.TryParse(giaBanText.Replace(".", ""), out _))
            {
                MessageBox.Show("Giá bán phải là số nguyên!");
                return;
            }
            else if (giaBan <= 0)
            {
                MessageBox.Show("Giá bán phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!string.IsNullOrEmpty(giaBanText))
            {
                giaBan = int.Parse(giaBanText.Replace(".", ""));
            }

            byte[] anh = null;
            if (pictureBox.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                    anh = ms.ToArray();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hình ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool updated = _hangHoaServices.UpdateHangHoa(idHangHoa, tenHangHoa, idNhomHang, mauSac, kichThuoc,
                                                          dacTinhKyThuat, donViTinh, soLuong, giaBan , anh);
            if (updated)
            {
                MessageBox.Show("Đã cập nhật hàng hóa!");
                onSuccess?.Invoke();
            }
        }

        public void HandleDelete(string idHangHoa, Action onSuccess)
        {
            if (string.IsNullOrEmpty(idHangHoa))
            {
                MessageBox.Show("Vui lòng chọn hàng hóa để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa hàng hóa này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool deleted = _hangHoaServices.DeleteHangHoa(idHangHoa);
                if (deleted)
                {
                    MessageBox.Show("Đã xóa hàng hóa!");
                    onSuccess?.Invoke();
                }
            }
        }
    }
}
