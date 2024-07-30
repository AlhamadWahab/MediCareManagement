using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class DoctorPatient
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid DoctorId { get; set; }
        [Required]
        public Guid PatientId { get; set; }
        public Doctor_Model.Doctor? Doctor { get; set; }
        public Patient_Model.Patient? Patient { get; set; }
    }
}
