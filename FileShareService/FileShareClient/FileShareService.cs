using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FileShareClient
{
    public class FileShareService : IFileShareService
    {
        public bool uploadFile(byte[] fileContentAsBytes, string filename)
        {
            bool uploadedFile = false;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var content = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri("http://localhost:9810/FileService/");
                    var fileContent = new ByteArrayContent(fileContentAsBytes);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = filename
                    };
                    content.Add(fileContent);
                    Task<HttpResponseMessage> result = client.PostAsync("upload", content);

                    result.Wait();

                    HttpResponseMessage response = result.Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        uploadedFile = true;
                    }
                    else
                    {
                        uploadedFile = false;
                    }
                }
            }
            return uploadedFile;
        }

        public byte[] downloadFile(string fileName)
        {
            byte[] fileContent = null;

            using (HttpClient httpClient = new HttpClient())
            {
                string actionURL = string.Concat("download?fn=", fileName);
                httpClient.BaseAddress = new Uri("http://localhost:9810/FileService/");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, actionURL);

                var result = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                //We can add timeout here if we want. However the calling service will have to wait for image to come back,
                //unless we code the calling service using asynch. However the idea of this service is that it can be used
                //for any calling client. Therefore we cant include specific async behaviour in the API
                result.Wait();

                fileContent = ProcessDownloadResponse(result);

                /*var result = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).
                        ContinueWith((response)
                            =>
                        {
                            fileContent = ProcessDownloadResponse(response);
                        });
                }
                return fileContent;*/
            }
            return fileContent;
        }


        private static byte[] ProcessDownloadResponse(Task<HttpResponseMessage> response)
        {
            if (response.Result.IsSuccessStatusCode)
            {
                return response.Result.Content.ReadAsByteArrayAsync().Result;
            }
            else
            {
                return null;
            }
        }
    }
}
