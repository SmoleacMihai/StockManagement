using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stockManagement
{
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
            GetCustomer();
            GetProduct();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mihai\OneDrive\Документы\StockDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void GetCustomer()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM CustomerTbl", Con);
            SqlDataReader sdr;
            sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId", typeof(int));
            dt.Load(sdr);
            CustCb.ValueMember = "CustCode";
            CustCb.DataSource = dt;
            Con.Close();
        }
        private void GetProduct()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TProductTbl", Con);
            SqlDataReader sdr;
            sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PrCode", typeof(int));
            dt.Load(sdr);
            ProductCb.ValueMember = "PrCode";
            ProductCb.DataSource = dt;
            Con.Close();
        }
        private void GetProductName()
        {
            Con.Close();
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TProductTbl where PrCode= '" + ProductCb.SelectedValue.ToString() + "'", Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                PrNameTb.Text = dr["PrName"].ToString();
            }
            Con.Close();
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
        int n = 0;
        private void addToBill_Click(object sender, EventArgs e)
        {
            if(ProductCb.Text == "" || QtyTb.Text == "") 
            {
                MessageBox.Show("Missing data!");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = ProductCb.Text;
                newRow.Cells[2].Value = PriceTb.Text;
                newRow.Cells[3].Value = QtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Stock Obj = new Stock();
            Obj.Show();
            this.Hide();
        }

        private void ProductCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProductName();
        }
    }
}
