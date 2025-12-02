using System.ComponentModel.DataAnnotations;

namespace Learnix.Models
{
    public class CourseStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
