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
        //private const string CONNECTION_STRING = "Server=tcp:bigdaydbserver.database.windows.net,1433;Database=BigDay;User ID=martin.sexton@bigdaydbserver;Password=Sydney20+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string CONNECTION_STRING = "Data Source=IEDUB4024176X\\sqlexpress;Initial Catalog=Wedding;Integrated Security=True;Connection Timeout=30;";
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
                        ,g.email
                        ,g.mobile_number
                        ,g.status
                        ,g.reference_identifier
                        ,g.nick_name
                        ,g.diet_comment
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
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                        g.Nickname = convertNullFieldToEmptyString(reader, 7);
                        g.DietComment = convertNullFieldToEmptyString(reader, 8);

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
                        ,g.reference_identifier
                        ,g.nick_name
                        ,g.diet_comment
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
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                        g.Nickname = convertNullFieldToEmptyString(reader, 7);
                        g.DietComment = convertNullFieldToEmptyString(reader, 8);

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
            if (guest.getRelatedGuest() != null)
            {
                if (guest.getRelatedGuest().getIdentifier() > 0)
                {
                    string updateAcceptedGuestQuery = "UPDATE Guest set firstname=@fn, surname=@sn, status=@status WHERE Id=@Id";
                    using (var conn = new SqlConnection(CONNECTION_STRING))
                    {
                        using (SqlCommand cmd = new SqlCommand(updateAcceptedGuestQuery, conn))
                        {
                            cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = guest.getRelatedGuest().getStatus();
                            cmd.Parameters.Add("@fn", SqlDbType.VarChar, 20).Value = guest.getRelatedGuest().getFirstname();
                            cmd.Parameters.Add("@sn", SqlDbType.VarChar, 20).Value = guest.getRelatedGuest().getSurname();
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = guest.getRelatedGuest().getIdentifier();

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                else
                {
                    //Add new user
                    addGuest(guest);
                }
            }
        }

        private void addGuest(IGuest guest)
        {
            int nextRef = getNextGuestReferenceNumber();

            string query = "INSERT INTO Guest(firstname,surname,status,reference_identifier) " +
                            "VALUES(@firstname,@surname,@status,@reference_identifier)";

            string query2 = "INSERT INTO Relationship(guest_identifier_a,guest_identifier_b,relationship) " +
                "VALUES(@identifiera,@identifierb,@relationship)";

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 20).Value = guest.getRelatedGuest().getFirstname().Trim();
                    cmd.Parameters.Add("@surname", SqlDbType.VarChar, 20).Value = guest.getRelatedGuest().getSurname();
                    cmd.Parameters.Add("@status", SqlDbType.VarChar, 50).Value = guest.getRelatedGuest().getStatus();
                    cmd.Parameters.Add("@reference_identifier", SqlDbType.Int).Value = nextRef;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    cmd.Parameters.Add("@identifiera", SqlDbType.VarChar, 20).Value = guest.getReferenceIdentifier();
                    cmd.Parameters.Add("@identifierb", SqlDbType.VarChar, 20).Value = nextRef;
                    cmd.Parameters.Add("@relationship", SqlDbType.VarChar, 50).Value = "Married";

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

        private IGuest getGuestByReferenceIdentifier(int id)
        {
            return getGuestByX("reference_identifier", id);
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
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                        g.Firstname = reader.GetString(reader.GetOrdinal("firstname"));
                        g.Surname = reader.GetString(reader.GetOrdinal("surname"));
                        g.Nickname = convertNullFieldToEmptyString(reader, reader.GetOrdinal("nick_name"));
                        g.DietComment = convertNullFieldToEmptyString(reader, reader.GetOrdinal("diet_comment"));
                        g.Email = convertNullFieldToEmptyString(reader, reader.GetOrdinal("email"));
                        g.MobileNumber = convertNullFieldToEmptyString(reader, reader.GetOrdinal("mobile_number"));
                        g.Status = reader.GetString(reader.GetOrdinal("status"));

                        //If a related guest is identifier then build up model accordingly
                        if (!reader.IsDBNull(reader.GetOrdinal("related_guest_id")))
                        {
                            Relationship relationship = new Relationship();
                            Guest relatedGuest = new Guest();

                            relatedGuest.Id = reader.GetInt32(reader.GetOrdinal("related_guest_id"));
                            relatedGuest.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("related_guest_reference_identifier"));
                            relatedGuest.Firstname = reader.GetString(reader.GetOrdinal("related_guest_firstname"));
                            relatedGuest.Surname = reader.GetString(reader.GetOrdinal("related_guest_surname"));
                            relatedGuest.Nickname = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_nick_name"));
                            relatedGuest.Email = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_email"));
                            relatedGuest.MobileNumber = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_mobile_number"));
                            relatedGuest.Status = reader.GetString(reader.GetOrdinal("related_guest_status"));
                            relatedGuest.DietComment = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_diet_comment"));

                            relationship.RelatedGuest = relatedGuest;
                            g.Relationship = relationship;
                        }
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
                        g.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("reference_identifier"));
                        g.Firstname = reader.GetString(reader.GetOrdinal("firstname"));
                        g.Surname = reader.GetString(reader.GetOrdinal("surname"));
                        g.Nickname = convertNullFieldToEmptyString(reader, reader.GetOrdinal("nick_name"));
                        g.DietComment = convertNullFieldToEmptyString(reader, reader.GetOrdinal("diet_comment"));
                        g.Email = convertNullFieldToEmptyString(reader, reader.GetOrdinal("email"));
                        g.MobileNumber = convertNullFieldToEmptyString(reader, reader.GetOrdinal("mobile_number"));
                        g.Status = reader.GetString(reader.GetOrdinal("status"));

                        //If a related guest is identifier then build up model accordingly
                        if(!reader.IsDBNull(reader.GetOrdinal("related_guest_id"))){
                            Relationship relationship = new Relationship();
                            Guest relatedGuest = new Guest();

                            relatedGuest.Id = reader.GetInt32(reader.GetOrdinal("related_guest_id"));
                            relatedGuest.ReferenceIdentifier = reader.GetInt32(reader.GetOrdinal("related_guest_reference_identifier"));
                            relatedGuest.Firstname = reader.GetString(reader.GetOrdinal("related_guest_firstname"));
                            relatedGuest.Surname = reader.GetString(reader.GetOrdinal("related_guest_surname"));
                            relatedGuest.Nickname = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_nick_name"));
                            relatedGuest.Email = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_email"));
                            relatedGuest.MobileNumber = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_mobile_number"));
                            relatedGuest.Status = reader.GetString(reader.GetOrdinal("related_guest_status"));
                            relatedGuest.DietComment = convertNullFieldToEmptyString(reader, reader.GetOrdinal("related_guest_diet_comment"));

                            relationship.RelatedGuest = relatedGuest;
                            g.Relationship = relationship;
                        }
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
