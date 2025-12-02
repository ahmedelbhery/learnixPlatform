using Learnix.Dtos.InstructorDtos;
using Learnix.Models;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.InstructorDashboardVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminService _adminService;
        private readonly IInstructorService _instructorService;

        public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, IInstructorService instructorService)
        {
            _adminService = adminService;
            _userManager = userManager;
            _instructorService = instructorService;
        }
        public async Task<IActionResult> Index()
        {
            Claim IDClaim = User.Claims
                  .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var admin = _adminService.GetById(IDClaim.Value);

            var stats = await _adminService.GetUserStatisticsAsync();
            ViewBag.TotalUsers = stats.totalUsers;
            ViewBag.Instructors = stats.instructors;

            var stats2 = await _adminService.GetInstructorStatisticsAsync();
            ViewBag.TotalCourses = stats2.totalCourses;
            ViewBag.TotalEarnings = admin?.Balance ?? 0;


            return View("Index");
        }
        public async Task<IActionResult> Users(string role = "all", int page = 1)
        {
            var stats = await _adminService.GetUserStatisticsAsync();
            ViewBag.TotalUsers = stats.totalUsers;
            ViewBag.Admins = stats.admins;
            ViewBag.Instructors = stats.instructors;
            ViewBag.Students = stats.students;


            int pageSize = 5;
            var model = await _adminService.GetAllUsersAsync(role, page, pageSize);
            ViewBag.CurrentRole = role;
            return View(model);
        }

        public async Task<IActionResult> Instructors(string status = "all", string specialty = "all",  int page = 1)
        {
            var stats = await _adminService.GetInstructorStatisticsAsync();
            ViewBag.TotalInstructors = stats.totalInstructors;
            ViewBag.TotalCourses = stats.totalCourses;
            ViewBag.TotalStudents = stats.totalStudents;
            ViewBag.TotalEarnings = stats.totalEarnings * (95m / 100);

            var specialties = await _adminService.GetSpecialtiesAsync();
            ViewBag.Specialties = specialties;

            int pageSize = 5;
            var model = await _adminService.GetAllInstructorsAsync(specialty, page, pageSize);

            ViewBag.CurrentSpecialty = specialty;


            return View(model);
        }

        public async Task<IActionResult> Courses(string status = "all", string category = "all", int page = 1)
        {
            var stats = await _adminService.GetCourseStatisticsAsync();
            ViewBag.TotalCourses = stats.totalCourses;
            ViewBag.PublishedCourses = stats.publishedCourses;
            ViewBag.PendingCourses = stats.pendingCourses;
            ViewBag.TotalEnrollments = stats.totalEnrollments;

            var categories = await _adminService.GetCourseCategoriesAsync();
            ViewBag.Categories = categories;

            var statuses = await _adminService.GetCourseStatusesAsync();
            ViewBag.Statuses = statuses;

            int pageSize = 5;
            var model = await _adminService.GetAllCoursesAsync(status, category, page, pageSize);

            ViewBag.CurrentStatus = status;
            ViewBag.CurrentCategory = category;

            return View(model);
        }


    }
}
