using DomainLayer.Entities.Patient_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface IPatientService
    {
        public Task<Patient> SetInsuranceProviderAsync(Patient patient, string provider);
        public Task<Patient> GetInsuranceProviderAsync(Patient patient);
    }
}
