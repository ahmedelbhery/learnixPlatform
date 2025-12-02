using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.PaymentsDtos
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }

        public decimal InstructorShare { get; set; }
        public decimal AdminShare { get; set; }


        public string StudentId { get; set; }
        public Student Student { get; set; }


        
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
