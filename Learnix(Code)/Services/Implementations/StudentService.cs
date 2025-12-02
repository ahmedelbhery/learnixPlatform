using Learnix.Dtos.StudentDtos;
using Learnix.Models;
using Learnix.Repoisatories.Implementations;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.StudentCourseDetailsVMs;
using Learnix.ViewModels.StudentVMs;

namespace Learnix.Services.Implementations
{
    public class StudentService : GenericService<Student,StudentDto,string> , IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IUnitOfWork unitOfWork, IStudentRepository studentRepository) : base(unitOfWork)
        {
            _studentRepository = studentRepository;
        }

        public int GetToltalNumberOfStudentsForSpeceifcInstructor(string InstructorID)
        {
           return _unitOfWork.Students.GetToltalNumberOfStudentsForSpeceifcInstructor(InstructorID);
        }


        #region mohammed Yasser & mohammed atef
        public async Task<CourseListViewModel> GetStudentCoursesAsync(string studentId)
        {
            var enrolledCourses = await _studentRepository.GetEnrolledCoursesByStudentIdAsync(studentId);

            var courseViewModels = new List<CourseCardViewModel>();

            foreach (var course in enrolledCourses)
            {
                var totalLessons = await _studentRepository.GetTotalLessonsCountAsync(course.Id);
                var completedLessons = await _studentRepository.GetCompletedLessonsCountAsync(studentId, course.Id);

                var progressPercentage = totalLessons > 0 ? (decimal)completedLessons / totalLessons * 100 : 0;

                string status;
                if (completedLessons == 0)
                    status = "Not Started";
                else if (completedLessons == totalLessons)
                    status = "Completed";
                else
                    status = "In Progress";

                courseViewModels.Add(new CourseCardViewModel
                {
                    CourseId = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImageUrl = course.ImageUrl,
                    InstructorName = course.Instructor?.User?.UserName ?? "Unknown Instructor",
                    Status = status,
                    ProgressPercentage = progressPercentage,
                    CompletedLessons = completedLessons,
                    TotalLessons = totalLessons,
                    Duration = course.Duration,
                    Category = course.Category?.Name,
                    Level = course.Level?.Name
                });
            }

            var completedCoursesCount = courseViewModels.Count(c => c.Status == "Completed");
            var inProgressCoursesCount = courseViewModels.Count(c => c.Status == "In Progress");
            var notStartedCoursesCount = courseViewModels.Count(c => c.Status == "Not Started");

            return new CourseListViewModel
            {
                TotalEnrolledCourses = enrolledCourses.Count,
                CompletedCourses = completedCoursesCount,
                InProgressCourses = inProgressCoursesCount,
                NotStartedCourses = notStartedCoursesCount,
                CertificatesCount = completedCoursesCount,
                Courses = courseViewModels
            };
        }

        public async Task<CourseDetailsViewModel> GetCourseDetailsAsync(int courseId, string studentId)
        {
            // Debug first
            await _studentRepository.DebugCourseDataAsync(courseId, studentId);

            var course = await _studentRepository.GetCourseWithDetailsAsync(courseId);
            if (course == null) return null;

            var sections = await _studentRepository.GetSectionsWithLessonsAsync(courseId);
            var reviews = await _studentRepository.GetCourseReviewsAsync(courseId);
            var announcements = await _studentRepository.GetCourseAnnouncementsAsync(courseId);
            var studentProgress = await _studentRepository.GetStudentProgressAsync(studentId, courseId);

            // Calculate progress more carefully
            var totalLessons = await _studentRepository.GetTotalLessonsCountAsync(courseId);
            var completedLessons = await _studentRepository.GetCompletedLessonsCountAsync(studentId, courseId);

            Console.WriteLine($"Progress Calculation - Total: {totalLessons}, Completed: {completedLessons}");

            decimal progressPercentage = 0;
            if (totalLessons > 0)
            {
                progressPercentage = (decimal)completedLessons / totalLessons * 100;
            }

            Console.WriteLine($"Final Progress Percentage: {progressPercentage}%");

            var sectionViewModels = sections.Select(section => new SectionViewModel
            {
                SectionId = section.Id,
                Name = section.Name,
                Order = section.Order,
                Lessons = section.Lessons.OrderBy(l => l.Order).Select(lesson => new LessonViewModel
                {
                    LessonId = lesson.LessonId,
                    Title = lesson.Title,
                    Description = lesson.Description,
                    LearningObjectives = lesson.LearningObjectives,
                    Duration = lesson.Duration,
                    VideoUrl = lesson.VideoUrl,
                    Order = lesson.Order,
                    IsCompleted = studentProgress.Any(sp => sp.LessonId == lesson.LessonId && sp.IsCompleted),
                    CompletionDate = studentProgress.FirstOrDefault(sp => sp.LessonId == lesson.LessonId)?.CompletionDate,
                    Materials = lesson.Materials?.Select(m => new LessonMaterialViewModel
                    {
                        FileName = m.FileName,
                        FilePath = m.FilePath,
                        FileSize = m.FileSize
                    }).ToList() ?? new List<LessonMaterialViewModel>(),
                    SectionId = section.Id,
                    SectionName = section.Name
                }).ToList()
            }).ToList();

            var reviewViewModels = reviews.Select(review => new ReviewViewModel
            {
                StudentName = review.Student?.User?.UserName ?? "Anonymous",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            }).ToList();

            var announcementViewModels = announcements.Select(announcement => new AnnouncementViewModel
            {
                Title = announcement.Title,
                Message = announcement.Message,
                PostedAt = announcement.PostedAt
            }).ToList();

            var firstLesson = sectionViewModels.SelectMany(s => s.Lessons).FirstOrDefault();

            return new CourseDetailsViewModel
            {
                CourseId = course.Id,
                Title = course.Title,
                Description = course.Description,
                LearningOutCome = course.LearningOutCome,
                Requirement = course.Requirement,
                Duration = course.Duration,
                ImageUrl = course.ImageUrl,
                InstructorName = course.Instructor?.User?.UserName ?? "Unknown Instructor",
                InstructorMajor = course.Instructor?.Major,
                Category = course.Category?.Name,
                Language = course.Language?.Name,
                Level = course.Level?.Name,
                ProgressPercentage = progressPercentage,
                CompletedLessons = completedLessons,
                TotalLessons = totalLessons,
                Sections = sectionViewModels,
                Reviews = reviewViewModels,
                Announcements = announcementViewModels,
                CurrentLesson = firstLesson
            };
        }

        public async Task<bool> MarkLessonAsCompletedAsync(string studentId, int lessonId)
        {
            try
            {
                Console.WriteLine($"=== Starting MarkLessonAsCompletedAsync ===");
                Console.WriteLine($"Student: {studentId}, Lesson: {lessonId}");

                // First validate
                if (!await _studentRepository.ValidateLessonAndStudentAsync(studentId, lessonId))
                {
                    Console.WriteLine("Validation failed!");
                    return false;
                }

                var existingProgress = await _studentRepository.GetLessonProgressAsync(studentId, lessonId);
                Console.WriteLine($"Existing progress: {(existingProgress != null ? $"ID={existingProgress.Id}, Completed={existingProgress.IsCompleted}" : "null")}");

                if (existingProgress != null)
                {
                    if (!existingProgress.IsCompleted)
                    {
                        existingProgress.IsCompleted = true;
                        existingProgress.CompletionDate = DateTime.Now;
                        var result = await _studentRepository.UpdateLessonProgressAsync(existingProgress);
                        Console.WriteLine($"Update result: {result}");

                        if (result)
                        {
                            // Verify the update
                            var updatedProgress = await _studentRepository.GetLessonProgressAsync(studentId, lessonId);
                            Console.WriteLine($"After update - Completed: {updatedProgress?.IsCompleted}");
                        }

                        return result;
                    }
                    Console.WriteLine("Lesson already completed");
                    return true;
                }
                else
                {
                    var newProgress = new StudentLessonProgress
                    {
                        StudentId = studentId,
                        LessonId = lessonId,
                        IsCompleted = true,
                        CompletionDate = DateTime.Now
                    };

                    var result = await _studentRepository.UpdateLessonProgressAsync(newProgress);
                    Console.WriteLine($"Create new progress result: {result}");

                    if (result)
                    {
                        // Verify the creation
                        var createdProgress = await _studentRepository.GetLessonProgressAsync(studentId, lessonId);
                        Console.WriteLine($"After creation - Exists: {createdProgress != null}, Completed: {createdProgress?.IsCompleted}");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MarkLessonAsCompletedAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> IsStudentEnrolledInCourseAsync(string studentId, int courseId)
        {
            var enrolledCourses = await _studentRepository.GetEnrolledCoursesByStudentIdAsync(studentId);
            return enrolledCourses.Any(c => c.Id == courseId);
        }

        public async Task<LessonViewModel> GetLessonDetailsAsync(int lessonId, string studentId)
        {
            var lesson = await _studentRepository.GetLessonWithDetailsAsync(lessonId);
            if (lesson == null) return null;

            var progress = await _studentRepository.GetLessonProgressAsync(studentId, lessonId);

            return new LessonViewModel
            {
                LessonId = lesson.LessonId,
                Title = lesson.Title,
                Description = lesson.Description,
                LearningObjectives = lesson.LearningObjectives,
                Duration = lesson.Duration,
                VideoUrl = lesson.VideoUrl,
                Order = lesson.Order,
                IsCompleted = progress?.IsCompleted ?? false,
                CompletionDate = progress?.CompletionDate,
                Materials = lesson.Materials?.Select(m => new LessonMaterialViewModel
                {
                    FileName = m.FileName,
                    FilePath = m.FilePath,
                    FileSize = m.FileSize
                }).ToList() ?? new List<LessonMaterialViewModel>(),
                SectionId = lesson.SectionId,
                SectionName = lesson.Section.Name
            };
        }

        #endregion
    }
}
