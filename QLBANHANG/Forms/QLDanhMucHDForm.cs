using QLBANHANG.Handlers;
using QLBANHANG.Models;
using QLBANHANG.Services;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using QLBANHANG.Forms;

namespace QLBANHANG
{
    public partial class QLDanhMucHDForm: Form
    {
        private readonly HoaDonServices _hoaDonService;
        private readonly HoaDonHandler _hoaDonHandler;

        private readonly NhanVienServices _nhanVienServices;
        private readonly KhachHangServices _khachHangServices;
        private readonly HangHoaServices _hangHoaServices;
        private bool isFieldsLocked = true; // Biến theo dõi trạng thái

        public QLDanhMucHDForm()
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();
            _hoaDonService = new HoaDonServices(connectionString);
            _hoaDonHandler = new HoaDonHandler(_hoaDonService);

            _nhanVienServices= new NhanVienServices(connectionString);
            _khachHangServices= new KhachHangServices(connectionString);
            _hangHoaServices = new HangHoaServices(connectionString);
        }
        private void QLDanhMucHDForm_Load(object sender, EventArgs e)
        {
            

            LoadDanhSachNhanVien(Cb_MaNV_3);
            LoadDanhSachKhachHang(Cb_MaKH_3);
            LoadDanhSachHangHoa(Cb_MaHH_3);
            LockFields_Form();
            LockFields();

            Dgv_HoaDon_3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Dgv_HoaDon_3.MultiSelect = false;
            Dgv_HoaDon_3.AllowUserToAddRows = false;
            Dgv_HoaDon_3.ReadOnly = true; // Ngăn chỉnh sửa toàn bộ DataGridView
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

        private void LoadDanhSachNhanVien(ComboBox cboNhanVien)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboNhanVien == null)
                {
                    throw new ArgumentNullException(nameof(cboNhanVien), "ComboBox không được null.");
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
                Cb_MaNV_3.DataSource = nhanViens;           // Nguồn dữ liệu
                Cb_MaNV_3.DisplayMember = "ID_NhanVien";    // Hiển thị tên nhóm hàng
                Cb_MaNV_3.ValueMember = "ID_NhanVien";      // Giá trị ẩn là ID_NhomHang
                Cb_MaNV_3.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhSachKhachHang(ComboBox cboKhachHang)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboKhachHang == null)
                {
                    throw new ArgumentNullException(nameof(cboKhachHang), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<KhachHangModel> khachHangs = _khachHangServices.GetKhachHang();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (khachHangs == null || khachHangs.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu khách hàng để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                Cb_MaKH_3.DataSource = khachHangs;           // Nguồn dữ liệu
                Cb_MaKH_3.DisplayMember = "ID_KhachHang";    // Hiển thị tên nhóm hàng
                Cb_MaKH_3.ValueMember = "ID_KhachHang";      // Giá trị ẩn là ID_NhomHang
                Cb_MaKH_3.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách nhóm hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachHangHoa(ComboBox cboHangHoa)
        {
            try
            {
                // Kiểm tra ComboBox có null không
                if (cboHangHoa == null)
                {
                    throw new ArgumentNullException(nameof(cboHangHoa), "ComboBox không được null.");
                }

                // Lấy danh sách nhóm hàng từ service
                List<HangHoaModel> hangHoas = _hangHoaServices.GetHangHoa();

                // Kiểm tra danh sách nhóm hàng có dữ liệu không
                if (hangHoas == null || hangHoas.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu hàng hoá để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Gán dữ liệu cho ComboBox
                Cb_MaHH_3.DataSource = hangHoas;           // Nguồn dữ liệu
                Cb_MaHH_3.DisplayMember = "ID_HangHoa";    // Hiển thị tên nhóm hàng
                Cb_MaHH_3.ValueMember = "ID_HangHoa";      // Giá trị ẩn là ID_NhomHang
                Cb_MaHH_3.SelectedIndex = -1;               // Không chọn mục nào mặc định
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                MessageBox.Show($"Lỗi khi tải danh sách hàng hoá: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_HoaDon_3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Dgv_HoaDon_3.Rows.Count)
            {
                DataGridViewRow row = Dgv_HoaDon_3.Rows[e.RowIndex];
                Txt_MaHD_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].ID_HoaDonBan;
                DTP_NgayBan_3.Text = row.Cells["Ngày bán"].Value?.ToString() ?? "";
                Cb_MaNV_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].ID_NhanVien;
                Txt_TenNV_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].TenNhanVien;
                Cb_MaKH_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].ID_KhachHang;
                Txt_TenKH_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].TenKhachHang;
                Txt_DiaChiKH_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].DiaChi;
                Txt_SDTKH_3.Text = _hoaDonService.GetHoaDon()[e.RowIndex].SoDienThoai;
            }
        }

