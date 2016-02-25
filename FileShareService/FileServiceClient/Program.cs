using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;

namespace FileServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task t = DownloadFile("");
            UploadFile("");
            Console.WriteLine("Continue thread");
            Console.ReadKey();
        }

        private static void UploadFile(string filename)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Please specify file name  with extension you wish to upload and Press Enter :- ");
            string fn = Console.ReadLine();
            string uploadRequestURI = "upload";

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var content = new MultipartFormDataContent())
                    {
                        client.BaseAddress = new Uri("http://localhost:9810/FileService/");
                        var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes("C:\\Uploads\\"+fn));//(System.IO.File.ReadAllBytes(fileName));
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = fn
                        };
                        content.Add(fileContent);
                        var result = client.PostAsync(uploadRequestURI, content).Result;
                    }
                }
            }
            catch (Exception e)
            {
                //Log the exception
            }
        }

        private static async Task DownloadFile(string filename)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Please specify file name  with extension you wish to download and Press Enter :- ");
            string fileName = Console.ReadLine();
            string localDownloadPath = string.Concat(@"c:\", fileName); // the path can be configurable
            bool overWrite = true;
            string actionURL = string.Concat("download?fn=", fileName);

            try
            {
                Console.WriteLine(string.Format("Start downloading @ {0}, {1} time ",
                    DateTime.Now.ToLongDateString(),
                    DateTime.Now.ToLongTimeString()));


                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://localhost:9810/FileService/");
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, actionURL);

                        await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).
                            ContinueWith((response)
                                =>
                            {
                                Console.WriteLine();
                                try
                                {
                                    ProcessDownloadResponse(localDownloadPath, overWrite, response);
                                }
                                catch (AggregateException aggregateException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(string.Format("Exception : ", aggregateException));
                                }
                            });
                    }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }

        private static void ProcessDownloadResponse(string localDownloadFilePath, bool overWrite, 
            Task<HttpResponseMessage> response)
        {
            if (response.Result.IsSuccessStatusCode)
            {
                Task<byte[]> httpStream = response.Result.Content.ReadAsByteArrayAsync();

                using (FileStream filestream = new FileStream(localDownloadFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    filestream.Write(httpStream.Result, 0, httpStream.Result.Length);
                }
            }
        }
    }
}
