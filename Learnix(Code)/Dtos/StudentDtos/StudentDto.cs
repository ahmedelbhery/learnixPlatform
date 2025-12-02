using Learnix.Models;

namespace Learnix.Dtos.StudentDtos
{
    public class StudentDto
    {
        public string Id { get; set; }
        public decimal Balance { get; set; } = 500;


        public ApplicationUser User { get; set; }
    }
}
