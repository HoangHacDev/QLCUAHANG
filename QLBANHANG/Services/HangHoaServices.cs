using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using QLBANHANG.Models;

namespace QLBANHANG.Services
{
    public class HangHoaServices
    {
        private readonly ConnectionString _connectionString;

        public HangHoaServices(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string InsertHangHoa(string tenHangHoa, string idNhomHang, string mauSac, string kichThuoc,
                               string dacTinhKyThuat, string donViTinh, int? soLuong, int? giaBan, byte[] anh = null)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HangHoa_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@TenHangHoa", tenHangHoa);
                        cmd.Parameters.AddWithValue("@ID_NhomHang", (object)idNhomHang ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MauSac", (object)mauSac ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@KichThuoc", (object)kichThuoc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DacTinhKyThuat", (object)dacTinhKyThuat ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DonViTinh", (object)donViTinh ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoLuong", (object)soLuong ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GiaBan", (object)giaBan ?? DBNull.Value);

                        // Thêm tham số cho ảnh (varbinary(max))
                        if (anh != null)
                        {
                            cmd.Parameters.AddWithValue("@Anh", anh);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Anh", DBNull.Value);
                        }

                        string newId = cmd.ExecuteScalar()?.ToString();
                        return newId; // Trả về ID_HangHoa vừa tạo (HH001, HH002, ...)
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm hàng hóa: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public List<HangHoaModel> GetHangHoa(string idHangHoa = null)
        {
            List<HangHoaModel> result = new List<HangHoaModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HangHoa_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.Parameters.AddWithValue("@ID_HangHoa", (object)idHangHoa ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new HangHoaModel
                                {
                                    ID_HangHoa = reader["ID_HangHoa"].ToString(),
                                    TenHangHoa = reader["TenHangHoa"].ToString(),
                                    TenNhomHang = reader["TenNhomHang"].ToString(),
                                    MauSac = reader["MauSac"] != DBNull.Value ? reader["MauSac"].ToString() : null,
                                    KichThuoc = reader["KichThuoc"] != DBNull.Value ? reader["KichThuoc"].ToString() : null,
                                    DacTinhKyThuat = reader["DacTinhKyThuat"] != DBNull.Value ? reader["DacTinhKyThuat"].ToString() : null,
                                    DonViTinh = reader["DonViTinh"] != DBNull.Value ? reader["DonViTinh"].ToString() : null,
                                    SoLuong = reader["SoLuong"] != DBNull.Value ? (int?)reader["SoLuong"] : null,
                                    Anh = reader["Anh"] != DBNull.Value ? (byte[])reader["Anh"] : null,
                                    GiaBan = reader["GiaBan"] != DBNull.Value ? (int?)reader["GiaBan"] : null
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu hàng hóa: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public List<HangHoaModel> GetSoLuong(string idHangHoa = null)
        {
            List<HangHoaModel> result = new List<HangHoaModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HangHoa_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT_ONE");
                        cmd.Parameters.AddWithValue("@ID_HangHoa", (object)idHangHoa ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new HangHoaModel
                                {
                                    ID_HangHoa = reader["ID_HangHoa"].ToString(),
                                    SoLuong = reader["SoLuong"] != DBNull.Value ? (int?)reader["SoLuong"] : null,
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu hàng hóa: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public bool UpdateHangHoa(string idHangHoa, string tenHangHoa, string idNhomHang, string mauSac, string kichThuoc,
                              string dacTinhKyThuat, string donViTinh, int? soLuong, int? giaBan, byte[] anh)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HangHoa_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@ID_HangHoa", idHangHoa);
                        cmd.Parameters.AddWithValue("@TenHangHoa", tenHangHoa);
                        cmd.Parameters.AddWithValue("@ID_NhomHang", (object)idNhomHang ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MauSac", (object)mauSac ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@KichThuoc", (object)kichThuoc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DacTinhKyThuat", (object)dacTinhKyThuat ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DonViTinh", (object)donViTinh ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoLuong", (object)soLuong ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@GiaBan", (object)giaBan ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Anh", (object)anh ?? DBNull.Value);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật hàng hóa: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DeleteHangHoa(string idHangHoa)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HangHoa_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@ID_HangHoa", idHangHoa);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu xóa thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hàng hóa: " + ex.Message);
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
