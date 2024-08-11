using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.ViewModelsDTO
{
    public class DP_AppVMDTO
    {
        [StringLength(200)]
        [Display(Name = "Reason of the visit")]
        public string? Reason { get; set; } 
        public bool IsFirstVisit { get; set; } 
        [Display(Name = "Patient Id")]
        public Guid Patient_Id { get; set; }

        [Display(Name = "Doctor Id")]
        public Guid Doctor_Id { get; set; }
        [ValidateNever]
        public DoctorDTO Doctor { get; set; } = new DoctorDTO();
        [ValidateNever]
        public PatientDTO Patient { get; set; } = new PatientDTO();
        [ValidateNever]
        public IEnumerable<SelectListItem> Doctors { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Patients { get; set; }
    }
}
