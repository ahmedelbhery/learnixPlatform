using Learnix.Dtos.SectionDtos;
using Learnix.Models;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.SectionVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learnix.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SectionController(ISectionService sectionService,UserManager<ApplicationUser> userManager, ICourseService courseService)
        {
            this._sectionService = sectionService;
            this._userManager = userManager;
            _courseService = courseService;
        }



        [HttpGet]
        public async Task<IActionResult> CreateSection()
        {
            var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            CreateSectionVM createSectionVM = new CreateSectionVM();

            createSectionVM.InstructorFirstName = CurrentUser.FirstName;
            createSectionVM.InstructorLasttName = CurrentUser.LastName;
            createSectionVM.InstructorImageUrl = CurrentUser.ImageUrl;

            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            createSectionVM.Courses = _sectionService.GetAllCoursesBelongsToInstructor(IDClaim.Value);

            return View("CreateSection",createSectionVM);
        }


        [HttpPost]
        public async Task<IActionResult> SaveSection(CreateSectionVM createSectionVM)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                bool IsOrderExists = _sectionService.CheckOrderExists(createSectionVM.CourseID, createSectionVM.SectionOrder);

                if (IsOrderExists)
                {
                    var CurrentUser1 = await _userManager.FindByNameAsync(User.Identity.Name);
                    createSectionVM.InstructorFirstName = CurrentUser1.FirstName;
                    createSectionVM.InstructorLasttName = CurrentUser1.LastName;
                    createSectionVM.InstructorImageUrl = CurrentUser1.ImageUrl;
                    createSectionVM.Courses = _sectionService.GetAllCoursesBelongsToInstructor(IDClaim.Value);
                    ModelState.AddModelError("SectionOrder", "This Order is already exists enter a valid order");
                    return View("CreateSection", createSectionVM);
                }
                else
                {
                    var SectionDto = new SectionDto() 
                    {
                        Name = createSectionVM.SectionName,
                        Order = createSectionVM.SectionOrder,
                        CourseID = createSectionVM.CourseID,
                    };

                    _sectionService.Add(SectionDto);

                    TempData["SuccessMessage"] = "Section added successfully!";
                    return RedirectToAction("CreateSection");
                }
            }


            var CurrentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            createSectionVM.InstructorFirstName = CurrentUser.FirstName;
            createSectionVM.InstructorLasttName = CurrentUser.LastName;
            createSectionVM.InstructorImageUrl = CurrentUser.ImageUrl;
            createSectionVM.Courses = _sectionService.GetAllCoursesBelongsToInstructor(IDClaim.Value);
            return View("CreateSection", createSectionVM);
        }


        [HttpGet]
        public IActionResult GetSectionsByCourse(int courseId)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _courseService.IsThisCourseBelongsToThisInstructor(courseId, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            var CrsDto = _courseService.GetById(courseId);
            if(CrsDto == null)
            {
                return View("NotFound");
            }
            var sections = _sectionService.GetSectionsbyCourseIDinOrder(courseId);
            return Json(sections);
        }


        [HttpGet]
        public IActionResult CheckOrderExists(int courseId, int order)
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
            bool exists = _sectionService.CheckOrderExists(courseId, order);
            return Json(exists);
        }


        public IActionResult DeleteSection(int Id)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _sectionService.ISThisSectionBelongsToThisInstructor(Id,IDClaim.Value);

            if (!result)
                return View("Forbidden");

            var seciondto = _sectionService.GetById(Id);
            if(seciondto == null)
            {
                return View("NotFound");
            }

            _sectionService.Delete(Id);
            
            return RedirectToAction("DisplayCourseContent", "Course", new { id = seciondto.CourseID });
        }






    }
}
