using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RS_01.DtoMappers;
using RS_01.Models;

namespace RS_01.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsApiController : Controller
    {
        private List<Student> _students;

        public StudentsApiController()
        {
            _students = new List<Student>();

            _students.Add(
                new Student(0,"Ante", "Antic", "01123", "aa@pmfst.hr", 280, true)
            );
            _students.Add(
                new Student(1, "Marko", "Markic", "00022", "mm@pmfst.hr", 330, false)
            );
            _students.Add(
                new Student(2, "Ivka", "Ivkic", "1123", "ii@pmfst.hr", 300, true)
            );
        }

        [HttpGet]
        public ActionResult<List<Student>> Get()
        {
            return _students;
        }

        [HttpGet("{id}")]
        public ActionResult<Student> Get(int id)
        {
            return _students.Find(x => x.Id == id);
        }

        [HttpPost("save")]
        public ActionResult Save([FromBody] JObject json)
        {
            var student = StudentDto.FromJson(json);
            _students.Add(student);
            return Ok();
        }
    }
}