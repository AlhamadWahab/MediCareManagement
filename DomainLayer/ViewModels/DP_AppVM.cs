using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.ViewModels
{
    public class DP_AppVM
    {
        [StringLength(200)]
        [Display(Name = "Reason of the visit")]
        public string? Reason { get; set; }
        public bool IsFirstVisit { get; set; }
        [Display(Name = "Patient Id")]
        public Guid Patient_Id { get; set; }

        [Display(Name = "Doctor Id")]
        public Guid Doctor_Id { get; set; }
        public Doctor Doctor { get; set; } = new Doctor();
        public Patient Patient { get; set; } = new Patient();
        public IEnumerable<SelectListItem> Doctors { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }
    }
}
