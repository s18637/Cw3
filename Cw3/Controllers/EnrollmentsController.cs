using Cw3.DAL;
using Cw3.DTOs;
using Cw3.Helpers;
using Cw3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        string SqlCon = "Data Source=db-mssql;Initial Catalog=s18637;Integrated Security=True";


        private readonly IStudentsDb _dbService;
        private IConfiguration Configuration;
        public EnrollmentsController(IStudentsDb dbService, IConfiguration configuration)
        {
            Configuration = configuration;
            _dbService = dbService;
        }
        [HttpPost("reToken/{reToken}")]
        /*[Authorize]*/
        public IActionResult reToken(string reToken)
        {
      
            var refreshToken = Guid.NewGuid();
            JwtSecurityToken token = null;
            using (var client = new SqlConnection(SqlCon))
            {
                using(var command = new SqlCommand())
                {
                    command.Connection = client;
                    client.Open();
                    command.CommandText = "Select IndexNumber, FirstName, LastName from student where reToken = @reToken";
                    command.Parameters.AddWithValue("reToken", reToken);
                    var dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        return BadRequest("Wrong refresh token");
                    }
                    var index = dr[0].ToString();
                    var imie = dr[1].ToString();
                    var nazwisko = dr[2].ToString();
                    string id = imie + " " + nazwisko;
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, index),
                        new Claim(ClaimTypes.Name, id),
                        new Claim(ClaimTypes.Role, "employee"),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    token = new JwtSecurityToken
                    (
                        issuer: "s18637",
                        audience: "Students",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: creds
                    );
                    dr.Close();
                    command.CommandText = "update Student set reToken = @reToken2 where IndexNumber=@login;";
                    command.Parameters.AddWithValue("reToken2", refreshToken);
                    command.Parameters.AddWithValue("login", index);
                    command.ExecuteNonQuery();

                }
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });
        }
        //metoda pomocnicza
        [HttpGet("update")]
        public IActionResult updatePass()
        {
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    client.Open();
                    command.CommandText = "Select haslo, indexNumber from student;";
                    var dr = command.ExecuteReader();
                    ICollection<string[]> colection = new List<string[]>();
                    while (dr.Read())
                    {
                        byte[] random = new byte[128 / 8];
                        string slat;
                        using(var generator = RandomNumberGenerator.Create())
                        {
                            generator.GetBytes(random);
                            slat = Convert.ToBase64String(random);
                        }
                        var haslo = dr[0].ToString();
                        var index = dr[1].ToString();
                        var valueBytes = KeyDerivation.Pbkdf2(
                            password: haslo,
                            salt: Encoding.UTF8.GetBytes(slat),
                            prf: KeyDerivationPrf.HMACSHA512,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8);
                        string[] list = new string[3];
                        list[0] = index;
                        list[1] = slat;
                        list[2] = Convert.ToBase64String(valueBytes);
                        colection.Add(list);


                    }
                    dr.Close();
                    foreach (string [] list in colection)
                    {
                        using (var com = new SqlCommand())
                        {
                            com.Connection = client;
                            com.CommandText = "update student set haslo = @newVal where indexNumber=@index; update student set salt = @salt where indexNumber=@index;";
                            com.Parameters.AddWithValue("newVal", list[2]);
                            com.Parameters.AddWithValue("index", list[0]);
                            com.Parameters.AddWithValue("salt", list[1]);
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
                    return Ok();
        }
        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            var refreshToken = Guid.NewGuid();
            JwtSecurityToken token = null;
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "Select FirstName, LastName, Haslo, salt from student where IndexNumber = @login";
                    command.Parameters.AddWithValue("login", request.Login);
                    client.Open();
                    var dr = command.ExecuteReader();
                    if (!dr.Read())
                    {
                        return NotFound("nie znaleziono takiego studenta");
                    }
                    var imie = dr[0].ToString();
                    var nazwisko = dr[1].ToString();
                    var haslo = dr[2].ToString();
                    var slat = dr[3].ToString();
                    var valueBytes = KeyDerivation.Pbkdf2(
                            password: request.Haslo,
                            salt: Encoding.UTF8.GetBytes(slat),
                            prf: KeyDerivationPrf.HMACSHA512,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8);
                    if (!haslo.Equals(Convert.ToBase64String(valueBytes)))
                    {
                        return BadRequest("Wrong password");
                    }
                    string id = imie + " " + nazwisko;
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, request.Login),
                        new Claim(ClaimTypes.Name, id),
                        new Claim(ClaimTypes.Role, "employee"),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    token = new JwtSecurityToken
                    (
                        issuer: "s18637",
                        audience: "Students",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: creds
                    );
                    dr.Close();
                    command.CommandText = "update Student set reToken = @reToken where IndexNumber=@login;";
                    command.Parameters.AddWithValue("reToken", refreshToken);
                    command.ExecuteNonQuery();
                }
            }
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });
        }
        //metoda do sprawdzania czy działa
        [HttpGet("test/{id}")]
        [Authorize(Roles = "employee")]
        public Student GetStudent(string id)
        {
            var st = new Student();
            using (var client = new SqlConnection(SqlCon))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT * FROM STUDENT WHERE IndexNumber=@id";
                    command.Parameters.AddWithValue("id", id);
                    client.Open();
                    var dr = command.ExecuteReader();
                    dr.Read();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                }
            }
            return st;
        }

        [HttpPost("{studia}")]
        [Authorize(Roles = "employee")]
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
        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult poromote(Enrollment se)
        {
            MyHelper myHelper = _dbService.Promote(se);
            if (myHelper.value == 0)
            {
                return StatusCode((int)HttpStatusCode.Created, myHelper.enrollment);
            }
            else
            {
                return NotFound(myHelper.message);
            }
        }
    }
}
