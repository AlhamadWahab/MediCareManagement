using DomainLayer.Entities.Doctor_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IDoctorService
    {
        public Task<Doctor> GetYearOfExperienceAsync(Doctor doctor);
        public Task<string> GetMedicalLicenseNumberAsync(Doctor doctor);
        public Task SetMedicalLicenseNumberAsync(Doctor doctor, string licenseNumber);
    }
}
