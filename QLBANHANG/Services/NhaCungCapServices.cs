using QLBANHANG.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace QLBANHANG.Services
{
    public class NhaCungCapServices
    {
        private readonly ConnectionString _connectionString;

        public NhaCungCapServices(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }


        public string InsertNhaCungCap(string tenNhaCungCap, string diaChi, string SoDienThoai, string email)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@TenNhaCungCap", tenNhaCungCap);
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
                    MessageBox.Show("Lỗi khi thêm Nhà cung cấp: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Lấy danh sách
        public List<NhaCungCapModel> GetNhaCungCap(string idNhaCC = null)
        {
            List<NhaCungCapModel> result = new List<NhaCungCapModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.Parameters.AddWithValue("@ID_NhaCungCap", (object)idNhaCC ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new NhaCungCapModel
                                {
                                    ID_NhaCungCap = reader["ID_NhaCungCap"].ToString(),
                                    TenNhaCungCap = reader["TenNhaCungCap"].ToString(),
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
                    MessageBox.Show("Lỗi khi lấy dữ liệu Nhà cung cấp: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }
        // Cập Nhật 
        public bool UpdateNhaCungCap(string idNhaCC, string tenNhaCungCap, string diaChi, string SoDienThoai, string email)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@ID_NhaCungCap", idNhaCC);
                        cmd.Parameters.AddWithValue("@TenNhaCungCap", tenNhaCungCap);
                        cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                        cmd.Parameters.AddWithValue("@SoDienThoai", SoDienThoai);
                        cmd.Parameters.AddWithValue("@Email", email);


                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật Nhà cung cấp: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Xóa 1
        public bool DeleteNhaCungCap(string idNhaCC)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@ID_NhaCungCap", idNhaCC);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu xóa thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa Nhà cung cấp: " + ex.Message);
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
