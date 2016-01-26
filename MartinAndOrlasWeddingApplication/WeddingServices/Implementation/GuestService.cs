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
        //private const string CONNECTION_STRING = "Server=tcp:martinandorlaweddingserver.database.windows.net,1433;Database=Wedding;User ID=sexton.martin@martinandorlaweddingserver;Password=Sydney19+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string CONNECTION_STRING = "Data Source=IEDUB4024176X\\sqlexpress;Initial Catalog=Wedding;Integrated Security=True;Connection Timeout=30;";
        public GuestService()
        {

        }

        IGuest IGuestService.RetrieveGuestByName(string firstname, string surname)
        {
            Guest g = null;
            string query = "SELECT g.Id, g.firstname, g.surname, g.attending_guest_name, g.email, g.mobile_number, g.status, g.reference_identifier FROM Guest AS g WHERE g.firstname = @firstname and g.surname = @surname";

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@firstname";
                    param1.Value = firstname;

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@surname";
                    param2.Value = surname;

                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        g = new Guest();
                        g.Id = (int)reader[0];
                        g.Firstname = reader.GetString(1);
                        g.Surname = reader.GetString(2);
                        g.AttendingGuestName = convertNullFieldToEmptyString(reader, 3);
                        g.Email = convertNullFieldToEmptyString(reader, 4);
                        g.MobileNumber = convertNullFieldToEmptyString(reader, 5);
                        g.Status = reader.GetString(6);
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                    }
                }
            }

            return g;        
        }

        IGuest IGuestService.RetrievePartner(int id)
        {
            int partnerReference = 0;
            string query = "select * from relationship where guest_identifier_a = @reference_identifier";

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@reference_identifier", SqlDbType.Int).Value = id;
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        partnerReference = reader.GetInt32(1);
                    }
                }
            }
            IGuest partner = this.getGuestByReferenceIdentifier(partnerReference);
            return partner;
        }

        IGuest IGuestService.RetrieveGuestByIdentifier(int id)
        {
            return getGuestByIdentifier(id);
        }

        void IGuestService.AddGuest(IGuest guest){
            string query = "INSERT INTO Guest(firstname,surname,email,mobile_number,status,attending_guest_name,reference_identifier) " +
                            "VALUES(@firstname,@surname,@email,@mobile,@status,@guest_name,@reference_identifier)";
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 20).Value = guest.getFirstname().Trim();
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar, 20).Value = guest.getSurname();
                    cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = guest.getEmail();
                    cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 50).Value = guest.getMobile();
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = guest.getStatus();
                    cmd.Parameters.Add("@guest_name", SqlDbType.VarChar, 100).Value = guest.getAttendingGuestName();
                    cmd.Parameters.Add("@reference_identifier", SqlDbType.Int).Value = getNextGuestReferenceNumber();


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
                        ,g.reference_identifier
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
                        g.Email = convertNullFieldToEmptyString(reader, 3);
                        g.MobileNumber = convertNullFieldToEmptyString(reader, 4);
                        g.Status = convertNullFieldToEmptyString(reader, 5);
                        g.AttendingGuestName = convertNullFieldToEmptyString(reader, 6);
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));

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
                        ,g.reference_identifier
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
                        g.Email = convertNullFieldToEmptyString(reader, 3);
                        g.MobileNumber = convertNullFieldToEmptyString(reader, 4);
                        g.Status = convertNullFieldToEmptyString(reader, 5);
                        g.AttendingGuestName = convertNullFieldToEmptyString(reader, 6);
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));

                        guests.Add(g);
                    }
                }
            }
            return guests;
        }

        private string convertNullFieldToEmptyString(IDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? null : reader.GetString(index);
        }

        private IGuest getGuestByIdentifier(int id)
        {
            Guest g = null;
            string query = "SELECT g.Id, g.firstname, g.surname, g.attending_guest_name, g.email, g.mobile_number, g.status, g.reference_identifier FROM Guest AS g WHERE g.Id = @guest_id";

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
                        g.Firstname = reader.GetString(1);
                        g.Surname = reader.GetString(2);
                        g.AttendingGuestName = convertNullFieldToEmptyString(reader, 3);
                        g.Email = convertNullFieldToEmptyString(reader, 4);
                        g.MobileNumber = convertNullFieldToEmptyString(reader, 5);
                        g.Status = reader.GetString(6);
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                    }
                }
            }

            return g;
        }

        private IGuest getGuestByReferenceIdentifier(int id)
        {
            Guest g = null;
            string query = "SELECT g.Id, g.firstname, g.surname, g.attending_guest_name, g.email, g.mobile_number, g.status, g.reference_identifier FROM Guest AS g WHERE g.reference_identifier = @reference_identifier";

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@reference_identifier";
                    param.Value = id;

                    cmd.Parameters.Add(param);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        g = new Guest();
                        g.Id = (int)reader[0];
                        g.Firstname = reader.GetString(1);
                        g.Surname = reader.GetString(2);
                        g.AttendingGuestName = convertNullFieldToEmptyString(reader, 3);
                        g.Email = convertNullFieldToEmptyString(reader, 4);
                        g.MobileNumber = convertNullFieldToEmptyString(reader, 5);
                        g.Status = reader.GetString(6);
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                    }
                }
            }

            return g;
        }

        private int getNextGuestReferenceNumber()
        {
            string query = "SELECT Max(reference_identifier) FROM Guest";
            int maxReferenceNumber = 0;
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        maxReferenceNumber = reader.GetInt32(0);
                        break;
                    }
                }
            }
            return ++maxReferenceNumber;
        }
    }
}
