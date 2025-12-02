using Learnix.Models;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.AdminVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminUserController(IAdminService adminService, UserManager<ApplicationUser> userManager, IImageService imageService)
        {
            _userManager = userManager;
            _adminService = adminService;
            _imageService = imageService;
        }


        public async Task<IActionResult> Details(string id)
        {
            var userVm = await _adminService.GetUserDetailsAsync(id);
            if (userVm == null)
                return View("NotFound");

            return View("Details", userVm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var userVm = await _adminService.GetUserDetailsAsync(id);
            if (userVm == null)
                return View("NotFound");

            return View("Edit", userVm);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserVM vm,IFormFile? Files)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, message = string.Join(" | ", errors) });
            }
            var existingUser = await _userManager.FindByEmailAsync(vm.Email);
            if (existingUser != null && existingUser.Id != vm.Id)
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return View("Edit", vm);
            }



            var uploadedImageName = _imageService.Upload(Files, "Users");

            if (!string.IsNullOrEmpty(uploadedImageName))
            {
                vm.ImageUrl = uploadedImageName;
            }


            var (success, message) = await _adminService.UpdateUserAsync(vm);

            return Json(new { success, message });
        }

    }
}
