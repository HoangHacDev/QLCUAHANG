namespace QLBANHANG
{
    partial class QLDanhMucKHForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ReloadDSKHBtn = new System.Windows.Forms.Button();
            this.TxtMaKH = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtSDTKH = new System.Windows.Forms.TextBox();
            this.TxtDiaChiKH = new System.Windows.Forms.TextBox();
            this.BtnThoatFormQLKH = new System.Windows.Forms.Button();
            this.TxtTenKH = new System.Windows.Forms.TextBox();
            this.BtnHuyONhapLieu = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnXoaKH = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnSuaKH = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnThemKH = new System.Windows.Forms.Button();
            this.DgvKhachHang = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.TxtEmailKH = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvKhachHang)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(275, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bản Danh Mục Khách Hàng";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TxtEmailKH);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.ReloadDSKHBtn);
            this.groupBox1.Controls.Add(this.TxtMaKH);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TxtSDTKH);
            this.groupBox1.Controls.Add(this.TxtDiaChiKH);
            this.groupBox1.Controls.Add(this.BtnThoatFormQLKH);
            this.groupBox1.Controls.Add(this.TxtTenKH);
            this.groupBox1.Controls.Add(this.BtnHuyONhapLieu);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.BtnXoaKH);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.BtnSuaKH);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.BtnThemKH);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 47);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(809, 235);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin khách hàng";
            // 
            // ReloadDSKHBtn
            // 
            this.ReloadDSKHBtn.Location = new System.Drawing.Point(416, 164);
            this.ReloadDSKHBtn.Margin = new System.Windows.Forms.Padding(4);
            this.ReloadDSKHBtn.Name = "ReloadDSKHBtn";
            this.ReloadDSKHBtn.Size = new System.Drawing.Size(100, 28);
            this.ReloadDSKHBtn.TabIndex = 9;
            this.ReloadDSKHBtn.Text = "Nạp Lại";
            this.ReloadDSKHBtn.UseVisualStyleBackColor = true;
            this.ReloadDSKHBtn.Click += new System.EventHandler(this.BtnReloadKHBtn_Click);
            // 
            // TxtMaKH
            // 
            this.TxtMaKH.Location = new System.Drawing.Point(146, 26);
            this.TxtMaKH.Margin = new System.Windows.Forms.Padding(4);
            this.TxtMaKH.Name = "TxtMaKH";
            this.TxtMaKH.Size = new System.Drawing.Size(240, 23);
            this.TxtMaKH.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Mã Khách hàng :";
            // 
            // TxtSDTKH
            // 
            this.TxtSDTKH.Location = new System.Drawing.Point(146, 161);
            this.TxtSDTKH.Margin = new System.Windows.Forms.Padding(4);
            this.TxtSDTKH.Name = "TxtSDTKH";
            this.TxtSDTKH.Size = new System.Drawing.Size(240, 23);
            this.TxtSDTKH.TabIndex = 5;
            // 
            // TxtDiaChiKH
            // 
            this.TxtDiaChiKH.Location = new System.Drawing.Point(146, 114);
            this.TxtDiaChiKH.Margin = new System.Windows.Forms.Padding(4);
            this.TxtDiaChiKH.Name = "TxtDiaChiKH";
            this.TxtDiaChiKH.Size = new System.Drawing.Size(240, 23);
            this.TxtDiaChiKH.TabIndex = 4;
            // 
            // BtnThoatFormQLKH
            // 
            this.BtnThoatFormQLKH.Location = new System.Drawing.Point(701, 164);
            this.BtnThoatFormQLKH.Margin = new System.Windows.Forms.Padding(4);
            this.BtnThoatFormQLKH.Name = "BtnThoatFormQLKH";
            this.BtnThoatFormQLKH.Size = new System.Drawing.Size(100, 28);
            this.BtnThoatFormQLKH.TabIndex = 6;
            this.BtnThoatFormQLKH.Text = "Thoát";
            this.BtnThoatFormQLKH.UseVisualStyleBackColor = true;
            this.BtnThoatFormQLKH.Click += new System.EventHandler(this.BtnThoatFormQLKH_Click);
            // 
            // TxtTenKH
            // 
            this.TxtTenKH.Location = new System.Drawing.Point(146, 71);
            this.TxtTenKH.Margin = new System.Windows.Forms.Padding(4);
            this.TxtTenKH.Name = "TxtTenKH";
            this.TxtTenKH.Size = new System.Drawing.Size(240, 23);
            this.TxtTenKH.TabIndex = 3;
            // 
            // BtnHuyONhapLieu
            // 
            this.BtnHuyONhapLieu.Location = new System.Drawing.Point(547, 20);
            this.BtnHuyONhapLieu.Margin = new System.Windows.Forms.Padding(4);
            this.BtnHuyONhapLieu.Name = "BtnHuyONhapLieu";
            this.BtnHuyONhapLieu.Size = new System.Drawing.Size(100, 28);
            this.BtnHuyONhapLieu.TabIndex = 5;
            this.BtnHuyONhapLieu.Text = "Hủy";
            this.BtnHuyONhapLieu.UseVisualStyleBackColor = true;
            this.BtnHuyONhapLieu.Click += new System.EventHandler(this.BtnHuyONhapLieu_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 164);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Số Điện Thoại :";
            // 
            // BtnXoaKH
            // 
            this.BtnXoaKH.Location = new System.Drawing.Point(416, 117);
            this.BtnXoaKH.Margin = new System.Windows.Forms.Padding(4);
            this.BtnXoaKH.Name = "BtnXoaKH";
            this.BtnXoaKH.Size = new System.Drawing.Size(100, 28);
            this.BtnXoaKH.TabIndex = 4;
            this.BtnXoaKH.Text = "Xóa";
            this.BtnXoaKH.UseVisualStyleBackColor = true;
            this.BtnXoaKH.Click += new System.EventHandler(this.BtnXoaKH_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Địa Chỉ :";
            // 
            // BtnSuaKH
            // 
            this.BtnSuaKH.Location = new System.Drawing.Point(416, 68);
            this.BtnSuaKH.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSuaKH.Name = "BtnSuaKH";
            this.BtnSuaKH.Size = new System.Drawing.Size(100, 28);
            this.BtnSuaKH.TabIndex = 3;
            this.BtnSuaKH.Text = "Sửa";
            this.BtnSuaKH.UseVisualStyleBackColor = true;
            this.BtnSuaKH.Click += new System.EventHandler(this.BtnSuaKH_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tên Khách Hàng :";
            // 
            // BtnThemKH
            // 
            this.BtnThemKH.Location = new System.Drawing.Point(416, 21);
            this.BtnThemKH.Margin = new System.Windows.Forms.Padding(4);
            this.BtnThemKH.Name = "BtnThemKH";
            this.BtnThemKH.Size = new System.Drawing.Size(100, 28);
            this.BtnThemKH.TabIndex = 2;
            this.BtnThemKH.Text = "Thêm";
            this.BtnThemKH.UseVisualStyleBackColor = true;
            this.BtnThemKH.Click += new System.EventHandler(this.BtnThemKH_Click);
            // 
            // DgvKhachHang
            // 
            this.DgvKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvKhachHang.Location = new System.Drawing.Point(16, 340);
            this.DgvKhachHang.Margin = new System.Windows.Forms.Padding(4);
            this.DgvKhachHang.Name = "DgvKhachHang";
            this.DgvKhachHang.RowHeadersWidth = 51;
            this.DgvKhachHang.Size = new System.Drawing.Size(809, 267);
            this.DgvKhachHang.TabIndex = 0;
            this.DgvKhachHang.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvKhachHang_CellClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 293);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(675, 22);
            this.textBox1.TabIndex = 7;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(724, 290);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 28);
            this.button6.TabIndex = 8;
            this.button6.Text = "Tìm";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // TxtEmailKH
            // 
            this.TxtEmailKH.Location = new System.Drawing.Point(146, 204);
            this.TxtEmailKH.Margin = new System.Windows.Forms.Padding(4);
            this.TxtEmailKH.Name = "TxtEmailKH";
            this.TxtEmailKH.Size = new System.Drawing.Size(240, 23);
            this.TxtEmailKH.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 207);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Email :";
            // 
            // QLDanhMucKHForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 620);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.DgvKhachHang);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "QLDanhMucKHForm";
            this.Text = "DMKhachHangForm";
            this.Load += new System.EventHandler(this.QLDanhMucKHForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvKhachHang)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DgvKhachHang;
        private System.Windows.Forms.TextBox TxtSDTKH;
        private System.Windows.Forms.TextBox TxtDiaChiKH;
        private System.Windows.Forms.TextBox TxtTenKH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnThemKH;
        private System.Windows.Forms.Button BtnSuaKH;
        private System.Windows.Forms.Button BtnXoaKH;
        private System.Windows.Forms.Button BtnHuyONhapLieu;
        private System.Windows.Forms.Button BtnThoatFormQLKH;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox TxtMaKH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ReloadDSKHBtn;
        private System.Windows.Forms.TextBox TxtEmailKH;
        private System.Windows.Forms.Label label6;
    }
}