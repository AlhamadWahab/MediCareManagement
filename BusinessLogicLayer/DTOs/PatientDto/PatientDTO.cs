using BusinessLogicLayer.DTOs.AppointmentDto;
using MediCareSecurity_IdentityManagementLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.DTOs.PatientDto
{
    public class PatientDTO 
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Telefon { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Mobile { get; set; }

        [MaxLength(200)]
        public string? Diagnoses { get; set; }

        [MaxLength(50)]
        public string? InsuranceProvider { get; set; }
        [JsonIgnore]
        public ICollection<AppointmentDTO>? Appointments { get; set; }
    }
}
