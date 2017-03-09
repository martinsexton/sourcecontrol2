using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DoneillServer.Models;
namespace DoneillServer.Controllers
{
    [RoutePrefix("WorkflowService")]
    public class HomeController : ApiController
    {
        [Route("Project")]
        [HttpPost]
        public HttpResponseMessage CreateProject()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        [Route("Project")]
        [HttpGet]
        public Project GetProject(int i)
        {
            Project p = new Project();

            p.identifier = 1;
            p.Name = "Test Name";
            p.FromDate = DateTime.Now;
            p.ContactNumber = "087 7943849";
            p.Details = "Details";

            return p;
        }
    }
}
