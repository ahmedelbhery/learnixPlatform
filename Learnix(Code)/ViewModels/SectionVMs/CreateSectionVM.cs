using Learnix.Dtos.CourseDtos;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.SectionVMs
{
    public class CreateSectionVM
    {
        public string? InstructorFirstName { get; set; }
        public string? InstructorLasttName { get; set; }
        public string? InstructorImageUrl { get; set; }


        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s\-\.,]{3,100}$", ErrorMessage = "Title must contain only letters, numbers, spaces, and punctuation.")]
        public string SectionName { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Section order must be greater than 0.")]
        public int SectionOrder { get; set; }


        [RegularExpression(@"^\d+$", ErrorMessage = "Select a Course")]
        public int CourseID { get; set; }
        public IEnumerable<CourseDto> Courses { get; set; } = Enumerable.Empty<CourseDto>();


        
    }
}
