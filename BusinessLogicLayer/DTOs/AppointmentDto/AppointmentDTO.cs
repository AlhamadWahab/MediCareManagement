using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using System.ComponentModel.DataAnnotations;


namespace BusinessLogicLayer.DTOs.AppointmentDto
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        [Display(Name = "Reason of the visit")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "The Name field should only contain alphabetic characters.")]
        public string? Reason { get; set; }
        [Required]
        public bool IsFirstVisit { get; set; }

        [Required]
        [Display(Name = "Patient Id")]
        public Guid PatientDTO_Id { get; set; }

        public PatientDTO? Patient { get; set; }

        [Required]
        [Display(Name = "Doctor Id")]
        public Guid DoctorDTO_Id { get; set; }

        public DoctorDTO? Doctor { get; set; }
    }
}
