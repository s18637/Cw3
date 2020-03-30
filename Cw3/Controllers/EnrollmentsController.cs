using Cw3.DAL;
using Cw3.Helpers;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IStudentsDb _dbService;
        public EnrollmentsController(IStudentsDb dbService)
        {
            _dbService = dbService;
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            MyHelper myHelper = _dbService.AddStudent(student);
            if (myHelper.value == 0)
            {
                return StatusCode((int)HttpStatusCode.Created);
            }
            else
            {
                return NotFound(myHelper.message);
            }
        }
    }
}
