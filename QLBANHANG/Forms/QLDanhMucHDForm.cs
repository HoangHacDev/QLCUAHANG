using QLBANHANG.Handlers;
using QLBANHANG.Services;
using System.Windows.Forms;

namespace QLBANHANG
{
    public partial class QLDanhMucHDForm: Form
    {
        private readonly HoaDonServices _hoaDonService;
        private readonly HoaDonHandler _hoaDonHandler;

        public QLDanhMucHDForm()
        {
            InitializeComponent();
            ConnectionString connectionString = new ConnectionString();
            _hoaDonService = new HoaDonServices(connectionString);
            _hoaDonHandler = new HoaDonHandler(_hoaDonService);
        }

        private void Btn_LoadDSHD_3_Click(object sender, System.EventArgs e)
        {
            _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3); // Load dữ liệu khi form mở
        }

        private void Btn_ThoatHD_3_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Bạn có chắc chắn muốn thoát không?",
            "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Btn_ThemHD_3_Click(object sender, System.EventArgs e)
        {
            _hoaDonHandler.HandleInsert(Cb_MaNV_3, Cb_MaKH_3, DTP_NgayBan_3.Value, newId =>
            {
                _hoaDonHandler.HandleLoadData(Dgv_HoaDon_3);
            });
        }
    }
}
