using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.LessonDtos;
using Learnix.Dtos.LessonStatusDtos;
using Learnix.Dtos.SectionDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.LessonVMs;

namespace Learnix.Services.Implementations
{
    public class LessonService : GenericService<Lesson,LessonDto,int> , ILessonService
    {
        private readonly ICourseService _courseService;
        private readonly ISectionService _sectionService;
        private readonly IGenericService<LessonStatus,LessonStatusDto,int> _lessonStatusService;
        private readonly IVideoService _videoService;
        public LessonService(IUnitOfWork unitOfWork,ICourseService courseService, IGenericService<LessonStatus, LessonStatusDto, int> lessonStatusService,ISectionService sectionService,IVideoService videoService) : base(unitOfWork) 
        {
            this._courseService = courseService;
            this._lessonStatusService = lessonStatusService;
            this._sectionService = sectionService;
            this._videoService = videoService;
        }

        public bool CheckLessonOrder(int sectionId, int order)
        {
            return _unitOfWork.Lessons.CheckLessonOrder(sectionId, order);
        }

        public IEnumerable<CourseDto> GetAllCourses()
        {
            return _courseService.GetAll();
        }

        public IEnumerable<LessonStatusDto> GetAllLessonStatus()
        {
            return _lessonStatusService.GetAll();
        }

        public int GetCourseIDforLesson(int SectionID)
        {
            return _unitOfWork.Lessons.GetCourseIDforLesson(SectionID);
        }

        public IEnumerable<LessonDto> GetLessonsBySectionId(int sectionId)
        {
            var Lessons = _unitOfWork.Lessons.GetLessonsBySectionId(sectionId);

            return Lessons.Select(MapToDto);


        }

        public IEnumerable<SectionDto> GetSectionsByCourseId(int courseId)
        {
            return _sectionService.GetSectionsbyCourseIDinOrder(courseId);
        }


        public async Task UpdateLesson(LessonEditVM vm)
        {
            
            var lesson = _unitOfWork.Lessons.GetByID(vm.LessonId);
               

            if (lesson == null)
                throw new Exception("Lesson not found");

            
            lesson.Title = vm.Title;
            lesson.Description = vm.Description;
            lesson.LearningObjectives = vm.LearningObjectives;
            lesson.Duration = vm.Duration;

            
            if (vm.VideoFile != null && vm.VideoFile.Length > 0)
            {
                await _videoService.DeleteVideoAsync(lesson.VideoUrl);
                lesson.VideoUrl = await _videoService.UploadVideoAsync(vm.VideoFile, "Lessons");
            }

            
            _unitOfWork.Complete();
        }


        public bool ISThisLessonBelongsToThisInstructor(int LessonID, string InstructorID)
        {
            return _unitOfWork.Lessons.ISThisLessonBelongsToThisInstructor(LessonID, InstructorID);
        }

    }
}
