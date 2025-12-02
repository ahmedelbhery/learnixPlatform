using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AccountVMs
{
    public class InstructorProfileVM
    {
        public string ID { get; set; }




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
        public string Email { get; set; }




        public string? RoleName { get; set; }




        //[RegularExpression(@"^https?:\/\/.*\.(jpg|jpeg|png|gif|bmp|webp)$",
        // ErrorMessage = "Please enter a valid image URL ending with an image extension.")]
        public string? ImageUrl { get; set; }






        [RegularExpression(@"^[\w\s.,'\""-]{0,300}$",
            ErrorMessage = "Bio can only contain letters, numbers, punctuation, and spaces (max 300 characters).")]
        public string? Bio { get; set; }





        [RegularExpression(@"^(Male|Female)$",
            ErrorMessage = "Gender must be Male or Female")]
        public string? Gender { get; set; }





        [RegularExpression(@"^[A-Za-z0-9\s,.-]{3,100}$",
            ErrorMessage = "Address can only contain letters, numbers, commas, and periods (3–100 chars).")]
        public string? Address { get; set; }





        [RegularExpression(@"^https?:\/\/(www\.)?linkedin\.com\/.*$",
            ErrorMessage = "Please enter a valid LinkedIn profile URL.")]
        public string? Linkedin { get; set; }





        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateOnly? DOB { get; set; }



        [RegularExpression(@"^\+?\d{0,3}?[- .]?\(?\d{1,4}?\)?[- .]?\d{3,4}[- .]?\d{3,4}$",
            ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }



        [StringLength(30, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s\-]{1,29}$", ErrorMessage = "Use only letters, spaces, or hyphens.")]
        public string? Major {  get; set; }
    }
}
