using Learnix.Dtos.InstructorDtos;
using Learnix.Models;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.InstructorDashboardVMs;
using Learnix.ViewModels.AdminVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Learnix.ViewModels.UserVMs;

namespace Learnix.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RequestReviewController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminService _adminService;
        private readonly IInstructorService _instructorService;

        public RequestReviewController(IAdminService adminService,
                                    UserManager<ApplicationUser> userManager,
                                    IInstructorService instructorService)
        {
            _adminService = adminService;
            _userManager = userManager;
            _instructorService = instructorService;
        }

        public async Task<IActionResult> InstructorReviewRequest(int page = 1)
        {
            var pageSize = 5;
            var pendingInstructors = await _adminService.GetPendingInstructorsAsync(page, pageSize);
            return View(pendingInstructors);
        }

        public async Task<IActionResult> ApproveInstructorRequest(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMsg"] = "Instructor ID is required.";
                return RedirectToAction("InstructorReviewRequest");
            }

            var result = await _adminService.ApproveInstructorAsync(id);

            if (result.Success)
            {
                TempData["SuccessMsg"] = result.Message;
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
            }

            return RedirectToAction("InstructorReviewRequest");
        }

        public async Task<IActionResult> RejectInstructorRequest(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMsg"] = "Instructor ID is required.";
                return RedirectToAction("InstructorReviewRequest");
            }

            var result = await _adminService.RejectInstructorAsync(id);

            if (result.Success)
            {
                TempData["SuccessMsg"] = result.Message;
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
            }

            return RedirectToAction("InstructorReviewRequest");
        }



        public async Task<IActionResult> DispalyProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return View("NotFound");

            var vm = new DisplayUserInfoVM()
            {
                ID = id,
                FullName = user.FirstName + " " + user.LastName,
                ImageUrl = user.ImageUrl,
                LinkedinUrl = user.Linkedin,
                Bio = user.Bio,
                Location = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,

            };

            return View("DispalyProfile", vm);
        }
    }
}