﻿using QLBANHANG.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBANHANG
{
    public partial class GiaoDienChinh: Form
    {
        public GiaoDienChinh()
        {
            InitializeComponent();
        }

        private void PhanLoaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLDanhMucMHForm hangHoaForm = new QLDanhMucMHForm();
            hangHoaForm.Show();
        }

        private void thốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemDSNhanVien_Click(object sender, EventArgs e)
        {
            QLDanhMucNVForm qLDanhMucNVForm = new QLDanhMucNVForm();
            qLDanhMucNVForm.Show();
        }

        private void ToolStripMenuItemPhanQuyen_Click(object sender, EventArgs e)
        {
            QLQuyenNVForm qLQuyenNVForm = new QLQuyenNVForm();
            qLQuyenNVForm.Show();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLDanhMucKHForm qLDanhMucKHForm = new QLDanhMucKHForm();
            qLDanhMucKHForm.Show();
        }

        private void hàngHóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLDanhMucNCCForm danhMucNCCForm = new QLDanhMucNCCForm();
            danhMucNCCForm.Show();
        }

        private void hoáĐơnNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //QLDanhMucNCCForm danhMucNCCForm = new QLDanhMucNCCForm();
            //danhMucNCCForm.Show();
        }
    }
}
