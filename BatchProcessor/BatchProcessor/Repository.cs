using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchProcessor
{
    class Repository
    {
        private const string CONNECTION_STRING = "Data Source=IEDUB4024176X\\sqlexpress;Initial Catalog=Wedding;Integrated Security=True;Connection Timeout=30;";

        public void AddRecord(string fn, string sn, DateTime cd, string filename)
        {
            string insert = "INSERT INTO Record(firstname,surname,created_date,filename)";
            string values = "VALUES(@firstname,@surname,@created_date,@filename)";

            string query = insert + values;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 20).Value = fn;
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar, 20).Value = sn;
                    cmd.Parameters.Add("@created_date", SqlDbType.DateTime).Value = cd;
                    cmd.Parameters.Add("@filename", SqlDbType.VarChar, 100).Value = filename;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
