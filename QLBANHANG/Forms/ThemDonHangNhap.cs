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

namespace QLBANHANG.Forms
{
    public partial class ThemDonHangNhap : Form
    {
        private readonly NhomHangServices _nhomhangService;
        private readonly NhomHangHandler _nhomhanghandler;

        private readonly HangHoaServices _hangHoaService;
        private readonly HangHoaHandler _hangHoahandler;

        private readonly HoaDonNhapService _hoaDonNhapService;
        private readonly HoaDonNhapHandler _hoaDonNhapHandler;

        private readonly DataGridView _parentGrid;
        private readonly string _idHoaDonNhap;
        public ThemDonHangNhap(string idHoaDonNhap, DataGridView parentGrid)
        {
            InitializeComponent();
            _idHoaDonNhap = idHoaDonNhap;
            ConnectionString connectionString = new ConnectionString();
            _nhomhangService = new NhomHangServices(connectionString);
            _nhomhanghandler = new NhomHangHandler(_nhomhangService);

            _hangHoaService = new HangHoaServices(connectionString);
            _hangHoahandler = new HangHoaHandler(_hangHoaService);

            _hoaDonNhapService = new HoaDonNhapService(connectionString);
            _hoaDonNhapHandler = new HoaDonNhapHandler(_hoaDonNhapService);

            _parentGrid = parentGrid;
        }

        private void ThemDonHangNhap_Load(object sender, EventArgs e)
        {
            TxtMaHDN_1.ReadOnly = true;
            TxtMaHDN_1.TabStop = false;
            TxtMaHDN_1.Enabled = false;

            TxtTongTienHDN_1.ReadOnly = true;
            TxtTongTienHDN_1.TabStop = false;
            TxtTongTienHDN_1.Enabled = false;

            TxtMaHH_1.ReadOnly = true;  // Ngăn người dùng gõ
            TxtMaHH_1.TabStop = false;  // Ngăn TextBox nhận focus bằng phím Tab
            TxtMaHH_1.Enabled = false;  // (Tùy chọn) Làm TextBox xám đi và không tương tác được

            TxtGiaNhap_1.ReadOnly = true;
            TxtGiaNhap_1.TabStop = false;
            TxtGiaNhap_1.Enabled = false;

            DgvHoaDonNhap_1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvHoaDonNhap_1.MultiSelect = false;
            DgvHoaDonNhap_1.AllowUserToAddRows = false;
            DgvHoaDonNhap_1.ReadOnly = true; // Ngăn chỉnh sửa toàn bộ DataGridView

            TxtMaHDN_1.Text = _idHoaDonNhap;
            TxtSoLuong_1.TextChanged += TxtSoLuong_1_TextChanged_1;

            LoadDSHangHoa();
        }

        private void LoadDSHangHoa()
        {
            _hangHoahandler.HandleLoadData(DgvHoaDonNhap_1);
        }

        private void DgvHoaDonNhap_1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHoaDonNhap_1.Rows.Count)
            {
                DgvHoaDonNhap_1.Rows[e.RowIndex].Selected = true;
                DataGridViewRow row = DgvHoaDonNhap_1.Rows[e.RowIndex];
                TxtMaHH_1.Text = _hangHoaService.GetHangHoa()[e.RowIndex].ID_HangHoa;
                TxtGiaNhap_1.Text = row.Cells["Giá bán"].Value?.ToString() ?? "";
                TxtTongTienHDN_1.Text = row.Cells["Giá bán"].Value?.ToString() ?? "";
                TxtSoLuong_1.Text = "1"; // Giả sử TxtSoLuong_1 là textbox số lượng
            }
        }


        private void TxtSoLuong_1_TextChanged_1(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        // Hàm tính tổng tiền
        private void TinhTongTien()
        {
            try
            {
                // Lấy giá trị từ các textbox
                decimal giaBan = string.IsNullOrEmpty(TxtGiaNhap_1.Text) ? 0 : Convert.ToDecimal(TxtGiaNhap_1.Text);
                int soLuong = string.IsNullOrEmpty(TxtSoLuong_1.Text) ? 0 : Convert.ToInt32(TxtSoLuong_1.Text);

                // Kiểm tra số âm (dự phòng nếu KeyPress bị bypass)
                if (soLuong < 0)
                {
                    MessageBox.Show("Số lượng không thể là số âm!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtSoLuong_1.Text = "0";
                    soLuong = 0;
                }

                // Lấy số lượng tồn từ DataGridView
                //int selectedRowIndex = DgvHoaDonNhap_1.SelectedRows.Count > 0 ? DgvHoaDonNhap_1.SelectedRows[0].Index : -1;
                //if (selectedRowIndex >= 0)
                //{
                //    int soLuongTon = Convert.ToInt32(DgvHoaDonNhap_1.Rows[selectedRowIndex].Cells["Số lượng"].Value);

                //    // Kiểm tra số lượng nhập có vượt quá số lượng tồn không
                //    if (soLuong > soLuongTon)
                //    {
                //        MessageBox.Show($"Số lượng nhập ({soLuong}) vượt quá số lượng tồn kho ({soLuongTon})!",
                //            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        TxtSoLuong_1.Text = soLuongTon.ToString(); // Đặt lại số lượng tối đa có thể nhập
                //        soLuong = soLuongTon;
                //    }
                //}

                // Nếu số lượng là 0 hoặc 1 thì giữ nguyên giá ban đầu
                if (soLuong == 0 || soLuong == 1)
                {
                    TxtTongTienHDN_1.Text = giaBan.ToString();
                }
                else
                {
                    // Tính tổng tiền = giá bán * số lượng
                    decimal tongTien = giaBan * soLuong;
                    TxtTongTienHDN_1.Text = tongTien.ToString("N0");
                }
            }
            catch (FormatException)
            {
                // Xử lý khi nhập sai định dạng số
                TxtTongTienHDN_1.Text = TxtGiaNhap_1.Text; // Giữ nguyên giá ban đầu nếu lỗi
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void TxtSoLuong_1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số và phím điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Chặn dấu trừ
            if (e.KeyChar == '-')
            {
                e.Handled = true;
            }
        }

        private void BtnThoatCTHDN_1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát Form nhập hoá đơn không?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnThemCTHDN_1_Click(object sender, EventArgs e)
        {
            _hoaDonNhapHandler.HandleInsertCTHD(TxtMaHDN_1, TxtMaHH_1, TxtSoLuong_1, TxtGiaNhap_1, newId =>
            {
                if (_parentGrid != null)
                {
                    _hoaDonNhapHandler.HandleLoadData(_parentGrid); // Load lại danh sách trên form chính
                }
                this.Close(); // Đóng form con
            });
        }
    }
}
