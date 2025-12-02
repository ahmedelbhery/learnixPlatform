using Learnix.Services.Interfaces;
using Learnix.ViewModels.CoursesVMs;
using Microsoft.AspNetCore.Mvc;

namespace Learnix.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: /Courses
        public async Task<IActionResult> Index(CourseFilterViewModel? filter = null)
        {
            var courses = await _courseService.GetAllCoursesAsync(filter);
            ViewBag.Filter = filter ?? new CourseFilterViewModel();
            return View(courses);
        }

        // GET: /Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseDetailsAsync(id);
            if (course == null)
            {
                return View("NotFound");
            }
            return View(course);
        }

        // GET: /Courses/Search
        public async Task<IActionResult> Search(string query)
        {
            var filter = new CourseFilterViewModel { Search = query };
            var courses = await _courseService.GetAllCoursesAsync(filter);
            return View("Index", courses);
        }
    }
}
