using QLBANHANG.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                        cmd.Parameters.AddWithValue("@HoTen", GioiTinh);
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
                                    Hoten = reader["Hoten"].ToString(),
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
                    MessageBox.Show("Lỗi khi lấy dữ liệu khách hàng: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }
        // Cập Nhật 
        public bool UpdateKhachHang(string idKhachHang, string tenKhachHang, string diaChi, string SoDienThoai, string email)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_KhachHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@ID_KhachHang", idKhachHang);
                        cmd.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", SoDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);


                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật khách hàng " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Xóa 1
        public bool DeleteKhachHang(string idKhachHang)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhomHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@ID_KhachHang", idKhachHang);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu xóa thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
