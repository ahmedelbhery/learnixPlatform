using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Implementations;
using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Repoisatories.Implementations
{
    public class StudentLessonProgressRepository : GenericRepository<StudentLessonProgress,int>,IStudentLessonProgressRepository
    {
        public StudentLessonProgressRepository(LearnixContext context) : base(context) { }


        public async Task<double> GetCourseProgressAsync(string studentId, int courseId)
        {
            var data = await _context.Lessons
                .Where(l => l.Section.CourseID == courseId)
                .GroupJoin(
                    _context.StudentLessonProgresses
                        .Where(p => p.StudentId == studentId && p.IsCompleted),
                    lesson => lesson.LessonId,
                    progress => progress.LessonId,
                    (lesson, progress) => new
                    {
                        Completed = progress.Any() ? 1 : 0
                    }
                )
                .ToListAsync();

            int total = data.Count;
            if (total == 0)
                return 0;

            int completed = data.Sum(x => x.Completed);

            return Math.Round((double)completed / total * 100, 2);
        }

    }
}
