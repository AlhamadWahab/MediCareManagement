using DomainLayer.Entities.Appointment_Model;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities.Doctor_Model
{
    public class Doctor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "The Name field should only contain alphabetic characters.")]
        public string? Name { get; set; }
        [Required]
        [StringLength(100)]
        public string? Specialty { get; set; }

        [Required]
        [MaxLength(100)]
        public string? PraxisAdress { get; set; }
        [Required]
        [Phone]
        public string? Telefon { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(60)]
        public string? MedicalLicenseNumber { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
