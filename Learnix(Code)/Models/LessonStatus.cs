using System.ComponentModel.DataAnnotations;

namespace Learnix.Models
{
    public class LessonStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
