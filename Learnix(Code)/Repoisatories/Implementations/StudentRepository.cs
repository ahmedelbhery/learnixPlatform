using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Repoisatories.Implementations
{
    public class StudentRepository : GenericRepository<Student,string>, IStudentRepository
    {
        public StudentRepository(LearnixContext context) : base(context) { }

        public int GetToltalNumberOfStudentsForSpeceifcInstructor(string InstructorID)
        {
            var TotalNumberOfStudents = _context.Courses.Where(C => C.InstructorID == InstructorID)
                                        .SelectMany(C => C.Enrollments).Select(E => E.StudentId)
                                        .Distinct().Count();

            return TotalNumberOfStudents;
        }

        #region mohammed Yasser & mohammed atef
        public async Task<List<Course>> GetEnrolledCoursesByStudentIdAsync(string studentId)
        {
            return await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                    .ThenInclude(i => i.User)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Category)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Level)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Language)
                .Select(e => e.Course)
                .ToListAsync();
        }

        public async Task<Course> GetCourseWithDetailsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                    .ThenInclude(i => i.User)
                .Include(c => c.Category)
                .Include(c => c.Language)
                .Include(c => c.Level)
                .Include(c => c.Status)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<List<StudentLessonProgress>> GetStudentProgressAsync(string studentId, int courseId)
        {
            return await _context.StudentLessonProgresses
                .Include(slp => slp.Lesson)
                .Where(slp => slp.StudentId == studentId && slp.Lesson.Section.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<List<Section>> GetSectionsWithLessonsAsync(int courseId)
        {
            return await _context.Sections
                .Where(s => s.CourseID == courseId)
                .Include(s => s.Lessons)
                    .ThenInclude(l => l.Materials)
                .Include(s => s.Lessons)
                    .ThenInclude(l => l.Status)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<List<Review>> GetCourseReviewsAsync(int courseId)
        {
            return await _context.Reviews
                .Where(r => r.CourseID == courseId)
                .Include(r => r.Student)
                    .ThenInclude(s => s.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Announcement>> GetCourseAnnouncementsAsync(int courseId)
        {
            return await _context.Announcements
                .Where(a => a.CourseID == courseId)
                .OrderByDescending(a => a.PostedAt)
                .ToListAsync();
        }

        public async Task<int> GetTotalLessonsCountAsync(int courseId)
        {
            return await _context.Lessons
                .Where(l => l.Section.CourseID == courseId)
                .CountAsync();
        }

        public async Task<int> GetCompletedLessonsCountAsync(string studentId, int courseId)
        {
            return await _context.StudentLessonProgresses
                .Where(slp => slp.StudentId == studentId &&
                             slp.IsCompleted &&
                             slp.Lesson.Section.CourseID == courseId)
                .CountAsync();
        }

        public async Task<StudentLessonProgress> GetLessonProgressAsync(string studentId, int lessonId)
        {
            return await _context.StudentLessonProgresses
                .FirstOrDefaultAsync(slp => slp.StudentId == studentId && slp.LessonId == lessonId);
        }

        public async Task<Lesson> GetLessonWithDetailsAsync(int lessonId)
        {
            return await _context.Lessons
                .Include(l => l.Materials)
                .Include(l => l.Section)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
        }

        public async Task<bool> LessonExistsAsync(int lessonId)
        {
            return await _context.Lessons.AnyAsync(l => l.LessonId == lessonId);
        }

        public async Task<bool> UpdateLessonProgressAsync(StudentLessonProgress progress)
        {
            try
            {
                // Verify the lesson exists
                var lessonExists = await _context.Lessons.AnyAsync(l => l.LessonId == progress.LessonId);
                if (!lessonExists)
                {
                    return false;
                }

                // Verify the student exists
                var studentExists = await _context.Students.AnyAsync(s => s.Id == progress.StudentId);
                if (!studentExists)
                {
                    return false;
                }

                if (progress.Id == 0)
                {
                    await _context.StudentLessonProgresses.AddAsync(progress);
                }
                else
                {
                    _context.StudentLessonProgresses.Update(progress);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error updating lesson progress: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> ValidateLessonAndStudentAsync(string studentId, int lessonId)
        {
            var lessonExists = await _context.Lessons.AnyAsync(l => l.LessonId == lessonId);
            var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);

            Console.WriteLine($"Lesson exists: {lessonExists}, Student exists: {studentExists}");

            if (!lessonExists)
            {
                var availableLessons = await _context.Lessons.Select(l => new { l.LessonId, l.Title }).ToListAsync();
                Console.WriteLine("Available lessons:");
                foreach (var lesson in availableLessons)
                {
                    Console.WriteLine($"Lesson ID: {lesson.LessonId}, Title: {lesson.Title}");
                }
            }

            return lessonExists && studentExists;
        }

        public async Task DebugCourseDataAsync(int courseId, string studentId)
        {
            Console.WriteLine($"=== Debugging Course Data for Course {courseId}, Student {studentId} ===");

            // Check total lessons
            var totalLessons = await GetTotalLessonsCountAsync(courseId);
            Console.WriteLine($"Total Lessons: {totalLessons}");

            // Check completed lessons
            var completedLessons = await GetCompletedLessonsCountAsync(studentId, courseId);
            Console.WriteLine($"Completed Lessons: {completedLessons}");

            // Check individual progress records
            var progressRecords = await _context.StudentLessonProgresses
                .Where(slp => slp.StudentId == studentId && slp.Lesson.Section.CourseID == courseId)
                .Select(slp => new { slp.LessonId, slp.IsCompleted, LessonTitle = slp.Lesson.Title })
                .ToListAsync();

            Console.WriteLine("Progress Records:");
            foreach (var record in progressRecords)
            {
                Console.WriteLine($"Lesson {record.LessonId} ({record.LessonTitle}): Completed = {record.IsCompleted}");
            }

            // Check sections and lessons
            var sections = await _context.Sections
                .Where(s => s.CourseID == courseId)
                .Include(s => s.Lessons)
                .ToListAsync();

            Console.WriteLine("Sections and Lessons:");
            foreach (var section in sections)
            {
                Console.WriteLine($"Section {section.Id}: {section.Name} ({section.Lessons.Count} lessons)");
                foreach (var lesson in section.Lessons)
                {
                    Console.WriteLine($"  - Lesson {lesson.LessonId}: {lesson.Title}");
                }
            }
        }
        #endregion

    }
}
