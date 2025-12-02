using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = await _studentService.GetStudentCoursesAsync(studentId);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _studentService.IsStudentEnrolledInCourseAsync(studentId, id))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var viewModel = await _studentService.GetCourseDetailsAsync(id, studentId);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkLessonCompleted([FromBody] MarkLessonCompletedRequest request)
        {
            try
            {
                Console.WriteLine($"MarkLessonCompleted called with LessonId: {request.LessonId}");

                var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine($"Student ID: {studentId}");

                if (request.LessonId <= 0)
                {
                    Console.WriteLine("Invalid lesson ID");
                    return Json(new { success = false, message = "Invalid lesson ID" });
                }

                var result = await _studentService.MarkLessonAsCompletedAsync(studentId, request.LessonId);
                Console.WriteLine($"Service result: {result}");

                return Json(new
                {
                    success = result,
                    message = result ? "Lesson marked as completed" : "Failed to mark lesson as completed"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MarkLessonCompleted: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class MarkLessonCompletedRequest
        {
            public int LessonId { get; set; }
        }



        public async Task<IActionResult> Lesson(int courseId, int lessonId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await _studentService.IsStudentEnrolledInCourseAsync(studentId, courseId))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var courseDetails = await _studentService.GetCourseDetailsAsync(courseId, studentId);
            if (courseDetails == null)
            {
                return NotFound();
            }

            var lesson = courseDetails.Sections
                .SelectMany(s => s.Lessons)
                .FirstOrDefault(l => l.LessonId == lessonId);

            if (lesson == null)
            {
                return NotFound();
            }

            courseDetails.CurrentLesson = lesson;

            return View("Lesson", courseDetails);
        }



    }
}
