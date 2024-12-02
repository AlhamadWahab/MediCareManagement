using MediCareSecurity_IdentityManagementLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Controllers.RoleController
{
    [Authorize(Roles = "Manager")]
    public class ManageRolesController(RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            if(await _roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Role is exist!");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            await _roleManager.CreateAsync(new IdentityRole(model.RoleName.Trim()));
            return RedirectToAction(nameof(Index));
        }
    }
}
