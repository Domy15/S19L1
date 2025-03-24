using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S19L1.Models;
using S19L1.Services;

namespace S19L1.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            var result = await _studentService.CreateStudentAsync(student);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Something went wrong"
                });
            }

            return Ok(new 
            { 
                message = "Student added successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudentsAsync();

            if(students == null)
            {
                return BadRequest(new
                {
                    message = "Something went wrong"
                });
            }

            var count = students.Count();

            var text = count == 1 ? $"{count} student found" : $"{count} students found";

            return Ok(new
            {
                message = text,
                students = students
            });
        }
    }
}
