using Learnix.Dtos.CategoryDtos;
using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.CourseLanguageDtos;
using Learnix.Dtos.CourseLevelDtos;
using Learnix.Dtos.CourseStatusDtos;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.CoursesVMs
{
    public class CreateCourseVM
    {
        public string? InstructorFirstName { get; set; }
        public string? InstructorLasttName { get; set; }
        public string? InstructorImageUrl { get; set; }



        public string? ImageUrl { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }


        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s\-\.,]{3,100}$", ErrorMessage = "Title must contain only letters, numbers, spaces, and punctuation.")]
        public string Title { get; set; }




        [Required]
        [RegularExpression(@"^.{10,1000}$", ErrorMessage = "Description must be at least 10 characters.")]
        public string Description { get; set; }




        [Required]
        [RegularExpression(@"^.{5,500}$", ErrorMessage = "Learning outcome must be at least 5 characters.")]
        public string LearningOutCome { get; set; }




        [Required]
        [RegularExpression(@"^[0-9]+(\.[0-9]+)?$", ErrorMessage = "Duration must be a valid number of hours (e.g., 1, 1.5, 2.75).")]
        public string Duration { get; set; }




        [Required]
        [RegularExpression(@"^.{3,300}$", ErrorMessage = "Requirement must be 3–300 characters.")]
        public string Requirement { get; set; }




        //[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number with up to 2 decimals.")]
        //public double? Price { get; set; }

        [Range(0.01, double.MaxValue,ErrorMessage = "Price must be greater than 0.")]
        public double? Price { get; set; }





        public bool IsFree { get; set; }




        [RegularExpression(@"^\d+$", ErrorMessage = "Select a Category")]
        public int CategoryID { get; set; }




        [RegularExpression(@"^\d+$", ErrorMessage = "Select Course Level")]
        public int CourseLevelID { get; set; }




        [RegularExpression(@"^\d+$", ErrorMessage = "Select Course Language")]
        public int CourseLanguageID { get; set; }




        //[RegularExpression(@"^\d+$", ErrorMessage = "Select Course Status")]
        //public int CourseStatusID { get; set; }




        public IEnumerable<CategoryDto>? Categories { get; set; } = Enumerable.Empty<CategoryDto>();
        public IEnumerable<CourseLevelDto>? CourseLevels { get; set; } = Enumerable.Empty<CourseLevelDto>();
        public IEnumerable<CourseLanguageDto>? CourseLanguages { get; set; } = Enumerable.Empty<CourseLanguageDto>();
       // public IEnumerable<CourseStatusDto>? CourseStatuses { get; set; } = Enumerable.Empty<CourseStatusDto>();
    }
}
