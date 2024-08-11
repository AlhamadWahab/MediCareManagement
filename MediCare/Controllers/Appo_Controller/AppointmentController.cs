using AutoMapper;
using BusinessLogicLayer.DTOs.AppointmentDto;
using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using BusinessLogicLayer.ViewModelsDTO;
using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using DomainLayer.Interfaces.Bases_;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediCare.Controllers.Appo_Controller
{
    public class AppointmentController(IRepository repository, IMapper mapper) : Controller
    {
        private readonly IRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        // GET: PatientController
        public async Task<IActionResult> Index()
        {
            IEnumerable<Appointment> appointments = await _repository.AppointmentService.GetAllAsync();

            // Ensure that Doctor and Patient are eagerly loaded
            foreach (var appointment in appointments)
            {
                appointment.Doctor = await _repository.DoctorService.GetByIdAsync(appointment.DoctorId);
                appointment.Patient = await _repository.PatientService.GetByIdAsync(appointment.PatientId);
            }

            IEnumerable<AppointmentDTO> appointmentDTOs = _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            IEnumerable<DP_AppVM> dP_Apps = _mapper.Map<IEnumerable<DP_AppVM>>(appointmentDTOs);
            IEnumerable<DP_AppVMDTO> _AppVMDTOs = _mapper.Map<IEnumerable<DP_AppVMDTO>>(dP_Apps);

            return View(_AppVMDTOs);
        }

        // GET: PatientController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            Appointment appointment = await _repository.AppointmentService.GetByIdAsync(id);
            AppointmentDTO appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            if (appointmentDTO == null)
            {
                return NotFound();
            }
            return View(appointmentDTO);
        }

        // GET: PatientController/Create
        public async Task<IActionResult> Create()
        {
            IEnumerable<Doctor> doctors = await _repository.DoctorService.GetAllAsync();
            IEnumerable<Patient> patients = await _repository.PatientService.GetAllAsync();
            DP_AppVMDTO dP = new DP_AppVMDTO
            {
                Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList(),
                Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };
            return View(dP);
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DP_AppVMDTO dP_App)
        {

            if (ModelState.IsValid)
            {
                IEnumerable<Doctor> doctors = await _repository.DoctorService.GetAllAsync();
                IEnumerable<Patient> patients = await _repository.PatientService.GetAllAsync();
                dP_App.Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();
                dP_App.Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
                return View(dP_App);
            }
            try
            {
                Doctor doctor = await _repository.DoctorService.GetByIdAsync(dP_App.Doctor_Id);
                Patient patient = await _repository.PatientService.GetByIdAsync(dP_App.Patient_Id);
                if (doctor == null || patient == null)
                {
                    ModelState.AddModelError("", "Selected Doctor or Patient not found.");
                    return View(dP_App);
                }
                DoctorDTO doctorDTO = _mapper.Map<DoctorDTO>(doctor);
                PatientDTO patientDTO = _mapper.Map<PatientDTO>(patient);
                Appointment appointment = new Appointment
                {
                    DoctorId = dP_App.Doctor_Id,
                    PatientId = dP_App.Patient_Id,
                    Doctor = doctor,
                    Patient = patient
                };
                await _repository.AppointmentService.AddAsync(appointment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while creating the appointment.");
                return View(dP_App);
            }
        }

        // GET: PatientController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            Appointment appointment = await _repository.AppointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            AppointmentDTO appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);

            return View(appointmentDTO);
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AppointmentDTO appointmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Appointment appointment = _mapper.Map<Appointment>(appointmentDTO);
                    await _repository.AppointmentService.UpdateAsync(id, appointment);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: PatientController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            Appointment appointment = await _repository.AppointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            AppointmentDTO appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            return View(appointmentDTO);
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Appointment appointment = await _repository.AppointmentService.GetByIdAsync(id);
                    if (appointment == null)
                    {
                        return NotFound();
                    }
                    await _repository.AppointmentService.DeleteByIdAsync(id);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
