using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class SectionRepository : GenericRepository<Section,int>, ISectionRepository
    {
        public SectionRepository(LearnixContext context) : base(context) { }

        public bool CheckOrderExists(int courseId, int order)
        {
            bool result = _dbSet.Any(s => s.CourseID == courseId && s.Order == order);

            return result;
        }

        public IEnumerable<Section> GetSectionsbyCourseIDinOrder(int CourseID)
        {
            var sections = _context.Sections.Where(s => s.CourseID == CourseID).OrderBy(s => s.Order).ToList();
            return sections;
        }

        public bool ISThisSectionBelongsToThisInstructor(int SectionID, string InstructorID)
        {
            return _dbSet.Any(s => s.Id == SectionID && s.Course.InstructorID == InstructorID);

        }
    }
}
