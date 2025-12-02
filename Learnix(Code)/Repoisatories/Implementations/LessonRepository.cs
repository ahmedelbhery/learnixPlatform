using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class LessonRepository : GenericRepository<Lesson,int>, ILessonRepository
    {
        public LessonRepository(LearnixContext context) : base(context) { }

        public bool CheckLessonOrder(int sectionId, int order)
        {
            bool result = _context.Lessons.Any(l => l.SectionId == sectionId && l.Order == order);
            return result;
        }

        public int GetCourseIDforLesson(int SectionID)
        {
            return _context.Sections.Where(S=> S.Id == SectionID).Select(S=> S.CourseID).FirstOrDefault();
        }

        public IEnumerable<Lesson> GetLessonsBySectionId(int sectionId)
        {
            var Lessons = _dbSet.Where(L => L.SectionId == sectionId).OrderBy(l => l.Order).ToList();
            return Lessons;
        }

        public bool ISThisLessonBelongsToThisInstructor(int LessonID, string InstructorID)
        {
            return _dbSet.Any(L => L.LessonId == LessonID && L.Section.Course.InstructorID == InstructorID);
        }

    }
}
