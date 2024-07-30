using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;

namespace DomainLayer.Interfaces.Bases_
{
    public interface IRepository : IDisposable
    {
        public IService<Doctor> DoctorService { get; }
        public IService<Patient> PatientService { get; }
        public IService<Appointment> AppointmentService { get; }
        public Task CommitAsync();
    }
}
