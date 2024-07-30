using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities.Appointment_Model
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(200)]
        [Display(Name = "Reason of the visit")]
        public string? Reason { get; set; }
        [Required]
        public bool IsFirstVisit { get; set; }

        [Required]
        [Display(Name = "Patient Id")]
        public Guid PatientId { get; set; }
        public Patient_Model.Patient? Patient { get; set; }

        [Required]
        [Display(Name = "Patient Id")]
        public Guid DoctorId { get; set; }
        public Doctor_Model.Doctor? Doctor { get; set; }
    }
}
