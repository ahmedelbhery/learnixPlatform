using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AccountVMs
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can contain only letters, numbers, dots, underscores, or hyphens.")]
        public string UserName { get; set; }




        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
           ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }



        public bool RememberMe { get; set; }


    }
}
