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
    public partial class frmJob : Form
    {
        DataTable dtJob = new DataTable("NgheNghiep");
        DataCommunicate dc = new DataCommunicate();
        bool blnAdd = false;

        void DieuKhienBinhThuong() {
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
            txtId.ReadOnly = true;
            txtName.ReadOnly = true;
            btnClose.Enabled = true;
            dgvJob.Enabled = true;
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
            txtId.Clear();
            txtName.Clear();
            txtId.Focus();
            dgvJob.Enabled = false;
        }

        void DieuKhienChinhSua() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            txtName.ReadOnly = false;
            txtName.Focus();
            dgvJob.Enabled = false;
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
                string[] data = new string[2];
                data[0] = txtId.Text;
                data[1] = txtName.Text;
                dc.InsertData(data, "NgheNghiep");
                this.dtJob.Rows.Add(data[0],data[1]);
                this.GanDuLieu();
                this.blnAdd = false;
            }
            else
            {
                string[] fieldsData = new string[1];
                fieldsData[0] = "TenNgheNghiep";
                string[] data = new string[1];
                data[0] = txtName.Text;
                string[] fieldsPlace=new string[1];
                fieldsPlace[0] = "MaNgheNghiep";
                string[] place=new string[1];
                place[0] = txtId.Text;
                dc.UpdateData(fieldsData, data, fieldsPlace, place, "NgheNghiep");
                int curRow = dgvJob.CurrentRow.Index;
                this.dtJob.Rows[curRow][0] = txtId.Text;
                this.dtJob.Rows[curRow][1] = txtName.Text;
            }
            this.DieuKhienBinhThuong();
        }

        private void btnLuu_Click(object sender, EventArgs e) {
            string[] data = new string[1];
            string[] fields = new string[1];
            if (txtId.Text == "") {
                MessageBox.Show("Lỗi chưa nhập mã nghề nghiệp!");
                txtId.Focus();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Lỗi chưa nhập tên nghề nghiệp!");
                txtName.Focus();
                return;
            }
            data[0]=txtId.Text;
            fields[0]="MaNgheNghiep";
            if ((blnAdd)&&this.dc.IsExisted(fields, data, "NgheNghiep"))
            {
                MessageBox.Show("Mã nghề này đã có rồi!");
                txtId.Clear();
                txtId.Focus();
                return;
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
            fields[0] = "MaNgheNghiep";
            if (this.dc.IsExisted(fields,data,"ThanhVien"))
            {
                MessageBox.Show("Nghề này vẫn còn người làm!");
            }
            else {
                DialogResult dlDongY;
                dlDongY = MessageBox.Show("Bạn thật sự muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dlDongY == DialogResult.Yes) {
                    this.dc.DeleteData(fields, data, "NgheNghiep");
                    this.dtJob.Rows.RemoveAt(dgvJob.CurrentRow.Index);
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
            if (this.dtJob.Rows.Count > 0)
            {
                txtId.Text = dgvJob[0, dgvJob.CurrentRow.Index].Value.ToString();
                txtName.Text = dgvJob[1, dgvJob.CurrentRow.Index].Value.ToString();
            }
            else
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                txtId.Clear();
                txtName.Clear();
            }
        }

        public frmJob()
        {
            InitializeComponent();
        }

        private void frmJob_Load(object sender, EventArgs e)
        {
            this.dc.GetDataSource("NgheNghiep", this.dtJob);
            dgvJob.DataSource = this.dtJob;
            txtId.MaxLength = 8;
            txtName.MaxLength = 40;
            this.GanDuLieu();
            dgvJob.Width = 290;
            dgvJob.Columns[0].Width = 70;
            dgvJob.Columns[0].HeaderText = "Stt";
            dgvJob.Columns[1].Width = 160;
            dgvJob.Columns[1].HeaderText = "Nghề";
            dgvJob.AllowUserToAddRows = false;
            dgvJob.AllowUserToDeleteRows = false;
            dgvJob.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.DieuKhienBinhThuong();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        
    }
}
