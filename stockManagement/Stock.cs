using System;
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
using Microsoft.Office.Interop.Excel;
using System.Runtime.ConstrainedExecution;
using System.Drawing.Printing;

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
            System.Data.DataTable dt = new System.Data.DataTable();
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
            System.Data.DataTable dt = new System.Data.DataTable();
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

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ShowProduct();
            GetCategories();
            GetSupliers();
            textBox1.Text = "";
            DeleteText();
        }

        private void srchBetweenDatesBtn_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            Con.Open();
            string sql = "select * from TProductTbl where PrDate Between @date1 and @date2";
            SqlDataAdapter da = new SqlDataAdapter(sql, Con);
            da.SelectCommand.Parameters.AddWithValue("@date1", firstDateForFiltering.Value);
            da.SelectCommand.Parameters.AddWithValue("@date2", secondDateForFiltering.Value);
            da.Fill(dt);
            Con.Close();
            ProductDGV.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application apps = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = apps.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Sheets["Sheet1"];
            
            apps.Visible= true;

            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported DVG";

            object clr = System.Drawing.ColorTranslator.ToOle(Color.Green);
            

            for (int i = 1; i < ProductDGV.Columns.Count + 1; i++) 
            {
                worksheet.Cells[1, i] = ProductDGV.Columns[i - 1].HeaderText;
                worksheet.Cells[1, i].Interior.Color = clr;
            }

            for(int i = 0; i < ProductDGV.Rows.Count - 1; i++) 
            {
                for(int j = 0; j < ProductDGV.Columns.Count; j++) 
                {
                    worksheet.Cells[i + 2, j + 1] = ProductDGV.Rows[i].Cells[j].Value.ToString();
                }
            }
            try
            {
                workbook.SaveAs("D:\\OUTPUT.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            } catch { }

            if(checkBox1.Checked)
            {
                workbook.PrintOut(
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
        }
    }
}
    