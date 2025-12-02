using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.LessonVMs
{
    public class LessonEditVM
    {
        public int LessonId { get; set; }
        public int SectionId { get; set; }
        public int CourseId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s\-\.,]{3,100}$", ErrorMessage = "Title must contain only letters, numbers, spaces, and punctuation.")]
        public string Title { get; set; }


        [Required]
        [RegularExpression(@"^.{10,1000}$", ErrorMessage = "Description must be at least 10 characters.")]
        public string Description { get; set; }


        [Required]
        [RegularExpression(@"^.{5,500}$", ErrorMessage = "Objective must be at least 5 characters.")]
        public string LearningObjectives { get; set; }


        public string? VideoUrl { get; set; }

        [Required]
        [RegularExpression("^(?:[1-9]\\d*)$", ErrorMessage = "Duration must be a positive number in minutes.")]
        public string? Duration { get; set; }


        public IFormFile? VideoFile { get; set; }
    }
}


