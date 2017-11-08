using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDAL
{
    public class QueryExecutor
    {
        public static string ConnectionString
        {
            get
            {
                return "Data Source=DESKTOP-JT1KEIM;Initial Catalog=Firma;User ID=rpppuwp;Password=rpppuwp";
            }
        }

        public static DataTable ExecuteReaderQuery(string queryText)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = queryText;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable result = new DataTable();
                            adapter.Fill(result);
                            return result;
                            
                            //using (SqlDataReader reader = cmd.ExecuteReader())
                            //{
                            //    return reader.GetSchemaTable();
                            //}
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            } catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return null;
            }
        }
    }
}
