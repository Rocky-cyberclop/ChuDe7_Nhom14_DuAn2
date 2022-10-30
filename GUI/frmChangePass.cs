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
    public partial class frmChangePass : Form
    {
        public frmChangePass()
        {
            InitializeComponent();
        }

        private void frmChangePass_Load(object sender, EventArgs e)
        {
            txtMaHo.ReadOnly = true;
            txtSTT.ReadOnly = true;
            txtMaHo.Text = ObjectLoginValue.strId;
            txtSTT.Text = ObjectLoginValue.strIndex;
            txtOldPass.Focus();
            txtOldPass.PasswordChar = '*';
            txtNewPass.PasswordChar = '*';
            txtConfirm.PasswordChar = '*';
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text == txtOldPass.Text)
            {
                MessageBox.Show("Mật khẩu cũ trùng mật khẩu mới!");
            }
            else {
                if (txtNewPass.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Mật khẩu mới và xác nhận không trùng!");
                }
                else {
                    if (ObjectLoginValue.ChangePassword(txtOldPass.Text, txtNewPass.Text)[0] == "Yes")
                    {
                        MessageBox.Show("Đổi thành công!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Đổi thất bại! Hãy kiểm tra lại mật khẩu cũ");
                    }    
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        
    }
}
