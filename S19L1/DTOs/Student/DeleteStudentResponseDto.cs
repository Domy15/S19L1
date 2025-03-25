using System.ComponentModel.DataAnnotations;

namespace S19L1.DTOs.Student
{
    public class DeleteStudentResponseDto
    {
        [Required]
        public string Message { get; set; }
    }
}
