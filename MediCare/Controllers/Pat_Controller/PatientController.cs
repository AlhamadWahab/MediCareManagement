using AutoMapper;
using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using DomainLayer.Entities.Patient_Model;
using DomainLayer.Interfaces.Bases_;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Controllers.Pat_Controller
{
    public class PatientController(IRepository repository, IMapper mapper) : Controller
    {
        private readonly IRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        // GET: PatientController
        public async Task<IActionResult> Index()
        {
            IEnumerable<Patient> patients = await _repository.PatientService.GetAllAsync();
            IEnumerable<PatientDTO> patientDTOs = _mapper.Map<List<PatientDTO>>(patients);
            return View();
        }

        // GET: PatientController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            Patient patient = await _repository.PatientService.GetByIdAsync(id);
            PatientDTO patientDTO = _mapper.Map<PatientDTO>(patient);
            if (patientDTO == null)
            {
                return NotFound();
            }
            return View(patientDTO);
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
            "Email, Diagnoses, InsuranceProvider")] PatientDTO patientDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Patient patient = _mapper.Map<Patient>(patientDTO);
                    await _repository.PatientService.AddAsync(patient);
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
            Patient patient = await _repository.PatientService.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            PatientDTO patientDTO = _mapper.Map<PatientDTO>(patient);
            return View(patientDTO);
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name, Address, Telefon, Mobile, " +
            "Email, Diagnoses, InsuranceProvider")] PatientDTO patientDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Patient patient = _mapper.Map<Patient>(patientDTO);
                    await _repository.PatientService.UpdateAsync(id, patient);
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
            Patient patient = await _repository.PatientService.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            PatientDTO patientDTO = _mapper.Map<PatientDTO>(patient);
            return View(patientDTO);
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
                    Patient patient = await _repository.PatientService.GetByIdAsync(id);
                    if (patient == null)
                    {
                        return NotFound();
                    }
                    await _repository.PatientService.DeleteByIdAsync(id);
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
