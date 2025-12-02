using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }



        [ForeignKey("Student")]
        public string StudentID { get; set; }
        public Student Student { get; set; }



        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
