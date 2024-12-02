using DomainLayer.Entities.Appointment_Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DomainLayer.Entities.Doctor_Model
{
    public class Doctor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string? Specialty { get; set; }

        [MaxLength(100)]
        public string? PraxisAdress { get; set; }

        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Telefon { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(60)]
        public string? MedicalLicenseNumber { get; set; }
        public string? ProfilePicture { get; set; }
        [JsonIgnore]
        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
