using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            mnuManager.Enabled = false;
            mnuLogout.Enabled = false;
            mnuChangePass.Enabled = false;
            frmLogin form = new frmLogin(this);
            form.ShowDialog();
        }

        private void frmHamlet_Click(object sender, EventArgs e) {
            frmHamlet form = new frmHamlet();
            form.ShowDialog();
        }

        private void frmFamilies_Click(object sender, EventArgs e) {
            frmFamilies form = new frmFamilies();
            form.ShowDialog();
        }

        private void frmMember_Click(object sender, EventArgs e) {
            frmMember form = new frmMember();
            form.ShowDialog();
        }

        private void frmLogin_Click(object sender, EventArgs e) {
            frmLogin form = new frmLogin(this);
            form.ShowDialog();
        }

        private void frmLogout_Click(object sender, EventArgs e) {
            ObjectLoginValue.Logout();
            mnuManager.Enabled = false;
            mnuLogout.Enabled = false;
            mnuChangePass.Enabled = false;
            mnuLogin.Enabled = true;
            frmLogin form = new frmLogin(this);
            form.ShowDialog();
        }

        private void frmChangePass_Click(object sender, EventArgs e) {
            frmChangePass form = new frmChangePass();
            form.ShowDialog();
        }

        private void mnuInfor_Click(object sender, EventArgs e) {
            MessageBox.Show("Bài đồ án quản lý hộ gia đình phiên bản 3 lớp!", "Giới thiệu");
        }

        private void mnuExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
