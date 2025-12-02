namespace Learnix.ViewModels.CourseDetailsVMs
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<LessonViewModel> Lessons { get; set; }
    }
}
