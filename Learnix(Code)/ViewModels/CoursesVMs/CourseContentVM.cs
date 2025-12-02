using Learnix.ViewModels.SectionVMs;

namespace Learnix.ViewModels.CoursesVMs
{
    public class CourseContentVM
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<SectionVM> Sections { get; set; }
    }
}

