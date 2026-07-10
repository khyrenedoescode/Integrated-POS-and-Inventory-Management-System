using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    public partial class Supplier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        public Supplier()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadSupplier();
        }

        public void LoadSupplier()
        {
            dgvSupplier.Rows.Clear();
            int i = 0;

            string connectionString = "Data Source=LAPTOP-V7CATOCU\\SQLEXPRESS;Initial Catalog=MdemyPOSFINAL;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.GetSuppliers()", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i++;
                            dgvSupplier.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());
                        }
                    }
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            SupplierModule supplierModule = new SupplierModule(this);
            supplierModule.ShowDialog();
        }

        private void dgvSupplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSupplier.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                SupplierModule supplierModule = new SupplierModule(this);
                supplierModule.lblId.Text = dgvSupplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                supplierModule.txtSupplier.Text = dgvSupplier.Rows[e.RowIndex].Cells[2].Value.ToString();
                supplierModule.txtAddress.Text = dgvSupplier.Rows[e.RowIndex].Cells[3].Value.ToString();
                supplierModule.txtConPerson.Text = dgvSupplier.Rows[e.RowIndex].Cells[4].Value.ToString();
                supplierModule.txtPhone.Text = dgvSupplier.Rows[e.RowIndex].Cells[5].Value.ToString();
                supplierModule.txtEmail.Text = dgvSupplier.Rows[e.RowIndex].Cells[6].Value.ToString();
                supplierModule.txtFaxNo.Text = dgvSupplier.Rows[e.RowIndex].Cells[7].Value.ToString();

                supplierModule.btnSave.Enabled = false;
                supplierModule.btnUpdate.Enabled = true;
                supplierModule.ShowDialog();
            }
            else if(colName=="Delete")
            {
                if (MessageBox.Show("Delete this record? click yes to confirm", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Delete from tbSupplier where id like '" + dgvSupplier.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully deleted.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
            LoadSupplier();
        }
    }
}
