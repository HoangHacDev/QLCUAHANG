using QLBANHANG.Models;
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
    public partial class ĐangNhapForm : Form
    {
        private bool isExpanded = false;
        private int originalHeight;
        private Timer expandTimer;
        private int targetHeight;

        private readonly NhanVienServices _nhanVienServices;

        public ĐangNhapForm()
        {
            InitializeComponent();

            originalHeight = this.Height;
            expandTimer = new Timer();
            expandTimer.Interval = 10; // Khoảng thời gian giữa các lần cập nhật (10ms)
            expandTimer.Tick += ExpandTimer_Tick;

            ConnectionString connectionString = new ConnectionString();
            _nhanVienServices = new NhanVienServices(connectionString);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (isExpanded)
            {
                targetHeight = originalHeight;
                isExpanded = false;
            }
            else
            {
                targetHeight = originalHeight + 280;
                isExpanded = true;
            }

            expandTimer.Start();
        }

        private void ExpandTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height < targetHeight)
            {
                this.Height += 10; // Tăng chiều cao 10 pixel mỗi lần
            }
            else if (this.Height > targetHeight)
            {
                this.Height -= 10;
            }
            else
            {
                expandTimer.Stop();
            }
        }

        private void BtnDangKyUser_Click(object sender, EventArgs e)
        {
            // Chạy tác vụ bất đồng bộ mà không chặn UI
            Task.Run(async () =>
            {
                await HandleDangKyAsync();
            });
        }

        private async Task HandleDangKyAsync()
        {
            string tenNhanVien = txtTenNhanVien.Text;
            DateTime ngaySinh = dtpNgaySinh.Value;
            string gioiTinh = cboGioiTinh.Text;
            string diaChi = txtDiaChi.Text;
            string soDienThoai = txtSoDienThoai.Text;
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            string result = await _nhanVienServices.DangKyNhanVienVaUser(
                tenNhanVien, ngaySinh, gioiTinh, diaChi, soDienThoai, email, username, password);

            if (result != null)
            {
                // Vì đây là tác vụ chạy trên thread khác, cần invoke về UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("Đăng ký thành công!");
                });
            }
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            // Chạy tác vụ bất đồng bộ mà không chặn UI
            Task.Run(async () =>
            {
                await HandleDangNhapAsync();
            });
        }

        private async Task HandleDangNhapAsync()
        {
            string username = Txt_Username.Text;
            string password = Txt_Password.Text;

            LoginModel result = await _nhanVienServices.DangNhapAsync(username, password);

            if (result.IsSuccess)
            {
                // Dùng Invoke để thao tác với UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show($"Đăng nhập thành công! Role: {result.Role}, ID Nhân viên: {result.ID_NhanVien}");

                    // Tạo instance của form chính (giả sử tên là MainForm)
                    GiaoDienChinh giaoDienChinh = new GiaoDienChinh();

                    // Hiển thị form chính
                    giaoDienChinh.Show();

                    // Ẩn form đăng nhập hiện tại
                    this.Hide();
                    // Hoặc đóng form đăng nhập hoàn toàn: this.Close();
                });

            }
            else
            {
                // Dùng Invoke cho trường hợp lỗi
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show(result.Message);
                });
            }
        }
    }
}
