using AutoMapper;
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
        public async Task<IActionResult> Index()
        {
            IEnumerable<Patient> patients = await _repository.PatientService.GetAllAsync();
            IEnumerable<PatientDTO> patientDTOs = _mapper.Map<IEnumerable<PatientDTO>>(patients);
            return View(patientDTOs);
        }

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


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Address, Telefon, Mobile, Email, Diagnoses, " +
            "InsuranceProvider")] PatientDTO patientDTO)
        {
            try
            {
                if (!IsValidName(patientDTO.Name ?? ""))
                {
                    ModelState.AddModelError("Name", "The Name field should only contain alphabetic characters.");
                    return View(patientDTO);
                }
                if (ModelState.IsValid)
                {
                    Patient patient = _mapper.Map<Patient>(patientDTO);
                    await _repository.PatientService.AddAsync(patient);
                    TempData["succes"] = "Patient Infos that you added, they have been created successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name, Address, Telefon, Mobile, Email, Diagnoses, \" +\r\n            \"InsuranceProvider")] PatientDTO patientDTO)
        {
            try
            {
                if (!IsValidName(patientDTO.Name ?? ""))
                {
                    ModelState.AddModelError("Name", "The Name field should only contain alphabetic characters.");
                    return View(patientDTO);
                }
                if (ModelState.IsValid)
                {
                    Patient patient = _mapper.Map<Patient>(patientDTO);
                    await _repository.PatientService.UpdateAsync(id, patient);
                    TempData["succes"] = "Patient Infos that you Updated, they have been created successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
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
                    TempData["succes"] = "Patient Infos that you deleted, they have been deleted successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        private bool IsValidName(string name)
        {
            // Check if the name contains only alphabetic characters
            return name.All(char.IsLetter);
        }
    }
}
