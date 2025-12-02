using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.ViewModels.CourseDetailsVMs;
using Learnix.ViewModels.CoursesVMs;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System.Linq.Expressions;

namespace Learnix.Repoisatories.Implementations
{
    public class CourseRepository : GenericRepository<Course,int> , ICourseRepository
    {
        public CourseRepository(LearnixContext context) : base(context) { }

        public async Task<Course> GetAllCourseContent(int courseID)
        {
            var course = await _context.Courses
             .Include(c => c.Sections)
                 .ThenInclude(s => s.Lessons)
             .FirstOrDefaultAsync(c => c.Id == courseID);

            return course;
        }
        public IEnumerable<Course> GetAllCoursesForSpecificInstructor(string InstructorID)
        {
            var Courses = _dbSet.Where(C => C.InstructorID == InstructorID).ToList();
            return Courses;
        }
        public IEnumerable<Course> GetAllDraftedandRejectedCourses(string? search = null, int pageIndex = 1, int pageSize = 10)
        {


          var query = _dbSet
         .Where(c => c.StatusID == 1 || c.StatusID == 1002)
         .Include(c => c.Status)
         .Include(c => c.Instructor).ThenInclude(i => i.User)
         .Include(c => c.Category)
         .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                query = query.Where(c =>
                    c.Instructor.User.FirstName.ToLower().Contains(search) ||
                    c.Instructor.User.LastName.ToLower().Contains(search) ||
                    c.Title.ToLower().Contains(search) ||
                    c.Category.Name.ToLower().Contains(search)
                );
            }

