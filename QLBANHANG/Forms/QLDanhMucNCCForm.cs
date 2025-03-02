using QLBANHANG.Handlers;
using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QLBANHANG.Forms
{
    public partial class QLDanhMucNCCForm: Form
    {
        private readonly NhaCungCapServices _nhaCungCapServices;
        private readonly HoaDonNhapService _hoaDonNhapService;

        private readonly NhanVienServices _nhanVienServices;

        private readonly HangHoaServices _hangHoaServices;

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

            _hangHoaServices = new HangHoaServices(connectionString);
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
                try
                {
                    // Lấy danh sách nhà cung cấp
                    var nhaCungCapList = _nhaCungCapServices.GetNhaCungCap();

                    // Kiểm tra xem danh sách có đủ phần tử không
                    if (nhaCungCapList != null && e.RowIndex < nhaCungCapList.Count)
                    {
                        // Lấy dữ liệu từ dòng được chọn
                        DataGridViewRow row = DgvNhaCungCap.Rows[e.RowIndex];
                        TxtMaNCC.Text = nhaCungCapList[e.RowIndex].ID_NhaCungCap;
                        TxtTenNCC.Text = row.Cells["Tên nhà cung cấp"].Value?.ToString() ?? "";
                        TxtDiaChiNCC.Text = row.Cells["Địa chỉ"].Value?.ToString() ?? "";
                        TxtSDTNCC.Text = row.Cells["Số điện thoại"].Value?.ToString() ?? "";
                        TxtEmailNCC.Text = row.Cells["Email"].Value?.ToString() ?? "";
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu nhà cung cấp không đồng bộ với bảng hiển thị!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
        }

        private void QLDanhMucNCCForm_Load(object sender, EventArgs e)
        {
            TxtMaNCC.ReadOnly = true;
            TxtMaNCC.TabStop = false;
            TxtMaNCC.Enabled = false;

            TxtMaHDN.ReadOnly = true;
            TxtMaHDN.TabStop = false;
            TxtMaHDN.Enabled = false;

            TxtTongTien_HDN.ReadOnly = true;
            TxtTongTien_HDN.TabStop = false;
            TxtTongTien_HDN.Enabled = false;

            LoadDanhSachNhanVienCB(CbNhanVien_HDN);
            LoadDanhSachNhaCungCapCB(CbNCC_HDN);

            DgvHoaDonNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvHoaDonNhap.MultiSelect = false;
            DgvHoaDonNhap.AllowUserToAddRows = false;
            DgvHoaDonNhap.ReadOnly = true; // Ngăn chỉnh sửa toàn bộ DataGridView
            DgvHoaDonNhap.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue; // Màu xanh tùy chỉnh
            DgvHoaDonNhap.DefaultCellStyle.SelectionForeColor = Color.White;      // Màu chữ khi chọn

            DgvHH_HD.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DgvHH_HD.MultiSelect = false;
            DgvHH_HD.AllowUserToAddRows = false;
            DgvHH_HD.ReadOnly = true; // Ngăn chỉnh sửa toàn bộ DataGridView
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TxtSDTNCC_HD_TextChanged(object sender, EventArgs e)
        {

        }
        //------------------------------------------------------------------

        private void BtnLoadHDN(object sender, EventArgs e)
        {
            _hoaDonNhapHandler.HandleLoadData(DgvHoaDonNhap); // Load dữ liệu khi form mở
        }

        private void DgvHoaDonNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHoaDonNhap.Rows.Count)
            {
                // Chọn toàn bộ hàng
                DgvHoaDonNhap.Rows[e.RowIndex].Selected = true;

                DataGridViewRow row = DgvHoaDonNhap.Rows[e.RowIndex];
                string idHoaDonNhap = _hoaDonNhapService.GetHoaDon()[e.RowIndex].ID_HoaDonNhap;
                TxtMaHDN.Text = _hoaDonNhapService.GetHoaDon()[e.RowIndex].ID_HoaDonNhap; ;
                TxtGhiChu_DHN.Text = row.Cells["Ghi Chú"].Value?.ToString() ?? "";
                DTPNgayLapHD.Text = row.Cells["Ngày nhập"].Value?.ToString() ?? "";
                TxtTongTien_HDN.Text = row.Cells["Tổng tiền"].Value?.ToString() ?? "";

                // Kiểm tra cột "Tên nhân viên" tồn tại và có giá trị
                if (row.Cells["Tên nhân viên"].Value != null)
                {
                    string tenNhanvien = row.Cells["Tên nhân viên"].Value.ToString();
                    foreach (var item in CbNhanVien_HDN.Items)
                    {
                        //Console.WriteLine(item.ToString());
                        string itemValue = item.GetType().GetProperty(CbNhanVien_HDN.DisplayMember)?.GetValue(item)?.ToString();
                        if (itemValue == tenNhanvien)
                        {
                            CbNhanVien_HDN.SelectedItem = item;
                            break;
                        }

                    }
                }
                else
                {
                    CbNhanVien_HDN.SelectedIndex = -1; // Đặt về rỗng nếu không có giá trị
                }

                if (row.Cells["Tên nhà cung cấp"].Value != null)
                {
                    string tenNhaCC = row.Cells["Tên nhà cung cấp"].Value.ToString();
                    foreach (var item in CbNCC_HDN.Items)
                    {
                        if (item.GetType().GetProperty(CbNCC_HDN.DisplayMember)?.GetValue(item)?.ToString() == tenNhaCC)
                        {
                            CbNCC_HDN.SelectedItem = item; // Chọn giá trị phù hợp trong ComboBox
                            break;
                        }
                    }
                }
                else
                {
                    CbNCC_HDN.SelectedIndex = -1; // Nếu không có giá trị, đặt ComboBox về trạng thái rỗng
                }

                LoadChiTietHoaDonNhap(idHoaDonNhap);
            }
        }

        private void LoadChiTietHoaDonNhap(string idHoaDonNhap)
        {
            // Lấy danh sách chi tiết hóa đơn từ GetCTHoaDon
            List<ChiTietHoaDonNhapModel> chiTietList = _hoaDonNhapService.GetCTHoaDon(idHoaDonNhap);

            // Gán dữ liệu trực tiếp vào DgvHH_HD
            DgvHH_HD.DataSource = chiTietList;
            // Ẩn cột ID_HangHoa và ID_HoaDonNhap
            if (DgvHH_HD.Columns["ID_HangHoa"] != null)
            {
                DgvHH_HD.Columns["ID_HangHoa"].Visible = false;
            }
            if (DgvHH_HD.Columns["ID_HoaDonNhap"] != null)
            {
                DgvHH_HD.Columns["ID_HoaDonNhap"].Visible = false;
            }

            // Thêm cột STT nếu chưa có
            if (!DgvHH_HD.Columns.Contains("STT"))
            {
                DataGridViewTextBoxColumn sttColumn = new DataGridViewTextBoxColumn
                {
                    Name = "STT",
                    HeaderText = "STT",
                    Width = 50 // Chiều rộng cố định cho cột STT
                };
                DgvHH_HD.Columns.Insert(0, sttColumn); // Thêm cột STT vào vị trí đầu tiên
            }

            // Điền số thứ tự
            for (int i = 0; i < DgvHH_HD.Rows.Count; i++)
            {
                DgvHH_HD.Rows[i].Cells["STT"].Value = i + 1;
            }

            if (DgvHH_HD.Columns["TenHanghoa"] != null && DgvHH_HD.Columns["SoLuong"] != null && DgvHH_HD.Columns["DonGia"] != null && DgvHH_HD.Columns["ThanhTien"] != null)
            {
                DgvHH_HD.Columns["TenHanghoa"].HeaderText = "Tên Hàng Hoá";
                DgvHH_HD.Columns["SoLuong"].HeaderText = "Số Lượng";
                DgvHH_HD.Columns["DonGia"].HeaderText = "Đơn Giá";
                DgvHH_HD.Columns["ThanhTien"].HeaderText = "Thành Tiền";

            }

            // Chỉnh các cột để lấp đầy chiều rộng
            DgvHH_HD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // (Tùy chọn) Giữ cột STT cố định chiều rộng nếu cần
            DgvHH_HD.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvHH_HD.Columns["STT"].Width = 50; // Đảm bảo cột STT không bị co giãn
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
                cboNhomHang.DisplayMember = "TenNhanVien";          // Hiển thị tên 
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

        private void BtnXoaHD_NCC_Click(object sender, EventArgs e)
        {
            if (DgvHoaDonNhap.CurrentRow != null)
            {
                string id = TxtMaHDN.Text;
                _hoaDonNhapHandler.HandleDelete(id, () =>
                {
                    _hoaDonNhapHandler.HandleLoadData(DgvHoaDonNhap);
                    ClearDL();
                });
            }
        }

        private void BtnSuaHD_NCC_Click(object sender, EventArgs e)
        {
            if (DgvHoaDonNhap.CurrentRow != null)
            {
                string id = TxtMaHDN.Text;
                string tongtien = TxtTongTien_HDN.Text;
                _hoaDonNhapHandler.HandleUpdate(id, DTPNgayLapHD.Value, tongtien, TxtGhiChu_DHN, CbNhanVien_HDN, CbNCC_HDN, () =>
                {
                    _hoaDonNhapHandler.HandleLoadData(DgvHoaDonNhap);
                });
            }
        }

        private void BtnHuyODL_HDN_Click(object sender, EventArgs e)
        {
            CbNCC_HDN.SelectedIndex = -1;
            CbNhanVien_HDN.SelectedIndex = -1;
            TxtGhiChu_DHN.Clear();
            DTPNgayLapHD.Value = DateTime.Now;
            TxtMaHDN.Clear();
        }

        private void ClearDL()
        {
            CbNCC_HDN.SelectedIndex = -1;
            CbNhanVien_HDN.SelectedIndex = -1;
            TxtGhiChu_DHN.Clear();
            DTPNgayLapHD.Value = DateTime.Now;
            TxtMaHDN.Clear();
        }

        private void BtnThoatFormNH_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
          "Bạn có chắc chắn muốn thoát QL Hoá đơn nhập không?",
          "Xác nhận thoát",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void DgvHoaDonNhap_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHoaDonNhap.Rows.Count)
            {
                // Lấy mã hóa đơn nhập từ hàng được nhấp đúp
                string idHoaDonNhap = _hoaDonNhapService.GetHoaDon()[e.RowIndex].ID_HoaDonNhap;

                // Mở form mới và truyền mã hóa đơn nhập
                ThemDonHangNhap themDonHangNhap = new ThemDonHangNhap(idHoaDonNhap, DgvHoaDonNhap);
                themDonHangNhap.ShowDialog(); // Hiển thị form dưới dạng modal (hoặc Show() nếu không muốn modal)
            }
        }

        private void DgvHH_HD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHH_HD.Rows.Count)
            {
                // Lấy dòng được chọn trong DataGridView
                DataGridViewRow selectedRow = DgvHH_HD.Rows[e.RowIndex];
                // Lấy ID_HoaDonNhap và ID_HangHoa từ dữ liệu của dòng được chọn
                string idHoaDonNhap = selectedRow.Cells["ID_HoaDonNhap"].Value?.ToString();
                string idHangHoa = selectedRow.Cells["ID_HangHoa"].Value?.ToString();
                string soluongHD = selectedRow.Cells["SoLuong"].Value?.ToString();
                string dongia = selectedRow.Cells["DonGia"].Value?.ToString();
                string thanhtien = selectedRow.Cells["ThanhTien"].Value?.ToString();

                // Lấy số lượng tồn kho từ database
                string soLuongTonKho = "0"; // Giá trị mặc định
                List<HangHoaModel> hangHoaList = _hangHoaServices.GetSoLuong(idHangHoa);
                if (hangHoaList.Count > 0 && hangHoaList[0].SoLuong.HasValue)
                {
                    soLuongTonKho = hangHoaList[0].SoLuong.Value.ToString();
                }

                // Kiểm tra xem giá trị có tồn tại không trước khi sử dụng
                if (!string.IsNullOrEmpty(idHoaDonNhap) && !string.IsNullOrEmpty(idHangHoa))
                {
                    // Truyền dữ liệu vào form SuaCTHDN 
                    SuaCTHDN themDonHangNhap = new SuaCTHDN(idHoaDonNhap, idHangHoa, soluongHD, dongia, thanhtien, soLuongTonKho, DgvHH_HD);
                    themDonHangNhap.ShowDialog();
                }
                else
                {
                    Console.WriteLine("Không tìm thấy ID_HoaDonNhap hoặc ID_HangHoa.");
                }
            }
        }
    }
}
