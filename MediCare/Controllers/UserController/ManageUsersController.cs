using AutoMapper;
using DomainLayer.Entities.Doctor_Model;
using DomainLayer.Entities.Patient_Model;
using DomainLayer.Interfaces.Bases_;
using MediCareSecurity_IdentityManagementLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Protocol.Core.Types;

namespace MediCare.Controllers.UserController
{
    [Authorize(Roles = "Manager")]
    public class ManageUsersController(IRepository repository, UserManager<MediCareAppUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly IRepository _repository = repository;
        private readonly UserManager<MediCareAppUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IActionResult> Index()
        {
            //var users = await _userManager.Users.Select(u => new UserFormViewModel
            //{
            //    Id = u.Id,
            //    UserName = u.UserName,
            //    Email = u.Email,
            //    Roles = _userManager.GetRolesAsync(u).Result
            //}).ToListAsync();
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userListWithRoles = new List<UserFormViewModel>();

            foreach (var user in users)
            {
                var userMapped = _mapper.Map<UserFormViewModel>(user);
                userMapped.Roles = await _userManager.GetRolesAsync(user);
                userListWithRoles.Add(userMapped);
            }

            return View(userListWithRoles);
        }
      
        public async Task<IActionResult> ManageSpecificUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { return NotFound(); }
            var userRoles = await _roleManager.Roles.ToListAsync();
            var viewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = userRoles.Select(r => new CheckboxRolesOfUserViewModel 
                { IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result, RoleName = r.Name }).ToList() // not ToListAsync: because it not go to select from db, it selects from memory
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageSpecificUser(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if(user == null) { return NotFound(); }

            var selectAllUserRoles = await _userManager.GetRolesAsync(user);
            foreach(var role in model.Roles)
            {
                if(selectAllUserRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
                if (!selectAllUserRoles.Any(r => r == role.RoleName) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                    if (role.RoleName == "Doctor")
                    {
                        Doctor newDoctor = new Doctor
                        {
                            Id = Guid.Parse(user.Id),
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Telefon = user.PhoneNumber
                        };
                        await _repository.DoctorService.AddAsync(newDoctor);
                        await _repository.CommitAsync();
                    }
                    if(role.RoleName == "Patient")
                    {
                        Patient newPatient = new Patient
                        {
                            Id = Guid.Parse(user.Id),
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Telefon = user.PhoneNumber
                        };
                        await _repository.PatientService.AddAsync(newPatient);
                        await _repository.CommitAsync();
                    }
                }
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}

