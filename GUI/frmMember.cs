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
    public partial class frmMember : Form
    {
        DataTable dtMember = new DataTable("ThanhVien");
        DataTable dtFamilies = new DataTable("HoGiaDinh");
        DataTable dtJob = new DataTable("NgheNghiep");
        DataTable dtLevel = new DataTable("TrinhDo");
        DataTable dtInstance = new DataTable("DoiTuong");
        DataTable dtPrivilege = new DataTable("QuyenSD");
        DataCommunicate dc = new DataCommunicate();
        bool blnThem = false;
        
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
            cbFamilies.Enabled = false;
            txtIndex.ReadOnly = true;
            txtName.ReadOnly = true;
            grpSex.Enabled = false;
            dtpDateBirth.Enabled = false;
            cbPrivilege.Enabled = false;
            txtRelation.ReadOnly = true;
            cbJob.Enabled = false;
            cbLevel.Enabled = false;
            cbInstance.Enabled = false;
            btnClose.Enabled = true;
            dgvMember.Enabled = true;
        }

        void DieuKhienThem() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            cbFamilies.Enabled = true;
            cbFamilies.SelectedIndex = 0;
            txtIndex.ReadOnly = false;
            txtIndex.Clear();
            txtName.ReadOnly = false;
            txtName.Clear();
            grpSex.Enabled = true;
            rdioFemale.Checked = true;
            dtpDateBirth.Enabled = true;
            dtpDateBirth.Value = DateTime.Today;
            cbPrivilege.Enabled = true;
            cbPrivilege.SelectedIndex = 1;
            txtRelation.ReadOnly = false;
            txtRelation.Clear();
            cbJob.Enabled = true;
            cbJob.SelectedIndex = 0;
            cbLevel.Enabled = true;
            cbLevel.SelectedItem = 0;
            cbInstance.Enabled = true;
            cbInstance.SelectedIndex = 0;
            dgvMember.Enabled = false;
            txtIndex.Focus();
        }

        void DieuKhienChinhSua() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            cbFamilies.Enabled = true;
            txtIndex.ReadOnly = false;
            txtName.ReadOnly = false;
            grpSex.Enabled = true;
            dtpDateBirth.Enabled = true;
            cbPrivilege.Enabled = true;
            txtRelation.ReadOnly = false;
            cbJob.Enabled = true;
            cbLevel.Enabled = true;
            cbInstance.Enabled = true;
            dgvMember.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e) {
            this.blnThem = true;
            this.DieuKhienThem();
        }

        private void btnSua_Click(object sender, EventArgs e) {
            this.DieuKhienChinhSua();
        }

        void ThucThiLuu() {
            if (this.blnThem == true)
            {
                string[] fields = new string[2];
                fields[0] = "MaHo";
                fields[1] = "SttThanhVien";
                string[] data = new string[2];
                data[0] = cbFamilies.SelectedValue.ToString();
                data[1] = txtIndex.Text;
                if (dc.IsExisted(fields, data, "ThanhVien")) {
                    MessageBox.Show("Thành viên này đã tồn tại!");
                    return;
                }
                data = new string[11];
                data[0] = cbFamilies.SelectedValue.ToString();
                data[1] = txtIndex.Text;
                data[2] = txtName.Text;
                data[3] = "0";
                if (rdioMale.Checked) {
                    data[3] = "1";
                }
                data[4] = dtpDateBirth.Value.ToString();
                data[5] = txtRelation.Text;
                data[6] = cbJob.SelectedValue.ToString();
                data[7] = cbLevel.SelectedValue.ToString();
                data[8] = cbInstance.SelectedValue.ToString();
                data[9] = ObjectLoginValue.EncodePass(data[1]);
                data[10] = cbPrivilege.SelectedValue.ToString();
                dc.InsertData(data, "ThanhVien");
                if (data[3] == "1") data[3] = "true";
                else data[3] = "false";
                this.dtMember.Rows.Add(data[0], data[1],
                    data[2], Boolean.Parse(data[3]),
                    data[4], data[5], 
                    data[6], data[7],
                    data[8], data[10]);
                this.GanDuLieu();
                blnThem = false;
            }
            else {
                string[] fieldsData = new string[10];
                fieldsData[0] = "SttThanhVien";
                fieldsData[1] = "HoTenThanhVien";
                fieldsData[2] = "Phai";
                fieldsData[3] = "NgaySinh";
                fieldsData[4] = "QuanHeVoiChuHo";
                fieldsData[5] = "MaNgheNghiep";
                fieldsData[6] = "MaTrinhDo";
                fieldsData[7] = "MaDoiTuong";
                fieldsData[8] = "QuyenSD";
                fieldsData[9] = "MaHo";
                string[] data = new string[10];
                data[0] = txtIndex.Text;
                data[1] = txtName.Text;
                data[2] = "0";
                if (rdioMale.Checked)
                {
                    data[2] = "1";
                }
                data[3] = dtpDateBirth.Value.ToString();
                data[4] = txtRelation.Text;
                data[5] = cbJob.SelectedValue.ToString();
                data[6] = cbLevel.SelectedValue.ToString();
                data[7] = cbInstance.SelectedValue.ToString();
                data[8] = cbPrivilege.SelectedValue.ToString();
                data[9] = cbFamilies.SelectedValue.ToString();
                string[] fieldsPlace = new string[2];
                fieldsPlace[0] = "MaHo";
                fieldsPlace[1] = "SttThanhVien";
                string[] place = new string[2];
                place[0] = dgvMember[0, dgvMember.CurrentRow.Index].Value.ToString();
                place[1] = dgvMember[1, dgvMember.CurrentRow.Index].Value.ToString();
                this.dc.UpdateData(fieldsData, data, fieldsPlace, place, "ThanhVien");
                int curRow = dgvMember.CurrentRow.Index;
                this.dtMember.Rows[curRow][0] = data[9];
                this.dtMember.Rows[curRow][1] = data[0];
                this.dtMember.Rows[curRow][2] = data[1];
                if (data[2] == "1") data[2] = "true";
                else data[2] = "false";
                this.dtMember.Rows[curRow][3] = Boolean.Parse(data[2]);
                this.dtMember.Rows[curRow][4] = data[3];
                this.dtMember.Rows[curRow][5] = data[4];
                this.dtMember.Rows[curRow][6] = data[5];
                this.dtMember.Rows[curRow][7] = data[6];
                this.dtMember.Rows[curRow][8] = data[7];
                this.dtMember.Rows[curRow][10] = data[8];
                this.GanDuLieu();
            }
            this.DieuKhienBinhThuong();
        }

        private void btnLuu_Click(object sender, EventArgs e) {
            if (cbFamilies.SelectedIndex == -1)
            {
                MessageBox.Show("Lỗi chưa chọn mã hộ!");
                cbFamilies.Focus();
                return;
            }
            if (txtIndex.Text == "")
            {
                MessageBox.Show("Lỗi chưa nhập số thứ tự thành viên!");
                txtIndex.Focus();
                return;
            }
            if (txtName.Text == "") {
                MessageBox.Show("Lỗi chưa nhập họ tên!");
                txtName.Focus();
                return;
            }
            if (txtRelation.Text == "") {
                MessageBox.Show("Lỗi chưa nhập quan hệ với chủ hộ!");
                txtRelation.Focus();
                return;
            }
            if (cbJob.SelectedIndex == -1) { 
                MessageBox.Show("Lỗi chưa chọn nghề nghiệp !");
                cbJob.Focus();
                return;
            }
            if (cbLevel.SelectedIndex == -1)
            {
                MessageBox.Show("Lỗi chưa chọn trình độ!");
                cbLevel.Focus();
                return;
            }
            if (cbInstance.SelectedIndex == -1)
            {
                MessageBox.Show("Lỗi chưa chọn đối tượng!");
                cbInstance.Focus();
                return;
            }
            else {
                this.ThucThiLuu();
            }
        }

        private void btnKhongLuu_Click(object sender, EventArgs e) {
            this.blnThem = false;
            this.GanDuLieu();
            this.DieuKhienBinhThuong();
        }

        private void btnXoa_Click(object sender, EventArgs e) {
            string[] fields = new string[2];
            fields[0] = "MaHo";
            fields[1] = "SttThanhVien";
            string[] data = new string[2];
            data[0] = dgvMember[0, dgvMember.CurrentRow.Index].Value.ToString();
            data[1] = dgvMember[1, dgvMember.CurrentRow.Index].Value.ToString();
            DialogResult dlDongY;
            dlDongY = MessageBox.Show("Bạn thật sự muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dlDongY == DialogResult.Yes)
            {
                this.dc.DeleteData(fields, data, "ThanhVien");
                this.dtMember.Rows.RemoveAt(dgvMember.CurrentRow.Index);
                this.GanDuLieu();
            }    
            this.DieuKhienBinhThuong();
        }

        private void dgvMon_CellClick(object sender, DataGridViewCellEventArgs e) {
            this.GanDuLieu();
        }

        void GanDuLieu() {
            if (this.dtMember.Rows.Count > 0)
            {
                cbFamilies.SelectedValue = dgvMember[0, dgvMember.CurrentRow.Index].Value.ToString();
                txtIndex.Text = dgvMember[1, dgvMember.CurrentRow.Index].Value.ToString();
                txtName.Text = dgvMember[2, dgvMember.CurrentRow.Index].Value.ToString();
                rdioFemale.Checked = true;
                if (dgvMember[3, dgvMember.CurrentRow.Index].Value.ToString() == "True") {

                    rdioMale.Checked = true;
                }
                dtpDateBirth.Value = DateTime.Parse(dgvMember[4, dgvMember.CurrentRow.Index].Value.ToString());
                txtRelation.Text = dgvMember[5, dgvMember.CurrentRow.Index].Value.ToString();
                cbJob.SelectedValue = dgvMember[6, dgvMember.CurrentRow.Index].Value.ToString();
                cbLevel.SelectedValue = dgvMember[7, dgvMember.CurrentRow.Index].Value.ToString();
                cbInstance.SelectedValue = dgvMember[8, dgvMember.CurrentRow.Index].Value.ToString();
                cbPrivilege.SelectedValue = dgvMember[10, dgvMember.CurrentRow.Index].Value.ToString();
            }
            else {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                cbFamilies.SelectedIndex = -1;
                txtIndex.Clear();
                txtName.Clear();
                dtpDateBirth.Value = DateTime.Today;
                cbPrivilege.SelectedIndex = -1;
                txtRelation.Clear();
                cbJob.SelectedIndex = -1;
                cbLevel.SelectedIndex = -1;
                cbInstance.SelectedValue = -1;
            }
        }

        public frmMember()
        {
            InitializeComponent();
        }

        private void frmMon_Load(object sender, EventArgs e)
        {
            dc.GetDataSource("ThanhVien", this.dtMember);
            dc.GetDataSource("HoGiaDinh", this.dtFamilies);
            dc.GetDataSource("NgheNghiep", this.dtJob);
            dc.GetDataSource("TrinhDo", this.dtLevel);
            dc.GetDataSource("DoiTuong", this.dtInstance);
            this.dtInstance.Rows.Add("null", "Không biết");
            this.dtPrivilege.Columns.Add("QuyenSD");
            this.dtPrivilege.Rows.Add("Admin");
            this.dtPrivilege.Rows.Add("User");
            cbFamilies.DataSource = this.dtFamilies;
            cbFamilies.DisplayMember = "DiaChiNha";
            cbFamilies.ValueMember = "MaHo";
            cbPrivilege.DataSource = this.dtPrivilege;
            cbPrivilege.DisplayMember = "QuyenSD";
            cbPrivilege.ValueMember = "QuyenSD";
            cbJob.DataSource = this.dtJob;
            cbJob.DisplayMember = "TenNgheNghiep";
            cbJob.ValueMember = "MaNgheNghiep";
            cbLevel.DataSource = this.dtLevel;
            cbLevel.DisplayMember = "DienGiai";
            cbLevel.ValueMember = "MaTrinhDo";
            cbInstance.DataSource = this.dtInstance;
            cbInstance.DisplayMember = "TenDoiTuong";
            cbInstance.ValueMember = "MaDoiTuong";
            dgvMember.DataSource = this.dtMember;
            dgvMember.Width = 1000;
            dgvMember.AllowUserToAddRows = false;
            dgvMember.AllowUserToDeleteRows = false;
            dgvMember.Columns[0].Width = 80;
            dgvMember.Columns[0].HeaderText = "Mã hộ";
            dgvMember.Columns[1].Width = 100;
            dgvMember.Columns[1].HeaderText = "Thành viên thứ";
            dgvMember.Columns[2].Width = 150;
            dgvMember.Columns[2].HeaderText = "Họ tên";
            dgvMember.Columns[3].Width = 40;
            dgvMember.Columns[3].HeaderText = "Phái";
            dgvMember.Columns[4].Width = 100;
            dgvMember.Columns[4].HeaderText = "Ngày sinh";
            dgvMember.Columns[5].Width = 100;
            dgvMember.Columns[5].HeaderText = "Vai trò";
            dgvMember.Columns[6].Width = 100;
            dgvMember.Columns[6].HeaderText = "Nghề nghiệp";
            dgvMember.Columns[7].Width = 100;
            dgvMember.Columns[7].HeaderText = "Trình độ";
            dgvMember.Columns[8].Width = 100;
            dgvMember.Columns[8].HeaderText = "Đối tượng";
            dgvMember.Columns[9].Visible = false;
            dgvMember.Columns[10].Width = 70;
            dgvMember.Columns[10].HeaderText = "Quyền";
            dgvMember.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.GanDuLieu();
            this.DieuKhienBinhThuong();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
