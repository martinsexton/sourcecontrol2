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
        private const string CONNECTION_STRING = "Server=tcp:bigdaydbserver.database.windows.net,1433;Database=BigDay;User ID=martin.sexton@bigdaydbserver;Password=Sydney20+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private const string CONNECTION_STRING = "Data Source=IEDUB4024176X\\sqlexpress;Initial Catalog=Wedding;Integrated Security=True;Connection Timeout=30;";
        public GuestService()
        {

        }

        IGuest IGuestService.RetrieveGuestByName(string firstname, string surname)
        {
            return getGuestByName(firstname, surname);
        }

        IGuest IGuestService.RetrieveGuestByIdentifier(int id)
        {
            return getGuestByIdentifier(id);
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
                        ,g.diet_comment
                        ,g.status
                        ,g.guest_name
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
                        g.DietComment = convertNullFieldToEmptyString(reader, 3);
                        g.Status = convertNullFieldToEmptyString(reader, 4);
                        g.GuestName = convertNullFieldToEmptyString(reader, 5);

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
                        ,g.diet_comment
                        ,g.status
                        ,g.guest_name
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
                        g.DietComment = convertNullFieldToEmptyString(reader, 3);
                        g.Status = convertNullFieldToEmptyString(reader, 4);
                        g.GuestName = convertNullFieldToEmptyString(reader, 5);

                        guests.Add(g);
                    }
                }
            }
            return guests;
        }

        void IGuestService.AddGuest(IGuest guest)
        {
            addGuest(guest);
        }

        void IGuestService.UpdateGuest(IGuest guest)
        {
            string query = "UPDATE Guest set status=@status, diet_comment=@diet_comment WHERE id = @id";
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = guest.getStatus();
                    cmd.Parameters.Add("@diet_comment", SqlDbType.VarChar, 50).Value = guest.getDietComment();
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = guest.getIdentifier();

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private void addGuest(IGuest guest)
        {
            string insert = "INSERT INTO Guest(firstname,surname,status";
            string values = "VALUES(@firstname,@surname,@status";

            if(guest.getDietComment() != null){
                insert += ",diet_comment";
                values += ",@diet_comment";
            }
            if (guest.getGuestName() != null)
            {
<<<<<<< HEAD
                insert += ",diet_comment";
=======
                insert += ",guest_name";
>>>>>>> c3571c688adaa767c8b3384d6b31352b4edcacfe
                values += ",@guest_name";
            }
            insert += ")";
            values += ")";

            string query = insert + values;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 20).Value = guest.getFirstname().Trim();
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar, 20).Value = guest.getSurname();
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = guest.getStatus();
                    if (guest.getDietComment() != null)
                    {
                        cmd.Parameters.Add("@diet_comment", SqlDbType.VarChar, 50).Value = guest.getDietComment();
                    }
                    if (guest.getGuestName() != null)
                    {
                        cmd.Parameters.Add("@guest_name", SqlDbType.VarChar, 50).Value = guest.getGuestName();
                    }
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        private string convertNullFieldToEmptyString(IDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? null : reader.GetString(index);
        }

        private IGuest getGuestByIdentifier(int id)
        {
            return getGuestByX("Id", id);
        }

        private IGuest getGuestByName(string firstname, string surname)
        {
            Guest g = null;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("RetrieveGuestInformationByName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@firstname";
                    param.Value = firstname;

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@surname";
                    param2.Value = surname;

                    cmd.Parameters.Add(param);
                    cmd.Parameters.Add(param2);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        g = new Guest();
                        g.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        g.Firstname = reader.GetString(reader.GetOrdinal("firstname"));
                        g.Surname = reader.GetString(reader.GetOrdinal("surname"));
                        g.DietComment = convertNullFieldToEmptyString(reader, reader.GetOrdinal("diet_comment"));
                        g.Status = reader.GetString(reader.GetOrdinal("status"));
                        g.GuestName = reader.GetString(reader.GetOrdinal("guest_name"));
                    }
                }
            }

            return g;
        }

        private IGuest getGuestByX(string filterColumn, int identifier)
        {
            Guest g = null;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("RetrieveGuestInformation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@" + filterColumn;
                    param.Value = identifier;

                    cmd.Parameters.Add(param);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        g = new Guest();
                        g.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        g.Firstname = reader.GetString(reader.GetOrdinal("firstname"));
                        g.Surname = reader.GetString(reader.GetOrdinal("surname"));
                        g.DietComment = convertNullFieldToEmptyString(reader, reader.GetOrdinal("diet_comment"));
                        g.Status = reader.GetString(reader.GetOrdinal("status"));
                        g.GuestName = convertNullFieldToEmptyString(reader, reader.GetOrdinal("guest_name"));
                    }
                }
            }

            return g;
        }
    }
}