        private void LockFields()
        {
            Cb_MaHH_3.Enabled = false;
            Cb_MaHH_3.TabStop = false;
            Btn_Them_CTHD_3.Enabled = false;
            TxtSoLuong_3.ReadOnly = true;
            TxtSoLuong_3.TabStop = false;
            TxtSoLuong_3.Enabled = false;
            isFieldsLocked = true;
        }

        private void UnlockFields()
        {
            Cb_MaHH_3.Enabled = true;
            Cb_MaHH_3.TabStop = true;
            Btn_Them_CTHD_3.Enabled = true;
            TxtSoLuong_3.ReadOnly = false;
            TxtSoLuong_3.TabStop = true;
            TxtSoLuong_3.Enabled = true;
            isFieldsLocked = false;
        }

        private void LockFields_Form()
        {
            Txt_MaHD_3.ReadOnly = true;
            Txt_MaHD_3.TabStop = false;
            Txt_MaHD_3.Enabled = false;

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

            Txt_Tong_3.ReadOnly = true;
            Txt_Tong_3.TabStop = false;
            Txt_Tong_3.Enabled = false;

            TxtGiaNhap_3.ReadOnly = true;
            TxtGiaNhap_3.TabStop = false;
            TxtGiaNhap_3.Enabled = false;

            Cb_MaKH_3.SelectedIndex = -1;
        }

        private void Btn_Them_CTHD_3_Click(object sender, EventArgs e)
        {
            if (Dgv_HoaDon_3.CurrentRow != null)
            {
                string id = Txt_MaHD_3.Text;
                _hoaDonHandler.HandleInsertCTHD(id, Cb_MaHH_3, Txt_SDTKH_3, TxtGiaNhap_3, newId =>
                {
                    _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3);
                });
            }
        }

        private void Btn_Status_3_Click(object sender, EventArgs e)
        {
            if (isFieldsLocked)
            {
                UnlockFields();
                Btn_Status_3.Text = "Khóa"; // Khi mở thì hiển thị "Khóa"
            }
            else
            {
                LockFields();
                Btn_Status_3.Text = "Mở"; // Khi khóa thì hiển thị "Mở"
            }
        }

        private void Btn_XoaHD_3_Click(object sender, EventArgs e)
        {
            if (Dgv_HoaDon_3.CurrentRow != null)
            {
                string id = Txt_MaHD_3.Text;
                _hoaDonHandler.HandleDelete(id, () =>
                {
                    _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3);
                    ClearDL();
                });
            }
        }

        private void Btn_SuaHD_3_Click(object sender, EventArgs e)
        {
            if (Dgv_HoaDon_3.CurrentRow != null)
            {
                string id = Txt_MaHD_3.Text;
                _hoaDonHandler.HandleUpdate(id,Cb_MaNV_3, Cb_MaKH_3  ,DTP_NgayBan_3.Value, () =>
                {
                    _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3);
                });
            }
        }

        private void ClearDL()
        {
            Cb_MaKH_3.SelectedIndex = -1;
            Cb_MaNV_3.SelectedIndex = -1;
            Txt_MaHD_3.Clear();
            DTP_NgayBan_3.Value = DateTime.Now;
            Txt_TenNV_3.Clear();
            Txt_TenKH_3.Clear();
            Txt_DiaChiKH_3.Clear();
            Txt_SDTKH_3.Clear();
        }

        private void Btn_XoaDL_3_Click(object sender, EventArgs e)
        {
            ClearDL();
        }

        private void Dgv_HoaDon_3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Dgv_HoaDon_3.Rows.Count)
            {
                // Lấy mã hóa đơn nhập từ hàng được nhấp đúp
                string idHoaDonBan = _hoaDonService.GetHoaDon()[e.RowIndex].ID_HoaDonBan;

                // Mở form mới và truyền mã hóa đơn 
                SuaHoaDon_MatHangForm themCTHD = new SuaHoaDon_MatHangForm(idHoaDonBan, Dgv_HoaDon_3);
                themCTHD.ShowDialog(); // Hiển thị form dưới dạng modal (hoặc Show() nếu không muốn modal)
            }
        }
    }
}
