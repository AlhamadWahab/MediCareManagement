using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("PatientId")]
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Required]
        [Display(Name = "Doctor Id")]
        [ForeignKey("DoctorId")]
        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
