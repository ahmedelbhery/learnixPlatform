using Learnix.Models;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AdminVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learnix.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminInstructorController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IImageService _imageService;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminInstructorController(IAdminService adminService, UserManager<ApplicationUser> userManager, IImageService imageService)
        {
            _userManager = userManager;
            _adminService = adminService;
            _imageService = imageService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var instructorVm = await _adminService.GetInstructorDetailsAsync(id);
            if (instructorVm == null)
                return View("NotFound");

            return View("Details", instructorVm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var instructorVm = await _adminService.GetInstructorDetailsAsync(id);
            if (instructorVm == null)
                return View("NotFound");

            return View("Edit", instructorVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminInstructorVM vm, IFormFile? Files)
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
                return Json(new { success = false, message = "This email is already in use." });
            }



                
                var uploadedImageName = _imageService.Upload(Files, "Users") ;

                if (!string.IsNullOrEmpty(uploadedImageName))
                {
                    vm.ImageUrl = uploadedImageName;
                }

            var (success, message) = await _adminService.UpdateInstructorAsync(vm);

            return Json(new { success, message });
        }
    }
}