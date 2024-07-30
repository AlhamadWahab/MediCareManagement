using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Interfaces;

namespace BusinessLogicLayer.Service_Pattern.Doctor_Service
{
    public class DoctorService : IDoctorService
    {
        public Task<string> GetMedicalLicenseNumberAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        public Task<Doctor> GetYearOfExperienceAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        public Task SetMedicalLicenseNumberAsync(Doctor doctor, string licenseNumber)
        {
            throw new NotImplementedException();
        }
    }
}
