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
    //This class use binding source
    public partial class frmHamlet : Form
    {
        DataTable dtHamlet = new DataTable("KhomAp");
        DataCommunicate dc = new DataCommunicate();
        bool blnAdd = false;

        void DieuKhienBinhThuong() {
            if (ObjectLoginValue.strPrivilege == "Admin")
            {
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            else {
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                btnSua.Enabled = false;
            }
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
            txtId.ReadOnly = true;
            txtName.ReadOnly = true;
            txtGroup.ReadOnly = true;
            txtDescribe.ReadOnly = true;
            btnClose.Enabled = true;
            dgvHamlet.Enabled = true;
        }

        void DieuKhienThem() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            txtId.ReadOnly = false;
            txtName.ReadOnly = false;
            txtGroup.ReadOnly = false;
            txtDescribe.ReadOnly = false;
            txtId.Clear();
            txtName.Clear();
            txtGroup.Clear();
            txtDescribe.Clear();
            txtId.Focus();
            dgvHamlet.Enabled = false;
        }

        void DieuKhienChinhSua() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            txtName.ReadOnly = false;
            txtGroup.ReadOnly = false;
            txtDescribe.ReadOnly = false;
            txtName.Focus();
            dgvHamlet.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e) {
            this.blnAdd = true;
            this.DieuKhienThem();
        }

        private void btnSua_Click(object sender, EventArgs e) {
            this.DieuKhienChinhSua();
        }

        private void ExecuteSave() {
            if (this.blnAdd == true)
            {
                string[] data = new string[4];
                data[0] = txtId.Text;
                data[1] = txtName.Text;
                data[2] = txtGroup.Text;
                data[3] = txtDescribe.Text;
                dc.InsertData(data, "KhomAp");
                this.dtHamlet.Rows.Add(data[0],data[1],data[2],data[3]);
                this.GanDuLieu();
                this.blnAdd = false;
            }
            else
            {
                string[] fieldsData = new string[3];
                fieldsData[0] = "TenAp";
                fieldsData[1] = "SoTo";
                fieldsData[2] = "DacDiem";
                string[] data = new string[3];
                data[0] = txtName.Text;
                data[1] = txtGroup.Text;
                data[2] = txtDescribe.Text;
                string[] fieldsPlace=new string[1];
                fieldsPlace[0]="MaAp";
                string[] place=new string[1];
                place[0]=txtId.Text;
                dc.UpdateData(fieldsData, data, fieldsPlace, place, "KhomAp");
                int curRow = dgvHamlet.CurrentRow.Index;
                this.dtHamlet.Rows[curRow][0] = txtId.Text;
                this.dtHamlet.Rows[curRow][1] = txtName.Text;
                this.dtHamlet.Rows[curRow][2] = txtGroup.Text;
                this.dtHamlet.Rows[curRow][3] = txtDescribe.Text;
            }
            this.DieuKhienBinhThuong();
        }

        private void btnLuu_Click(object sender, EventArgs e) {
            int group;
            string[] data = new string[1];
            string[] fields = new string[1];
            if (txtId.Text == "") {
                MessageBox.Show("Lỗi chưa nhập mã ấp!");
                txtId.Focus();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Lỗi chưa nhập tên ấp!");
                txtName.Focus();
                return;
            }
            if ((!int.TryParse(txtGroup.Text, out group)) || group <= 0) {
                MessageBox.Show("Lỗi nhập số tổ!");
                txtGroup.Focus();
                return;
            }
            data[0]=txtId.Text;
            fields[0]="MaAp";
            if ((blnAdd)&&this.dc.IsExisted(fields, data, "KhomAp"))
            {
                MessageBox.Show("Mã ấp này đã có rồi!");
                txtId.Clear();
                txtId.Focus();
            }
            else {
                this.ExecuteSave();
            }
        }

        private void btnKhongLuu_Click(object sender, EventArgs e) {
            this.blnAdd = false;
            this.GanDuLieu();
            this.DieuKhienBinhThuong();
        }

        private void btnXoa_Click(object sender, EventArgs e) {
            string[] data = new string[1];
            data[0] = txtId.Text;
            string[] fields = new string[1];
            fields[0] = "MaAp";
            if (this.dc.IsExisted(fields,data,"HoGiaDinh"))
            {
                MessageBox.Show("Khóm ấp này còn có các hộ gia đình, hãy cân nhắc xóa các hộ trước!");
            }
            else {
                DialogResult dlDongY;
                dlDongY = MessageBox.Show("Bạn thật sự muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dlDongY == DialogResult.Yes) {
                    this.dc.DeleteData(fields, data, "KhomAp");
                    this.dtHamlet.Rows.RemoveAt(dgvHamlet.CurrentRow.Index);
                    this.GanDuLieu();
                    MessageBox.Show("Xóa thành công!");
                }
            }
            this.DieuKhienBinhThuong();
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e) {
            this.GanDuLieu();
        }

        private void GanDuLieu() {
            if (this.dtHamlet.Rows.Count > 0)
            {
                txtId.Text = dgvHamlet[0, dgvHamlet.CurrentRow.Index].Value.ToString();
                txtName.Text = dgvHamlet[1, dgvHamlet.CurrentRow.Index].Value.ToString();
                txtGroup.Text = dgvHamlet[2, dgvHamlet.CurrentRow.Index].Value.ToString();
                txtDescribe.Text = dgvHamlet[3, dgvHamlet.CurrentRow.Index].Value.ToString();
            }
            else
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                txtId.Clear();
                txtName.Clear();
                txtGroup.Clear();
                txtDescribe.Clear();
            }
        }

        public frmHamlet()
        {
            InitializeComponent();
        }

        private void frmLop_Load(object sender, EventArgs e)
        {
            this.dc.GetDataSource("KhomAp", this.dtHamlet);
            dgvHamlet.DataSource = this.dtHamlet;
            txtId.MaxLength = 8;
            txtName.MaxLength = 40;
            this.GanDuLieu();
            dgvHamlet.Width = 460;
            dgvHamlet.Columns[0].Width = 80;
            dgvHamlet.Columns[0].HeaderText = "Mã ấp";
            dgvHamlet.Columns[1].Width = 120;
            dgvHamlet.Columns[1].HeaderText = "Tên ấp";
            dgvHamlet.Columns[2].Width = 80;
            dgvHamlet.Columns[2].HeaderText = "Số tổ";
            dgvHamlet.Columns[3].Width = 120;
            dgvHamlet.Columns[3].HeaderText = "Đặc điểm";
            dgvHamlet.AllowUserToAddRows = false;
            dgvHamlet.AllowUserToDeleteRows = false;
            dgvHamlet.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.DieuKhienBinhThuong();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        
    }
}
