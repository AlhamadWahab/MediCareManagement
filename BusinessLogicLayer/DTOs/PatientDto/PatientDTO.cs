using BusinessLogicLayer.DTOs.AppointmentDto;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTOs.PatientDto
{
    public class PatientDTO
    {
        public Guid PatientDTO_Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "The Name field should only contain alphabetic characters.")]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Address { get; set; }
        [Required]
        [Phone]
        public string? Telefon { get; set; }

        public string? Mobile { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Diagnoses { get; set; }
        [Required]
        [MaxLength(50)]
        public string? InsuranceProvider { get; set; }
        public ICollection<AppointmentDTO>? Appointments { get; set; }
    }
}
