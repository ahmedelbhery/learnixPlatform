using Microsoft.AspNetCore.Identity;

namespace Learnix.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Linkedin { get; set; }
        public DateOnly? DOB { get; set; }
    }
}
