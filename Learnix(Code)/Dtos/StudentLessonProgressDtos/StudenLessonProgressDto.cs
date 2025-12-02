using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.StudentLessonProgressDtos
{
    public class StudenLessonProgressDto
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletionDate { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
