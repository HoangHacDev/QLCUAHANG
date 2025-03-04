using QLBANHANG.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace QLBANHANG.Services
{
    public class NhanVienServices
    {
        // Gọi lớp kết nối SQL
        private readonly ConnectionString _connectionString;

        public NhanVienServices(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string InsertNhanVien(string tenNhanVien, DateTime NgaySinh, string GioiTinh, string diaChi, string SoDienThoai, string email)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhanVien_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@HoTen", tenNhanVien);
                        cmd.Parameters.AddWithValue("@NgaySinh", NgaySinh);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", SoDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);


                        // Lấy mã vừa tạo. convert to string
                        string newId = cmd.ExecuteScalar()?.ToString();
                        return newId; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm Nhân viên: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Lấy danh sách
        public List<NhanVienModel> GetNhanVien(string idNhanVien = null)
        {
            List<NhanVienModel> result = new List<NhanVienModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhanVien_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.Parameters.AddWithValue("@ID_NhanVien", (object)idNhanVien ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new NhanVienModel
                                {
                                    ID_NhanVien = reader["ID_NhanVien"].ToString(),
                                    TenNhanVien = reader["Hoten"].ToString(),
                                    NgaySinh = (DateTime)reader["NgaySinh"],
                                    GioiTinh = reader["GioiTinh"].ToString(),
                                    DiaChi = reader["DiaChi"].ToString(),
                                    SoDienThoai = reader["SoDienThoai"].ToString(),
                                    Email = reader["Email"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu Nhân viên: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }
        // Cập Nhật 
        public bool UpdateNhanVien(string idNhanVien, string tenNhanVien, DateTime NgaySinh, string GioiTinh, string diaChi, string SoDienThoai, string email)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhanVien_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
                        cmd.Parameters.AddWithValue("@HoTen", tenNhanVien);
                        cmd.Parameters.AddWithValue("@NgaySinh", NgaySinh);
                        cmd.Parameters.AddWithValue("@GioiTinh", GioiTinh);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", SoDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);


                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật Nhân viên " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Xóa 1
        public bool DeleteNhanVien(string idNhanVien)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhanVien_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu xóa thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa Nhân viên: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// LOGIN 
        public async Task<string> DangKyNhanVienVaUser(string tenNhanVien, DateTime ngaySinh, string gioiTinh, string diaChi, string soDienThoai, string email, string username, string password, string role = "Admin")
        {
            if (String.IsNullOrEmpty(tenNhanVien))
            {
                MessageBox.Show("Vui lòng nhập tên !");
                return null;
            }
            if (String.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản !");
                return null;
            }
            if (String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu !");
                return null;
            }
            List<TaiKhoanNhanVienModel> danhSachTaiKhoan = GetTaiKhoan();
            if (danhSachTaiKhoan.Any(tk => tk.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Username đã tồn tại! Vui lòng chọn username khác.");
                return null;
            }

            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    
                    // Bước 1: Tạo nhân viên và lấy mã nhân viên mới
                    string newIdNhanVien;
                    using (SqlCommand cmd = new SqlCommand("sp_NhanVien_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@HoTen", tenNhanVien);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);

                        newIdNhanVien = cmd.ExecuteScalar()?.ToString();
                        if (string.IsNullOrEmpty(newIdNhanVien))
                        {
                            MessageBox.Show("Không thể tạo nhân viên!");
                            return null;
                        }
                    }

                    // Bước 2: Đợi 1-2 giây
                    await Task.Delay(2000); // Đợi 1.5 giây (có thể điều chỉnh)

                    // Bước 3: Tạo user trong bảng login
                    using (SqlCommand cmd = new SqlCommand("sp_Login_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@ID_NhanVien", newIdNhanVien);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Role", role);

                        string newIdLogin = cmd.ExecuteScalar()?.ToString();
                        if (string.IsNullOrEmpty(newIdLogin))
                        {
                            MessageBox.Show("Không thể tạo user!");
                            return null;
                        }

                        return newIdLogin; // Trả về mã đăng nhập vừa tạo nếu cần
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public List<TaiKhoanNhanVienModel> GetTaiKhoan()
        {
            List<TaiKhoanNhanVienModel> result = new List<TaiKhoanNhanVienModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Login_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new TaiKhoanNhanVienModel
                                {
                                    ID_Login = reader["ID_Login"].ToString(),
                                    ID_NhanVien = reader["ID_NhanVien"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    Role = reader["Role"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu tài khoản Nhân viên: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public async Task<LoginModel> DangNhapAsync(string username, string password)
        {
            LoginModel result = new LoginModel();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null)
                {
                    result.Message = "Không thể kết nối đến database!";
                    return result;
                }

                try
                {
                    //await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_Login", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter roleParam = new SqlParameter("@Role", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                        SqlParameter idNhanVienParam = new SqlParameter("@ID_NhanVien", SqlDbType.VarChar, 6) { Direction = ParameterDirection.Output };

                        cmd.Parameters.Add(resultParam);
                        cmd.Parameters.Add(roleParam);
                        cmd.Parameters.Add(idNhanVienParam);

                        await cmd.ExecuteNonQueryAsync();

                        int loginResult = Convert.ToInt32(resultParam.Value ?? 0);
                        string role = roleParam.Value != DBNull.Value ? roleParam.Value.ToString() : null;
                        string idNhanVien = idNhanVienParam.Value != DBNull.Value ? idNhanVienParam.Value.ToString() : null;

                        if (loginResult == 1)
                        {
                            result.IsSuccess = true;
                            result.Role = role;
                            result.ID_NhanVien = idNhanVien;
                            result.Message = "Đăng nhập thành công!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Đăng nhập thất bại! Tên đăng nhập hoặc mật khẩu không đúng.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = "Lỗi khi đăng nhập: " + ex.Message;
                }
            }
            return result;
        }
    }
}
