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
    public partial class frmForgetPass : Form
    {
        frmLogin loginFrm;

        public frmForgetPass()
        {
            InitializeComponent();
        }

        public frmForgetPass(frmLogin loginFrm)
        {
            InitializeComponent();
            this.loginFrm = loginFrm;
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
        SqlCommandBuilder cb;
        SqlDataReader dl;

        private void Connect()
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

        private void updateData()
        {
            cb = new SqlCommandBuilder(da);
            da = cb.DataAdapter;
            da.Update(ds.Tables["UserTable"]);
        }

        private void kosong()
        {
            txtUser.Clear();
            txtPass.Clear();
        }

        private void frmForgetPass_Load(object sender, EventArgs e)
        {
            Connect();
            loadData();
            txtUser.MaxLength = 6;
            txtPass.MaxLength = 50;
            txtPass.PasswordChar = '*';
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            loadData();
            dr = ds.Tables["UserTable"].Rows.Find(txtUser.Text);

            if (!string.IsNullOrWhiteSpace(txtUser.Text) && !string.IsNullOrWhiteSpace(txtPass.Text))
            {
                if (dr != null)
                {
                    dr[1] = txtPass.Text;
                    updateData();
                    MessageBox.Show("Password for" + txtUser.Text + " has been Changed.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kosong();

                    MessageBox.Show("Back to Login?", "Return", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmLogin loginFrm = new frmLogin(this);
                    this.Hide();
                    loginFrm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("User ID " + txtUser.Text + " does not exists in database.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Complete the data.", "Edit User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                btnChange.PerformClick();
                e.SuppressKeyPress = true;  // Menghindari suara bip
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            frmLogin loginFrm = new frmLogin(this);
            this.Hide();
            loginFrm.ShowDialog();
        }

        private void frmForgetPass_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin loginFrm = new frmLogin(this);
            this.Hide();
            loginFrm.ShowDialog();
        }
    }
}
