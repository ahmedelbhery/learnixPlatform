namespace Learnix.ViewModels.StudentCourseDetailsVMs
{
    public class SectionViewModel
    {
        public int SectionId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<LessonViewModel> Lessons { get; set; }
    }
}
