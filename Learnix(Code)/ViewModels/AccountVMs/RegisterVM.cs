using Learnix.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AccountVMs
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can contain only letters, numbers, dots, underscores, or hyphens.")]
        [Remote(action: "VerifyUserName", controller: "Account", AdditionalFields = "ID",
        ErrorMessage = "Username is already in use.")]
        public string UserName { get; set; }





        [Required(ErrorMessage = "First name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string FirstName { get; set; }





        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string LastName { get; set; }





        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }





        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }






        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Remote(action: "VerifyEmail", controller: "Account", AdditionalFields = "ID",
        ErrorMessage = "Email is already in use.")]
        public string Email { get; set; }






        [Required(ErrorMessage = "Please select a role.")]
        [RegularExpression(@"^(Instructor|Student)$", ErrorMessage = "Invalid role selected.")]
        public string RoleName { get; set; }

    }
}
