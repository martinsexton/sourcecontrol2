using SignOnRepository.api;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignOnRepository.impl
{
    public class SignOnRepository : ISignOnRepository
    {
        private const string CONNECTION_STRING = "Server=tcp:bigdaydbserver.database.windows.net,1433;Database=BigDay;User ID=martin.sexton@bigdaydbserver;Password=Sydney20+;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private const string CONNECTION_STRING = "Data Source=IEDUB4024176X\\sqlexpress;Initial Catalog=Wedding;Integrated Security=True;Connection Timeout=30;";

        public List<ISignOnRequest> retrieveAuthenticationRequests()
        {
            List<ISignOnRequest> requests = new List<ISignOnRequest>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ar.username
                        ,ar.device_identifier
                        ,ar.success
                        ,ar.longitude
                        ,ar.latitude
                        ,ar.encoded_image
                        ,ar.id
                    FROM authentication_request AS ar;";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ISignOnRequest request = new SignOnRequest();
                        request.setUsername(reader.GetString(0));
                        request.setDeviceIdentifier(reader.GetString(1));
                        request.setSuccess(reader.GetBoolean(2));
                        request.setLongitude(reader.GetString(3));
                        request.setLatitude(reader.GetString(4));
                        request.setBase64Image(reader.GetString(5));
                        request.setId(reader.GetInt32(6));

                        requests.Add(request);
                    }
                }
            }
            return requests;
        }


        public ISignOnRequest retrieveAuthenticationRequestById(int id)
        {
            List<ISignOnRequest> requests = new List<ISignOnRequest>();
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT ar.username
                        ,ar.device_identifier
                        ,ar.success
                        ,ar.longitude
                        ,ar.latitude
                        ,ar.encoded_image
                    FROM authentication_request AS ar WHERE ar.id = @identifier;";

                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@identifier";
                param.Value = id;

                cmd.Parameters.Add(param);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ISignOnRequest request = new SignOnRequest();
                        request.setUsername(reader.GetString(0));
                        request.setDeviceIdentifier(reader.GetString(1));
                        request.setSuccess(reader.GetBoolean(2));
                        request.setLongitude(reader.GetString(3));
                        request.setLatitude(reader.GetString(4));
                        request.setBase64Image(reader.GetString(5));

                        requests.Add(request);
                    }
                }
            }
            return requests.ElementAt(0);
        }
    }
}
