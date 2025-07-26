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
                constr = "Data Source = localhost; Initial Catalog = Otto Cafe Payroll; Integrated Security = true";
                con = new SqlConnection(constr);
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            frmRegistrasi registrasiFrm = new frmRegistrasi(this);
            this.Hide();
            registrasiFrm.ShowDialog();
        }
    }
}
