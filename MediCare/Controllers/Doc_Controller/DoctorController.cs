using AutoMapper;
using BusinessLogicLayer.DTOs.DoctorDto;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Interfaces.Bases_;
using MediCareSecurity_IdentityManagementLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneNumbers;

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
            await _repository.CommitAsync();
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
            await _repository.CommitAsync();
            return View(doctorDTO);
        }

        // GET: DoctorController/Create
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DoctorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
        public async Task<IActionResult> Create([Bind(" FirstName, LastName, Specialty, PraxisAdress, " +
            "Telefon, Email, MedicalLicenseNumber, ProfilePicture")] DoctorDTO doctorDTO, IFormFile file)
        {
            try
            {
                if (!IsValidName(doctorDTO.FirstName))
                {
                    ModelState.AddModelError("FirstName", "First Name field should only contain alphabetic characters.");
                    //await _repository.CommitAsync();
                    return View(doctorDTO);
                }
                if (!IsValidName(doctorDTO.LastName))
                {
                    ModelState.AddModelError("LastName", "Last Name field should only contain alphabetic characters.");
                    //await _repository.CommitAsync();
                    return View(doctorDTO);
                }

                if (ModelState.IsValid)
                {
                    var doctors = await _repository.DoctorService.GetAllAsync();
                    foreach (var d in doctors)
                    {
                        if (doctorDTO.Email == d.Email)
                        {
                            ModelState.AddModelError("Email", "Email is already in use!.");
                            return View(doctorDTO);
                        }
                        if (doctorDTO.MedicalLicenseNumber == d.MedicalLicenseNumber)
                        {
                            ModelState.AddModelError("MedicalLicenseNumber", "MedicalLicenseNumber is already in use!.");
                            return View(doctorDTO);
                        }
                    }
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
                    await _repository.CommitAsync();
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
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
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
        [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, FirstName, LastName, Specialty, PraxisAdress, " +
            "Telefon, Email, MedicalLicenseNumber, ProfilePicture")] DoctorDTO doctorDTO, IFormFile ProfilePicture)
        {
            try
            {

                Doctor existingDoctor = await _repository.DoctorService.GetByIdAsync(id);
                if (existingDoctor == null)
                {
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return View(doctorDTO);
                }

                if (ProfilePicture != null)
                {
                    var uploadesFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Doctor");
                    Directory.CreateDirectory(uploadesFile);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfilePicture.FileName);
                    var filePath = Path.Combine(uploadesFile, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(fileStream);
                    }
                    doctorDTO.ProfilePicture = $"/images/Doctor/{uniqueFileName}";
                }
                else
                {
                    doctorDTO.ProfilePicture = existingDoctor.ProfilePicture;
                }

                Doctor updatedDoctor = _mapper.Map<Doctor>(doctorDTO);

                await _repository.DoctorService.UpdateAsync(id, updatedDoctor);
                await _repository.CommitAsync();

                TempData["succes"] = "Doctor Infos that you updated have been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(doctorDTO); 
            }
        }

        // GET: DoctorController/Delete/5
        [Authorize(Roles = UserRole.ManagerRole)]
        public async Task<IActionResult> Delete(Guid id)
        {
            Doctor doctor = await _repository.DoctorService.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            DoctorDTO doctorDTO = _mapper.Map<DoctorDTO>(doctor);
            await _repository.CommitAsync();
            return View(doctorDTO);
        }

        // POST: DoctorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.ManagerRole)]
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
                    await _repository.CommitAsync();
                    TempData["succes"] = "Doctor Infos that you deleted, they have been deleted successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        /// <summary>
        /// Check if the name contains only alphabetic characters.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValidName(string name)
        { 
            return name.All(char.IsLetter);
        }
        /// <summary>
        /// Checks if the given phone number is a mobile number.
        /// Utilizes the libphonenumber library for accurate validation.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>True if the phone number is a mobile number; otherwise, false.</returns>
        //private bool IsMobileNumber(string phoneNumber)
        //{
        //    var phoneUtil = PhoneNumberUtil.GetInstance();
        //    try
        //    {
        //        var number = phoneUtil.Parse(phoneNumber, null); 
        //        var numberType = phoneUtil.GetNumberType(number);
        //        return numberType == PhoneNumberType.MOBILE; 
        //    }
        //    catch (NumberParseException)
        //    {
        //        return false; 
        //    }
        //}
    }
}
