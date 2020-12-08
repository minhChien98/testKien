using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace BaiKiemTra3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con;

        private void Form1_Load(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["QLSP"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();



            var query = "SELECT * from LoaiSanPham";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cbnLoaiSP.DataSource = dt.DefaultView;
            cbnLoaiSP.DisplayMember = "tenLoai";
            cbnLoaiSP.ValueMember = "maLoai";



            HienThi();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
        }

        public void HienThi()
        {
            string SqlSELECT = "SELECT maSP, tenSP, tenLoai, soLuong, donGia, 'Xóa' as [Xóa], 'Sửa' as [Sửa] FROM SanPham JOIN LoaiSanPham ON LoaiSanPham.maLoai = SanPham.loaiSP";
            SqlCommand cmd = new SqlCommand(SqlSELECT, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            DsSanPham.DataSource = dt;
            for (int i = 0; i < DsSanPham.Rows.Count; i++)
            {
                DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                DataGridViewLinkCell linkCell1 = new DataGridViewLinkCell();
                DsSanPham[5, i] = linkCell;
                DsSanPham[6, i] = linkCell1;
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            double donGia;
            try
            {
                donGia = Convert.ToDouble(txtDonGia.Text);
            }catch (Exception exp)
            {
                MessageBox.Show("Bạn nhập sai format");
                return;
            }

            string SqlINSERT = "insert into SanPham values ('"+ txtMaSP.Text +"', N'" + txtTenSP.Text +"', '"+ cbnLoaiSP.SelectedValue.ToString() +"', "+ Convert.ToInt64(txtSoLuong.Text) +", "+ Convert.ToDouble(txtDonGia.Text) +")";

            try
            {
                SqlCommand cmd = new SqlCommand(SqlINSERT, con);
                int dr = cmd.ExecuteNonQuery();
                if (dr != 0)
                {
                    HienThi();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Nhập trùng khóa chính");
                return;
            }
        }

        private void DsSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (e.ColumnIndex == 5)
            {
                if (MessageBox.Show("Bạn có chắc chắm muốn xóa không?", "Đang xóa...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string SqlSELECT = "delete from SanPham where maSP = '"+ DsSanPham.Rows[rowIndex].Cells[0].Value.ToString() +"'";
                    SqlCommand cmd = new SqlCommand(SqlSELECT, con);
                    int dr = cmd.ExecuteNonQuery();
                    if (dr != 0)
                    {
                        HienThi();
                    }
                }
            }
            if (e.ColumnIndex == 6)
            {
                Form2 newForm = new Form2(DsSanPham.Rows[rowIndex].Cells[0].Value.ToString(), DsSanPham.Rows[rowIndex].Cells[1].Value.ToString(), DsSanPham.Rows[rowIndex].Cells[2].Value.ToString(), DsSanPham.Rows[rowIndex].Cells[3].Value.ToString(), DsSanPham.Rows[rowIndex].Cells[4].Value.ToString());
                newForm.ShowDialog();
            }
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            string query = "SELECT maSP, tenSP, tenLoai, soLuong, donGia, 'Xóa' as [Xóa] FROM SanPham JOIN LoaiSanPham ON LoaiSanPham.maLoai = SanPham.loaiSP where SanPham.maSP LIKE '%"+ txtTimKiem.Text.ToString() + "%' OR SanPham.tenSP LIKE N'%" + txtTimKiem.Text.ToString() + "%'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            DsSanPham.DataSource = dt;
            for (int i = 0; i < DsSanPham.Rows.Count; i++)
            {
                DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                DataGridViewLinkCell linkCell1 = new DataGridViewLinkCell();
                DsSanPham[5, i] = linkCell;
                DsSanPham[6, i] = linkCell1;
            }
        }
    }
}
