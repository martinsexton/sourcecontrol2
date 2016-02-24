using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;

namespace FileShareService.Controllers
{
    public class FileServiceController : System.Web.Mvc.Controller
    {
        // GET: FileService
        [HttpGet]
        public System.Web.Mvc.FileResult DownloadFile(string fn)
        //public HttpResponseMessage DownloadFile(string fn)
        {
            //HttpResponseMessage response = new HttpResponseMessage();
            string filename = "C:\\FileShareService\\Files\\Downloads\\" + fn;
            FileInfo info = new FileInfo(filename);
            HttpResponseMessage response = null;

            if (info.Exists)
            {
                response = new HttpResponseMessage();
                response.Headers.AcceptRanges.Add("bytes");
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new StreamContent(new FileStream(filename, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "SampleImg";
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentLength = info.Length;
            }
            //return response;
            return File(response.Content.ReadAsByteArrayAsync().Result, "application/octet-stream", "test.txt");
        }
    }
}