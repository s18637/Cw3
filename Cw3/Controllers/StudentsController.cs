using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cw3.DAL;
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
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            /*_dbService.AddStudent(student);*/
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpGet("info/{id}")]
        public IActionResult GetInfo(string id)
        {
            return Ok(_dbService.GetStudyInfo(id));
        }


        [HttpPut("{id}")]
        public IActionResult UpdateStudent(string Name,int id)
        {
            //find student in database
            //jesli istnieje update, jesli nie to nie znaleziono
            if (id == 1)
            {
                //zmiana imienia studenta
                return Ok("Aktualizacja zakonczona");
            }
            return NotFound("nie znaleziono studneta o danym id");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            //find student in database
            //jesli istnieje to usun, jesli nie to nie znaleziono
            if (id == 1)
            {
                //usuniecie studenta z bazy
                return Ok("Usunieto");
            }

            return NotFound("nie znaleziono studneta o danym id");
        }


    }
}