            return query
                .OrderBy(c => c.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();


        }
        public IEnumerable<Course> GetAllDraftedCoursesForSpecificInstructor(string InstructorID)
        {
            var DraftedCourses = _dbSet.Where(C => C.InstructorID == InstructorID && C.StatusID == 1).ToList();
            return DraftedCourses;
        }
        public IEnumerable<Course>? GetAllLearnixCourses()
        {
            var Crs = _dbSet.Include(C => C.Category).Include(C => C.Instructor).ThenInclude(I => I.User).Include(C => C.Status)
                           .Include(C => C.Language).Include(C => C.Level);

            if (Crs == null)
                return null;

            return Crs;
        }
        public IEnumerable<Course> GetAllPublishedCoursesForSpecificInstructor(string InstructorID)
        {
            var PublishedCourses = _dbSet.Where(C => C.InstructorID == InstructorID && C.StatusID == 2).ToList();
            return PublishedCourses;
        }
        public IEnumerable<Course> GetAllRejectedCoursesForSpecificInstructor(string InstructorID)
        {
            var RejectedCourses = _dbSet.Where(C => C.InstructorID == InstructorID && C.StatusID == 1002).ToList();
            return RejectedCourses;
        }
        public string GetCategoryNameforCourse(int courseID)
        {
           var CatName = _dbSet.Where(c => c.Id == courseID).Select(c => c.Category.Name).FirstOrDefault() ?? "Unknown Category";
           return CatName;
        }
        public Course? GetCourseDetails(int courseID)
        {
            var Crs = _dbSet.Where(C => C.Id == courseID).Include(C => C.Category).Include(C => C.Instructor).ThenInclude(I => I.User).Include(C => C.Status)
                            .Include(C => C.Language).Include(C => C.Level).FirstOrDefault();

            if (Crs == null)
                return null;

            return Crs;
        }
        /*
         
        public async Task<PagedResult<Course>> GetPagedAsync(CourseListFilterVm filter, CancellationToken ct = default)
        {
            var query = _context.Courses
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.Instructor).ThenInclude(i => i.User) // adjust based on your navigation
                .Include(c => c.Level) // CourseLevels
                .Where(c => c.StatusID != null); // optional: only published

            // Search by title or category or instructor name
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.Trim();
                query = query.Where(c =>
                    c.Title.Contains(s) ||
                    (c.Category != null && c.Category.Name.Contains(s)) ||
                    (c.Instructor != null && (c.Instructor.User.FirstName + " " + c.Instructor.User.LastName).Contains(s))
                );
            }

            if (filter.CategoryId.HasValue)
                query = query.Where(c => c.CategoryID == filter.CategoryId.Value);

            if (!string.IsNullOrEmpty(filter.InstructorId))
                query = query.Where(c => c.InstructorID == filter.InstructorId);

            if (filter.LevelId.HasValue)
                query = query.Where(c => c.LevelID == filter.LevelId.Value);

            if (filter.OnlyFree)
                query = query.Where(c => c.IsFree);

            if (filter.MinPrice.HasValue)
                query = query.Where(c => (c.Price ?? 0) >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(c => (c.Price ?? 0) <= filter.MaxPrice.Value);

            // Sorting - simple: featured -> price asc -> title
            query = query.OrderByDescending(c => c.Id); // replace with better sorting if you have popularity/rating

            var total = await query.CountAsync(ct);

            // Pagination
            var skip = (Math.Max(1, filter.Page) - 1) * filter.PageSize;
            var items = await query
                .Skip(skip)
                .Take(filter.PageSize)
                .ToListAsync(ct);

            return new PagedResult<Course>
            {
                CurrentPage = Math.Max(1, filter.Page),
                PageSize = filter.PageSize,
                TotalItems = total,
                Items = items
            };
        }
         
         
         */
        public string GetStatusNameforCourse(int courseID)
        {
            var StName = _dbSet.Where(c => c.Id == courseID).Select(c => c.Status.Name).FirstOrDefault() ?? "Unknown Status";
            return StName;
        }
        public IEnumerable<Course> GetTheTop3EnrolledCoursesForSpecificInstructor(string InstructorID)
        {
            var Top3Crs = _dbSet.Where(c => c.InstructorID == InstructorID && c.StatusID == 2)
                                .OrderByDescending(c => c.Enrollments.Count)
                                .Take(3)
                                .ToList();

            return Top3Crs;
        }
        public IEnumerable<Course> GetTheTop3ProfitableCoursesForSpecificInstructor(string InstructorID)
        {
            var Top3Crs = _dbSet.Where(c => c.InstructorID == InstructorID && c.StatusID==2)
                                 .OrderByDescending(c => (c.Enrollments.Count * c.Price))
                                 .Take(3)
                                 .ToList();

            return Top3Crs;
        }
        public double? GetTotalEarningMonyPerCourse(int courseID)
        {
            var Crs = GetByID(courseID);
            double? TotalEarning = _context.Enrollments.Where(E => E.CourseId == courseID).Count() * Crs.Price;

            if(TotalEarning == null)
                return 0;

            else
                return TotalEarning;
        }
        public int GetTotalNumberOfCoursebyInstructorID(string InstructorID)
        {
            var TotalNumberOfCourses = _dbSet.Where(C => C.InstructorID == InstructorID).Count();
            return TotalNumberOfCourses;
        }
        public int GetTotalNumberofDraftedCoursesforSpecificInstructor(string InstructorID)
        {
            //StatusID = 1 (Draft)
            var NumofDraftedCourses = _dbSet.Where(C => C.InstructorID == InstructorID && C.StatusID == 1).Count();
            return NumofDraftedCourses;
        }
        public int GetTotalNumberofPublishedCoursesforSpecificInstructor(string InstructorID)
        {
            //StatusID = 2 (Publish)
            var NumofDraftedCourses = _dbSet.Where(C => C.InstructorID == InstructorID && C.StatusID == 2).Count();
            return NumofDraftedCourses;
        }
        public int GetTotalNumberofRejectedCoursesforSpecificInstructor(string InstructorID)
        {
            //StatusID = 1002 (Rejected)
            var NumofDraftedCourses = _dbSet.Where(C => C.InstructorID == InstructorID && C.StatusID == 1002).Count();
            return NumofDraftedCourses;
        }
        public int GetTotalNumberOfStudentsPerCourse(int courseID)
        {
            var NumOfStds = _context.Enrollments
                            .Where(e => e.CourseId == courseID)
                            .Select(e => e.StudentId)
                            .Distinct()
                            .Count();
            return NumOfStds;
        }
        public bool IsThisCourseBelongsToThisInstructor(int CourseID, string InstructorID)
        {
            string? ActualInstructorID = _dbSet.Where(C => C.Id == CourseID).Select(C => C.InstructorID).FirstOrDefault();

            return (ActualInstructorID == InstructorID);
        }
        public IEnumerable<Course> GetTop3PaidCourses()
        {
           var Top3Crs = _dbSet.Where(c => c.StatusID==2).Include(c => c.Category).Include(c => c.Instructor).ThenInclude(i => i.User)
                                 .OrderByDescending(c => (c.Enrollments.Count * c.Price))
                                 .Take(3)
                                 .ToList();

            return Top3Crs;
        }



