using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S19L1.Models;
using S19L1.Services;
using S19L1.DTOs.Student;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequestDto studentDto)
        {
            try
            {
                var result = await _studentService.CreateStudentAsync(studentDto);

                return result ? Ok(new CreateStudentResponseDto() { Message = "Student added successfully" })
                    : BadRequest(new CreateStudentResponseDto() { Message = "Something went wrong!" });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _studentService.GetStudentsAsync();

                if (students == null)
                {
                    return BadRequest(new
                    {
                        message = "Something went wrong"
                    });
                }

                var count = students.Count();

                var text = count == 1 ? $"{count} student found" : $"{count} students found";

                return Ok(new
                GetStudentResponseDto()
                {
                    Message = text,
                    Students = students
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            try
            {
                var result = await _studentService.GetStudentByIdAsync(id);

                return result != null ? Ok(new { message = "Customer found", Student = result })
                    : BadRequest(new { message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(id);

                return result ? Ok(new DeleteStudentResponseDto() { Message = "Student deleted successfully" })
                    : BadRequest(new DeleteStudentResponseDto() { Message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudent([FromQuery] Guid id, [FromBody] UpdateStudentRequestDto student)
        {
            try
            {
                var result = await _studentService.UpdateStudentAsync(id, student);

                return result ? Ok(new UpdateStudentResponseDto() { Message = "Student updated" })
                    : BadRequest(new UpdateStudentResponseDto() { Message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
