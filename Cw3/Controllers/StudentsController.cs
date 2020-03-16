using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IDbService _dbService;
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet]
        public string GetStudent(string orderBy)
        {
            return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            } else if (id == 2)
            {
                return Ok("Nowak");
            }

            return NotFound("nie znaleziono studenta");
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //add to database
            //generate index
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
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