using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AdminVMs
{
    public class AdminInstructorVM
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string LastName { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Remote(action: "VerifyEmail", controller: "Account", AdditionalFields = "ID",
        ErrorMessage = "Email is already in use.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can contain only letters, numbers, dots, underscores, or hyphens.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Specialty must be between 2 and 50 characters.")]
        public string Specialty { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Courses count cannot be negative.")]
        public int CoursesCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Students count cannot be negative.")]
        public int StudentsCount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Earnings cannot be negative.")]
        public double Earnings { get; set; }
    }
}