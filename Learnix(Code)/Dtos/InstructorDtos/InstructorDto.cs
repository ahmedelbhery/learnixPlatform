using Learnix.Models;

namespace Learnix.Dtos.InstructorDtos
{
    public class InstructorDto
    {
        public string Id { get; set; }
        public string? Major { get; set; }
        public decimal Balance { get; set; } = 0;
        public int? StatusId { get; set; }

        public ApplicationUser User { get; set; }

    }
}
