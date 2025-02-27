using QLBANHANG.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QLBANHANG.Services
{
    public class KhachHangServices
    {
        // Gọi lớp kết nối SQL
        private readonly ConnectionString _connectionString;

        public KhachHangServices(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }


        public string InsertkhachHang(string tenKhachHang, string diaChi, string SoDienThoai, string email)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_KhachHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@TenKhachHang", tenKhachHang);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", SoDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);


                        // Lấy mã vừa tạo. convert to string
                        string newId = cmd.ExecuteScalar()?.ToString();
                        return newId; // Trả về ID_NhomHang vừa được tạo (MHxxx)
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm Khách Hàng: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Lấy danh sách
        public List<KhachHangModel> GetKhachHang(string idKhacHang = null)
        {
            List<KhachHangModel> result = new List<KhachHangModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_KhachHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.Parameters.AddWithValue("@ID_KhachHang", (object)idKhacHang ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new KhachHangModel
                                {
                                    ID_KhachHang = reader["ID_KhachHang"].ToString(),
                                    TenKhachHang = reader["TenKhachHang"].ToString(),
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
        public bool UpdateKhachHang(string idKhachHang, string tenKhachHang,  string diaChi, string SoDienThoai, string email)
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
