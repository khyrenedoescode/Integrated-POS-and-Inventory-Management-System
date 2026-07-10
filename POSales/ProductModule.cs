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
    public partial class ProductModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string stitle = "Point Of Sales";
        Product product;
        public ProductModule(Product pd)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.myConnection());
            product = pd;
            LoadBrand();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cboCategory.DataSource = dbcon.getTable("SELECT * FROM tbCategory");
            cboCategory.DisplayMember = "category";
            cboCategory.ValueMember = "id";
        }

        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            cboBrand.DataSource = dbcon.getTable("SELECT * FROM tbBrand");
            cboBrand.DisplayMember = "brand";
            cboBrand.ValueMember = "id";
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtPdesc.Clear();
            txtPrice.Clear();
            cboBrand.SelectedIndex = 0;
            cboCategory.SelectedIndex = 0;
            UDReOrder.Value = 1;

            txtPcode.Enabled = true;
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to save this product?", "Save Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlCommand cmd = new SqlCommand("InsertProduct", cn))

                    {
                        cn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductCode", txtPcode.Text);
                        cmd.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                        cmd.Parameters.AddWithValue("@ProductDescription", txtPdesc.Text);
                        cmd.Parameters.AddWithValue("@BrandID", cboBrand.SelectedValue);
                        cmd.Parameters.AddWithValue("@CategoryID", cboCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@Price", double.Parse(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@ReorderLevel", UDReOrder.Value);
                        cmd.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Product added successfully!", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    Clear();
                    product.LoadProduct();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
{
    try
    {
        if (MessageBox.Show("Are you sure want to update this product?", "Update Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            {
                using (SqlCommand cm = new SqlCommand("UpdateProduct", cn))
                {
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@ProductCode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@ProductDescription", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@BrandID", cboBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@CategoryID", cboCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@ReorderLevel", (int)UDReOrder.Value);

                    cn.Open();
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Product has been successfully updated.", stitle);
                }
            }

            Clear();
            this.Dispose();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
}


        private void ProductModule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
