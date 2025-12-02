using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Instructor
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public string? Major { get; set; }
        public decimal Balance { get; set; } = 0;


        public ApplicationUser User { get; set; }

        [ForeignKey("Status")]
        public int? StatusId { get; set; }
        public InstructorStatus Status { get; set; }



        public ICollection<Course> Courses { get; set; }
    }
}
