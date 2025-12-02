using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }
        public DateTime EnrollementDate { get; set; }
       


        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }


        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
