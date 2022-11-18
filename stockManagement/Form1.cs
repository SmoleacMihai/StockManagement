namespace stockManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (usernameInput.Text == "Administrator" && passwordInput.Text == "12345678")
            {
                Stock Obj = new Stock();
                Obj.Show();
                this.Hide();
            }
        }
    }
}