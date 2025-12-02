using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AccountVMs
{
    public class UpdateAvatarVM
    {

        public string? ImageUrl { get; set; }


        [Required(ErrorMessage = "You have to upload a Photo")]
        public IFormFile Avatar { get; set; }
    }
}
