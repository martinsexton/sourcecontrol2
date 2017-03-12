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


        public void CreateTimesheet(ITimeSheet t)
        {
            string insert = "INSERT INTO dbo.TimeSheet(engineer_name,week_end_date)  output INSERTED.ID ";
            string values = "VALUES(@engineerName,@weekEndDate)";

            string insert2 = "INSERT INTO dbo.TimeSheetItem(timesheet_id,day_of_week,project,start_time,end_time) ";
            string values2 = "VALUES(@timesheetId,@dayOfWeek,@project,@startTime,@endTime)";

            string query = insert + values;
            string query2 = insert2 + values2;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                int timesheetIdentity = 0;

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@engineerName", SqlDbType.VarChar, 50).Value = t.getEngineerName();
                    cmd.Parameters.Add("@weekEndDate", SqlDbType.DateTime).Value = t.getWeekEndDate();

                    conn.Open();
                    timesheetIdentity = (int)cmd.ExecuteScalar();
                    conn.Close();
                }

                foreach(ITimeSheetItem item in t.getItems()){
                    using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                    {
                        cmd2.Parameters.Add("@timesheetId", SqlDbType.VarChar, 50).Value = timesheetIdentity;
                        cmd2.Parameters.Add("@dayOfWeek", SqlDbType.VarChar, 10).Value = item.getDay();
                        cmd2.Parameters.Add("@project", SqlDbType.VarChar, 10).Value = item.getProjectName();
                        cmd2.Parameters.Add("@startTime", SqlDbType.VarChar, 10).Value = item.getStartTime();
                        cmd2.Parameters.Add("@endTime", SqlDbType.VarChar, 10).Value = item.getEndTime();

                        conn.Open();
                        cmd2.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }            
        }


        public List<ITimeSheet> RetrieveTimesheets()
        {
            List<ITimeSheet> timesheets = new List<ITimeSheet>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT
                        ts.Id
                        ,ts.engineer_name
                        ,ts.week_end_date
                    FROM TimeSheet AS ts;";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Timesheet ts = new Timesheet();
                        ts.identifier = reader.GetInt32(0);
                        ts.engineerName = reader.GetString(1);
                        ts.weekEndDate = reader.GetDateTime(2);

                        timesheets.Add(ts);
                    }
                }
            }
            return timesheets;
        }


        public List<ITimeSheetItem> RetrieveTimesheetItems(int timesheetId)
        {
            List<ITimeSheetItem> timesheetitems = new List<ITimeSheetItem>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT
                        tsi.Id
                        ,tsi.timesheet_id
                        ,tsi.day_of_week
                        ,tsi.project
                        ,tsi.start_time
                        ,tsi.end_time
                    FROM timesheetitem AS tsi WHERE tsi.timesheet_id=@timesheetid;";

                conn.Open();

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@timesheetid";
                param.Value = timesheetId;

                cmd.Parameters.Add(param);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TimesheetItem tsi = new TimesheetItem();
                        tsi.identifier = reader.GetInt32(0);
                        tsi.timesheetIdentifier = reader.GetInt32(1);
                        tsi.dayOfWeek = reader.GetString(2);
                        tsi.project = reader.GetString(3);
                        tsi.startTime = reader.GetString(4);
                        tsi.endTime = reader.GetString(4);

                        timesheetitems.Add(tsi);
                    }
                }
            }
            return timesheetitems;
        }
    }
}
