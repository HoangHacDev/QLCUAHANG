using QLBANHANG.Handlers;
using QLBANHANG.Models;
using QLBANHANG.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QLBANHANG
{
    public partial class QLDanhMucMHForm: Form
    {
        private readonly NhomHangServices _nhomhangService;
        private readonly NhomHangHandler _nhomhanghandler;

        private readonly HangHoaServices _hangHoaService;
        private readonly HangHoaHandler _hangHoahandler;

        public QLDanhMucMHForm()
        {
            InitializeComponent();

            // Khởi tạo connection, service và handler
            ConnectionString connectionString = new ConnectionString();
            _nhomhangService = new NhomHangServices(connectionString);
            _nhomhanghandler = new NhomHangHandler(_nhomhangService);

            _hangHoaService = new HangHoaServices(connectionString);
            _hangHoahandler = new HangHoaHandler(_hangHoaService);
        }

        private void QLDanhMucMHForm_Load(object sender, EventArgs e)
        {
            TxtID_NhomHang.ReadOnly = true;
            TxtID_NhomHang.TabStop = false;
            TxtID_NhomHang.Enabled = false;

            TxtID_HangHoa.ReadOnly = true;  // Ngăn người dùng gõ
            TxtID_HangHoa.TabStop = false;  // Ngăn TextBox nhận focus bằng phím Tab
            TxtID_HangHoa.Enabled = false;  // (Tùy chọn) Làm TextBox xám đi và không tương tác được

            LoadDanhSachNhomHang(CbNhomHang);
        }

        // Load danh sách lên dgv
        private void BtnLoadMatHang_Click(object sender, EventArgs e)
        {
            _nhomhanghandler.HandleLoadData(DgvNhomHang); // Load dữ liệu khi form mở
        }

        // Thêm 1 Nhóm hàng
        private void BtnThemLMH_Click(object sender, EventArgs e)
        {
            _nhomhanghandler.HandleInsert(TxtTenNhomHang, newId =>
            {
                _nhomhanghandler.HandleLoadData(DgvNhomHang);
            });
        }

        // Sửa 1 nhóm hàng
        private void BtnSuaLMH_Click(object sender, EventArgs e)
        {
            if (DgvNhomHang.CurrentRow != null)
            {
                // Lấy ID từ dữ liệu gốc (nếu cần giữ ID trong logic)
                //string id = _nhomhangService.GetNhomHang()[DgvNhomHang.CurrentRow.Index].ID_NhomHang;
                string id = TxtID_NhomHang.Text;
                string ten = TxtTenNhomHang.Text.Trim();
                _nhomhanghandler.HandleUpdate(id, ten, () =>
                {
                    _nhomhanghandler.HandleLoadData(DgvNhomHang);
                });
            }
        }

        // Xóa 1 nhóm hàng
        private void BtnXoaLMH_Click(object sender, EventArgs e)
        {
            if (DgvNhomHang.CurrentRow != null)
            {
                // Lấy ID từ dữ liệu gốc
                //string id = _nhomhangService.GetNhomHang()[DgvNhomHang.CurrentRow.Index].ID_NhomHang;
                string id = TxtID_NhomHang.Text;
                _nhomhanghandler.HandleDelete(id, () =>
                {
                    _nhomhanghandler.HandleLoadData(DgvNhomHang);
                });
            }
        }

        // SK Click 1 hàng gửi lên textbox
        private void DgvNhomHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có click vào dòng hợp lệ không (tránh header hoặc ngoài dữ liệu)
            if (e.RowIndex >= 0 && e.RowIndex < DgvNhomHang.Rows.Count)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow row = DgvNhomHang.Rows[e.RowIndex];
                string tenNhomHang = row.Cells["Tên nhóm hàng"].Value.ToString();
                // Điền dữ liệu vào TextBox để người dùng chỉnh sửa nếu cần
               
                // Lấy ID từ danh sách gốc (vì ID_NhomHang bị ẩn trên grid)
                string idNhomHang = _nhomhangService.GetNhomHang()[e.RowIndex].ID_NhomHang;

                TxtID_NhomHang.Text = idNhomHang;
                TxtTenNhomHang.Text = tenNhomHang;
            }
        }

        private void BtnXoaTxt_Click(object sender, EventArgs e)
        {
            TxtID_NhomHang.Clear();
            TxtTenNhomHang.Clear();
        }

        private void BtnThoatForm_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát Quản lý Danh Mục Hàng Hoá không?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        ////////////////////////////// DANH MUC HANG HOA
        
        private void BtnThoatForm2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát Quản lý Danh Mục Hàng Hoá không?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnLoadDSHangHoa_Click(object sender, EventArgs e)
        {
            _hangHoahandler.HandleLoadData(DgvHangHoa, CbNhomHang);
        }

        private void BtnThemHangHoa_Click(object sender, EventArgs e)
        {
            _hangHoahandler.HandleInsert(TxtTenHangHoa, CbNhomHang, TxtMauSac, TxtKichThuoc, TxtDacTinhKyThuat,
                          TxtDVTinh, NumricSoLuong, TxtGiaBan, PTBHanghoa, newId =>
                          {
                              _hangHoahandler.HandleLoadData(DgvHangHoa); // Cập nhật danh sách sau khi thêm
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

        private void BtnReloadTxtMH_Click(object sender, EventArgs e)
        {
            ClearValues();
        }

        private void ClearValues()
        {
            TxtID_HangHoa.Clear();
            TxtTenHangHoa.Clear();
            TxtMauSac.Clear();
            TxtDacTinhKyThuat.Clear();
            TxtGiaBan.Clear();
            TxtKichThuoc.Clear();
            TxtDVTinh.Clear();
            NumricSoLuong.Value = 0;
            CbNhomHang.SelectedIndex = -1;
            PTBHanghoa.Image = null;
        }

        private void BtnChonHinh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    PTBHanghoa.Image = Image.FromFile(ofd.FileName);

                    // Thêm dòng này để set chế độ hiển thị
                    PTBHanghoa.SizeMode = PictureBoxSizeMode.Zoom; // Hoặc StretchImage
                }
            }
        }

        private void BtnBoChonHInh_Click(object sender, EventArgs e)
        {
            PTBHanghoa.Image = null;
        }

        private void DgvHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvHangHoa.Rows.Count)
            {
                DataGridViewRow row = DgvHangHoa.Rows[e.RowIndex];
                TxtID_HangHoa.Text = row.Cells["ID_HangHoa"].Value.ToString();
                TxtTenHangHoa.Text = row.Cells["Tên hàng hóa"].Value.ToString();
                // Điền dữ liệu vào ComboBox (cboNhomHang)

                if (row.Cells["Nhóm hàng"].Value != null)
                {
                    string tenNhomHang = row.Cells["Nhóm hàng"].Value.ToString();
                    foreach (var item in CbNhomHang.Items)
                    {
                        if (item.GetType().GetProperty(CbNhomHang.DisplayMember)?.GetValue(item)?.ToString() == tenNhomHang)
                        {
                            CbNhomHang.SelectedItem = item; // Chọn giá trị phù hợp trong ComboBox
                            break;
                        }
                    }
                }
                else
                {
                    CbNhomHang.SelectedIndex = -1; // Nếu không có giá trị, đặt ComboBox về trạng thái rỗng
                }
                TxtMauSac.Text = row.Cells["Màu sắc"].Value?.ToString() ?? "";
                TxtKichThuoc.Text = row.Cells["Kích thước"].Value?.ToString() ?? "";
                TxtDacTinhKyThuat.Text = row.Cells["Đặc tính kỹ thuật"].Value?.ToString() ?? "";
                TxtDVTinh.Text = row.Cells["Đơn vị tính"].Value?.ToString() ?? "";

                // Điền dữ liệu vào NumericUpDown (NumricSoLuong)
                if (int.TryParse(row.Cells["Số lượng"].Value?.ToString(), out int soLuong))
                {
                    // Kiểm tra giá trị có nằm trong phạm vi Minimum và Maximum của NumericUpDown không
                    if (soLuong >= NumricSoLuong.Minimum && soLuong <= NumricSoLuong.Maximum)
                    {
                        NumricSoLuong.Value = soLuong; // Gán giá trị số lượng vào NumericUpDown
                    }
                    else
                    {
                        // Nếu giá trị nằm ngoài phạm vi, đặt giá trị mặc định (ví dụ: Minimum)
                        NumricSoLuong.Value = NumricSoLuong.Minimum;
                        MessageBox.Show("Giá trị số lượng nằm ngoài phạm vi cho phép!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    NumricSoLuong.Value = NumricSoLuong.Minimum; // Nếu không hợp lệ, đặt giá trị mặc định là Minimum
                }

                if (row.Cells["Giá bán"].Value != null)
                {
                    string giaBanText = row.Cells["Giá bán"].Value.ToString();

                    // Loại bỏ dấu phân cách (nếu có) để chuyển về dạng số nguyên
                    giaBanText = giaBanText.Replace(".", "").Replace(",", ""); // Loại bỏ cả dấu chấm và dấu phẩy
                    TxtGiaBan.Text = giaBanText; // Hiển thị giá trị số nguyên
                }
                else
                {
                    TxtGiaBan.Text = ""; // Nếu không có giá trị, đặt TextBox về trạng thái rỗng
                }

                if (row.Cells["Ảnh"].Value != null && row.Cells["Ảnh"].Value is byte[] anhBytes)
                {
                    using (MemoryStream ms = new MemoryStream(anhBytes))
                    {
                        PTBHanghoa.Image = Image.FromStream(ms); // Hiển thị ảnh lên PictureBox
                        PTBHanghoa.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                else
                {
                    PTBHanghoa.Image = null; // Nếu không có ảnh, đặt PictureBox về trạng thái rỗng
                }
            }
        }

        private void BtnSuaHangHoa_Click(object sender, EventArgs e)
        {
            if (DgvHangHoa.CurrentRow != null)
            {
                string id = TxtID_HangHoa.Text;
                _hangHoahandler.HandleUpdate(id, TxtTenHangHoa, CbNhomHang, TxtMauSac, TxtKichThuoc, TxtDacTinhKyThuat,
                                      TxtDVTinh, NumricSoLuong, TxtGiaBan, PTBHanghoa, () =>
                                      {
                                          _hangHoahandler.HandleLoadData(DgvHangHoa);
                                      });
            }
        }

        private void BtnXoaHangHoa_Click(object sender, EventArgs e)
        {
            if (DgvHangHoa.CurrentRow != null)
            {
                string id = TxtID_HangHoa.Text;
                _hangHoahandler.HandleDelete(id, () =>
                {
                    _hangHoahandler.HandleLoadData(DgvHangHoa);
                    ClearValues();
                });
            }
        }
    }
}
