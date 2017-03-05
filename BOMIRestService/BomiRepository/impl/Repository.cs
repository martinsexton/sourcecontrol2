using BomiRepository.api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomiRepository
{
    public class Repository : IRepository
    {
        private const string CONNECTION_STRING = "Server=tcp:bigdaydbserver.database.windows.net,1433;Database=BigDay;User ID=martin.sexton@bigdaydbserver;Password=Sydney20+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void recordAuthenticationRequest(IAuthenticationRequest request)
        {
            addRequest(request);
        }

        private void addRequest(IAuthenticationRequest request)
        {
            string insert = "INSERT INTO authentication_request(username,device_identifier,success";
            string values = "VALUES(@username,@device_identifier,@success";

            if (!request.getLongitude().Equals(""))
            {
                insert += ",longitude";
                values += ",@longitude";
            }
            if (!request.getLatitude().Equals(""))
            {
                insert += ",latitude";
                values += ",@latitude";
            }
            if (!request.getBase64Image().Equals(""))
            {
                insert += ",encoded_image";
                values += ",@encoded_image";
            }
            insert += ")";
            values += ")";

            string query = insert + values;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@username", SqlDbType.VarChar, 20).Value = request.getUsername().Trim();
                    cmd.Parameters.Add("@device_identifier", SqlDbType.VarChar, 200).Value = request.getDeviceIdentifier().Trim();
                    cmd.Parameters.Add("@success", SqlDbType.Bit).Value = request.getSuccess();
                    if (!request.getLongitude().Equals(""))
                    {
                        cmd.Parameters.Add("@longitude", SqlDbType.VarChar, 100).Value = request.getLongitude().Trim();
                    }
                    if (!request.getLatitude().Equals(""))
                    {
                        cmd.Parameters.Add("@latitude", SqlDbType.VarChar, 100).Value = request.getLatitude().Trim();
                    }
                    if (!request.getBase64Image().Equals(""))
                    {
                        cmd.Parameters.Add("@encoded_image", SqlDbType.VarChar).Value = request.getBase64Image();
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
