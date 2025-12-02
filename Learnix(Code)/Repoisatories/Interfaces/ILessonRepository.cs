using Learnix.Models;

namespace Learnix.Repoisatories.Interfaces
{
    public interface ILessonRepository : IGenericRepository<Lesson,int>
    {
        public bool CheckLessonOrder(int sectionId, int order);
        public IEnumerable<Lesson> GetLessonsBySectionId(int sectionId);

        int GetCourseIDforLesson(int SectionID);
        public bool ISThisLessonBelongsToThisInstructor(int LessonID, string InstructorID);
        
    }
}
