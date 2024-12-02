using AutoMapper;
using BusinessLogicLayer.DTOs.DoctorDto;
using BusinessLogicLayer.DTOs.PatientDto;
using DomainLayer.Entities.Patient_Model;
using DomainLayer.Interfaces.Bases_;
using MediCareSecurity_IdentityManagementLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneNumbers;

namespace MediCare.Controllers.Pat_Controller
{
    [Authorize(Roles = UserRole.ManagerRole + ",Admin")]
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


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, Address, Telefon, Mobile, Email, Diagnoses, " +
            "InsuranceProvider")] PatientDTO patientDTO)
        {
            try
            {
                if (!IsValidName(patientDTO.FirstName))
                {
                    ModelState.AddModelError("FirstName", "First Name field should only contain alphabetic characters.");
                    return View(patientDTO);
                }
                if (!IsValidName(patientDTO.LastName))
                {
                    ModelState.AddModelError("LastName", "Last Name field should only contain alphabetic characters.");
                    return View(patientDTO);
                }

                if (ModelState.IsValid)
                {
                    var patients = await _repository.PatientService.GetAllAsync();
                    foreach(var p in patients)
                    {
                        if(patientDTO.Email == p.Email)
                        {
                            ModelState.AddModelError("Email", "Email is already in user!.");
                            return View(patientDTO);
                        }
                        if(patientDTO.Mobile ==  p.Mobile)
                        {
                            ModelState.AddModelError("Mobile", "Mobile is already in user!.");
                            return View(patientDTO);
                        }
                    }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id, FirstName, LastName, Address, Telefon, Mobile, Email, Diagnoses, InsuranceProvider")] PatientDTO patientDTO)
        {
            try
            {
                if (!IsValidName(patientDTO.FirstName))
                {
                    ModelState.AddModelError("FirstName", "First Name field should only contain alphabetic characters.");
                    return View(patientDTO);
                }
                if (!IsValidName(patientDTO.LastName ?? ""))
                {
                    ModelState.AddModelError("LastName", "Last Name field should only contain alphabetic characters.");
                    return View(patientDTO);
                }

                if (ModelState.IsValid)
                {
                    Patient patient = _mapper.Map<Patient>(patientDTO);
                    await _repository.PatientService.UpdateAsync(id, patient);
                    await _repository.CommitAsync();
                    TempData["succes"] = "Patient Infos that you Updated, they have been created successfully.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }


        [Authorize(Roles = UserRole.ManagerRole)]
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
        [Authorize(Roles = UserRole.ManagerRole)]
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
