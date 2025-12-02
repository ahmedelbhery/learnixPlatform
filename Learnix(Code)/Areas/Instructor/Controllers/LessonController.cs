using Learnix.Dtos.LessonDtos;
using Learnix.Dtos.LessonMaterialsDtos;
using Learnix.Dtos.SectionDtos;
using Learnix.Models;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.LessonVMs;
using Learnix.ViewModels.SectionVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace Learnix.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    [RequestSizeLimit(500_000_000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly ICourseService _courseService;
        private readonly ISectionService _sectionService;
        private readonly IVideoService _videoService;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonController(ILessonService lessonService, UserManager<ApplicationUser> userManager,IVideoService videoService,IFileService fileService,ICourseService courseService,ISectionService sectionService)
        {
            this._lessonService = lessonService;
            this._userManager = userManager;
            this._videoService = videoService;
            this._fileService = fileService;
            this._courseService = courseService;
            this._sectionService = sectionService;
        }


        [HttpGet]
        public async Task<IActionResult> CreateLesson()
        {
            var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            CreateLessonVM createLessonVM = new CreateLessonVM();
            //createLessonVM.Courses = _lessonService.GetAllCourses();
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            createLessonVM.Courses = _sectionService.GetAllCoursesBelongsToInstructor(IDClaim.Value);
            createLessonVM.LessonStatuses = _lessonService.GetAllLessonStatus();
            createLessonVM.InstructorFirstName = CurrentUser.FirstName;
            createLessonVM.InstructorLasttName = CurrentUser.LastName;
            createLessonVM.InstructorImageUrl = CurrentUser.ImageUrl;
            return View("CreateLesson",createLessonVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveLesson(CreateLessonVM createLessonVM)
        {

            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            bool exists = _lessonService.CheckLessonOrder(createLessonVM.SectionID, createLessonVM.LessonOrder);

            if (exists)
            {
                var CurrentUser1 = await _userManager.FindByNameAsync(User.Identity.Name);
                //createLessonVM.Courses = _lessonService.GetAllCourses();
                
                createLessonVM.Courses = _sectionService.GetAllCoursesBelongsToInstructor(IDClaim.Value);
                createLessonVM.LessonStatuses = _lessonService.GetAllLessonStatus();
                createLessonVM.InstructorFirstName = CurrentUser1.FirstName;
                createLessonVM.InstructorLasttName = CurrentUser1.LastName;
                createLessonVM.InstructorImageUrl = CurrentUser1.ImageUrl;

                ModelState.AddModelError("LessonOrder", "Enter a Valid Order");

                return View("CreateLesson", createLessonVM);
            }

            if (ModelState.IsValid)
            {
                var VideoPath = await _videoService.UploadVideoAsync(createLessonVM.VideoFile, "Lessons");
                
                var LessonDto = new LessonDto()
                {
                    Title = createLessonVM.Title,
                    Description = createLessonVM.Description,
                    LearningObjectives = createLessonVM.Objective,
                    Duration = createLessonVM.Duration,
                    Order = createLessonVM.LessonOrder,
                    SectionId = createLessonVM.SectionID,
                    VideoUrl = VideoPath,
                    StatusID = createLessonVM.LessonStatusID
                };

                _lessonService.Add(LessonDto);
                TempData["SuccessMessage"] = "Lesson added successfully!";
                return RedirectToAction("CreateLesson");

            }



            var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //createLessonVM.Courses = _lessonService.GetAllCourses();
            createLessonVM.Courses = _sectionService.GetAllCoursesBelongsToInstructor(IDClaim.Value);
            createLessonVM.LessonStatuses = _lessonService.GetAllLessonStatus();
            createLessonVM.InstructorFirstName = CurrentUser.FirstName;
            createLessonVM.InstructorLasttName = CurrentUser.LastName;
            createLessonVM.InstructorImageUrl = CurrentUser.ImageUrl;

            return View("CreateLesson", createLessonVM);
        }

        [HttpGet]
        public IActionResult GetSectionsByCourseId(int courseId)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _courseService.IsThisCourseBelongsToThisInstructor(courseId, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            var CrsDto = _courseService.GetById(courseId);
            if (CrsDto == null)
            {
                return View("NotFound");
            }
            var sections = _lessonService.GetSectionsByCourseId(courseId);

            return Json(sections);
        }

        [HttpGet]
        public IActionResult CheckLessonOrder(int SectionID, int LessonOrder)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _sectionService.ISThisSectionBelongsToThisInstructor(SectionID, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            var SecDto = _sectionService.GetById(SectionID);
            if (SecDto == null)
            {
                return View("NotFound");
            }

            bool exists = _lessonService.CheckLessonOrder(SectionID, LessonOrder);

            return Json(!exists);
        }

        [HttpGet]
        public IActionResult GetLessonsBySectionId(int sectionId)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _sectionService.ISThisSectionBelongsToThisInstructor(sectionId, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            //var SecDto = _sectionService.GetById(sectionId);

            //if (SecDto == null)
            //{
            //    return Json(false);
            //}

            //try
            //{
            //    var SecDto = _sectionService.GetById(sectionId);
            //    if (SecDto == null)
            //        return Json(false);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { error = ex.Message });
            //}


            var lessons = _lessonService.GetLessonsBySectionId(sectionId);

            return Json(lessons);
        }

        public IActionResult DisplayLesson(int id)
        {

            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _lessonService.ISThisLessonBelongsToThisInstructor(id, IDClaim.Value);

            if (!result)
                return View("Forbidden");

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

            return View("DisplayLesson",LessonInfoVM);
        }

        [HttpGet]
        public IActionResult EditLesson(int id)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _lessonService.ISThisLessonBelongsToThisInstructor(id, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            var LessonDto = _lessonService.GetById(id);
            if (LessonDto == null)
                return View("NotFound");
            var LessonEditVM = new LessonEditVM()
            {
                LessonId=LessonDto.LessonId,
                Title=LessonDto.Title,
                Description=LessonDto.Description,
                Duration = LessonDto.Duration,
                SectionId=LessonDto.SectionId,
                VideoUrl=LessonDto.VideoUrl,
                CourseId = _lessonService.GetCourseIDforLesson(LessonDto.SectionId),
                LearningObjectives=LessonDto.LearningObjectives,
            };


            return View("EditLesson",LessonEditVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditLesson(LessonEditVM lessonEditVM)
        {
            
            if (!ModelState.IsValid)
                return View("EditLesson", lessonEditVM);

            await _lessonService.UpdateLesson(lessonEditVM);

            return RedirectToAction("EditLesson", new { id = lessonEditVM.LessonId });
        }

        public async Task<IActionResult> DeleteLesson(int id)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _lessonService.ISThisLessonBelongsToThisInstructor(id, IDClaim.Value);

            if (!result)
                return View("Forbidden");

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
