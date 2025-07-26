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

namespace Simpel_Login_User__1
{
    public partial class frmLogin : Form
    {
        frmRegistrasi registrasiFrm;
        frmForgetPass forgetPassFrm;

        public frmLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public frmLogin(frmRegistrasi registrasiFrm)
        {
            InitializeComponent();
            this.registrasiFrm = registrasiFrm;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public frmLogin(frmForgetPass forgetPassFrm)
        {
            InitializeComponent();
            this.forgetPassFrm = forgetPassFrm;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        SqlConnection con;
        string constr;
        SqlDataAdapter da;
        SqlCommand cmd;
        string query;
        DataSet ds;
        DataRow dr;
        DataColumn[] dc = new DataColumn[1];

        private void koneksi()
        {
            try
            {
                constr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Simpel Login User #1;Integrated Security=True;";
                con = new SqlConnection(constr);
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadData()
        {
            ds = new DataSet();
            query = "SELECT * FROM [User].[User]";  // pakai bracket karena User adalah reserved keyword
            cmd = new SqlCommand(query, con);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "UserTable");  // pakai nama alias supaya aman
            dc[0] = ds.Tables["UserTable"].Columns[0];
            ds.Tables["UserTable"].PrimaryKey = dc;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            koneksi();
            loadData();
            txtUser.MaxLength = 6;
            txtPass.MaxLength = 50;
            txtPass.PasswordChar = '*';
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            frmRegistrasi registrasiFrm = new frmRegistrasi(this);
            this.Hide();
            registrasiFrm.ShowDialog();
        }

        private void btnForgerPass_Click(object sender, EventArgs e)
        {
            frmForgetPass forgetPassFrm = new frmForgetPass(this);
            this.Hide();
            forgetPassFrm.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            dr = ds.Tables["UserTable"].Rows.Find(txtUser.Text);

            if (dr != null)
            {
                if (dr[1].ToString() == txtPass.Text)
                {
                    MessageBox.Show("Login Successful", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // You can add code here to proceed to the next form or functionality after successful login
                    txtUser.Clear();
                    txtPass.Clear();
                    txtUser.Focus();
                }
                else
                {
                    MessageBox.Show("Your Password is Wrong", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUser.Clear();
                    txtPass.Clear();
                    txtUser.Focus();
                }
            }
            else
            {
                MessageBox.Show("User ID " + txtUser.Text + " not found", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUser.Clear();
                txtPass.Clear();
                txtUser.Focus();
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPass.Focus();
                e.SuppressKeyPress = true;  // Menghindari suara bip
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.SuppressKeyPress = true;  // Menghindari suara bip
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
