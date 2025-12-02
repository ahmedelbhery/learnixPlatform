using Learnix.Models;
using Learnix.Others;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.CoursesVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseService _courseService;
        private readonly IEmailService _emailService;

        public CourseController(UserManager<ApplicationUser> userManager, ICourseService courseService, IEmailService emailService)
        {
            _userManager = userManager;
            _courseService = courseService;
            _emailService = emailService;
        }

        public async Task<IActionResult> CoursesReview(string? search, int page = 1)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var Admin = await _userManager.FindByIdAsync(IDClaim.Value);

            int pageSize = 10;


            var vm = new AllCourseReviewsVM();
            vm.AdminFirstName = Admin.FirstName;
            vm.AdminLasttName = Admin.LastName;
            vm.AdminImageUrl = Admin.ImageUrl;
            vm.courseReviewVMs = _courseService.GetAllDraftedandRejectedCourses(search, page, pageSize);
            int totalCount = _courseService.GetAllDraftedandRejectedCourses(search).Count();
            vm.PageIndex = page;
            vm.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            vm.SearchTerm = search;


            return View("CoursesReview",vm);
        }

        public async Task<IActionResult> DisplayCourseContent(int id)
        {

            CourseContentVM courseContentVM = new CourseContentVM();
            courseContentVM = await _courseService.GetCourseContentAsync(id);

            if (courseContentVM == null)
                return View("NotFound");

            return View("DisplayCourseContent", courseContentVM);
             
        }

        public IActionResult DisplayCourseInfo(int id)
        {
            CourseDetailsVM courseDetailsVM = _courseService.GetCourseDetails(id);
            if (courseDetailsVM == null)
                return View("NotFound");

            return View("DisplayCourseInfo", courseDetailsVM);
        }

        public async Task<IActionResult> ApproveCourse(int id,string InstructorEmail)
        {
            var Crs = _courseService.GetById(id);
            if (Crs == null)
                return View("NotFound");

            _courseService.ApproveCourseStatus(id);

            TempData["SuccessMessage"] = "Course Approved successfully!";

            var email = new Email()
            {
                Subject = $"Your Course \"{Crs.Title}\" Has Been Approved",
                Body = $@"Dear Instructor,

We are pleased to inform you that your course ""{Crs.Title}"" has been successfully reviewed and approved.

Your course is now officially published on the platform and available for students to enroll in.

Thank you for your dedication and effort in creating high-quality educational content.

Best regards,
The Administration Team",
                ReceiverEmail = InstructorEmail,
            };

            await _emailService.SendEmailAsync(email);

            return RedirectToAction("CoursesReview");
        }

        public async Task<IActionResult> RejectCourse(int id, string InstructorEmail)
        {
            var Crs = _courseService.GetById(id);
            if (Crs == null)
                return View("NotFound");

            _courseService.RejectCourseStatus(id);

            TempData["SuccessMessage"] = "Course Rejected successfully!";

            var email = new Email()
            {
                Subject = $"Your Course \"{Crs.Title}\" Requires Changes",
                Body = $@"Dear Instructor,

Thank you for submitting your course ""{Crs.Title}"" for review. After careful evaluation, we regret to inform you that it cannot be approved at this time.

Please review the admin feedback, make the required updates, and resubmit your course for another review.

We appreciate your contribution and commitment to maintaining high-quality learning materials.

Best regards,
The Administration Team",
                ReceiverEmail = InstructorEmail,
            };

            await _emailService.SendEmailAsync(email);

            return RedirectToAction("CoursesReview");
        }
    }
}
