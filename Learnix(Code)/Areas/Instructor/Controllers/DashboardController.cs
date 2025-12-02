using AutoMapper;
using Learnix.Dtos.InstructorDtos;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.InstructorDashboardVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class DashboardController : Controller
    {
        private readonly IInstructorService _instructorService;
       

        public DashboardController(IInstructorService instructorService)
        {
            this._instructorService = instructorService;
           
        }

        public async Task<IActionResult> Index()
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            InstructorMainDashboardDto instructorMainDashboardDto = await _instructorService.MainDashboard(IDClaim.Value);

            InstructorMainDashboardVM instructorMainDashboardVM = new InstructorMainDashboardVM()
            {
                InstructorFirstName = instructorMainDashboardDto.InstructorFirstName,
                InstructorLasttName = instructorMainDashboardDto.InstructorLasttName,
                InstructorImageUrl = instructorMainDashboardDto.InstructorImageUrl,
                TotalNumofInstructorCourses = instructorMainDashboardDto.TotalNumofInstructorCourses,
                TotalNumofInstructorStudents = instructorMainDashboardDto.TotalNumofInstructorStudents,
                TotalEarning = instructorMainDashboardDto.TotalEarning,
                topPerformingCoursebyStudentCapacity = instructorMainDashboardDto.topPerformingCoursebyStudentCapacity,
                topPerformingCoursebyEarning = instructorMainDashboardDto.topPerformingCoursebyEarning
            };

            return View("Index", instructorMainDashboardVM);
        }
    }
}
