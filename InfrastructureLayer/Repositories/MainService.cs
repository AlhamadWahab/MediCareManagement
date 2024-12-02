using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Interfaces.Bases_;
using InfrastructureLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Repositories
{
    public class MainService<T>(MediCareDbContext dbContext) : IService<T>, IDisposable where T : class
    {
        private readonly MediCareDbContext _MediDb = dbContext;
        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            await _MediDb.Set<T>().AddAsync(entity);
            await _MediDb.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteByIdAsync(Guid id)
        {
            T? entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"Entity with ID {id} not found", nameof(id));
            }
            _MediDb.Set<T>().Remove(entity);
            await _MediDb.SaveChangesAsync();
            return entity;
        }

        public void Dispose() => _MediDb.Dispose();

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _MediDb.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _MediDb.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(Guid id, T entity)
        {
            T? existingEntity = await GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException("Entity not Found");
            }
            _MediDb.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _MediDb.SaveChangesAsync();
            return existingEntity;
        }
        // Special Method 

        public async Task<Appointment> GetAppointmentWithDetailsAsync(Guid id)
        {
            return await _MediDb.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsDoctorDetailsAsync(Guid doctorId)
        {
            return await _MediDb.Appointments
                .Include(a => a.Doctor)  
                .Include(a => a.Patient) 
                .Where(a => a.DoctorId == doctorId) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsPatientsDetailsAsync(Guid patientId)
        {
            return await _MediDb.Appointments
                .Include(a => a.Doctor)  
                .Include(a => a.Patient) 
                .Where(a => a.PatientId == patientId) 
                .ToListAsync();
        }

    }
}
