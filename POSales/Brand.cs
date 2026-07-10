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
    public partial class Brand : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();

        public Brand()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            LoadBrand();
        }

        public void LoadBrand()
        {
            dgvBrand.Rows.Clear();
            int i = 0;

            string connectionString = "Data Source=LAPTOP-V7CATOCU\\SQLEXPRESS;Initial Catalog=MdemyPOSFINAL;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT id, brand FROM dbo.GetBrands() ORDER BY brand", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i++;
                            dgvBrand.Rows.Add(i, reader["id"].ToString(), reader["brand"].ToString());
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BrandModule moduleForm = new BrandModule(this);
            moduleForm.ShowDialog();
        }

        private void dgvBrand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //For update and delete brand by cell click from tbBrand
            string colName = dgvBrand.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tbBrand WHERE id LIKE '" + dgvBrand[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Brand has been successfully deleted.", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else if (colName == "Edit")
            {
                BrandModule brandModule = new BrandModule(this);
                brandModule.lblId.Text = dgvBrand[1, e.RowIndex].Value.ToString();
                brandModule.txtBrand.Text = dgvBrand[2, e.RowIndex].Value.ToString();
                brandModule.btnSave.Enabled = false;
                brandModule.btnUpdate.Enabled = true;
                brandModule.ShowDialog();
            }
            LoadBrand();
        }
    }
}
