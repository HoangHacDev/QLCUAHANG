using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using QLBANHANG.Models;

namespace QLBANHANG.Services
{
    public class NhomHangServices
    {
        // Gọi lớp kết nối SQL
        private readonly ConnectionString _connectionString;
        // Nhúng kết nối SQL vào contructor
        public NhomHangServices(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        //Thêm Nhóm Hàng
        public string InsertNhomHang(string tenNhomHang)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhomHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@TenNhomHang", tenNhomHang);

                        // Lấy mã vừa tạo. convert to string
                        string newId = cmd.ExecuteScalar()?.ToString();
                        return newId; // Trả về ID_NhomHang vừa được tạo (MHxxx)
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm nhóm hàng: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Lấy danh sách
        public List<NhomHangModel> GetNhomHang(string idNhomHang = null)
        {
            List<NhomHangModel> result = new List<NhomHangModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhomHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.Parameters.AddWithValue("@ID_NhomHang", (object)idNhomHang ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new NhomHangModel
                                {
                                    ID_NhomHang = reader["ID_NhomHang"].ToString(),
                                    TenNhomHang = reader["TenNhomHang"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu nhóm hàng: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }
        // Cập Nhật 
        public bool UpdateNhomHang(string idNhomHang, string tenNhomHang)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_NhomHang_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@ID_NhomHang", idNhomHang);
                        cmd.Parameters.AddWithValue("@TenNhomHang", tenNhomHang);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật nhóm hàng: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        // Xóa 1
        public bool DeleteNhomHang(string idNhomHang)
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
                        cmd.Parameters.AddWithValue("@ID_NhomHang", idNhomHang);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu xóa thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhóm hàng: " + ex.Message);
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
