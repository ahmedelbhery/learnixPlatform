using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.LessonDtos;
using Learnix.Dtos.LessonStatusDtos;
using Learnix.Dtos.SectionDtos;
using Learnix.Models;
using Learnix.ViewModels.LessonVMs;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace Learnix.Services.Interfaces
{
    public interface ILessonService : IGenericService<Lesson, LessonDto,int>
    {
        IEnumerable<CourseDto> GetAllCourses();
        IEnumerable<LessonStatusDto> GetAllLessonStatus();
        public bool CheckLessonOrder(int sectionId, int order);
        public IEnumerable<LessonDto> GetLessonsBySectionId(int sectionId);
        public IEnumerable<SectionDto> GetSectionsByCourseId(int courseId);
        public int GetCourseIDforLesson(int SectionID);
        public Task UpdateLesson(LessonEditVM vm);
        public bool ISThisLessonBelongsToThisInstructor(int LessonID, string InstructorID);
        
    }
}
