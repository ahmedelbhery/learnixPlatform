using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningObjectives { get; set; }
        public string? VideoUrl { get; set; } 
        public string? Duration { get; set; }
        public int Order {  get; set; }

        
        public ICollection<LessonMaterial> Materials { get; set; }
        public ICollection<StudentLessonProgress> StudentLessonProgresses { get; set; }


        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }


        [ForeignKey("Status")]
        public int? StatusID { get; set; }
        public LessonStatus? Status { get; set; }
    }
}
