using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using FileShareClient;

namespace FileServiceClient
{
    class Program
    {
        static IFileShareService service = new FileShareService();

        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                UploadFile("background2.png");
            }
            //UploadFile("background2.png");
            //UploadFile("powerpoint.pptx");
            //UploadFile("pdffile.pdf");

            //DownloadFile("background2.png");
            //DownloadFile("powerpoint.pptx");
            //DownloadFile("pdffile.pdf");

            Console.WriteLine("Continue thread");
            Console.ReadKey();
        }

        private static void UploadFile(string fn)
        {
            byte[] fileContent = System.IO.File.ReadAllBytes("C:\\Uploads\\" + fn);

            Random rnd = new Random();
            bool result = service.uploadFile(fileContent, fn + rnd.Next());

            Console.WriteLine("Result of file upload: " + result);

        }

        private static void DownloadFile(string fileName)
        {
            string localDownloadPath = string.Concat(@"c:\temp_downloads\", fileName); // the path can be configurable
            byte[] fileContent = service.downloadFile(fileName);

            using (FileStream filestream = new FileStream(localDownloadPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                filestream.Write(fileContent, 0, fileContent.Length);
            }
        }
    }
}
