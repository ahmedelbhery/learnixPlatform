using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.SectionDtos
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }


        public int CourseID { get; set; }
       // public Course Course { get; set; }

       // public ICollection<Lesson> Lessons { get; set; }
    }
}
