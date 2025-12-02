using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.LessonMaterialsDtos
{
    public class LessonMaterialDto
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string FilePath { get; set; }
        public long? FileSize { get; set; }


        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
