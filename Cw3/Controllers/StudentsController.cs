using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cw3.DAL;
using Cw3.Helpers;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsDb _dbService;
        public StudentsController(IStudentsDb dbService)
        {
            _dbService = dbService;
        }
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            return Ok(_dbService.GetStudent(id));
        }
        [HttpPost("{studia}")]
        public IActionResult CreateStudent(Student student, string studia)
        {
            MyHelper helper = _dbService.AddStudent(student, studia);
            /*_dbService.AddStudent(student);*/
            if (helper.value == 0)
            {
                return StatusCode((int)HttpStatusCode.Created, helper.enrollment);
            }
            return NotFound(helper.message);
        }
        [HttpGet("info/{id}")]
        public IActionResult GetInfo(string id)
        {
            return Ok(_dbService.GetStudyInfo(id));
        }

        [HttpGet("linq")]
        public IActionResult GetList()
        {
            return Ok(_dbService.GetList());
        }


        [HttpPut("update")]
        public IActionResult UpdateStudent(Student student)
        {
            MyHelper help = _dbService.UpdateStudent(student);
            if (help.value == -1)
            {
                return NotFound(help.message);
            }
            return Ok(help.message);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(string id)
        {
            MyHelper helper = _dbService.DeleteStudent(id);
            if (helper.value == -1)
            {
                return NotFound(helper.message);
            }
            return Ok(helper.message);
        }


    }
}