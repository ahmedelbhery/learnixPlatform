using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        public IActionResult DeleteSection(int Id)
        {
            
            var seciondto = _sectionService.GetById(Id);
            if (seciondto == null)
            {
                return View("NotFound");
            }

            _sectionService.Delete(Id);

            return RedirectToAction("DisplayCourseContent", "Course", new { id = seciondto.CourseID });
        }
    }
}
