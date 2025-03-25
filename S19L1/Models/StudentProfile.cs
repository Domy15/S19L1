using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace S19L1.Models
{
    public class StudentProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [StringLength(16)]
        public required string FiscalCode { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
