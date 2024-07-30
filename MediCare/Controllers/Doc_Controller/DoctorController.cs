using AutoMapper;
using BusinessLogicLayer.DTOs.DoctorDto;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Interfaces.Bases_;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Controllers.Doc_Controller
{
    public class DoctorController(IRepository repository, IMapper mapper) : Controller
    {
        private readonly IRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        // GET: DoctorController
        public async Task<IActionResult> Index()
        {
            IEnumerable<Doctor> doctors = await _repository.DoctorService.GetAllAsync();
            IEnumerable<DoctorDTO> doctorDTOs = _mapper.Map<List<DoctorDTO>>(doctors);
            return View(doctorDTOs);
        }

        // GET: DoctorController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            Doctor doctor = await _repository.DoctorService.GetByIdAsync(id);
            DoctorDTO doctorDTO = _mapper.Map<DoctorDTO>(doctor);
            if (doctorDTO == null)
            {
                return NotFound();
            }
            return View(doctorDTO);
        }

        // GET: DoctorController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DoctorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Specialty, PraxisAdress, " +
            "Telefon, Email, MedicalLicenseNumber")] DoctorDTO doctorDTO)
        {
            try
            {
                if (!IsValidName(doctorDTO.Name??""))
                {
                    ModelState.AddModelError("Name", "The Name field should only contain alphabetic characters.");
                    return View(doctorDTO);
                }
                if (ModelState.IsValid)
                {        
                    Doctor doctor = _mapper.Map<Doctor>(doctorDTO);
                    await _repository.DoctorService.AddAsync(doctor);
                    TempData["succes"] = "Doctor Infos that you added, they have been created successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: DoctorController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            Doctor doctor = await _repository.DoctorService.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            DoctorDTO doctorDTO = _mapper.Map<DoctorDTO>(doctor);
            return View(doctorDTO);
        }

        // POST: DoctorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name, Specialty, PraxisAdress," +
            " Telefon, Email, MedicalLicenseNumber")] DoctorDTO doctorDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Doctor doctor = _mapper.Map<Doctor>(doctorDTO);
                    await _repository.DoctorService.UpdateAsync(id, doctor);
                    TempData["succes"] = "Doctor Infos that you updated, they have been updated successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: DoctorController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            Doctor doctor = await _repository.DoctorService.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            DoctorDTO doctorDTO = _mapper.Map<DoctorDTO>(doctor);
            return View(doctorDTO);
        }

        // POST: DoctorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Doctor doctor = await _repository.DoctorService.GetByIdAsync(id);
                    if (doctor == null)
                    {
                        return NotFound();
                    }
                    await _repository.DoctorService.DeleteByIdAsync(id);
                    TempData["succes"] = "Doctor Infos that you deleted, they have been deleted successfully.";
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
