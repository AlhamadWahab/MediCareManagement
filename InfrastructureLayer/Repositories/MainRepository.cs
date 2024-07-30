using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using DomainLayer.Interfaces.Bases_;
using InfrastructureLayer.Data;

namespace InfrastructureLayer.Repositories
{
    public class MainRepository : IRepository
    {
        public MainRepository(MediCareDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DoctorService = new MainService<Doctor>(_dbContext);
            PatientService = new MainService<Patient>(_dbContext);
            AppointmentService = new MainService<Appointment>(_dbContext);
        }
        private readonly MediCareDbContext _dbContext;
        public IService<Doctor> DoctorService { get; set; }
        public IService<Patient> PatientService { get; set; }
        public IService<Appointment> AppointmentService { get; set; }
        public async Task CommitAsync() => await _dbContext.SaveChangesAsync();
        /// <summary>
        /// Disposes the current database context instance.
        /// </summary>
        public void Dispose() => _dbContext.Dispose();
    }
}
