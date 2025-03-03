using QLBANHANG.Handlers;
using QLBANHANG.Models;
using QLBANHANG.Services;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace QLBANHANG
{
    public partial class QLDanhMucHDForm: Form
    {
        private readonly HoaDonServices _hoaDonService;
        private readonly HoaDonHandler _hoaDonHandler;

        public QLDanhMucHDForm()
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();
            _hoaDonService = new HoaDonServices(connectionString);
            _hoaDonHandler = new HoaDonHandler(_hoaDonService);
        }
        private void QLDanhMucHDForm_Load(object sender, EventArgs e)
        {
            Txt_TenNV_3.ReadOnly = true;
            Txt_TenNV_3.TabStop = false;
            Txt_TenNV_3.Enabled = false;

            Txt_TenKH_3.ReadOnly = true;
            Txt_TenKH_3.TabStop = false;
            Txt_TenKH_3.Enabled = false;

            Txt_DiaChiKH_3.ReadOnly = true;
            Txt_DiaChiKH_3.TabStop = false;
            Txt_DiaChiKH_3.Enabled = false;

            Txt_SDTKH_3.ReadOnly = true;
            Txt_SDTKH_3.TabStop = false;
            Txt_SDTKH_3.Enabled = false;

            Txt_TenMH_3.ReadOnly = true;
            Txt_TenMH_3.TabStop = false;
            Txt_TenMH_3.Enabled = false;


            Txt_TenMH_3.ReadOnly = true;
            Txt_TenMH_3.TabStop = false;
            Txt_TenMH_3.Enabled = false;
        }

        private void Btn_LoadDSHD_3_Click(object sender, System.EventArgs e)
        {
            _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3); // Load dữ liệu khi form mở
        }

        private void Btn_ThoatHD_3_Click(object sender, System.EventArgs e)
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

        private void Btn_ThemHD_3_Click(object sender, System.EventArgs e)
        {
            _hoaDonHandler.HandleInsert(Cb_MaNV_3, Cb_MaKH_3, DTP_NgayBan_3.Value, newId =>
            {
                _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3);
            });
        }

        private void LoadDanhSachNhomHang(ComboBox cboNhomHang)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboNhomHang == null)
                {
                    throw new ArgumentNullException(nameof(cboNhomHang), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<NhomHangModel> nhomHangs = _nhomhangService.GetNhomHang();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (nhomHangs == null || nhomHangs.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu nhóm hàng để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                cboNhomHang.DataSource = nhomHangs;           // Nguồn dữ liệu
                cboNhomHang.DisplayMember = "TenNhomHang";    // Hiển thị tên nhóm hàng
                cboNhomHang.ValueMember = "ID_NhomHang";      // Giá trị ẩn là ID_NhomHang
                cboNhomHang.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách nhóm hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhSachNhomHang(ComboBox cboNhomHang)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboNhomHang == null)
                {
                    throw new ArgumentNullException(nameof(cboNhomHang), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<NhomHangModel> nhomHangs = _nhomhangService.GetNhomHang();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (nhomHangs == null || nhomHangs.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu nhóm hàng để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                cboNhomHang.DataSource = nhomHangs;           // Nguồn dữ liệu
                cboNhomHang.DisplayMember = "TenNhomHang";    // Hiển thị tên nhóm hàng
                cboNhomHang.ValueMember = "ID_NhomHang";      // Giá trị ẩn là ID_NhomHang
                cboNhomHang.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách nhóm hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhSachNhomHang(ComboBox cboNhomHang)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboNhomHang == null)
                {
                    throw new ArgumentNullException(nameof(cboNhomHang), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<NhomHangModel> nhomHangs = _nhomhangService.GetNhomHang();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (nhomHangs == null || nhomHangs.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu nhóm hàng để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                cboNhomHang.DataSource = nhomHangs;           // Nguồn dữ liệu
                cboNhomHang.DisplayMember = "TenNhomHang";    // Hiển thị tên nhóm hàng
                cboNhomHang.ValueMember = "ID_NhomHang";      // Giá trị ẩn là ID_NhomHang
                cboNhomHang.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách nhóm hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }
}
