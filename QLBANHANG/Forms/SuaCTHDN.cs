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
    public partial class SuaCTHDN: Form
    {
        private readonly DataGridView _parentGrid;
        private readonly string _idHoaDonNhap;
        private readonly string _idHangHoa;
        private readonly string _SoLuong;
        private readonly string _DonGia;
        private readonly string _ThanhTien;
        private readonly string _SOluongTonKho;


        private readonly HoaDonNhapService _hoaDonNhapService;
        private readonly HoaDonNhapHandler _hoaDonNhapHandler;
        public SuaCTHDN(string idHoaDonNhap,string idHangHoa , string soluong, string dongia, string thanhtien, string soluongtonkho,DataGridView parentGrid)
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();

            _hoaDonNhapService = new HoaDonNhapService(connectionString);
            _hoaDonNhapHandler = new HoaDonNhapHandler(_hoaDonNhapService);

            _idHoaDonNhap = idHoaDonNhap;
            _idHangHoa = idHangHoa;
            _SoLuong = soluong;
            _DonGia = dongia;
            _ThanhTien = thanhtien;
            _SOluongTonKho = soluongtonkho;
            _parentGrid = parentGrid;

            TxtMaHDN_2.Text = idHoaDonNhap;
            TxtMaHH_2.Text = idHangHoa;
            TxtSoLuong_1.Text = soluong;
            TxtGiaNhap_1.Text = dongia;
            TxtTongTienHDN_1.Text = thanhtien;
        }

        private void BtnSuaCTHDN_2_Click(object sender, EventArgs e)
        {
            _hoaDonNhapHandler.HandleUpdateCTHD(TxtMaHDN_2.Text, TxtMaHH_2.Text, TxtSoLuong_1, TxtGiaNhap_1, () =>
            {
                if (_parentGrid != null)
                {
                    _hoaDonNhapHandler.HandleLoadData(_parentGrid); // Load lại danh sách trên form chính
                }
                this.Close(); // Đóng form con
            });
        }

        private void SuaCTHDN_Load(object sender, EventArgs e)
        {
            TxtTongTienHDN_1.ReadOnly = true;
            TxtTongTienHDN_1.TabStop = false;
            TxtTongTienHDN_1.Enabled = false;

            TxtGiaNhap_1.ReadOnly = true;
            TxtGiaNhap_1.TabStop = false;
            TxtGiaNhap_1.Enabled = false;
        }

        private void BtnThoatCTHDN_1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát ?",
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

                // Lấy số lượng tồn từ DataGridView
                    int soLuongTon = Convert.ToInt32(_SOluongTonKho);

                    // Kiểm tra số lượng nhập có vượt quá số lượng tồn không
                    if (soLuong > soLuongTon)
                    {
                        MessageBox.Show($"Số lượng nhập ({soLuong}) vượt quá số lượng tồn kho ({soLuongTon})!",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtSoLuong_1.Text = soLuongTon.ToString(); // Đặt lại số lượng tối đa có thể nhập
                        soLuong = soLuongTon;
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
    }
}
