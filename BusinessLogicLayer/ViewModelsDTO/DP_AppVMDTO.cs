using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
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
        public DoctorDTO Doctor { get; set; } = new DoctorDTO();
        public PatientDTO Patient { get; set; } = new PatientDTO();
        public IEnumerable<SelectListItem> Doctors { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }
    }
}
