using System.ComponentModel.DataAnnotations;

namespace S19L1.DTOs.Student
{
    public class UpdateStudentRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
