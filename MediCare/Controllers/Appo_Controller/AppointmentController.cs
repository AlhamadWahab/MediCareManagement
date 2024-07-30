using AutoMapper;
using BusinessLogicLayer.DTOs.AppointmentDto;
using DomainLayer.Entities.Appointment_Model;
using DomainLayer.Interfaces.Bases_;
using Microsoft.AspNetCore.Mvc;

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
            IEnumerable<AppointmentDTO> appointmentDTOs = _mapper.Map<List<AppointmentDTO>>(appointments);
            return View(appointmentDTOs);
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Address, Telefon, Mobile, " +
            "Email, Diagnoses, InsuranceProvider")] AppointmentDTO appointmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Appointment appointment = _mapper.Map<Appointment>(appointmentDTO);
                    await _repository.AppointmentService.AddAsync(appointment);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name, Address, Telefon, Mobile, " +
            "Email, Diagnoses, InsuranceProvider")] AppointmentDTO appointmentDTO)
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
