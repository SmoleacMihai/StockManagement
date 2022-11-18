using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stockManagement
{
    public partial class Supliers : Form
    {
        public Supliers()
        {
            InitializeComponent();
            ShowSuplier();
        }
        private void ShowSuplier()
        {
            Con.Open();
            string Querry = "SELECT * FROM SuplierTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Querry, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SuplierDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void DeleteText()
        {
            SupNameTb.Text = "";
            SupPhoneTb.Text = "";
            SupAdressTb.Text = "";
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mihai\OneDrive\Документы\StockDb.mdf;Integrated Security=True;Connect Timeout=30");
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

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (SupNameTb.Text == "" || SupPhoneTb.Text == "" || SupAdressTb.Text == "")
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into SuplierTbl values(@SN, @SPhone, @SAdress)", Con);
                    cmd.Parameters.AddWithValue("@SN", SupNameTb.Text);
                    cmd.Parameters.AddWithValue("@SPhone", SupPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@SAdress", SupAdressTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Suplier saved!!!");
                    Con.Close();
                    ShowSuplier();
                    DeleteText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (SupNameTb.Text == "" || SupPhoneTb.Text == "" || SupAdressTb.Text == "")
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update SuplierTbl set SupName=@SN, SupPhone=@SPhone, SupAdd=@SAdress where SupCode=@SKey", Con);
                    cmd.Parameters.AddWithValue("@SN", SupNameTb.Text);
                    cmd.Parameters.AddWithValue("@SPhone", SupPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@SAdress", SupAdressTb.Text);
                    cmd.Parameters.AddWithValue("@SKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Suplier Edited!!!");
                    Con.Close();
                    ShowSuplier();
                    DeleteText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
        }
        int key = 0;
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select The Suplier", "Info");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from SuplierTbl where SupCode=@SKey", Con);
                    cmd.Parameters.AddWithValue("@SKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(@"Suplier Deleted!");
                    Con.Close();
                    ShowSuplier();
                    DeleteText();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void SuplierDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SuplierDGV.ReadOnly = true;
            SupNameTb.Text = SuplierDGV.Rows[e.RowIndex].Cells["SupName"].Value.ToString();
            SupPhoneTb.Text = SuplierDGV.Rows[e.RowIndex].Cells["SupPhone"].Value.ToString();
            SupAdressTb.Text = SuplierDGV.Rows[e.RowIndex].Cells["SupAdd"].Value.ToString();
            if (SupNameTb.Text == "")
            {
                key = e.RowIndex;
            }
            else
            {
                key = Convert.ToInt32(SuplierDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }
    }
}
