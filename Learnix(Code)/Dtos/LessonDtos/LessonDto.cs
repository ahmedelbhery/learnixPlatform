using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.LessonDtos
{
    public class LessonDto
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningObjectives { get; set; }
        public string? VideoUrl { get; set; }
        public string? Duration { get; set; }
        public int Order { get; set; }



        
        public int SectionId { get; set; }
        public Section Section { get; set; }


       
        public int StatusID { get; set; }
        public LessonStatus Status { get; set; }
    }
}
