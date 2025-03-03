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
    public partial class SuaHoaDon_MatHangForm : Form
    {
        private readonly DataGridView _parentGrid;
        private readonly string _idHoaDonBan;

        private readonly HoaDonServices _hoaDonService;
        private readonly HoaDonHandler _hoaDonHandler;

        private readonly HangHoaServices _hangHoaServices;

        public SuaHoaDon_MatHangForm(string idHoaDonBan, DataGridView parentGrid)
        {
            InitializeComponent();
            _parentGrid = parentGrid;
            _idHoaDonBan = idHoaDonBan;

            ConnectionString connectionString = new ConnectionString();
            _hoaDonService = new HoaDonServices(connectionString);
            _hoaDonHandler = new HoaDonHandler(_hoaDonService);

            _hangHoaServices = new HangHoaServices(connectionString);
        }

        private void SuaHoaDon_MatHangForm_Load(object sender, EventArgs e)
        {
            TxtMaHDN_1.Text = _idHoaDonBan;
            LoadDSCTHD();
            LockFields();
        }

        private void LoadDSCTHD()
        {
            _hoaDonHandler.HandleLoadDataCTHD(DgvHoaDonBan_1, _idHoaDonBan);

            DgvHoaDonBan_1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvHoaDonBan_1.MultiSelect = false;
            DgvHoaDonBan_1.AllowUserToAddRows = false;
            DgvHoaDonBan_1.ReadOnly = true; // Ngăn chỉnh sửa toàn bộ DataGridView

            TxtSoLuong_1.TextChanged += TxtSoLuong_1_TextChanged;
        }

        private void DgvHoaDonBan_1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHoaDonBan_1.Rows.Count)
            {
                DataGridViewRow row = DgvHoaDonBan_1.Rows[e.RowIndex];
                TxtMaHH_1.Text = _hoaDonService.GetCTHoaDon(_idHoaDonBan)[e.RowIndex].ID_HangHoa;
                TxtGiaNhap_1.Text = row.Cells["Giá bán"].Value?.ToString() ?? "";
                TxtTongTienHDN_1.Text = row.Cells["Giá bán"].Value?.ToString() ?? "";
                TxtSoLuong_1.Text = row.Cells["Số lượng"].Value?.ToString() ?? "";
            }
        }

        private void LockFields()
        {
            TxtMaHDN_1.ReadOnly = true;
            TxtMaHDN_1.TabStop = false;
            TxtMaHDN_1.Enabled = false;

            TxtMaHH_1.ReadOnly = true;
            TxtMaHH_1.TabStop = false;
            TxtMaHH_1.Enabled = false;

            TxtGiaNhap_1.ReadOnly = true;
            TxtGiaNhap_1.TabStop = false;
            TxtGiaNhap_1.Enabled = false;

            TxtTongTienHDN_1.ReadOnly = true;
            TxtTongTienHDN_1.TabStop = false;
            TxtTongTienHDN_1.Enabled = false;

        }

        private void BtnThoatCTHDN_1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát không?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void TxtSoLuong_1_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

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

                int? soLuongTon = 0;
                if (!string.IsNullOrEmpty(TxtMaHH_1.Text))
                {
                    var hangHoaList = _hangHoaServices.GetSoLuong(TxtMaHH_1.Text);
                    if (hangHoaList.Count > 0 && hangHoaList[0].SoLuong.HasValue)
                    {
                        soLuongTon = hangHoaList[0].SoLuong.Value;
                    }
                }

                if (soLuongTon.HasValue && soLuong > soLuongTon.Value)
                {
                    MessageBox.Show($"Số lượng nhập ({soLuong}) vượt quá số lượng tồn kho ({soLuongTon})!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtSoLuong_1.Text = soLuongTon.Value.ToString();
                    soLuong = soLuongTon.Value;
                }

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

        private void BtnSuaCTHDN_1_Click(object sender, EventArgs e)
        {
            _hoaDonHandler.HandleUpdateCTHD(_idHoaDonBan, TxtMaHH_1.Text, TxtSoLuong_1, TxtGiaNhap_1, () =>
            {
                if (_parentGrid != null)
                {
                    _hoaDonHandler.HandleLoadData(_parentGrid); // Load lại danh sách trên form chính
                }
                this.Close(); // Đóng form con
            });
        }
    }
}
