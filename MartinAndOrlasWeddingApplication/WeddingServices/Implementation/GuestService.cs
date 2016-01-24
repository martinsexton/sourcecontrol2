using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingServices.Interface;
using System.Data.SqlClient;

namespace WeddingServices.Implementation
{
    public class GuestService : IGuestService
    {
        public GuestService()
        {

        }

        List<IGuest> IGuestService.RetrieveAllGuests()
        {
            List<IGuest> guests = new List<IGuest>();
            using (var conn = new SqlConnection("Server=tcp:martinandorlaweddingserver.database.windows.net,1433;Database=Wedding;User ID=sexton.martin@martinandorlaweddingserver;Password=Sydney19+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT
                        g.Id
                        ,g.firstname
                        ,g.surname
                        ,g.guest_name
                        ,g.email
                        ,g.status
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
                        //Need to setup guest name
                        g.Email = reader.GetString(4);
                        g.Status = reader.GetString(5);

                        guests.Add(g);
                    }
                }
            }
            return guests;
        }
    }
}
