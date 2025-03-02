using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLBANHANG
{
    public class ConnectionString
    {
        private readonly string connectionString = "Server=MSI\\HOANGHACSQL;Database=QLCUAHANG;Trusted_Connection=yes;connection timeout=30;User Id=sa;Password=;";

        public SqlConnection KetNoiSQLServer()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return null;
            }
        }
    }
}
