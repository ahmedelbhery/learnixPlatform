using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.AdminVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Learnix.Repoisatories.Implementations.AdminRepository;

namespace Learnix.Repoisatories.Implementations
{
    public class AdminRepository : GenericRepository<Admin, string>, IAdminRepository
    {
        private readonly LearnixContext _context;
        public AdminRepository(LearnixContext context) : base(context) { _context = context; }


        // Instructor - Implementation
        public async Task<List<string>> GetInstructorSpecialtiesAsync()
        {
            return await _context.Instructors
                .Select(i => i.Major)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IQueryable<Instructor>> GetInstructorsWithUserAsync()
        {
            return _context.Instructors
                .Include(i => i.User)
                .AsQueryable();
        }

        public async Task<IQueryable<Instructor>> GetPendingInstructorsWithUserAsync()
        {
            return _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Status)
                .Where(i => i.Status.Name == "Pending" || i.Status.Name == "Rejected")
                .AsQueryable();
        }

        public async Task<Instructor> GetInstructorWithUserByIdAsync(string id)
        {
            return await _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateInstructorAsync(Instructor instructor)
        {
            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCoursesCountByInstructorIdsAsync(List<string> instructorIds)
        {
            return await _context.Courses
                .CountAsync(c => instructorIds.Contains(c.InstructorID));
        }

        public async Task<int> GetStudentsCountByInstructorIdsAsync(List<string> instructorIds)
        {
            return await _context.Enrollments
                .Where(e => instructorIds.Contains(e.Course.InstructorID))
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync();
        }

        public async Task<decimal> GetEarningsByInstructorIdsAsync(List<string> instructorIds)
        {
            return await _context.Payments
                .Where(p => instructorIds.Contains(p.Course.InstructorID))
                .SumAsync(p => p.Amount);
        }

        public async Task<int> GetTotalCoursesCountAsync()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<InstructorStatus> GetInstructorStatus(string status)
        {
            var Status = await _context.InstructorStatus
               .FirstOrDefaultAsync(s => s.Name == status);
            return Status;
        }

        public async Task<int> GetTotalStudentsCountAsync()
        {
            return await _context.Enrollments
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync();
        }

        public async Task<decimal> GetTotalEarningsAsync()
        {
            return await _context.Payments.SumAsync(p => p.Amount);
        }





        // Courses - Implementation
        public async Task<IQueryable<Course>> GetCoursesWithIncludesAsync()
        {
            return _context.Courses
                .Include(c => c.Instructor.User)
                .Include(c => c.Category)
                .Include(c => c.Language)
                .Include(c => c.Level)
                .Include(c => c.Status)
                .AsQueryable();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor.User)
                .Include(c => c.Category)
                .Include(c => c.Language)
                .Include(c => c.Level)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCoursesCountAsync()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<int> GetCoursesCountByStatusAsync(string status)
        {
            return await _context.Courses.CountAsync(c => c.Status.Name == status);
        }

        public async Task<int> GetEnrollmentsCountAsync()
        {
            return await _context.Enrollments.CountAsync();
        }

        public async Task<List<string>> GetCourseCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetCourseStatusesAsync()
        {
            return await _context.CourseStatuses
                .Select(cs => cs.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> GetEnrollmentsCountByCourseIdAsync(int courseId)
        {
            return await _context.Enrollments.CountAsync(e => e.CourseId == courseId);
        }

        public async Task<int> GetLessonsCountByCourseIdAsync(int courseId)
        {
            return await _context.Lessons.CountAsync(l => l.Section.CourseID == courseId);
        }

        public async Task<int> GetDurationSumByCourseIdAsync(int courseId)
        {
            var lessons = await _context.Lessons
                .Where(l => l.Section.CourseID == courseId)
                .Select(l => l.Duration)
                .ToListAsync();

            int totalDuration = 0;

            foreach (var durationStr in lessons)
            {
                if (string.IsNullOrEmpty(durationStr))
                    continue;

                if (durationStr.EndsWith("m"))
                {
                    if (int.TryParse(durationStr.TrimEnd('m'), out int minutes))
                    {
                        totalDuration += minutes;
                    }
                }

                else if (durationStr.EndsWith("h"))
                {
                    if (int.TryParse(durationStr.TrimEnd('h'), out int hours))
                    {
                        totalDuration += hours * 60;
                    }
                }

                else if (int.TryParse(durationStr, out int minutes))
                {
                    totalDuration += minutes;
                }
            }

            return totalDuration;
        }

        public async Task<Course?> GetCourseWithIncludesByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor.User)
                .Include(c => c.Category)
                .Include(c => c.Language)
                .Include(c => c.Level)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


    }
}
