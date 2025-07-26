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
    public partial class frmRegistrasi : Form
    {
        frmLogin loginFrm;

        public frmRegistrasi()
        {
            InitializeComponent();
        }

        public frmRegistrasi(frmLogin loginFrm)
        {
            InitializeComponent();
            this.loginFrm = loginFrm;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Library Deklarasi //
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
        // == BATAS == //

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

        private void frmRegistrasi_Load(object sender, EventArgs e)
        {
            Connect();
            loadData();
            txtUser.MaxLength = 6;
            txtPass.MaxLength = 50;
            txtPass.PasswordChar = '*';
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUser.Text) && !string.IsNullOrWhiteSpace(txtPass.Text))
            {
                loadData();

                if (dr == null)
                {
                    dr = ds.Tables["UserTable"].NewRow();
                    dr[0] = txtUser.Text;
                    dr[1] = txtPass.Text;
                    ds.Tables["UserTable"].Rows.Add(dr);
                    updateData();

                    MessageBox.Show("Employee ID " + txtUser.Text + " has been inserted.", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kosong();
                }
                else
                {
                    MessageBox.Show("Employee ID " + txtUser.Text + " exists in database.", "Add User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Complete the data.", "Add Employee", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Connect();
            loadData();
        }

        private void frmRegistrasi_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
