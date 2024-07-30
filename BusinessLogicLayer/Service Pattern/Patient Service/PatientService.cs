using DomainLayer.Entities.Patient_Model;
using DomainLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service_Pattern.Patient_Service
{
    public class PatientService : IPatientService
    {
        public Task<Patient> GetInsuranceProviderAsync(Patient patient)
        {
            throw new NotImplementedException();
        }

        public Task<Patient> SetInsuranceProviderAsync(Patient patient, string provider)
        {
            throw new NotImplementedException();
        }
    }
}
