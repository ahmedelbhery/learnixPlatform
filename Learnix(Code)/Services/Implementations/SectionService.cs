using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.SectionDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class SectionService : GenericService<Section,SectionDto,int> , ISectionService
    {
        private readonly ICourseService _courseService;
        public SectionService(IUnitOfWork unitOfWork,ICourseService courseService) : base(unitOfWork) 
        {
            this._courseService = courseService;
        }



        public bool CheckOrderExists(int courseId, int order)
        {
           return _unitOfWork.Sections.CheckOrderExists(courseId, order);
        }

        public IEnumerable<CourseDto> GetAllCoursesBelongsToInstructor(string InstructorID)
        {
            var courses = _courseService.GetAllCoursesBelongstoInstructor(InstructorID);
            return courses;
        }

        public IEnumerable<SectionDto> GetSectionsbyCourseIDinOrder(int CourseID)
        {
            var SectionDtos = new List<SectionDto>();

            var Sections = _unitOfWork.Sections.GetSectionsbyCourseIDinOrder(CourseID);

            foreach (var section in Sections)
            {
                var sectionDto = new SectionDto() 
                {
                    Id = section.Id,
                    Name = section.Name,
                    Order = section.Order,
                    CourseID = section.CourseID,
                };

                SectionDtos.Add(sectionDto);
            }


            return SectionDtos;
        }

        public bool ISThisSectionBelongsToThisInstructor(int SectionID, string InstructorID)
        {
            return _unitOfWork.Sections.ISThisSectionBelongsToThisInstructor(SectionID, InstructorID);

        }

    }
}
