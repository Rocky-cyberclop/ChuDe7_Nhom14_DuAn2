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
    public partial class frmInstance : Form
    {
        DataTable Instance = new DataTable("DoiTuong");
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
            dgvInstance.Enabled = true;
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
            dgvInstance.Enabled = false;
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
            dgvInstance.Enabled = false;
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
                dc.InsertData(data, "DoiTuong");
                this.Instance.Rows.Add(data[0],data[1]);
                this.GanDuLieu();
                this.blnAdd = false;
            }
            else
            {
                string[] fieldsData = new string[1];
                fieldsData[0] = "TenDoiTuong";
                string[] data = new string[1];
                data[0] = txtName.Text;
                string[] fieldsPlace=new string[1];
                fieldsPlace[0] = "MaDoiTuong";
                string[] place=new string[1];
                place[0] = txtId.Text;
                dc.UpdateData(fieldsData, data, fieldsPlace, place, "DoiTuong");
                int curRow = dgvInstance.CurrentRow.Index;
                this.Instance.Rows[curRow][0] = txtId.Text;
                this.Instance.Rows[curRow][1] = txtName.Text;
            }
            this.DieuKhienBinhThuong();
        }

        private void btnLuu_Click(object sender, EventArgs e) {
            string[] data = new string[1];
            string[] fields = new string[1];
            if (txtId.Text == "") {
                MessageBox.Show("Lỗi chưa nhập mã đối tượng!");
                txtId.Focus();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Lỗi chưa nhập mô tả đối tượng!");
                txtName.Focus();
                return;
            }
            data[0]=txtId.Text;
            fields[0]="MaDoiTuong";
            if ((blnAdd)&&this.dc.IsExisted(fields, data, "DoiTuong"))
            {
                MessageBox.Show("Mã đối tượng này đã có rồi!");
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
            fields[0] = "MaDoiTuong";
            if (this.dc.IsExisted(fields,data,"ThanhVien"))
            {
                MessageBox.Show("Đối tượng loại này vẫn còn trong xã hội!");
            }
            else {
                DialogResult dlDongY;
                dlDongY = MessageBox.Show("Bạn thật sự muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dlDongY == DialogResult.Yes) {
                    this.dc.DeleteData(fields, data, "DoiTuong");
                    this.Instance.Rows.RemoveAt(dgvInstance.CurrentRow.Index);
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
            if (this.Instance.Rows.Count > 0)
            {
                txtId.Text = dgvInstance[0, dgvInstance.CurrentRow.Index].Value.ToString();
                txtName.Text = dgvInstance[1, dgvInstance.CurrentRow.Index].Value.ToString();
            }
            else
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                txtId.Clear();
                txtName.Clear();
            }
        }

        public frmInstance()
        {
            InitializeComponent();
        }

        private void frmInstance_Load(object sender, EventArgs e)
        {
            this.dc.GetDataSource("DoiTuong", this.Instance);
            dgvInstance.DataSource = this.Instance;
            txtId.MaxLength = 8;
            txtName.MaxLength = 40;
            this.GanDuLieu();
            dgvInstance.Width = 290;
            dgvInstance.Columns[0].Width = 70;
            dgvInstance.Columns[0].HeaderText = "Stt";
            dgvInstance.Columns[1].Width = 175;
            dgvInstance.Columns[1].HeaderText = "Diễn giải";
            dgvInstance.AllowUserToAddRows = false;
            dgvInstance.AllowUserToDeleteRows = false;
            dgvInstance.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.DieuKhienBinhThuong();
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        
    }
}
