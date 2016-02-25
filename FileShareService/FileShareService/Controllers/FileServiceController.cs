﻿using System;
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
    public class FileServiceController : ApiController //: System.Web.Mvc.Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FileServiceController));

        [HttpGet]
        [Route("download")]
        public HttpResponseMessage DownloadFile(string fn)
        {
            logger.Info("Entering DownloadFile on FileServiceController");

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
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentLength = info.Length;
                response.Content.Headers.ContentDisposition.FileName = fn;
            }
            return response;
        }

        [Route("upload")]
        [HttpPost]
        public Task<IEnumerable<string>> UploadFile()
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                string fullPath = "C:\\FileShareService\\locker\\";
                var streamProvider = new MultipartFormDataStreamProvider(fullPath);

                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);

                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var info = new FileInfo(i.LocalFileName);
                        return "File uploaded as " + info.FullName + " (" + info.Length + ")";
                    });
                    return fileInfo;

                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
        }
    }
}