using persistancelayer.api;
using persistancelayer.api.model;
using persistancelayer.impl.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer
{
    public class PersistanceLayer : IPersistanceLayer
    {
        private const string CONNECTION_STRING = "Server=tcp:denisoneill.database.windows.net,1433;Database=denisoneill;User ID=doneill;Password=Y3llowsub;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void CreateProject(IProject p)
        {
            string insert = "INSERT INTO dbo.Project(name,start_date,contact_number,details) ";
            string values = "VALUES(@name,@startdate,@contactnumber,@details)";
            string query = insert + values;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = p.getName();
                    cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = p.getStartDate();
                    cmd.Parameters.Add("@contactnumber", SqlDbType.VarChar, 50).Value = p.getContactNumber();
                    cmd.Parameters.Add("@details", SqlDbType.VarChar, 1000).Value = p.getDetails();

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }


        public List<IProject> RetrieveProjects()
        {
            List<IProject> projects = new List<IProject>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT
                        p.Id
                        ,p.name
                        ,p.start_date
                        ,p.contact_number
                        ,p.details
                    FROM Project AS p;";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Project p = new Project();
                        p.Id = reader.GetInt32(0);
                        p.Name = reader.GetString(1);
                        p.StartDate = reader.GetDateTime(2);
                        p.ContactNumber = reader.GetString(3);
                        p.Details = reader.GetString(4);

                        projects.Add(p);
                    }
                }
            }
            return projects;
        }
    }
}
