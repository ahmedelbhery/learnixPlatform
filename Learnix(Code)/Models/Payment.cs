using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }


        public decimal InstructorShare { get; set; }
        public decimal AdminShare { get; set; }


        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Student Student { get; set; }


        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
