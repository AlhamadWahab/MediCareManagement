using AutoMapper;
using BusinessLogicLayer.DTOs.DoctorDto;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Interfaces.Bases_;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.Controllers.Doc_Controller
{
    public class DoctorController(IRepository repository, IMapper mapper, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: DoctorController
        public async Task<IActionResult> Index()
        {
            IEnumerable<Doctor> doctors = await _repository.DoctorService.GetAllAsync();
            IEnumerable<DoctorDTO> doctorDTOs = _mapper.Map<IEnumerable<DoctorDTO>>(doctors);
            _repository.CommitAsync();
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
            _repository.CommitAsync();
            return View(doctorDTO);
        }

        // GET: DoctorController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: DoctorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Specialty, PraxisAdress, " +
            "Telefon, Email, MedicalLicenseNumber")] DoctorDTO doctorDTO, IFormFile file)
        {
            try
            {
                if (!IsValidName(doctorDTO.Name ?? ""))
                {
                    ModelState.AddModelError("Name", "The Name field should only contain alphabetic characters.");
                    _repository.CommitAsync();
                    return View(doctorDTO);
                }
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string drImgPath = Path.Combine(wwwRootPath, @"images\Doctor");
                        using (var fileStream = new FileStream(Path.Combine(drImgPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        doctorDTO.ProfilePicture = @"\images\Doctor\" + fileName;
                    }

                    Doctor doctor = _mapper.Map<Doctor>(doctorDTO);
                    await _repository.DoctorService.AddAsync(doctor);
                    _repository.CommitAsync();
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
            _repository.CommitAsync();
            return View(doctorDTO);
        }

        // POST: DoctorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, Name, Specialty, PraxisAdress, " +
            "Telefon, Email, MedicalLicenseNumber, ProfilePicture")] DoctorDTO doctorDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Doctor doctor = _mapper.Map<Doctor>(doctorDTO);
                    await _repository.DoctorService.UpdateAsync(id, doctor);
                    _repository.CommitAsync();
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
            _repository.CommitAsync();
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
                    _repository.CommitAsync();
                    TempData["succes"] = "Doctor Infos that you deleted, they have been deleted successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> ImageSet(DoctorDTO doctorDTO, IFormFile file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string drImgPath = Path.Combine(wwwRootPath, @"images\Doctor");
        //            using (var fileStream = new FileStream(Path.Combine(drImgPath, fileName), FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //            doctorDTO.ProfilePicture = @"\images\Doctor" + fileName;
        //        }
        //        Doctor doctor = _mapper.Map<Doctor>(doctorDTO);
        //        await _repository.DoctorService.AddAsync(doctor);
        //        _repository.CommitAsync();
        //        TempData["succes"] = "Doctor Image that you added, they have been created successfully.";
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        private bool IsValidName(string name)
        {
            // Check if the name contains only alphabetic characters
            return name.All(char.IsLetter);
        }
    }
}
