using Learnix.Data;
using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.CourseLanguageDtos;
using Learnix.Dtos.CourseLevelDtos;
using Learnix.Dtos.CourseStatusDtos;
using Learnix.Dtos.EnrollementDtos;
using Learnix.Dtos.PaymentsDtos;
using Learnix.Models;
using Learnix.Others;
using Learnix.Repoisatories.Implementations;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.CourseDetailsVMs;
using Learnix.ViewModels.CoursesVMs;
using Learnix.ViewModels.LessonVMs;
using Learnix.ViewModels.SectionVMs;
using Microsoft.AspNetCore.Identity;

namespace Learnix.Services.Implementations
{
    public class CourseService : GenericService<Course,CourseDto,int> , ICourseService
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentService _studentService;
        private readonly IImageService _imageService;
        private readonly IEmailService _emailService;
        private readonly IStudentLessonProgressService _studentLessonProgressService;
        private readonly IEnrollementService _enrollementService;
        private readonly IGenericService<CourseLanguage, CourseLanguageDto, int> _courseLanguageService;
        private readonly IGenericService<CourseLevel, CourseLevelDto, int> _courseLevelService;
        private readonly IGenericService<CourseStatus, CourseStatusDto, int> _courseStatusService;

        public CourseService(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IGenericService<CourseLanguage, CourseLanguageDto, int> courseLanguageService,
            IGenericService<CourseLevel, CourseLevelDto, int> courseLevelService,
            IGenericService<CourseStatus, CourseStatusDto, int> courseStatusService,
            UserManager<ApplicationUser> userManager, IStudentService StudentService,
            IImageService imageService,
            IEmailService emailService, IStudentLessonProgressService studentLessonProgressService, IEnrollementService enrollementService, ICourseRepository courseRepository) : base(unitOfWork)
        {
            this._categoryService = categoryService;
            this._courseLanguageService = courseLanguageService;
            this._courseLevelService = courseLevelService;
            this._courseStatusService = courseStatusService;
            this._userManager = userManager;
            this._studentService = StudentService;
            this._imageService = imageService;
            this._emailService = emailService;
            this._studentLessonProgressService = studentLessonProgressService;
            this._enrollementService = enrollementService;
            _courseRepository = courseRepository;
        }











