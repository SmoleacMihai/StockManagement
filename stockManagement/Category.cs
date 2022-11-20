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
    public partial class Category : Form
    {
        public Category()
        {
            InitializeComponent();
            ShowCategory();
            CountCat();
        }
        private void ShowCategory()
        {
            Con.Open();
            string Querry = "SELECT * FROM CategoryTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Querry, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CategoryDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void CountCat()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT count(*) FROM CategoryTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            CatNumLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void DeleteText()
        {
            CategoryNameTb.Text = "";
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
            if (CategoryNameTb.Text == "")
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CategoryTbl values(@CN)", Con);
                    cmd.Parameters.AddWithValue("@CN", CategoryNameTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category saved!!!");
                    Con.Close();
                    ShowCategory();
                    CountCat();
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
            if (CategoryNameTb.Text == "")
            {
                MessageBox.Show("Missing Data");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CategoryTbl set CatName=@CN where CatId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CategoryNameTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Edited!!!");
                    Con.Close();
                    ShowCategory();
                    CountCat();
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
                MessageBox.Show("Select The Category", "Info");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CategoryTbl where CatId=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(@"Category Deleted!");
                    Con.Close();
                    ShowCategory();
                    CountCat();
                    DeleteText();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void CategoryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CategoryDGV.ReadOnly = true;
            CategoryNameTb.Text = CategoryDGV.Rows[e.RowIndex].Cells["CatName"].Value.ToString();
            if (CategoryNameTb.Text == "")
            {
                key = e.RowIndex;
            }
            else
            {
                key = Convert.ToInt32(CategoryDGV.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Supliers Obj = new Supliers();
            Obj.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Stock Obj = new Stock();
            Obj.Show();
            this.Hide();
        }

        private void Category_Load(object sender, EventArgs e)
        {

        }
    }
}
