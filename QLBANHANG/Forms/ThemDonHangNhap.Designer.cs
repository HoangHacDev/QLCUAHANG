namespace QLBANHANG.Forms
{
    partial class ThemDonHangNhap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DgvHoaDonNhap_1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtGiaNhap_1 = new System.Windows.Forms.TextBox();
            this.TxtSoLuong_1 = new System.Windows.Forms.TextBox();
            this.TxtMaHDN_1 = new System.Windows.Forms.TextBox();
            this.TxtMaHH_1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnThemCTHDN_1 = new System.Windows.Forms.Button();
            this.BtnThoatCTHDN_1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtTongTienHDN_1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.DgvHoaDonNhap_1)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvHoaDonNhap_1
            // 
            this.DgvHoaDonNhap_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvHoaDonNhap_1.Location = new System.Drawing.Point(12, 102);
            this.DgvHoaDonNhap_1.Name = "DgvHoaDonNhap_1";
            this.DgvHoaDonNhap_1.RowHeadersWidth = 51;
            this.DgvHoaDonNhap_1.RowTemplate.Height = 24;
            this.DgvHoaDonNhap_1.Size = new System.Drawing.Size(1034, 236);
            this.DgvHoaDonNhap_1.TabIndex = 0;
            this.DgvHoaDonNhap_1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvHoaDonNhap_1_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Số lượng :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(287, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Đơn Giá :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Mã HĐ Nhập :";
            // 
            // TxtGiaNhap_1
            // 
            this.TxtGiaNhap_1.Location = new System.Drawing.Point(390, 57);
            this.TxtGiaNhap_1.Name = "TxtGiaNhap_1";
            this.TxtGiaNhap_1.Size = new System.Drawing.Size(114, 22);
            this.TxtGiaNhap_1.TabIndex = 4;
            // 
            // TxtSoLuong_1
            // 
            this.TxtSoLuong_1.Location = new System.Drawing.Point(122, 58);
            this.TxtSoLuong_1.Name = "TxtSoLuong_1";
            this.TxtSoLuong_1.Size = new System.Drawing.Size(130, 22);
            this.TxtSoLuong_1.TabIndex = 5;
            this.TxtSoLuong_1.TextChanged += new System.EventHandler(this.TxtSoLuong_1_TextChanged_1);
            this.TxtSoLuong_1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSoLuong_1_KeyPress);
            // 
            // TxtMaHDN_1
            // 
            this.TxtMaHDN_1.Location = new System.Drawing.Point(122, 16);
            this.TxtMaHDN_1.Name = "TxtMaHDN_1";
            this.TxtMaHDN_1.Size = new System.Drawing.Size(130, 22);
            this.TxtMaHDN_1.TabIndex = 6;
            // 
            // TxtMaHH_1
            // 
            this.TxtMaHH_1.Location = new System.Drawing.Point(390, 15);
            this.TxtMaHH_1.Name = "TxtMaHH_1";
            this.TxtMaHH_1.Size = new System.Drawing.Size(114, 22);
            this.TxtMaHH_1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(287, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mã Hàng Hoá :";
            // 
            // BtnThemCTHDN_1
            // 
            this.BtnThemCTHDN_1.Location = new System.Drawing.Point(564, 14);
            this.BtnThemCTHDN_1.Name = "BtnThemCTHDN_1";
            this.BtnThemCTHDN_1.Size = new System.Drawing.Size(75, 23);
            this.BtnThemCTHDN_1.TabIndex = 9;
            this.BtnThemCTHDN_1.Text = "Thêm";
            this.BtnThemCTHDN_1.UseVisualStyleBackColor = true;
            this.BtnThemCTHDN_1.Click += new System.EventHandler(this.BtnThemCTHDN_1_Click);
            // 
            // BtnThoatCTHDN_1
            // 
            this.BtnThoatCTHDN_1.Location = new System.Drawing.Point(971, 15);
            this.BtnThoatCTHDN_1.Name = "BtnThoatCTHDN_1";
            this.BtnThoatCTHDN_1.Size = new System.Drawing.Size(75, 23);
            this.BtnThoatCTHDN_1.TabIndex = 12;
            this.BtnThoatCTHDN_1.Text = "Thoát";
            this.BtnThoatCTHDN_1.UseVisualStyleBackColor = true;
            this.BtnThoatCTHDN_1.Click += new System.EventHandler(this.BtnThoatCTHDN_1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(561, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tổng tiền :";
            // 
            // TxtTongTienHDN_1
            // 
            this.TxtTongTienHDN_1.Location = new System.Drawing.Point(664, 58);
            this.TxtTongTienHDN_1.Name = "TxtTongTienHDN_1";
            this.TxtTongTienHDN_1.Size = new System.Drawing.Size(382, 22);
            this.TxtTongTienHDN_1.TabIndex = 14;
            // 
            // ThemDonHangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 350);
            this.Controls.Add(this.TxtTongTienHDN_1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnThoatCTHDN_1);
            this.Controls.Add(this.BtnThemCTHDN_1);
            this.Controls.Add(this.TxtMaHH_1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtMaHDN_1);
            this.Controls.Add(this.TxtSoLuong_1);
            this.Controls.Add(this.TxtGiaNhap_1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DgvHoaDonNhap_1);
            this.Name = "ThemDonHangNhap";
            this.Text = "Nhập Hàng";
            this.Load += new System.EventHandler(this.ThemDonHangNhap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvHoaDonNhap_1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvHoaDonNhap_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtGiaNhap_1;
        private System.Windows.Forms.TextBox TxtSoLuong_1;
        private System.Windows.Forms.TextBox TxtMaHDN_1;
        private System.Windows.Forms.TextBox TxtMaHH_1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnThemCTHDN_1;
        private System.Windows.Forms.Button BtnThoatCTHDN_1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtTongTienHDN_1;
    }
}