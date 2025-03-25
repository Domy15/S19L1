using System.ComponentModel.DataAnnotations;

namespace S19L1.DTOs.Student
{
    public class GetStudentResponseDto
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public List<GetStudentRequestDto> Students { get; set; }
    }
}
