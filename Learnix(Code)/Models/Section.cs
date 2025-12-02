using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public Course Course { get; set; }


        public ICollection<Lesson> Lessons { get; set; }

    }
}
