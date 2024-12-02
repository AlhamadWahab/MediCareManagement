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
using MediCareSecurity_IdentityManagementLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MediCare.Controllers.Appo_Controller
{
    public class AppointmentController(IRepository repository, IMapper mapper) : Controller
    {
        private readonly IRepository _repository = repository;
        private readonly IMapper _mapper = mapper;



        // GET: PatientController
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
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


        // GET: PatientController/Create
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Doctor> doctors = await _repository.DoctorService.GetAllAsync();
            IEnumerable<Patient> patients = await _repository.PatientService.GetAllAsync();
            DP_AppVMDTO dP = new DP_AppVMDTO
            {
                Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                }).ToList(),
                Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName}"
                }).ToList()
            };
            await _repository.CommitAsync();
            return View(dP);
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
        public async Task<IActionResult> Create(DP_AppVMDTO dP_App)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<Doctor> doctors = await _repository.DoctorService.GetAllAsync();
                IEnumerable<Patient> patients = await _repository.PatientService.GetAllAsync();
                dP_App.Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = $"{d.FirstName} {d.LastName}"
                }).ToList();
                dP_App.Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.FirstName} {p.LastName}"
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
                    Patient = patient,
                    IsFirstVisit = dP_App.IsFirstVisit,
                    Reason = dP_App.Reason
                };
                await _repository.AppointmentService.AddAsync(appointment);
                await _repository.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while creating the appointment.");
                return View(dP_App);
            }
        }

        /// <summary>
        /// Retrieves a specific appointment with details.
        /// </summary>
        /// <param name="id">The unique identifier of the appointment.</param>
        /// <returns>An IActionResult containing the appointment details or a NotFound response.</returns>
        [HttpGet]
        [Authorize(Roles = UserRole.ManagerRole)]
        public async Task<IActionResult> GetSpecialAppointmentById(Guid id)
        {
            var appointment = await _repository.AppointmentService.GetAppointmentWithDetailsAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }
            AppointmentDTO dTO = _mapper.Map<AppointmentDTO>(appointment);

            return Ok(dTO);
        }

        /// <summary>
        /// Retrieves all appointments for a specific doctor.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <returns>An IActionResult containing a list of appointments or a NotFound response.</returns>
        [HttpGet]
        [Authorize(Roles = UserRole.DoctorRole)]
        public async Task<IActionResult> GetAppointmentsForDoctor()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(doctorId))
            {
                Unauthorized();
            }
                var appointments = await _repository.AppointmentService.GetAppointmentsDoctorDetailsAsync(Guid.Parse(doctorId));

            if (appointments == null || !appointments.Any())
            {
                return NotFound();
            }
            IEnumerable<AppointmentDTO> dTOs = _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            return View(dTOs);
        }


        /// <summary>
        /// Retrieves all appointments for a specific patient.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <returns>An IActionResult containing a list of appointments or a NotFound response.</returns>
        [HttpGet]
        [Authorize(Roles = UserRole.PatientRole)]
        public async Task<IActionResult> GetAppointmentsForPatient()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(patientId)) { Unauthorized(); }
                
            var appointments = await _repository.AppointmentService.GetAppointmentsPatientsDetailsAsync(Guid.Parse(patientId));

            if (appointments == null || !appointments.Any())
            {
                return NotFound();
            }
            IEnumerable<AppointmentDTO> dTOs = _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            return View(dTOs);
        }


        // GET: PatientController/Edit/5
        [Authorize(Roles = UserRole.ManagerRole)]
        public async Task<IActionResult> Edit(Guid id)
        {
            Appointment appointment = await _repository.AppointmentService.GetAppointmentWithDetailsAsync(id);
            AppointmentDTO appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            if (appointmentDTO == null)
            {
                return NotFound();
            }
            return View(appointmentDTO);
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.ManagerRole)]
        public async Task<IActionResult> Edit(Guid id, AppointmentDTO appointmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Appointment existingAppointment = await _repository.AppointmentService.GetAppointmentWithDetailsAsync(id);
                    if (existingAppointment == null)
                    {
                        return NotFound();
                    }

                    existingAppointment.Patient.LastName = appointmentDTO.Patient.LastName;
                    existingAppointment.Patient.FirstName = appointmentDTO.Patient.FirstName;
                    //existingAppointment.Patient.InsuranceProvider = appointmentDTO.Patient.InsuranceProvider;
                    existingAppointment.Doctor.FirstName = appointmentDTO.Doctor.FirstName;
                    existingAppointment.Doctor.LastName = appointmentDTO.Doctor.LastName;
                    //existingAppointment.Doctor.Specialty = appointmentDTO.Doctor.Specialty;
                    existingAppointment.IsFirstVisit = appointmentDTO.IsFirstVisit;
                    existingAppointment.Reason = appointmentDTO.Reason;

                    await _repository.AppointmentService.UpdateAsync(id, existingAppointment);
                    await _repository.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(appointmentDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(appointmentDTO);
            }
        }

        // GET: PatientController/Delete/5
        [Authorize(Roles = UserRole.ManagerRole)]
        public async Task<IActionResult> Delete(Guid id)
        {
            Appointment appointment = await _repository.AppointmentService.GetAppointmentWithDetailsAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            AppointmentDTO appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            await _repository.CommitAsync();
            return View(appointmentDTO);
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.ManagerRole)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                Appointment appointment = await _repository.AppointmentService.GetAppointmentWithDetailsAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    await _repository.AppointmentService.DeleteByIdAsync(id);
                    await _repository.CommitAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the appointment: {ex.Message}");
                return View();
            }
        }


        #region Api Calls
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
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

            return Json(new { data = _AppVMDTOs });
        }

        #endregion

    }
}