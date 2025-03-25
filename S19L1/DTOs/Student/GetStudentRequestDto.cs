using System.ComponentModel.DataAnnotations;

namespace S19L1.DTOs.Student
{
    public class GetStudentRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
