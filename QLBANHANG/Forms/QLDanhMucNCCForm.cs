using QLBANHANG.Handlers;
using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QLBANHANG.Forms
{
    public partial class QLDanhMucNCCForm: Form
    {
        private readonly NhaCungCapServices _nhaCungCapServices;
        private readonly HoaDonNhapService _hoaDonNhapService;

        private readonly NhanVienServices _nhanVienServices;

        private readonly NhaCungCapHandler _nhaCungCapHandler;
        private readonly HoaDonNhapHandler _hoaDonNhapHandler;
        public QLDanhMucNCCForm()
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();
            _nhaCungCapServices = new NhaCungCapServices(connectionString);
            _nhaCungCapHandler = new NhaCungCapHandler(_nhaCungCapServices);

            _hoaDonNhapService = new HoaDonNhapService(connectionString);
            _hoaDonNhapHandler = new HoaDonNhapHandler(_hoaDonNhapService);

            _nhanVienServices = new NhanVienServices(connectionString);

        }

        private void BtnThemNCC_Click(object sender, EventArgs e)
        {
            _nhaCungCapHandler.HandleInsert(TxtTenNCC, TxtDiaChiNCC, TxtSDTNCC, TxtEmailNCC, newId =>
            {
                _nhaCungCapHandler.HandleLoadData(DgvNhaCungCap);
            });
        }

        private void BtnSuaNCC_Click(object sender, EventArgs e)
        {
            if (DgvNhaCungCap.CurrentRow != null)
            {
                string id = TxtMaNCC.Text;
                _nhaCungCapHandler.HandleUpdate(id, TxtTenNCC, TxtDiaChiNCC, TxtSDTNCC, TxtEmailNCC, () =>
                {
                    _nhaCungCapHandler.HandleLoadData(DgvNhaCungCap);
                });
            }
        }

        private void BtnXoaNCC_Click(object sender, EventArgs e)
        {
            if (DgvNhaCungCap.CurrentRow != null)
            {
                string id = TxtMaNCC.Text;
                _nhaCungCapHandler.HandleDelete(id, () =>
                {
                    _nhaCungCapHandler.HandleLoadData(DgvNhaCungCap);
                });
            }
        }

        private void BtnHuyODuLieuNCC_Click(object sender, EventArgs e)
        {
            TxtMaNCC.Clear();
            TxtTenNCC.Clear();
            TxtDiaChiNCC.Clear();
            TxtSDTNCC.Clear();
            TxtEmailNCC.Clear();
        }

        private void BtnLoadDSNCC_Click(object sender, EventArgs e)
        {
            _nhaCungCapHandler.HandleLoadData(DgvNhaCungCap); // Load dữ liệu khi form mở
        }

        private void BtnThoatFormNCC_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát Quản lý Nhà cung cấp không?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void DgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có click vào dòng hợp lệ không (tránh header hoặc ngoài dữ liệu)
            if (e.RowIndex >= 0 && e.RowIndex < DgvNhaCungCap.Rows.Count)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DgvNhaCungCap.Rows[e.RowIndex];
                TxtMaNCC.Text = _nhaCungCapServices.GetNhaCungCap()[e.RowIndex].ID_NhaCungCap;
                TxtTenNCC.Text = row.Cells["Tên nhà cung cấp"].Value?.ToString() ?? "";
                TxtDiaChiNCC.Text = row.Cells["Địa chỉ"].Value?.ToString() ?? "";
                TxtSDTNCC.Text = row.Cells["Số điện thoại"].Value?.ToString() ?? "";
                TxtEmailNCC.Text = row.Cells["Email"].Value?.ToString() ?? "";
            }
        }

        private void QLDanhMucNCCForm_Load(object sender, EventArgs e)
        {
            TxtMaNCC.ReadOnly = true;
            TxtMaNCC.TabStop = false;
            TxtMaNCC.Enabled = false;

            LoadDanhSachNhanVienCB(CbNhanVien_HDN);
            LoadDanhSachNhaCungCapCB(CbNCC_HDN);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TxtSDTNCC_HD_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnLoadHDN(object sender, EventArgs e)
        {
            _hoaDonNhapHandler.HandleLoadData(DgvHoaDonNhap); // Load dữ liệu khi form mở
        }

        private void DgvHoaDonNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHoaDonNhap.Rows.Count)
            {
                string idHoaDonNhap = _hoaDonNhapService.GetHoaDon()[e.RowIndex].ID_HoaDonNhap;
                LoadChiTietHoaDonNhap(idHoaDonNhap);
            }
        }

        private void LoadChiTietHoaDonNhap(string idHoaDonNhap)
        {
            // Lấy danh sách chi tiết hóa đơn từ GetCTHoaDon
            List<ChiTietHoaDonNhapModel> chiTietList = _hoaDonNhapService.GetCTHoaDon(idHoaDonNhap);

            // Gán dữ liệu trực tiếp vào DgvHH_HD
            DgvHH_HD.DataSource = chiTietList;
        }

        private void LoadDanhSachNhanVienCB(ComboBox cboNhomHang)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboNhomHang == null)
                {
                    throw new ArgumentNullException(nameof(cboNhomHang), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<NhanVienModel> nhanViens = _nhanVienServices.GetNhanVien();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (nhanViens == null || nhanViens.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu nhân viên để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                cboNhomHang.DataSource = nhanViens;           // Nguồn dữ liệu
                cboNhomHang.DisplayMember = "HoTen";    // Hiển thị tên 
                cboNhomHang.ValueMember = "ID_NhanVien";      // Giá trị ẩn là ID_NhomHang
                cboNhomHang.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachNhaCungCapCB(ComboBox cboNhaCC)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboNhaCC == null)
                {
                    throw new ArgumentNullException(nameof(cboNhaCC), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<NhaCungCapModel> NhaCungCaps = _nhaCungCapServices.GetNhaCungCap();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (NhaCungCaps == null || NhaCungCaps.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu Nhà cung cấp để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                cboNhaCC.DataSource = NhaCungCaps;           // Nguồn dữ liệu
                cboNhaCC.DisplayMember = "TenNhaCungCap";    // Hiển thị tên 
                cboNhaCC.ValueMember = "ID_NhaCungCap";      // Giá trị ẩn là ID_NhomHang
                cboNhaCC.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách Nhà cung cấp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemHD_NCC_Click(object sender, EventArgs e)
        {
            _hoaDonNhapHandler.HandleInsert(DTPNgayLapHD.Value , TxtGhiChu_DHN, CbNhanVien_HDN, CbNCC_HDN, newId =>
            {
                _hoaDonNhapHandler.HandleLoadData(DgvHoaDonNhap);
            });
        }
        //------------------------------------------------------------------
    }
}
