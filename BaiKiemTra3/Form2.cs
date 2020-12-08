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
    public partial class Form2 : Form
    {
        string maSP;
        string tenSP;
        string loaiSP;
        string soLuong;
        string donGia;
        public Form2(string ma, string ten, string loai, string sl, string dg)
        {
            InitializeComponent();
            maSP = ma;
            tenSP = ten;
            loaiSP = loai;
            soLuong = sl;
            donGia = dg;
        }

        SqlConnection con;
        private void Form2_Load(object sender, EventArgs e)
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


            txtMaSP.Text = maSP;
            txtTenSP.Text = tenSP;
            cbnLoaiSP.Text = loaiSP;
            txtSoLuong.Text = soLuong;
            txtDonGia.Text = donGia;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            double donGia;
            try
            {
                donGia = Convert.ToDouble(txtDonGia.Text);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Bạn nhập sai format");
                return;
            }

            string SqlINSERT = "update SanPham set tenSP = N'" + txtTenSP.Text + "', loaiSP = '" + cbnLoaiSP.SelectedValue.ToString() + "', soLuong = " + Convert.ToInt64(txtSoLuong.Text) + ", donGia = " + Convert.ToDouble(txtDonGia.Text) + " where SanPham.maSP = '"+maSP+"'";

            try
            {
                SqlCommand cmd = new SqlCommand(SqlINSERT, con);
                int dr = cmd.ExecuteNonQuery();

                if (dr != 0)
                {
                    Close();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Sửa thất bại");
                return;
            }
        }
    }
}
