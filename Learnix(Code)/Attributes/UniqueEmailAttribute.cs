using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;


namespace Learnix.Attributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var userManager = validationContext.GetService<UserManager<IdentityUser>>();
            var email = value as string;
            if (string.IsNullOrEmpty(email))
                return ValidationResult.Success;

            var existingUser = userManager.FindByEmailAsync(email).Result;
            if (existingUser != null)
            {
                // check if this is the same user (update case)
                var currentUser = validationContext.ObjectInstance as IdentityUser;
                if (currentUser != null && existingUser.Id == currentUser.Id)
                    return ValidationResult.Success;

                return new ValidationResult("Email is already taken.");
            }

            return ValidationResult.Success;
        }
    }
}
