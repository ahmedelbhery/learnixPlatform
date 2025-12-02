using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AdminVMs
{
    public class InstructorReviewVM
    {
        public string Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Major { get; set; }

        public string Status { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }
    }
}