        #region ahmed & abdelaziz

        public async Task<IEnumerable<Course>> GetAllAsync()
        {

            return await _context.Courses
           .Where(c => c.StatusID == 2
               && c.Sections.Any()
               && c.Sections.Any(s => s.Lessons.Any())
           )
           .Include(c => c.Instructor)
               .ThenInclude(i => i.User)
           .Include(c => c.Category)
           .Include(c => c.Level)
           .Include(c => c.Language)
           .Include(c => c.Status)
           .Include(c => c.Reviews)
           .Include(c => c.Enrollments)
           .Include(c => c.Sections)
               .ThenInclude(s => s.Lessons)
           .ToListAsync();
        }


        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                    .ThenInclude(i => i.User)
                .Include(c => c.Category)
                .Include(c => c.Level)
                .Include(c => c.Language)
                .Include(c => c.Status)
                .Include(c => c.Reviews)
                    .ThenInclude(r => r.Student)
                        .ThenInclude(s => s.User)
                .Include(c => c.Enrollments)
                .Include(c => c.Sections)
                    .ThenInclude(s => s.Lessons)
                        .ThenInclude(l => l.Materials)
                .Include(c => c.Sections)
                    .ThenInclude(s => s.Lessons)
                        .ThenInclude(l => l.Status)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetByFilterAsync(Expression<Func<Course, bool>> filter)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                    .ThenInclude(i => i.User)
                .Include(c => c.Category)
                .Include(c => c.Level)
                .Include(c => c.Language)
                .Include(c => c.Status)
                .Include(c => c.Reviews)
                .Include(c => c.Enrollments)
                .Where(filter)
                .ToListAsync();
        }


        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string instructorId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                    .ThenInclude(i => i.User)
                .Include(c => c.Category)
                .Include(c => c.Reviews)
                .Include(c => c.Enrollments)
                .Where(c => c.InstructorID == instructorId)
                .ToListAsync();
        }

        public async Task<InstructorStatistics> GetInstructorStatisticsAsync(string instructorId)
        {
            var statistics = new InstructorStatistics();


            var instructorCourses = await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.Reviews)
                .Where(c => c.InstructorID == instructorId)
                .ToListAsync();


            statistics.TotalCourses = instructorCourses.Count;

            statistics.TotalStudents = instructorCourses.Sum(c => c.Enrollments?.Count ?? 0);


            var allReviews = instructorCourses
                .SelectMany(c => c.Reviews ?? new List<Review>())
                .ToList();

            if (allReviews.Any())
            {
                statistics.AverageRating = allReviews.Average(r => r.Rating);
                statistics.TotalReviews = allReviews.Count;
            }
            else
            {
                statistics.AverageRating = 0;
                statistics.TotalReviews = 0;
            }

            return statistics;
        }

        #endregion



    }
}






