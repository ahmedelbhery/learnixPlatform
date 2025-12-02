using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.LessonVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly IVideoService _videoService;

        public LessonController(ILessonService lessonService, IVideoService videoService)
        {
            _lessonService = lessonService;
            _videoService = videoService;
        }

        public IActionResult DisplayLesson(int id)
        {

           
            var Lessondto = _lessonService.GetById(id);
            if (Lessondto == null)
                return View("NotFound");

            var LessonInfoVM = new LessonInfoVM()
            {
                LessonId = Lessondto.LessonId,
                Title = Lessondto.Title,
                Description = Lessondto.Description,
                LearningObjectives = Lessondto.LearningObjectives,
                VideoUrl = Lessondto.VideoUrl,
                Duration = Lessondto.Duration,
                Order = Lessondto.Order,
                SectionId = Lessondto.SectionId
            };

            LessonInfoVM.CourseID = _lessonService.GetCourseIDforLesson(Lessondto.SectionId);

            return View("DisplayLesson", LessonInfoVM);
        }

        public async Task<IActionResult> DeleteLesson(int id)
        {
            
            var LessonDto = _lessonService.GetById(id);

            if (LessonDto == null)
                return View("NotFound");

            await _videoService.DeleteVideoAsync(LessonDto.VideoUrl);

            var courseId = _lessonService.GetCourseIDforLesson(LessonDto.SectionId);

            _lessonService.Delete(id);

            return RedirectToAction("DisplayCourseContent", "Course", new { id = courseId });
        }

    }
}
