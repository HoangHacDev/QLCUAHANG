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
                                    TenNhanVien = reader["Hoten"].ToString(),
                                    TenKhachHang = reader["TenKhachHang"].ToString(),
                                    DiaChi = reader["DiaChi"].ToString(),
                                    SoDienThoai = reader["SoDienThoai"].ToString(),
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

        public bool UpdateHoaDonBan(string idHoaDon, string idNhanVien, string idKhachHang, DateTime ngayBan)
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


        public string InsertCTHoaDon(string idHoaDonBan, string idHangHoa, int soluong, int giaBan)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return null;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT_DETAIL");
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", idHoaDonBan);
                        cmd.Parameters.AddWithValue("@ID_HangHoa", idHangHoa);
                        cmd.Parameters.AddWithValue("@SoLuong", soluong);
                        cmd.Parameters.AddWithValue("@GiaBan", giaBan);

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
        public List<ChiTietHoaDonBanModel> GetCTHoaDon(string idHoaDon)
        {
            List<ChiTietHoaDonBanModel> result = new List<ChiTietHoaDonBanModel>();
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return result;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECT_DETAIL");
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", (object)idHoaDon ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new ChiTietHoaDonBanModel
                                {
                                    ID_HoaDonBan = reader["ID_HoaDonBan"].ToString(),
                                    ID_HangHoa = reader["ID_HangHoa"].ToString(),
                                    TenHangHoa = reader["TenHangHoa"].ToString(),
                                    SoLuong = (int)reader["SoLuong"],
                                    QuiCach = reader["QuiCach"].ToString(),
                                    GiaBan = (int)reader["GiaBan"],
                                    BaoHanh = reader["BaoHanh"].ToString(),
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

        public bool UpdateCTHoaDonBan(string idHoaDonBan, string idHanghoa, int soLuong, int donGia)
        {
            using (SqlConnection conn = _connectionString.KetNoiSQLServer())
            {
                if (conn == null) return false;

                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HoaDon_CRUD", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE_DETAIL");
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", idHoaDonBan);
                        cmd.Parameters.AddWithValue("@ID_HangHoa", idHanghoa);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@QuiCach", null);
                        cmd.Parameters.AddWithValue("@GiaBan", donGia);
                        cmd.Parameters.AddWithValue("@BaoHanh", null);


                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật Hàng hoá" + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool DaThuTienInsert(string idHoaDonBan, bool daThuTien)
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
                        cmd.Parameters.AddWithValue("@ID_HoaDonBan", idHoaDonBan);
                        cmd.Parameters.AddWithValue("@DaThuTien", daThuTien);

                        int rowsAffected = (int)cmd.ExecuteScalar();
                        return rowsAffected > 0; // Trả về true nếu cập nhật thành công
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi Cập nhật Hoá Đơn: " + ex.Message);
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
