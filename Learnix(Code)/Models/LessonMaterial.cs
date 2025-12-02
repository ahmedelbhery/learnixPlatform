using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class LessonMaterial
    {
        [Key]
        public int Id { get; set; }
        public string? FileName { get; set; } 
        public string FilePath { get; set; }
        public long? FileSize { get; set; }




        [ForeignKey("Lesson")]        
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
