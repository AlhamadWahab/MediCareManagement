using DomainLayer.Entities.Appointment_Model;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities.Patient_Model
{
    public class Patient
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "The Name field should only contain alphabetic characters.")]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Address { get; set; }
        [Required]
        [Phone]
        public string Telefon { get; set; } = string.Empty;

        public string? Mobile { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Diagnoses { get; set; }
        [Required]
        [MaxLength(50)]
        public string? InsuranceProvider { get; set; }
        public ICollection<DoctorPatient> DoctorPatients { get; set; }
            = new List<DoctorPatient>();
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
