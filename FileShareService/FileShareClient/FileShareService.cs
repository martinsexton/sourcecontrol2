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
        private HttpClient client;
        private const string downloadActionURL = "download?fn=";
        private const string uploadActionURL = "upload";

        public FileShareService()
        {
            //Instantiate HttpClient to be used by library
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("http://localhost:9810/FileService/");
        }
        public bool uploadFile(byte[] fileContentAsBytes, string filename)
        {
            bool uploadedFile = false;

            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(fileContentAsBytes);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };
                content.Add(fileContent);
                Task<HttpResponseMessage> result = client.PostAsync(uploadActionURL, content);

                result.Wait();

                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    uploadedFile = true;
                }
            }
            return uploadedFile;
        }

        public byte[] downloadFile(string fileName)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Concat(downloadActionURL, fileName));
            var result = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            //We can add timeout here if we want. However the calling service will have to wait for image to come back,
            //unless we code the calling service using asynch. However the idea of this service is that it can be used
            //for any calling client. Therefore we cant include specific async behaviour in the API
            result.Wait();

            return ProcessDownloadResponse(result);
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
