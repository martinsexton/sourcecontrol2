using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FileShareService.Controllers
{
    [RoutePrefix("FileService")]
    public class FileServiceController : ApiController
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FileServiceController));

        [HttpGet]
        [Route("download")]
        public HttpResponseMessage DownloadFile(string fn)
        {
            logger.Info("Entering DownloadFile on FileServiceController");

            string filename = "C:\\FileShareService\\locker\\" + fn;
            FileInfo info = new FileInfo(filename);
            HttpResponseMessage response = null;

            if (info.Exists)
            {
                response = new HttpResponseMessage();
                response.Headers.AcceptRanges.Add("bytes");
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Content = new StreamContent(new FileStream(filename, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentLength = info.Length;
                response.Content.Headers.ContentDisposition.FileName = fn;
            }
            return response;
        }

        [Route("upload")]
        [HttpPost]
        public Task<HttpResponseMessage> UploadFile()
        {
            logger.Info("Entering UploadFile on FileServiceController");

            if (Request.Content.IsMimeMultipartContent())
            {
                string fullPath = "C:\\FileShareService\\locker\\";
                var streamProvider = new CustomMultipartFormDataStreamProvider(fullPath);
                

                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);

                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return response;

                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }
}