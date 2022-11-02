using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    public partial class frmLogin : Form
    {
        private frmMain fMain;

        public frmLogin()
        {
            InitializeComponent();
        }

        public frmLogin(frmMain fm)
            : this()
        {
            fMain = fm;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtMaHo.Focus();
            txtMK.PasswordChar = '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (ObjectLoginValue.Login(txtMaHo.Text, txtSTT.Text, txtMK.Text) == "Logged")
            {
                fMain.mnuLogin.Enabled = false;
                fMain.mnuManager.Enabled = true;
                fMain.mnuChangePass.Enabled = true;
                fMain.mnuLogout.Enabled = true;
                if (ObjectLoginValue.strPrivilege == "Admin")
                {
                    fMain.mnuSocial.Enabled = true;
                }
                else {
                    fMain.mnuSocial.Enabled = false;
                }
                MessageBox.Show("Đăng nhập thành công!");
                MessageBox.Show(ObjectLoginValue.strName + " " + ObjectLoginValue.strPrivilege);
                this.Close();
            }
            else {
                MessageBox.Show(ObjectLoginValue.Login(txtMaHo.Text, txtSTT.Text, txtMK.Text));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        
    }
}
