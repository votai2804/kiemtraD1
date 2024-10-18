using BUS;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        private readonly SinhVienService sinhVienService1 = new SinhVienService();
        private readonly LopService linopService1 = new LopService();
        public frmSinhVien()
        {
            InitializeComponent();
            dgvSinhVien.AutoGenerateColumns = true;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                var listsv = sinhVienService1.GetAll();
                var listlop = linopService1.GetAll();
                filllopcombox(listlop);
                LoadSinhVienData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void filllopcombox(List<Lop> listlop)
        {
            // Đảm bảo danh sách lớp truyền vào là List<Lop>
            cbblop.DataSource = new BindingList<Lop>(listlop); // Sử dụng BindingList nếu cần
            cbblop.DisplayMember = "TenLop";  // Thuộc tính hiển thị
            cbblop.ValueMember = "MaLop";     // Thuộc tính giá trị
        }

        private void LoadSinhVienData()
        {
            var dsSinhVien = sinhVienService1.GetAll();

            dgvSinhVien.Rows.Clear();

            // Duyệt qua danh sách sinh viên và thêm từng sinh viên vào DataGridView
            foreach (var item in dsSinhVien)
            {
                // Thêm một dòng mới và lấy chỉ số của nó
                int index = dgvSinhVien.Rows.Add();

                // Gán các giá trị cho các cột
                dgvSinhVien.Rows[index].Cells[0].Value = item.MaLop;  // Mã sinh viên
                dgvSinhVien.Rows[index].Cells[1].Value = item.HotenSV;   // Họ tên sinh viên
                dgvSinhVien.Rows[index].Cells[2].Value = item.Ngaysinh.ToString("dd/MM/yyyy"); // Ngày sinh (định dạng ngày)
                if (item.Lop != null)
                    dgvSinhVien.Rows[index].Cells[3].Value = item.Lop.TenLop;  // Tên lớp
                else
                    dgvSinhVien.Rows[index].Cells[3].Value = "N/A";
            }

            }

            private void btnthem_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng Sinhvien mới
                Sinhvien sv = new Sinhvien
                {
                    MaSV = txtMssv.Text,
                    HotenSV = txtTen.Text,
                    MaLop = cbblop.SelectedValue.ToString(),
                    Ngaysinh = dtpsSinh.Value
                };

                // Gọi dịch vụ để thêm hoặc cập nhật sinh viên
                sinhVienService1.InsertUpdate(sv);

                // Cập nhật lại DataGridView
                LoadSinhVienData();

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSinhVien.SelectedRows.Count > 0)
                {
                    // Lấy sinh viên từ dòng đang chọn
                    int rowIndex = dgvSinhVien.SelectedRows[0].Index;
                    string maSV = dgvSinhVien.Rows[rowIndex].Cells[0].Value.ToString();

                    // Tạo đối tượng Sinhvien mới với thông tin cập nhật
                    Sinhvien sv = new Sinhvien
                    {
                        MaSV = maSV,
                        HotenSV = txtTen.Text,
                        MaLop = cbblop.SelectedValue.ToString(),
                        Ngaysinh = dtpsSinh.Value
                    };

                    // Gọi dịch vụ để cập nhật sinh viên
                    sinhVienService1.InsertUpdate(sv);

                    // Cập nhật lại DataGridView
                    LoadSinhVienData();

                    MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi sửa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSinhVien.SelectedRows.Count > 0)
                {
                    // Xác nhận việc xóa
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Lấy mã sinh viên từ dòng đang chọn
                        int rowIndex = dgvSinhVien.SelectedRows[0].Index;
                        string maSV = dgvSinhVien.Rows[rowIndex].Cells[0].Value.ToString();

                        // Tìm sinh viên cần xóa
                        Sinhvien sv = sinhVienService1.GetAll().FirstOrDefault(s => s.MaSV == maSV);

                        if (sv != null)
                        {
                            // Gọi dịch vụ để xóa sinh viên
                            sinhVienService1.Delete(sv);

                            // Cập nhật lại DataGridView
                            LoadSinhVienData();

                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xóa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            btnthem_Click(sender, e);
        }

        private void btnkhluu_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            txtMssv.Clear();
            txtTen.Clear();
            cbblop.SelectedIndex = -1;  // Đặt lại ComboBox về trạng thái không chọn
            dtpsSinh.Value = DateTime.Now;
        }
        private void btnthoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void ccchonsv(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng hiện tại mà người dùng nhấn vào
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                // Gán giá trị từ DataGridView vào các điều khiển trên form
                txtMssv.Text = row.Cells[0].Value.ToString();  // Mã sinh viên
                txtTen.Text = row.Cells[1].Value.ToString();   // Họ tên sinh viên
                dtpsSinh.Value = DateTime.Parse(row.Cells[2].Value.ToString());  // Ngày sinh

                // Nếu giá trị lớp không null, đặt giá trị cho ComboBox lớp
                if (row.Cells[3].Value != null)
                {
                    cbblop.Text = row.Cells[3].Value.ToString();  // Lớp học
                }
            }
        }
    }
}
