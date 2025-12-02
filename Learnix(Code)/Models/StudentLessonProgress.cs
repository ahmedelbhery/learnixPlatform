using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class StudentLessonProgress
    {
        [Key]
        public int Id { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletionDate { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}


