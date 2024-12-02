using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusinessLogicLayer.DTOs.AppointmentDto
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        [Display(Name = "Reason of the visit")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The Name field should only contain alphabetic characters and spaces.")]
        public string? Reason { get; set; }
        [Required]
        public bool IsFirstVisit { get; set; }

        [Required]
        [Display(Name = "Patient Id")]
        [ForeignKey("PatientId")]
        public Guid PatientDTO_Id { get; set; }

        public PatientDTO? Patient { get; set; }

        [Required]
        [Display(Name = "Doctor Id")]
        [ForeignKey("DoctorId")]
        public Guid DoctorDTO_Id { get; set; }

        public DoctorDTO? Doctor { get; set; }
    }
}
