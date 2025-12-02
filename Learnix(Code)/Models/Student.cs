using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Student
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public decimal Balance { get; set; } = 500;


        public ApplicationUser User { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<StudentLessonProgress> StudentLessonProgresses { get; set; }


    }
}
