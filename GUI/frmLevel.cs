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
    public partial class frmLevel : Form
    {
        DataTable dtJob = new DataTable("TrinhDo");
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
            dgvLevel.Enabled = true;
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
            dgvLevel.Enabled = false;
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
            dgvLevel.Enabled = false;
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
                dc.InsertData(data, "TrinhDo");
                this.dtJob.Rows.Add(data[0],data[1]);
                this.GanDuLieu();
                this.blnAdd = false;
            }
            else
            {
                string[] fieldsData = new string[1];
                fieldsData[0] = "DienGiai";
                string[] data = new string[1];
                data[0] = txtName.Text;
                string[] fieldsPlace=new string[1];
                fieldsPlace[0] = "MaTrinhDo";
                string[] place=new string[1];
                place[0] = txtId.Text;
                dc.UpdateData(fieldsData, data, fieldsPlace, place, "TrinhDo");
                int curRow = dgvLevel.CurrentRow.Index;
                this.dtJob.Rows[curRow][0] = txtId.Text;
                this.dtJob.Rows[curRow][1] = txtName.Text;
            }
            this.DieuKhienBinhThuong();
        }

        private void btnLuu_Click(object sender, EventArgs e) {
            string[] data = new string[1];
            string[] fields = new string[1];
            if (txtId.Text == "") {
                MessageBox.Show("Lỗi chưa nhập mã trình độ!");
                txtId.Focus();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Lỗi chưa nhập diễn giải!");
                txtName.Focus();
                return;
            }
            data[0]=txtId.Text;
            fields[0]="MaTrinhDo";
            if ((blnAdd)&&this.dc.IsExisted(fields, data, "TrinhDo"))
            {
                MessageBox.Show("Mã trình độ này đã có rồi!");
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
            fields[0] = "MaTrinhDo";
            if (this.dc.IsExisted(fields,data,"ThanhVien"))
            {
                MessageBox.Show("Trình độ vẫn còn nhiều người có!");
            }
            else {
                DialogResult dlDongY;
                dlDongY = MessageBox.Show("Bạn thật sự muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dlDongY == DialogResult.Yes) {
                    this.dc.DeleteData(fields, data, "TrinhDo");
                    this.dtJob.Rows.RemoveAt(dgvLevel.CurrentRow.Index);
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
                txtId.Text = dgvLevel[0, dgvLevel.CurrentRow.Index].Value.ToString();
                txtName.Text = dgvLevel[1, dgvLevel.CurrentRow.Index].Value.ToString();
            }
            else
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                txtId.Clear();
                txtName.Clear();
            }
        }

        public frmLevel()
        {
            InitializeComponent();
        }

        private void frmLevel_Load(object sender, EventArgs e)
        {
            this.dc.GetDataSource("TrinhDo", this.dtJob);
            dgvLevel.DataSource = this.dtJob;
            txtId.MaxLength = 8;
            txtName.MaxLength = 40;
            this.GanDuLieu();
            dgvLevel.Width = 290;
            dgvLevel.Columns[0].Width = 70;
            dgvLevel.Columns[0].HeaderText = "Stt";
            dgvLevel.Columns[1].Width = 160;
            dgvLevel.Columns[1].HeaderText = "Trình độ";
            dgvLevel.AllowUserToAddRows = false;
            dgvLevel.AllowUserToDeleteRows = false;
            dgvLevel.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.DieuKhienBinhThuong();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        
    }
}