        public async Task<AllInstructorCoursesVM> GetAllInstructorCoursesVMAsync(string UserName)
        {
            var CurrentUser = await _userManager.FindByNameAsync(UserName);

            var vm = new AllInstructorCoursesVM() 
            {
                InstructorFirstName = CurrentUser.FirstName,
                InstructorLasttName = CurrentUser.LastName,
                InstructorImageUrl = CurrentUser.ImageUrl,
                TotalNumofInstructorCourses = GetTotalNumberOfCoursebyInstructorID(CurrentUser.Id),
                TotalNumofInstructorStudents = _studentService.GetToltalNumberOfStudentsForSpeceifcInstructor(CurrentUser.Id),
                TotalNumofInstructorDraftedCourses = GetTotalNumberofDraftedCoursesforSpecificInstructor(CurrentUser.Id),
                TotalNumofInstructorPublishedCourses = GetTotalNumberofPublishedCoursesforSpecificInstructor(CurrentUser.Id),
                TotalNumofInstructorRejectedCourses = GetTotalNumberofRejectedCoursesforSpecificInstructor(CurrentUser.Id),
                AllCoursesDetails = GetAllCoursesForSpecificInstructor(CurrentUser.Id),
                DraftedCoursesDetails = GetAllDraftedCoursesForSpecificInstructor(CurrentUser.Id),
                PublishedCoursesDetails = GetAllPublishedCoursesForSpecificInstructor(CurrentUser.Id),
                RejectedCoursesDetails = GetAllRejectedCoursesForSpecificInstructor(CurrentUser.Id)
            };

            return vm;
        }
        public IEnumerable<CourseDetailsDto> GetAllCoursesForSpecificInstructor(string InstructorID)
        {
            var Courses = _unitOfWork.Courses.GetAllCoursesForSpecificInstructor(InstructorID);

            var CoursesDetailsDto = new List<CourseDetailsDto>();

            foreach(var course in Courses)
            {
                var CrsDto = new CourseDetailsDto()
                {
                    Id = course.Id,
                    Title = course.Title,
                    CategoryName = GetCategoryNameforCourse(course.Id),
                    CourseStatus = GetStatusNameforCourse(course.Id),
                    ImageUrl = course.ImageUrl,
                    TotalEarning = _unitOfWork.Courses.GetTotalEarningMonyPerCourse(course.Id) * (95.0 / 100),
                    TotalNumberOfStudent = _unitOfWork.Courses.GetTotalNumberOfStudentsPerCourse(course.Id)
                };

                CoursesDetailsDto.Add(CrsDto);
            }

            return CoursesDetailsDto;
        }
        public IEnumerable<DraftCourseDetailsDto> GetAllDraftedCoursesForSpecificInstructor(string InstructorID)
        {
            var Courses = _unitOfWork.Courses.GetAllDraftedCoursesForSpecificInstructor(InstructorID);

            var CoursesDetailsDto = new List<DraftCourseDetailsDto>();

            foreach (var course in Courses)
            {
                var CrsDto = new DraftCourseDetailsDto()
                {
                    Id = course.Id,
                    Title = course.Title,
                    CategoryName = GetCategoryNameforCourse(course.Id),
                    ImageUrl = course.ImageUrl
                };

                CoursesDetailsDto.Add(CrsDto);
            }

            return CoursesDetailsDto;
        }
        public IEnumerable<PublishedCourseDetailsDto> GetAllPublishedCoursesForSpecificInstructor(string InstructorID)
        {
            var Courses = _unitOfWork.Courses.GetAllPublishedCoursesForSpecificInstructor(InstructorID);

            var CoursesDetailsDto = new List<PublishedCourseDetailsDto>();

            foreach (var course in Courses)
            {
                var CrsDto = new PublishedCourseDetailsDto()
                {
                    Id = course.Id,
                    Title = course.Title,
                    CategoryName = GetCategoryNameforCourse(course.Id),
                    ImageUrl = course.ImageUrl,
                    TotalEarning = _unitOfWork.Courses.GetTotalEarningMonyPerCourse(course.Id) * (95.0 / 100),
                    TotalNumberOfStudent = _unitOfWork.Courses.GetTotalNumberOfStudentsPerCourse(course.Id)
                };

                CoursesDetailsDto.Add(CrsDto);
            }

            return CoursesDetailsDto;
        }
        public IEnumerable<RejectedCourseDetailsDto> GetAllRejectedCoursesForSpecificInstructor(string InstructorID)
        {
            var Courses = _unitOfWork.Courses.GetAllRejectedCoursesForSpecificInstructor(InstructorID);

            var CoursesDetailsDto = new List<RejectedCourseDetailsDto>();

            foreach (var course in Courses)
            {
                var CrsDto = new RejectedCourseDetailsDto()
                {
                    Id = course.Id,
                    Title = course.Title,
                    CategoryName = GetCategoryNameforCourse(course.Id),
                    ImageUrl = course.ImageUrl
                };

                CoursesDetailsDto.Add(CrsDto);
            }

            return CoursesDetailsDto;
        }
        public async Task<CreateCourseVM> GetCreateCourseVMAsync(string UserName)
        {
            CreateCourseVM createCourseVM = new CreateCourseVM();

            var CurrentUser = await _userManager.FindByNameAsync(UserName);

            
            createCourseVM.InstructorFirstName = CurrentUser.FirstName;
            createCourseVM.InstructorLasttName = CurrentUser.LastName;
            createCourseVM.InstructorImageUrl = CurrentUser.ImageUrl;

            createCourseVM.Categories = _categoryService.GetAll();
            createCourseVM.CourseLevels = _courseLevelService.GetAll();
            //createCourseVM.CourseStatuses = _courseStatusService.GetAll();
            createCourseVM.CourseLanguages = _courseLanguageService.GetAll();
            
            return createCourseVM;
        }
        public IEnumerable<TopPerformingCourseDto> GetTheTop3EnrolledCoursesForSpecificInstructor(string InstructorID)
        {
            var Top3Crs = _unitOfWork.Courses.GetTheTop3EnrolledCoursesForSpecificInstructor(InstructorID);

            var Top3Performingcrs = new List<TopPerformingCourseDto>();

            foreach(var crs  in Top3Crs)
            {
                var TopCrs = new TopPerformingCourseDto();
                TopCrs.Id = crs.Id;
                TopCrs.ImageUrl = crs.ImageUrl;
                TopCrs.Title = crs.Title;
                TopCrs.TotalNumberOfStudent = _unitOfWork.Courses.GetTotalNumberOfStudentsPerCourse(crs.Id);
                TopCrs.TotalEarning = _unitOfWork.Courses.GetTotalEarningMonyPerCourse(crs.Id);
                Top3Performingcrs.Add(TopCrs);
            }

            return Top3Performingcrs;
        }
        public IEnumerable<TopPerformingCourseDto> GetTheTop3ProfitableCoursesForSpecificInstructor(string InstructorID)
        {
            var Top3Crs = _unitOfWork.Courses.GetTheTop3ProfitableCoursesForSpecificInstructor(InstructorID);

            var Top3Performingcrs = new List<TopPerformingCourseDto>();

            foreach (var crs in Top3Crs)
            {
                var TopCrs = new TopPerformingCourseDto();
                TopCrs.Id = crs.Id;
                TopCrs.ImageUrl = crs.ImageUrl;
                TopCrs.Title = crs.Title;
                TopCrs.TotalNumberOfStudent = _unitOfWork.Courses.GetTotalNumberOfStudentsPerCourse(crs.Id);
                TopCrs.TotalEarning = _unitOfWork.Courses.GetTotalEarningMonyPerCourse(crs.Id);
                Top3Performingcrs.Add(TopCrs);
            }

            return Top3Performingcrs;
        }
        public double? GetTotalEarningMonyPerCourse(int courseID)
        {
            return _unitOfWork.Courses.GetTotalEarningMonyPerCourse(courseID);
        }
        public int GetTotalNumberOfCoursebyInstructorID(string InstructorID)
        {
           return _unitOfWork.Courses.GetTotalNumberOfCoursebyInstructorID(InstructorID);
        }
        public int GetTotalNumberofDraftedCoursesforSpecificInstructor(string InstructorID)
        {
            return _unitOfWork.Courses.GetTotalNumberofDraftedCoursesforSpecificInstructor(InstructorID);
        }
        public int GetTotalNumberofPublishedCoursesforSpecificInstructor(string InstructorID)
        {
            return _unitOfWork.Courses.GetTotalNumberofPublishedCoursesforSpecificInstructor(InstructorID);
        }
        public int GetTotalNumberofRejectedCoursesforSpecificInstructor(string InstructorID)
        {
            return _unitOfWork.Courses.GetTotalNumberofRejectedCoursesforSpecificInstructor(InstructorID);
        }
        public string GetCategoryNameforCourse(int courseID)
        {
            return _unitOfWork.Courses.GetCategoryNameforCourse(courseID);
        }
        public string GetStatusNameforCourse(int courseID)
        {
            return _unitOfWork.Courses.GetStatusNameforCourse(courseID);
        }
        public async Task<CourseContentVM> GetCourseContentAsync(int courseId)
        {
            var course = await _unitOfWork.Courses.GetAllCourseContent(courseId);

            if (course == null)
                return null;

            var result = new CourseContentVM
            {
                CourseId = course.Id,
                CourseName = course.Title,
                Sections = course.Sections
                    .OrderBy(s => s.Order)
                    .Select(section => new SectionVM
                    {
                        SectionId = section.Id,
                        SectionName = section.Name,
                        SectionOrder = section.Order,

                        Lessons = section.Lessons
                            .OrderBy(l => l.Order)
                            .Select(lesson => new LessonVM
                            {
                                LessonId = lesson.LessonId,
                                LessonName = lesson.Title,
                                LessonOrder = lesson.Order
                            }).ToList()

                    }).ToList()
            };

            return result;
            
        }
        public EditCourseVM GetEditCourseVM(int CourseId)
        {
            var CrsDto = _unitOfWork.Courses.GetByID(CourseId);

            var EditCrsVM = new EditCourseVM()
            {
                Id = CourseId,
                ImageUrl = CrsDto.ImageUrl,
                Title = CrsDto.Title,
                Description = CrsDto.Description,
                LearningOutCome = CrsDto.LearningOutCome,
                Duration = CrsDto.Duration,
                Requirement = CrsDto.Requirement,
                Price = CrsDto.Price,
                IsFree = CrsDto.IsFree,
                CategoryID = CrsDto.CategoryID,
                CourseLanguageID = CrsDto.LanguageID,
                CourseLevelID = CrsDto.LevelID,
                //CourseStatusID = CrsDto.StatusID,
                Categories = _categoryService.GetAll(),
                //CourseStatuses = _courseStatusService.GetAll(),
                CourseLanguages = _courseLanguageService.GetAll(),
                CourseLevels = _courseLevelService.GetAll(),
            };
            
            return EditCrsVM;
        }
        public bool UpdateCourse(EditCourseVM vm)
        {
            var Crs = _unitOfWork.Courses.GetByID(vm.Id);

            if (Crs == null)
                return false;

            Crs.Title = vm.Title;
            Crs.Description = vm.Description;
            Crs.LearningOutCome = vm.LearningOutCome;
            Crs.Duration = vm.Duration;
            Crs.Requirement = vm.Requirement;
            Crs.Price = vm.Price;
            Crs.IsFree = vm.IsFree;
            Crs.CategoryID = vm.CategoryID;
            Crs.LevelID = vm.CourseLevelID;
            //Crs.StatusID = vm.CourseStatusID;
            Crs.StatusID = 1;
            Crs.LanguageID = vm.CourseLanguageID;


            if(vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                _imageService.Delete(Crs.ImageUrl);
                var UpdatedImageUrl = _imageService.Upload(vm.ImageFile, "Courses");
                Crs.ImageUrl = UpdatedImageUrl;

            }


            _unitOfWork.Complete();
            return true;
        }
        public CourseDetailsVM? GetCourseDetails(int courseID)
        {
            var CrsDetailsVM = new CourseDetailsVM();

            var CrsDetailsEntity = _unitOfWork.Courses.GetCourseDetails(courseID);
            if (CrsDetailsEntity == null)
                return null;

            CrsDetailsVM.Id = CrsDetailsEntity.Id;
            CrsDetailsVM.Title = CrsDetailsEntity.Title;
            CrsDetailsVM.Duration = CrsDetailsEntity.Duration;
            CrsDetailsVM.ImageUrl = CrsDetailsEntity.ImageUrl;
            CrsDetailsVM.Description = CrsDetailsEntity.Description;
            CrsDetailsVM.LearningOutCome = CrsDetailsEntity.LearningOutCome;
            CrsDetailsVM.Requirement = CrsDetailsEntity.Requirement;
            CrsDetailsVM.Price = CrsDetailsEntity.Price;
            CrsDetailsVM.IsFree = CrsDetailsEntity.IsFree;
            CrsDetailsVM.InstructorName = CrsDetailsEntity.Instructor.User.FirstName + " " + CrsDetailsEntity.Instructor.User.LastName;
            CrsDetailsVM.CategoryName = CrsDetailsEntity.Category.Name;
            CrsDetailsVM.LanguageName = CrsDetailsEntity.Language.Name;
            CrsDetailsVM.LevelName = CrsDetailsEntity.Level.Name;
            CrsDetailsVM.StatusName = CrsDetailsEntity.Status.Name;

            return CrsDetailsVM;
            
        }
        public IEnumerable<CourseDetailsVM>? GetAllLearnixCourses()
        {
            
            var courses = _unitOfWork.Courses.GetAllLearnixCourses();

            return courses.Select(c => new CourseDetailsVM
            {
                Id = c.Id,
                Title = c.Title,
                Duration = c.Duration,
                ImageUrl = c.ImageUrl,
                Description = c.Description,
                LearningOutCome = c.LearningOutCome,
                Requirement = c.Requirement,
                Price = c.Price,
                IsFree = c.IsFree,
                InstructorName = c.Instructor.User.FirstName + " " + c.Instructor.User.LastName,
                CategoryName = c.Category.Name,
                LanguageName = c.Language.Name,
                LevelName = c.Level.Name,
                StatusName = c.Status.Name
            }).ToList();

        }
        public async Task<bool> EnrollInCourse(int courseId, string studentId)
        {
            using var transaction = _unitOfWork.BeginTransaction();

            var course = _unitOfWork.Courses.GetByID(courseId);
            var student = _unitOfWork.Students.GetByID(studentId);
            var instructor = _unitOfWork.Instructors.GetByID(course?.InstructorID);

            if (student == null || course == null || instructor == null)
                return false;

            
            if (_unitOfWork.Enrollements.IsStudentEnrolled(studentId, courseId))
                return false;

            try
            {
                decimal coursePrice = Convert.ToDecimal(course.Price);

                
                if (student.Balance < coursePrice)
                    return false;

               
                student.Balance -= coursePrice;

                
                decimal instructorShare = coursePrice * 0.95m;
                decimal adminShare = coursePrice * 0.05m;

                
                instructor.Balance += instructorShare;

                
                var admins = _unitOfWork.Admins.GetAll().ToList();
                if (admins.Count > 0)
                {
                    decimal sharePerAdmin = adminShare / admins.Count;

                    foreach (var admin in admins)
                    {
                        admin.Balance += sharePerAdmin;
                    }
                }

                
                var enrollment = new Enrollment
                {
                    CourseId = course.Id,
                    StudentId = student.Id,
                    EnrollementDate = DateTime.Now
                };
                _unitOfWork.Enrollements.Add(enrollment);

                
                var payment = new Payment
                {
                    Amount = coursePrice,
                    PaymentDate = DateTime.Now,
                    Status = "Success",
                    InstructorShare = instructorShare,
                    AdminShare = adminShare,
                    CourseId = course.Id,
                    StudentId = student.Id
                };
                _unitOfWork.Payments.Add(payment);

                
                _unitOfWork.Complete();
                transaction.Commit();


                var StdUser = await _userManager.FindByIdAsync(studentId);
                var InstructorUser = await _userManager.FindByIdAsync(course.InstructorID);

                var Email = new Email()
                {
                    ReceiverEmail = StdUser.Email,
                    Subject = $"Enrollment Confirmation – {course.Title}",
                    Body = $@"
Dear {StdUser.FirstName} {StdUser.LastName},

Congratulations! You have been successfully enrolled in the course:

**{course.Title}**
Instructor: {InstructorUser.FirstName} {InstructorUser.LastName}
Enrollment Date: {enrollment.EnrollementDate:dddd, MMMM dd, yyyy}

We are excited to have you in this course and look forward to your learning journey.  
If you have any questions or need assistance, feel free to reach out at any time.

Wishing you great success,
Learnix Team
"
                };
                await _emailService.SendEmailAsync(Email);

                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
        public async Task<AllCourseStudentsVM> GetAllCourseStudents(int courseId, string? search = null, int pageIndex = 1, int pageSize = 10)
        {

            var AllStdsVM = new AllCourseStudentsVM();

            var crs = GetById(courseId);

            AllStdsVM.CourseID = courseId;
            AllStdsVM.CourseName = crs.Title;
            

            var AllStds = _enrollementService.GetAllCourseStudents(courseId, search, pageIndex, pageSize);


            foreach ( var student in AllStds)
            {
                var CourseStdsVM = new CourseStudentsVM()
                {
                    StudentID = student.Id,
                    StudentImageUrl = student.User.ImageUrl,
                    StudentEmail = student.User.Email,
                    StudentName = student.User.FirstName + " " + student.User.LastName,
                    StudentProgressPercentage = await _studentLessonProgressService.GetCourseProgressAsync(student.Id, courseId)
                };

                AllStdsVM.AllCourseStudents.Add(CourseStdsVM);

            }


            

            return AllStdsVM;

        }
        public bool IsThisCourseBelongsToThisInstructor(int CourseID, string InstructorID)
        {
            return _unitOfWork.Courses.IsThisCourseBelongsToThisInstructor(CourseID, InstructorID);
        }
        public IEnumerable<CourseDto> GetAllCoursesBelongstoInstructor(string InstructorID)
        {
            var CoursesEntity = _unitOfWork.Courses.GetAllCoursesForSpecificInstructor(InstructorID);

            return CoursesEntity.Select(MapToDto);

           
        }
        public List<CourseReviewVM> GetAllDraftedandRejectedCourses(string? search = null, int pageIndex = 1, int pageSize = 10)
        {
            var CoursesEntity = _unitOfWork.Courses.GetAllDraftedandRejectedCourses(search, pageIndex, pageSize);

            var CoursesReviews = new List<CourseReviewVM>();

            foreach(var course in CoursesEntity)
            {
                var CrsVm = new CourseReviewVM()
                {
                    CourseId = course.Id,
                    CourseTitle = course.Title,
                    CourseImageUrl = course.ImageUrl,
                    CourseStatus = course.Status.Name,
                    CategoryName = course.Category.Name,
                    InstructorId = course.InstructorID,
                    InstructorFullName = course.Instructor.User.FirstName + " " + course.Instructor.User.LastName,
                    InstructorImageUrl = course.Instructor.User.ImageUrl,
                    InstructorEmail = course.Instructor.User.Email
                };

                CoursesReviews.Add(CrsVm);
            }


            return CoursesReviews;

        }
        public void ApproveCourseStatus(int courseID)
        {
            var Crs = _unitOfWork.Courses.GetByID(courseID);
            Crs.StatusID = 2;
            _unitOfWork.Complete();
           
        }
        public void RejectCourseStatus(int courseID)
        {
            var Crs = _unitOfWork.Courses.GetByID(courseID);
            Crs.StatusID = 1002;
            _unitOfWork.Complete();

        }

        public IEnumerable<HomePageCoursesVM> GetTop3PaidCourses()
        {

            var CoursesEntity = _unitOfWork.Courses.GetTop3PaidCourses();

            var TopCourses = new List<HomePageCoursesVM>();

            foreach (var course in CoursesEntity)
            {
                var CrsVm = new HomePageCoursesVM()
                {
                    Id = course.Id,
                    Title = course.Title,
                    ImageUrl = course.ImageUrl,
                    CategoryName = course.Category.Name,
                    Price = course.Price,
                    InstructorFullName = course.Instructor.User.FirstName + " " + course.Instructor.User.LastName,
                    InstructorImageUrl = course.Instructor.User.ImageUrl,
                    NumberofStudents = _unitOfWork.Courses.GetTotalNumberOfStudentsPerCourse(course.Id)
                };

                TopCourses.Add(CrsVm);
            }


            return TopCourses;
        }

        /*
         
         public async Task<PagedResult<CourseListItemVm>> GetCoursesAsync(CourseListFilterVm filter, CancellationToken ct = default)
        {
            var paged = await  _unitOfWork.C.GetPagedAsync(filter, ct);

            var vmItems = paged.Items.Select(c => new CourseListItemVm
            {
                Id = c.Id,
                Title = c.Title,
                ImageUrl = c.ImageUrl,
                ShortDescription = (c.Description?.Length > 150) ? c.Description.Substring(0, 150) + "…" : c.Description ?? "",
                CategoryName = c.Category?.Name ?? "Uncategorized",
                InstructorName = (c.Instructor != null && c.Instructor.User != null)
                    ? $"{c.Instructor.User.FirstName} {c.Instructor.User.LastName}"
                    : "Unknown",
                Price = c.Price,
                IsFree = c.IsFree,
                LevelName = c.Level?.Name ?? "",
                Duration = c.Duration ?? ""
                // Rating: compute from Reviews if you want
            }).ToList();

            return new PagedResult<CourseListItemVm>
            {
                CurrentPage = paged.CurrentPage,
                PageSize = paged.PageSize,
                TotalItems = paged.TotalItems,
                Items = vmItems
            };
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
            => Task.FromResult(_ctx.Categories.AsNoTracking().OrderBy(c => c.Name).AsEnumerable());

        public Task<IEnumerable<CourseLevel>> GetAllLevelsAsync()
            => Task.FromResult(_ctx.CourseLevels.AsNoTracking().OrderBy(l => l.Name).AsEnumerable());

        public Task<IEnumerable<ApplicationUser>> GetAllInstructorsAsync()
            => Task.FromResult(_ctx.Instructors.Include(i => i.User).Select(i => i.User).AsNoTracking().AsEnumerable());


         */



        #region ahmed & abdelaziz
        public async Task<IEnumerable<CourseListViewModel>> GetAllCoursesAsync(CourseFilterViewModel? filter = null)
        {
            var courses = await _courseRepository.GetAllAsync();

            if (filter != null)
            {
                courses = ApplyFilters(courses, filter);
            }

            return courses.Select(c => MapToCourseListViewModel(c));
        }

        public async Task<CourseDetailsViewModel?> GetCourseDetailsAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return course != null ? await MapToCourseDetailsViewModel(course) : null;
        }

        public async Task<IEnumerable<CourseListViewModel>> GetCoursesByInstructorAsync(string instructorId)
        {
            var courses = await _courseRepository.GetCoursesByInstructorAsync(instructorId);
            return courses.Select(c => MapToCourseListViewModel(c));
        }

        private IEnumerable<Course> ApplyFilters(IEnumerable<Course> courses, CourseFilterViewModel filter)
        {
            var query = courses.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(c =>
                    c.Title.Contains(filter.Search) ||
                    c.Description.Contains(filter.Search) ||
                    c.Instructor.User.UserName.Contains(filter.Search));
            }

            if (!string.IsNullOrEmpty(filter.Category))
            {
                query = query.Where(c => c.Category.Name == filter.Category);
            }

            if (!string.IsNullOrEmpty(filter.Level))
            {
                query = query.Where(c => c.Level.Name == filter.Level);
            }

            if (!string.IsNullOrEmpty(filter.Language))
            {
                query = query.Where(c => c.Language.Name == filter.Language);
            }

            if (!string.IsNullOrEmpty(filter.PriceRange))
            {
                query = filter.PriceRange switch
                {
                    "free" => query.Where(c => c.IsFree),
                    "0-50" => query.Where(c => !c.IsFree && c.Price <= 50),
                    "50-100" => query.Where(c => !c.IsFree && c.Price > 50 && c.Price <= 100),
                    "100-200" => query.Where(c => !c.IsFree && c.Price > 100 && c.Price <= 200),
                    "200+" => query.Where(c => !c.IsFree && c.Price > 200),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy switch
                {
                    "popular" => query.OrderByDescending(c => c.Enrollments.Count),
                    "newest" => query.OrderByDescending(c => c.Id),
                    "rating" => query.OrderByDescending(c => c.Reviews.Average(r => r.Rating)),
                    "price-low" => query.OrderBy(c => c.Price),
                    "price-high" => query.OrderByDescending(c => c.Price),
                    _ => query.OrderByDescending(c => c.Id)
                };
            }

            return query.ToList();
        }

        private CourseListViewModel MapToCourseListViewModel(Course course)
        {
            return new CourseListViewModel
            {
                Id = course.Id,
                ImageUrl = course.ImageUrl,
                Title = course.Title,
                Description = course.Description,
                InstructorName = course.Instructor?.User?.UserName ?? "Unknown",
                Price = (decimal?)course.Price,
                IsFree = course.IsFree,
                AverageRating = course.Reviews?.Any() == true ? course.Reviews.Average(r => r.Rating) : 0,
                TotalReviews = course.Reviews?.Count ?? 0,
                TotalStudents = course.Enrollments?.Count ?? 0,
                Duration = course.Duration,
                Category = course.Category?.Name ?? "Uncategorized",
                Level = course.Level?.Name ?? "All Levels",
                Language = course.Language?.Name ?? "English"
            };
        }

        private async Task<CourseDetailsViewModel> MapToCourseDetailsViewModel(Course course)
        {
            InstructorViewModel instructorVm = new InstructorViewModel();

            if (course.Instructor != null)
            {

                var instructorStats = await _courseRepository.GetInstructorStatisticsAsync(course.InstructorID);

                instructorVm = new InstructorViewModel
                {
                    Id = course.Instructor.Id,
                    Name = course.Instructor.User?.UserName ?? "Unknown Instructor",
                    Major = course.Instructor.Major,
                    TotalCourses = instructorStats.TotalCourses,
                    TotalStudents = instructorStats.TotalStudents,
                };
            }

            return new CourseDetailsViewModel
            {
                Id = course.Id,
                ImageUrl = course.ImageUrl,
                Title = course.Title,
                Description = course.Description,
                LearningOutCome = course.LearningOutCome,
                Duration = course.Duration,
                Requirement = course.Requirement,
                Price = (decimal?)course.Price,
                IsFree = course.IsFree,
                Instructor = instructorVm,
                Category = course.Category?.Name ?? "Uncategorized",
                Level = course.Level?.Name ?? "All Levels",
                Language = course.Language?.Name ?? "English",
                AverageRating = course.Reviews?.Any() == true ? Math.Round(course.Reviews.Average(r => r.Rating), 1) : 0,
                TotalReviews = course.Reviews?.Count ?? 0,
                TotalStudents = course.Enrollments?.Count ?? 0,
                TotalLessons = course.Sections?.Sum(s => s.Lessons?.Count ?? 0) ?? 0,
                Sections = course.Sections?.OrderBy(s => s.Order).Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                    Lessons = s.Lessons?.OrderBy(l => l.Order).Select(l => new LessonViewModel
                    {
                        LessonId = l.LessonId,
                        Title = l.Title,
                        Description = l.Description,
                        LearningObjectives = l.LearningObjectives,
                        VideoUrl = l.VideoUrl,
                        Duration = l.Duration,
                        Order = l.Order,
                        Materials = l.Materials?.Select(m => new LessonMaterialViewModel
                        {
                            Id = m.Id,
                            FileName = m.FileName,
                            FilePath = m.FilePath,
                            FileSize = m.FileSize
                        }).ToList() ?? new List<LessonMaterialViewModel>()
                    }).ToList() ?? new List<LessonViewModel>()
                }).ToList() ?? new List<SectionViewModel>(),
                Reviews = course.Reviews?.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    StudentName = r.Student?.User?.UserName ?? "Anonymous"
                }).ToList() ?? new List<ReviewViewModel>()
            };
        }
        


        #endregion

    }
}
