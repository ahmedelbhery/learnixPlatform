using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AdminVMs
{
    public class AdminUserVM
    {

        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can contain only letters, numbers, dots, underscores, or hyphens.")]
        public string UserName { get; set; }





        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string FirstName { get; set; }





        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string LastName { get; set; }




        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Remote(action: "VerifyEmail", controller: "Account", AdditionalFields = "ID",
        ErrorMessage = "Email is already in use.")]
        public string Email { get; set; }






        [Required(ErrorMessage = "Please select a role.")]
        [RegularExpression(@"^(Admin|Instructor|Student)$", ErrorMessage = "Invalid role selected.")]
        public string RoleName { get; set; }





        public string? ImageUrl { get; set; }
    }
}
