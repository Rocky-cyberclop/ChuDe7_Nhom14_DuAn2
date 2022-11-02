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
    public partial class frmFamilies : Form
    {
        DataTable dtFamilies = new DataTable("HoGiaDinh");
        DataTable dtHamlet = new DataTable("KhomAp");
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
            txtAddress.ReadOnly = true;
            dtpDateEstablish.Enabled = false;
            cbHamlet.Enabled = false;
            btnClose.Enabled = true;
            dgvFamilies.Enabled = true;
        }

        void DieuKhienThem() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            txtAddress.ReadOnly = false;
            dtpDateEstablish.Enabled = true;
            dtpDateEstablish.Value = DateTime.Today;
            cbHamlet.Enabled = true;
            cbHamlet.SelectedIndex = 0;
            txtAddress.Clear();
            txtAddress.Focus();
            dgvFamilies.Enabled = false;
        }

        void DieuKhienChinhSua() {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
            btnXoa.Enabled = false;
            btnClose.Enabled = false;
            txtAddress.ReadOnly = false;
            cbHamlet.Enabled = true;
            dtpDateEstablish.Enabled = true;
            txtAddress.Focus();
            dgvFamilies.Enabled = false;
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
                string[] data = new string[3];
                data[0] = txtAddress.Text;
                data[1] = dtpDateEstablish.Value.ToString();
                data[2] = cbHamlet.SelectedValue.ToString();
                dc.InsertData(data, "HoGiaDinh");
                this.dtFamilies.Clear();
                this.dc.GetDataSource("HoGiaDinh", this.dtFamilies);
                dgvFamilies.Rows[dgvFamilies.RowCount - 1].Selected = true;
                dgvFamilies.FirstDisplayedScrollingRowIndex = dgvFamilies.RowCount - 1;
                this.GanDuLieu();
                blnThem = false;
            }
            else {
                string[] fieldsData = new string[3];
                fieldsData[0] = "DiaChiNha";
                fieldsData[1] = "NgayLapHo";
                fieldsData[2] = "MaAp";
                string[] data = new string[3];
                data[0] = txtAddress.Text;
                data[1] = dtpDateEstablish.Value.ToString();
                data[2] = cbHamlet.SelectedValue.ToString();
                string[] fieldsPlace = new string[1];
                fieldsPlace[0] = "MaHo";
                string[] place = new string[1];
                place[0] = dgvFamilies[0, dgvFamilies.CurrentRow.Index].Value.ToString();
                this.dc.UpdateData(fieldsData, data, fieldsPlace, place, "HoGiaDinh");
                int curRow = dgvFamilies.CurrentRow.Index;
                this.dtFamilies.Rows[curRow][1] = data[0];
                this.dtFamilies.Rows[curRow][2] = data[1];
                this.dtFamilies.Rows[curRow][3] = data[2];
            }
            this.DieuKhienBinhThuong();
        }

        private void btnLuu_Click(object sender, EventArgs e) {
            if (txtAddress.Text == "")
            {
                MessageBox.Show("Lỗi chưa nhập địa chỉ!");
                txtAddress.Focus();
                return;
            }
            if (cbHamlet.SelectedIndex == -1) { 
                MessageBox.Show("Lỗi chưa chọn khóm ấp!");
                cbHamlet.Focus();
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
            string[] fields = new string[1];
            fields[0] = "MaHo";
            string[] data = new string[1];
            data[0] = dgvFamilies[0,dgvFamilies.CurrentRow.Index].Value.ToString();
            if (dc.IsExisted(fields,data,"ThanhVien"))
            {
                MessageBox.Show("Hộ này còn người! Hãy cân nhắc xóa các thành viên trước!");
            }
            else {
                DialogResult dlDongY;
                dlDongY = MessageBox.Show("Bạn thật sự muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dlDongY == DialogResult.Yes)
                {
                    this.dc.DeleteData(fields,data,"HoGiaDinh");
                    this.dtFamilies.Rows.RemoveAt(dgvFamilies.CurrentRow.Index);
                    this.GanDuLieu();
                }    
            }
            this.DieuKhienBinhThuong();
        }

        private void dgvMon_CellClick(object sender, DataGridViewCellEventArgs e) {
            this.GanDuLieu();
        }

        void GanDuLieu() {
            if (this.dtFamilies.Rows.Count > 0)
            {
                txtAddress.Text = dgvFamilies[1, dgvFamilies.CurrentRow.Index].Value.ToString();
                dtpDateEstablish.Value = DateTime.Parse(dgvFamilies[2, dgvFamilies.CurrentRow.Index].Value.ToString());
                cbHamlet.SelectedValue = dgvFamilies[3, dgvFamilies.CurrentRow.Index].Value.ToString();
            }
            else {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                txtAddress.Clear();
                dtpDateEstablish.Value = DateTime.Today;
                cbHamlet.SelectedIndex = -1;
            }
        }

        public frmFamilies()
        {
            InitializeComponent();
        }

        private void frmFamilies_Load(object sender, EventArgs e)
        {
            dc.GetDataSource("HoGiaDinh", this.dtFamilies);
            dc.GetDataSource("KhomAp", this.dtHamlet);
            cbHamlet.DataSource = this.dtHamlet;
            cbHamlet.DisplayMember = "TenAp";
            cbHamlet.ValueMember = "MaAp";
            cbHamlet.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbHamlet.AutoCompleteSource = AutoCompleteSource.ListItems;
            txtAddress.MaxLength = 30;
            dgvFamilies.DataSource = this.dtFamilies;
            dgvFamilies.Width = 430;
            dgvFamilies.AllowUserToAddRows = false;
            dgvFamilies.AllowUserToDeleteRows = false;
            dgvFamilies.Columns[0].Width = 80;
            dgvFamilies.Columns[0].HeaderText = "Mã hộ";
            dgvFamilies.Columns[1].Width = 120;
            dgvFamilies.Columns[1].HeaderText = "Địa chỉ";
            dgvFamilies.Columns[2].Width = 120;
            dgvFamilies.Columns[2].HeaderText = "Ngày lập hộ";
            dgvFamilies.Columns[3].Width = 50;
            dgvFamilies.Columns[3].HeaderText = "Ấp";
            dgvFamilies.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.GanDuLieu();
            this.DieuKhienBinhThuong();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
