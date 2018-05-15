using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    public class CertificationController : Controller
    {
        private readonly ICertificationRepository _repository;

        public CertificationController(ICertificationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/certification/user/{id}")]
        public IEnumerable<Certification> Get(string id)
        {
            return _repository.GetCertificationsByUserId(id);
        }
    }
}