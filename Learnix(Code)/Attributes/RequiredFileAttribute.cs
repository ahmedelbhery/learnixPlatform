using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Learnix.Attributes
{
    public class RequiredFileAttribute : ValidationAttribute
    {
        public long MaxFileSizeInBytes { get; set; } = long.MaxValue; // optional max size

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
                return new ValidationResult(ErrorMessage ?? "Please upload a file.");

            if (file.Length == 0)
                return new ValidationResult(ErrorMessage ?? "The file cannot be empty.");

            //if (file.Length > MaxFileSizeInBytes)
            //    return new ValidationResult(ErrorMessage ?? $"File size cannot exceed {MaxFileSizeInBytes / 1024} KB.");

            return ValidationResult.Success;
        }
    }
}


