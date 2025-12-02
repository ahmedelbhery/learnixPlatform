using Learnix.Dtos.CourseDtos;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.CoursesVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollementService _enrollementService;
        private readonly IImageService _imageService;
        public CourseController(ICourseService courseService,IImageService ImageService,IEnrollementService enrollementService)
        {
            this._courseService = courseService;
            this._imageService = ImageService;
            this._enrollementService = enrollementService;
        }

        public async Task<IActionResult> Index()
        {
            AllInstructorCoursesVM allInstructorCoursesVM = new AllInstructorCoursesVM();
            allInstructorCoursesVM = await _courseService.GetAllInstructorCoursesVMAsync(User.Identity.Name);

            return View("Index",allInstructorCoursesVM);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCourse()
        {


            CreateCourseVM createCourseVM = await _courseService.GetCreateCourseVMAsync(User.Identity.Name);


            return View("CreateCourse",createCourseVM);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCourse(CreateCourseVM createCourseVM)
        {
            if(ModelState.IsValid)
            {
             
                if (createCourseVM.ImageFile != null || createCourseVM.ImageFile.Length!=0)
                {
                    createCourseVM.ImageUrl = _imageService.Upload(createCourseVM.ImageFile, "Courses");

                    Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                    CourseDto courseDto = new CourseDto() 
                    {
                        Title = createCourseVM.Title,
                        ImageUrl = createCourseVM.ImageUrl,
                        Description = createCourseVM.Description,
                        LearningOutCome = createCourseVM.LearningOutCome,
                        Duration = createCourseVM.Duration,
                        Price = createCourseVM.Price,
                        IsFree = createCourseVM.IsFree,
                        Requirement = createCourseVM.Requirement,
                        InstructorID = IDClaim.Value,
                        CategoryID = createCourseVM.CategoryID,
                        //StatusID = createCourseVM.CourseStatusID,
                        StatusID = 1,
                        LevelID = createCourseVM.CourseLevelID,
                        LanguageID = createCourseVM.CourseLanguageID,
                    };

                    _courseService.Add(courseDto);

                    TempData["SuccessMessage"] = "Course added successfully!";
                    return RedirectToAction("CreateCourse");

                    //return Content("Added Successfully");
                }
                else
                {
                    var createCourseVMC = await _courseService.GetCreateCourseVMAsync(User.Identity.Name);
                    createCourseVM.Categories = createCourseVMC.Categories;
                    createCourseVM.CourseLevels = createCourseVMC.CourseLevels;
                   // createCourseVM.CourseStatuses = createCourseVMC.CourseStatuses;
                    createCourseVM.CourseLanguages = createCourseVMC.CourseLanguages;
                    ModelState.AddModelError("ImageFile", "Course Image is Required");
                    return View("CreateCourse", createCourseVM);
                }

                
            }

            else
            {
                var createCourseVMC = await _courseService.GetCreateCourseVMAsync(User.Identity.Name);
                createCourseVM.Categories = createCourseVMC.Categories;
                createCourseVM.CourseLevels = createCourseVMC.CourseLevels;
                //createCourseVM.CourseStatuses = createCourseVMC.CourseStatuses;
                createCourseVM.CourseLanguages = createCourseVMC.CourseLanguages;

                return View("CreateCourse", createCourseVM);
            }
            
        }

        public async Task<IActionResult> DisplayCourseContent(int id)
        {

            /*
             
              Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _courseService.IsThisCourseBelongsToThisInstructor(id, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            CourseContentVM courseContentVM = new CourseContentVM();
            courseContentVM = await _courseService.GetCourseContentAsync(id);

            if (courseContentVM == null)
                return View("NotFound");

            return View("DisplayCourseContent", courseContentVM);
             
             
             */


            if (User.IsInRole("Instructor"))
            {
                Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                bool result = _courseService.IsThisCourseBelongsToThisInstructor(id, IDClaim.Value);

                if (!result)
                    return View("Forbidden");
            }

           

            CourseContentVM courseContentVM = await _courseService.GetCourseContentAsync(id);

            if (courseContentVM == null)
                return View("NotFound");

            return View("DisplayCourseContent", courseContentVM);
        }
       
        public IActionResult DisplayCourseInfo(int id)
        {

            if (User.IsInRole("Instructor"))
            {
                Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                bool result = _courseService.IsThisCourseBelongsToThisInstructor(id, IDClaim.Value);

                if (!result)
                    return View("Forbidden");
            }
           

            CourseDetailsVM courseDetailsVM = _courseService.GetCourseDetails(id);
            if (courseDetailsVM == null)
                return View("NotFound");

            return View("DisplayCourseInfo",courseDetailsVM);
        }

        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _courseService.IsThisCourseBelongsToThisInstructor(id, IDClaim.Value);

            if(!result)
                return View("Forbidden");


            var CrsDto = _courseService.GetById(id);

            if (CrsDto == null)
                return View("NotFound");


            var EditCrsVM = _courseService.GetEditCourseVM(id);
            return View("EditCourse", EditCrsVM);
        }

        [HttpPost]
        public IActionResult SaveEditCourse(EditCourseVM editCourseVM)
        {
            if(ModelState.IsValid)
            {
                bool result = _courseService.UpdateCourse(editCourseVM);
                if(result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("NotFound");
                }
            }



            var EditCrsVM1 = _courseService.GetEditCourseVM(editCourseVM.Id);
            editCourseVM.CourseLevels = EditCrsVM1.CourseLevels;
            editCourseVM.Categories = EditCrsVM1.Categories;
            //editCourseVM.CourseLevels = EditCrsVM1.CourseLevels;
            editCourseVM.CourseLanguages = EditCrsVM1.CourseLanguages;

            return View("EditCourse", editCourseVM);
        }

        public async Task<IActionResult> DisplayCourseStudents(int id, string? search, int page = 1)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            bool result = _courseService.IsThisCourseBelongsToThisInstructor(id, IDClaim.Value);

            if (!result)
                return View("Forbidden");

            var crs = _courseService.GetById(id);
            if (crs == null)
                return View("NotFound");

            int pageSize = 10;

            var vm = await _courseService.GetAllCourseStudents(id, search, page, pageSize);

           
            int totalCount = _enrollementService.GetAllCourseStudents(id, search).Count();
            vm.PageIndex = page;
            vm.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            vm.SearchTerm = search;

            return View(vm);
        }
    }
}
