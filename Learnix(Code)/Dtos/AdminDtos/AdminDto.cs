using Learnix.Models;

namespace Learnix.Dtos.AdminDtos
{
    public class AdminDto
    {
        public string Id { get; set; }
        public decimal Balance { get; set; } = 0;


        public ApplicationUser User { get; set; }
    }
}
