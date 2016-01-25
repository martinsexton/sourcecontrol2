using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingServices.Interface;
using System.Data.SqlClient;
using System.Data;

namespace WeddingServices.Implementation
{
    public class GuestService : IGuestService
    {
        private const string CONNECTION_STRING = "Server=tcp:martinandorlaweddingserver.database.windows.net,1433;Database=Wedding;User ID=sexton.martin@martinandorlaweddingserver;Password=Sydney19+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private const string CONNECTION_STRING = "Data Source=IEDUB4024176X\\sqlexpress;Initial Catalog=Wedding;Integrated Security=True;Connection Timeout=30;";
        public GuestService()
        {

        }

        IGuest IGuestService.RetrieveGuestByIdentifier(int id)
        {
            Guest g = null;
            string query = "SELECT g.Id, g.firstname, g.surname, g.attending_guest_name, g.email, g.mobile_number, g.status FROM Guest AS g WHERE g.Id = @guest_id";

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@guest_id";
                    param.Value = id;

                    cmd.Parameters.Add(param);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        g = new Guest();
                        g.Id = (int)reader[0];
                        g.Firstname = (string)reader[1];
                        g.Surname = (string)reader[2];
                        g.AttendingGuestName = (string)reader[3];
                        g.Email = (string)reader[4];
                        g.MobileNumber = (string)reader[5];
                        g.Status = (string)reader[6];
                    }
                }
            }

            return g;
        }

        void IGuestService.AddGuest(IGuest guest){
            string query = "INSERT INTO Guest(firstname,surname,email,mobile_number,status,attending_guest_name) " +
                            "VALUES(@firstname,@surname,@email,@mobile,@status,@guest_name)";
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 20).Value = guest.getFirstname();
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar, 20).Value = guest.getSurname();
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = guest.getEmail();
                    cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 50).Value = guest.getMobile();
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = guest.getStatus();
                    cmd.Parameters.Add("@guest_name", SqlDbType.VarChar, 100).Value = guest.getAttendingGuestName();

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        List<IGuest> IGuestService.RetrieveAllGuests()
        {
            List<IGuest> guests = new List<IGuest>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT
                        g.Id
                        ,g.firstname
                        ,g.surname
                        ,g.email
                        ,g.mobile_number
                        ,g.status
                        ,g.attending_guest_name
                    FROM Guest AS g;";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guest g = new Guest();
                        g.Id = reader.GetInt32(0);
                        g.Firstname = reader.GetString(1);
                        g.Surname = reader.GetString(2);
                        g.Email = reader.GetString(3);
                        g.MobileNumber = reader.GetString(4);
                        g.Status = reader.GetString(5);
                        g.AttendingGuestName = reader.GetString(6);

                        guests.Add(g);
                    }
                }
            }
            return guests;
        }

        List<IGuest> IGuestService.RetrieveGuestsByStatus(string status)
        {
            List<IGuest> guests = new List<IGuest>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT
                        g.Id
                        ,g.firstname
                        ,g.surname
                        ,g.email
                        ,g.mobile_number
                        ,g.status
                        ,g.attending_guest_name
                    FROM Guest AS g WHERE g.status = @status;";

                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@status";
                param.Value = status;

                cmd.Parameters.Add(param);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guest g = new Guest();
                        g.Id = reader.GetInt32(0);
                        g.Firstname = reader.GetString(1);
                        g.Surname = reader.GetString(2);
                        g.Email = reader.GetString(3);
                        g.MobileNumber = reader.GetString(4);
                        g.Status = reader.GetString(5);
                        g.AttendingGuestName = reader.GetString(6);

                        guests.Add(g);
                    }
                }
            }
            return guests;
        }
    }
}
