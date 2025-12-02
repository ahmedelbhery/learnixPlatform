
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AdminVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnix.Areas.Admin.Controllers 
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminCourseController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IImageService _imageService;

        public AdminCourseController(IAdminService adminService, IImageService imageService)
        {
            _adminService = adminService;
            _imageService = imageService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _adminService.GetCourseDetailsAsync(id);
            if (course == null)
                return View("NotFound");

            return View(course);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _adminService.GetCourseDetailsAsync(id);
            if (course == null)
                return View("NotFound");

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminCourseVM vm, IFormFile? Files)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, message = string.Join(" | ", errors) });
            }


            var uploadedImageName = _imageService.Upload(Files, "Users");

            if (!string.IsNullOrEmpty(uploadedImageName))
            {
                vm.ImageUrl = uploadedImageName;
            }

            var (success, message) = await _adminService.UpdateCourseAsync(vm);

            return Json(new { success, message });
        }
    }
}
