using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSales
{
    class DBConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;
        public string myConnection()
        {
            con = @"Data Source=LAPTOP-V7CATOCU\SQLEXPRESS;Initial Catalog=MdemyPOSFINAL;Integrated Security=True";
            return con;
        }

        public DataTable getTable(string qury)
        {
            cn.ConnectionString = myConnection();
            cm = new SqlCommand(qury, cn);
            SqlDataAdapter adapter = new SqlDataAdapter(cm);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

      public void ExecuteQuery(string sql)
    {
        try
        {
            using (SqlConnection cn = new SqlConnection(con))
            {
                cn.Open();
                using (SqlCommand cm = new SqlCommand(sql, cn))
                {
                    cm.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error executing query: " + ex.Message);
        }
    }


        public string getPassword(string username)
        {
            string password = "";
            using (SqlConnection cn = new SqlConnection(myConnection()))
            {
                cn.Open();
                using (SqlCommand cm = new SqlCommand("SELECT password FROM tbUser WHERE username = @username", cn))
                {
                    cm.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            password = dr["password"].ToString();
                        }
                    }
                }
            }
            return password;
        }


        public double ExtractData(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                cn = new SqlConnection();
                cn.ConnectionString = myConnection();
                cn.Open();

                cm = new SqlCommand(sql, cn);

                if (parameters != null)
                {
                    cm.Parameters.AddRange(parameters);
                }

                double data = Convert.ToDouble(cm.ExecuteScalar());

                cn.Close();
                return data;
            }
            catch
            {
                return 0;
            }
        }
    }
}
