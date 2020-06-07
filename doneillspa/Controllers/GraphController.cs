using doneillspa.DataAccess;
using doneillspa.Models;
using doneillspa.Services;
using doneillspa.Specifications;
using doneillspa.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class GraphController : Controller
    {
        private readonly ITimesheetRepository _repository;
        private ApplicationContext _context;
        private ITimesheetService _timesheetService;

        public GraphController(ApplicationContext context, ITimesheetRepository repository, ITimesheetService timesheetService)
        {
            _repository = repository;
            _context = context;
            _timesheetService = timesheetService;
        }

        [HttpGet]
        [Route("api/projectcost/{code}")]
        public ProjectCostDto GetProjectCosts(string code)
        {
            return _timesheetService.DetermineProjectCosts(code);
        }
    }
}