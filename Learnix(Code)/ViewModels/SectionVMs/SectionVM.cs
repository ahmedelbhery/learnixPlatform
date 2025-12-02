using Learnix.ViewModels.LessonVMs;

namespace Learnix.ViewModels.SectionVMs
{
    public class SectionVM
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int SectionOrder { get; set; }
        public List<LessonVM> Lessons { get; set; }
    }
}
