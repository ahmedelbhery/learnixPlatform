using System.ComponentModel.DataAnnotations;

namespace Learnix.Models
{
    public class InstructorStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Instructor> Instructors { get; set; }
    }
}
