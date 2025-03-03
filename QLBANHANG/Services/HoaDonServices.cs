using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBANHANG.Models;

namespace QLBANHANG.Services
{
    public class HoaDonServices
    {
        private readonly ConnectionString _connectionString;

        public HoaDonServices(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string InsertHoaDon(string idNhanVien, string idKhachHang, DateTime ngayBan, bool daThuTien)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
                        cmd.Parameters.AddWithValue("@ID_KhachHang", idKhachHang);
                        cmd.Parameters.AddWithValue("@NgayBan", ngayBan);
                        cmd.Parameters.AddWithValue("@TongTien", 0);
                        cmd.Parameters.AddWithValue("@DaThuTien", daThuTien);

                        // Lấy mã vừa tạo. convert to string
                        string newId = cmd.ExecuteScalar()?.ToString();
                        return newId;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm Hoá Đơn: " + ex.Message);
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        // Lấy danh sách
        public List<HoaDonModel> GetHoaDon(string idHoaDon = null)
        {
            List<HoaDonModel> result = new List<HoaDonModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT_HOADON_ONLY");
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", (object)idHoaDon ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new HoaDonModel
                                {
                                    ID_HoaDonBan = reader["ID_HoaDonBan"].ToString(),
                                    ID_NhanVien = reader["ID_NhanVien"].ToString(),
                                    ID_KhachHang = reader["ID_KhachHang"].ToString(),
                                    NgayBan = (DateTime)reader["NgayBan"],
                                    TongTien = reader["TongTien"].ToString(),
                                    DaThuTien = (bool)reader["DaThuTien"],
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu Hoá đơn: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public bool UpdateHoaDonBan(string idHoaDon, string idNhanVien, string idKhachHang, DateTime ngayBan, int tongTien, string dathuTien)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", idHoaDon);
                        cmd.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
                        cmd.Parameters.AddWithValue("@ID_KhachHang", idKhachHang);
                        cmd.Parameters.AddWithValue("@NgayBan", ngayBan);
                        cmd.Parameters.AddWithValue("@TongTien", tongTien);
                        cmd.Parameters.AddWithValue("@DaThuTien", dathuTien);


                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật hoá đơn " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DeleteHoaDon(string idHoaDon)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", idHoaDon);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu xóa thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hoá đơn !: " + ex.Message);
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
