﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;

namespace stockManagement
{
    public partial class Stock : Form
    { 
        public Stock()
        {
            InitializeComponent();
            ShowProduct();
            GetCategories();
            GetSupliers();
        }
        private void ShowProduct()
        {
            Con.Open();
            string Querry = "SELECT * FROM TProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Querry, Con);
            SqlCommandBuilder builder= new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void ShowProductByName(string productName)
        {
            Con.Open();
            string Querry = "SELECT * FROM TProductTbl\nWHERE PrName LIKE '" + productName + "%' ";
            SqlDataAdapter sda = new SqlDataAdapter(Querry, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void GetCategories()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CategoryTbl", Con);
            SqlDataReader sdr;
            sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CatId", typeof(int));
            dt.Load(sdr);
            CatCb.ValueMember = "CatName";
            CatCb.DataSource = dt;
            Con.Close();
        }
        private void GetSupliers()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM SuplierTbl", Con);
            SqlDataReader sdr;
            sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("SupCode", typeof(int));
            dt.Load(sdr);
            SupCb.ValueMember = "SupName";
            SupCb.DataSource = dt;
            Con.Close();
        }
        private void DeleteText()
        {
            PrNameTb.Text = "";
            CatCb.Text = "";
            QtyTb.Text = "";
            BPriceTb.Text = "";
            SpriceTb.Text = "";
            PrDate.Text = "";
            ExDate.Text = "";
            SupCb.Text = "";
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mihai\OneDrive\Документы\StockDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void Stock_Load(object sender, EventArgs e)
        {

        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || QtyTb.Text == "" || SpriceTb.Text == "" || BPriceTb.Text == "" || SupCb.SelectedIndex == -1 || (CatCb.SelectedIndex == -1))
            {
                MessageBox.Show("Missing Data");
            } else
            {
                int Gain = Convert.ToInt32(SpriceTb.Text) - Convert.ToInt32(BPriceTb.Text);
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into TProductTbl values(@PN, @Pcat, @Pqty, @BPr, @SPr, @PDate, @ExDate, @Sup, @G)", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PCat", CatCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Pqty", QtyTb.Text);
                    cmd.Parameters.AddWithValue("@BPr", BPriceTb.Text);
                    cmd.Parameters.AddWithValue("@SPr", SpriceTb.Text);
                    cmd.Parameters.AddWithValue("@PDate", PrDate.Value.Date);
                    cmd.Parameters.AddWithValue("@ExDate", ExDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Sup", SupCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@G", Gain);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product saved!!!");
                    Con.Close();
                    ShowProduct();
                    DeleteText();
                }
                catch (Exception ex) { 
                    MessageBox.Show(ex.Message);
                }
            };
        }
        int key = 0;
        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProductDGV.ReadOnly = true;
            PrNameTb.Text = ProductDGV.Rows[e.RowIndex].Cells["PrName"].Value.ToString();
            CatCb.Text = ProductDGV.Rows[e.RowIndex].Cells["PrCategory"].Value.ToString();
            QtyTb.Text = ProductDGV.Rows[e.RowIndex].Cells["PrQty"].Value.ToString();
            BPriceTb.Text = ProductDGV.Rows[e.RowIndex].Cells["BpPrice"].Value.ToString();
            SpriceTb.Text = ProductDGV.Rows[e.RowIndex].Cells["SPrice"].Value.ToString();
            PrDate.Text = ProductDGV.Rows[e.RowIndex].Cells["PrDate"].Value.ToString();
            ExDate.Text = ProductDGV.Rows[e.RowIndex].Cells["ExDate"].Value.ToString();
            SupCb.Text = ProductDGV.Rows[e.RowIndex].Cells["SSup"].Value.ToString();
            if(PrNameTb.Text == "")
            {
                key = e.RowIndex;
            } else
            {
                key = Convert.ToInt32(ProductDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || QtyTb.Text == "" || SpriceTb.Text == "" || BPriceTb.Text == "" || SupCb.SelectedIndex == -1 || (CatCb.SelectedIndex == -1))
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                int Gain = Convert.ToInt32(SpriceTb.Text) - Convert.ToInt32(BPriceTb.Text);
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update TProductTbl set PrName=@PN, PrCategory=@Pcat, PrQty=@Pqty, BpPrice=@BPr, SPrice=@SPr, PrDate=@PDate, ExDate=@ExDate, SSup=@Sup, Gain=@G where PrCode=@PrKey", Con);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PCat", CatCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Pqty", QtyTb.Text);
                    cmd.Parameters.AddWithValue("@BPr", BPriceTb.Text);
                    cmd.Parameters.AddWithValue("@SPr", SpriceTb.Text);
                    cmd.Parameters.AddWithValue("@PDate", PrDate.Value.Date);
                    cmd.Parameters.AddWithValue("@ExDate", ExDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Sup", SupCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@G", Gain);
                    cmd.Parameters.AddWithValue("@PrKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product edited !!");
                    Con.Close();
                    ShowProduct();
                    DeleteText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select The Product", "Info");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from TProductTbl where PrCode=@PrKey", Con);
                    cmd.Parameters.AddWithValue("@PrKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(@"Product Deleted!");
                    Con.Close();
                    ShowProduct();
                    DeleteText();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // Use this since we are a WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Use this since we are a console app
                System.Environment.Exit(1);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Category Obj = new Category();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Supliers Obj = new Supliers();
            Obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Orders Obj = new Orders();
            Obj.Show();
            this.Hide();
        }

        private void srchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string Querry = "SELECT * FROM TProductTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Querry, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                ProductDGV.DataSource = ds.Tables[0];
                Con.Close();
                //(ProductDGV.DataSource as DataTable).DefaultView.RowFilter = 
                //    String.Format("PrName like '" + srchBtn.Text + "%' or SSup like '" + srchBtn.Text + "%' or PrCategory like '" + srchBtn.Text + "%'");
                //ShowProduct();
                //textBox1.Text = "";
                //DeleteText();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ShowProductByName(textBox1.Text);
        }
    }
}
    