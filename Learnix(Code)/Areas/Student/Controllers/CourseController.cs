using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }



        public async Task<IActionResult> Enroll(int id)
        {
            var crs = _courseService.GetById(id);
            if (crs == null)
                return View("NotFound");

            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = await _courseService.EnrollInCourse(id, IDClaim.Value);
            
            if (result)
            {
                return RedirectToAction("Index", "Student");
                //return Content("Enrolled Successfully");
            }
            else
            {
                return View("FailEnroll");
            }
        }
    }
}
