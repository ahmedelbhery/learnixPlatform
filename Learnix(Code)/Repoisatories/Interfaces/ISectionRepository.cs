using Learnix.Models;

namespace Learnix.Repoisatories.Interfaces
{
    public interface ISectionRepository : IGenericRepository<Section,int>
    {
        IEnumerable<Section> GetSectionsbyCourseIDinOrder(int CourseID);
        bool CheckOrderExists(int courseId, int order);
        bool ISThisSectionBelongsToThisInstructor(int SectionID,string InstructorID);
    }
}
