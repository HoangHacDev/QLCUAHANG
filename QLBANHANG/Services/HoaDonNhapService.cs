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
    public class HoaDonNhapService
    {
        private readonly ConnectionString _connectionString;

        public HoaDonNhapService(ConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string InsertHoaDonNhap( DateTime ngayBan, string ghiChu, string idNhanVien, string idNhaCC)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDonNhap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@NgayNhap", ngayBan);
                        cmd.Parameters.AddWithValue("@TongTien", 0);
                        cmd.Parameters.AddWithValue("@GhiChu", ghiChu);
                        cmd.Parameters.AddWithValue("@ID_NhanVien", idNhanVien);
                        cmd.Parameters.AddWithValue("@ID_NhaCungCap", idNhaCC);

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
        public List<HoaDonNhapModel> GetHoaDon(string idHoaDonNhap = null)
        {
            List<HoaDonNhapModel> result = new List<HoaDonNhapModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDonNhap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT");
                        cmd.Parameters.AddWithValue("@ID_HoaDonNhap", (object)idHoaDonNhap ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new HoaDonNhapModel
                                {
                                    ID_HoaDonNhap = reader["ID_HoaDonNhap"].ToString(),
                                    NgayNhap = (DateTime)reader["NgayNhap"],
                                    TongTien = reader["TongTien"].ToString(),
                                    GhiChu = reader["GhiChu"].ToString(),
                                    TenNhanVien = reader["HoTen"].ToString(),
                                    TenNhaCungCap = reader["TenNhaCungCap"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu Hoá đơn nhập: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        public List<ChiTietHoaDonNhapModel> GetCTHoaDon(string idHoaDonNhap = null)
        {
            List<ChiTietHoaDonNhapModel> result = new List<ChiTietHoaDonNhapModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDonNhap_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT_DETAIL");
                        cmd.Parameters.AddWithValue("@ID_HoaDonNhap", (object)idHoaDonNhap ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new ChiTietHoaDonNhapModel
                                {
                                    ID_HoaDonNhap = reader["ID_HoaDonNhap"].ToString(),
                                    ID_HangHoa = reader["ID_HangHoa"].ToString(),
                                    TenHanghoa = reader["TenHanghoa"].ToString(),
                                    SoLuong = (int)(reader["SoLuong"] != DBNull.Value ? (int?)reader["SoLuong"] : null),
                                    DonGia = (int)(reader["DonGia"] != DBNull.Value ? (int?)reader["DonGia"] : null),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu Chi tiết Hoá đơn nhập: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }
    }
}
