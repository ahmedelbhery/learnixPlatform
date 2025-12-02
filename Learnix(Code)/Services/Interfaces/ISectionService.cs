using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.SectionDtos;
using Learnix.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace Learnix.Services.Interfaces
{
    public interface ISectionService : IGenericService<Section, SectionDto,int>
    {
        IEnumerable<CourseDto> GetAllCoursesBelongsToInstructor(string InstructorID);
        public IEnumerable<SectionDto> GetSectionsbyCourseIDinOrder(int CourseID);
        public bool CheckOrderExists(int courseId, int order);
        public bool ISThisSectionBelongsToThisInstructor(int SectionID, string InstructorID);
       
    }
}
