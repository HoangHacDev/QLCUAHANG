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
            this.DgvHangHoa_1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtGiaNhap_1 = new System.Windows.Forms.TextBox();
            this.TxtSoLuong_1 = new System.Windows.Forms.TextBox();
            this.TxtMaHDN_1 = new System.Windows.Forms.TextBox();
            this.TxtMaHH_1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DgvHangHoa_1)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvHangHoa_1
            // 
            this.DgvHangHoa_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvHangHoa_1.Location = new System.Drawing.Point(12, 100);
            this.DgvHangHoa_1.Name = "DgvHangHoa_1";
            this.DgvHangHoa_1.RowHeadersWidth = 51;
            this.DgvHangHoa_1.RowTemplate.Height = 24;
            this.DgvHangHoa_1.Size = new System.Drawing.Size(597, 172);
            this.DgvHangHoa_1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(318, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Số lượng :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Giá nhập :";
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
            this.TxtGiaNhap_1.Location = new System.Drawing.Point(422, 54);
            this.TxtGiaNhap_1.Name = "TxtGiaNhap_1";
            this.TxtGiaNhap_1.Size = new System.Drawing.Size(162, 22);
            this.TxtGiaNhap_1.TabIndex = 4;
            // 
            // TxtSoLuong_1
            // 
            this.TxtSoLuong_1.Location = new System.Drawing.Point(422, 16);
            this.TxtSoLuong_1.Name = "TxtSoLuong_1";
            this.TxtSoLuong_1.Size = new System.Drawing.Size(162, 22);
            this.TxtSoLuong_1.TabIndex = 5;
            // 
            // TxtMaHDN_1
            // 
            this.TxtMaHDN_1.Location = new System.Drawing.Point(122, 16);
            this.TxtMaHDN_1.Name = "TxtMaHDN_1";
            this.TxtMaHDN_1.Size = new System.Drawing.Size(162, 22);
            this.TxtMaHDN_1.TabIndex = 6;
            // 
            // TxtMaHH_1
            // 
            this.TxtMaHH_1.Location = new System.Drawing.Point(122, 54);
            this.TxtMaHH_1.Name = "TxtMaHH_1";
            this.TxtMaHH_1.Size = new System.Drawing.Size(162, 22);
            this.TxtMaHH_1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mã Hàng Hoá :";
            // 
            // ThemDonHangNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 279);
            this.Controls.Add(this.TxtMaHH_1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtMaHDN_1);
            this.Controls.Add(this.TxtSoLuong_1);
            this.Controls.Add(this.TxtGiaNhap_1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DgvHangHoa_1);
            this.Name = "ThemDonHangNhap";
            this.Text = "Giao diện thêm đơn hàng cho hóa đơn nhập";
            ((System.ComponentModel.ISupportInitialize)(this.DgvHangHoa_1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvHangHoa_1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtGiaNhap_1;
        private System.Windows.Forms.TextBox TxtSoLuong_1;
        private System.Windows.Forms.TextBox TxtMaHDN_1;
        private System.Windows.Forms.TextBox TxtMaHH_1;
        private System.Windows.Forms.Label label4;
    }
}