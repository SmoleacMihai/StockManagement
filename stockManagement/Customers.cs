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
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            ShowCustomer();
        }
        private void ShowCustomer()
        {
            Con.Open();
            string Querry = "SELECT * FROM CustomerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Querry, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustomerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void DeleteText()
        {
            CustNameTb.Text = "";
            GenCb.Text = "";
            CustPhoneTb.Text = "";
            CustAdressTb.Text = "";
            CustEmailTb.Text = "";
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mihai\OneDrive\Документы\StockDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (CustNameTb.Text == "" || CustPhoneTb.Text == "" || CustAdressTb.Text == "" || CustEmailTb.Text == "" || (GenCb.SelectedIndex == -1))
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTbl values(@CN, @CPhone, @CEmail, @CAdress, @CGen)", Con);
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CPhone", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CEmail", CustEmailTb.Text);
                    cmd.Parameters.AddWithValue("@CAdress", CustAdressTb.Text);
                    cmd.Parameters.AddWithValue("@CGen", GenCb.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer saved!!!");
                    Con.Close();
                    ShowCustomer();
                    DeleteText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
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
        int key = 0;
        private void CustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CustomerDGV.ReadOnly = true;
            CustNameTb.Text = CustomerDGV.Rows[e.RowIndex].Cells["CustName"].Value.ToString();
            GenCb.Text = CustomerDGV.Rows[e.RowIndex].Cells["CustGen"].Value.ToString();
            CustEmailTb.Text = CustomerDGV.Rows[e.RowIndex].Cells["CustEmail"].Value.ToString();
            CustPhoneTb.Text = CustomerDGV.Rows[e.RowIndex].Cells["CustPhone"].Value.ToString();
            CustAdressTb.Text = CustomerDGV.Rows[e.RowIndex].Cells["CustAdress"].Value.ToString();
            if (CustNameTb.Text == "")
            {
                key = e.RowIndex;
            }
            else
            {
                key = Convert.ToInt32(CustomerDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (CustNameTb.Text == "" || CustPhoneTb.Text == "" || CustAdressTb.Text == "" || CustEmailTb.Text == "" || (GenCb.SelectedIndex == -1))
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName=@CN, CustPhone=@CPhone, CustEmail=@CEmail, CustAdress=@CAdress, CustGen=@CGen where CustId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CPhone", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CEmail", CustEmailTb.Text);
                    cmd.Parameters.AddWithValue("@CAdress", CustAdressTb.Text);
                    cmd.Parameters.AddWithValue("@CGen", GenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Edited!!!");
                    Con.Close();
                    ShowCustomer();
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
                MessageBox.Show("Select The Customer", "Info");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where CustId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(@"Customer Deleted!");
                    Con.Close();
                    ShowCustomer();
                    DeleteText();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
    }
}
