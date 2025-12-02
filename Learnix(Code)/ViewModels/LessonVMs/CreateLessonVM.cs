using Learnix.Attributes;
using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.CourseStatusDtos;
using Learnix.Dtos.LessonStatusDtos;
using Learnix.Dtos.SectionDtos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.LessonVMs
{
    public class CreateLessonVM
    {
        public string? InstructorFirstName { get; set; }
        public string? InstructorLasttName { get; set; }
        public string? InstructorImageUrl { get; set; }



        public string? VideoUrl { get; set; }

        [RequiredFile(ErrorMessage = "Please select a file to upload.", MaxFileSizeInBytes = 50 * 1024 * 1024)] 
        public IFormFile VideoFile { get; set; }




        //public string? AttachementUrl { get; set; }

        
        //[RequiredFile(ErrorMessage = "Please select a video to upload.", MaxFileSizeInBytes = 50 * 1024 * 1024)] 
        //public IFormFile AttachementFile { get; set; }




        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s\-\.,]{3,100}$", ErrorMessage = "Title must contain only letters, numbers, spaces, and punctuation.")]
        public string Title { get; set; }




        [Required]
        [RegularExpression(@"^.{10,1000}$", ErrorMessage = "Description must be at least 10 characters.")]
        public string Description { get; set; }




        [Required]
        [RegularExpression(@"^.{5,500}$", ErrorMessage = "Objective must be at least 5 characters.")]
        public string Objective { get; set; }




        [Required]
        [RegularExpression("^(?:[1-9]\\d*)$", ErrorMessage = "Duration must be a positive number in minutes.")]
        public string Duration { get; set; }




        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Lesson order must be greater than 0.")]
        [Remote("CheckLessonOrder","Lesson",AdditionalFields = "SectionID",ErrorMessage = "This Order is already Exist in this Section")]
        public int LessonOrder { get; set; }




        [RegularExpression(@"^\d+$", ErrorMessage = "Select a Course")]
        public int CourseID { get; set; }
        public IEnumerable<CourseDto> Courses { get; set; } = Enumerable.Empty<CourseDto>();




        [RegularExpression(@"^\d+$", ErrorMessage = "Select a Section")]
        public int SectionID { get; set; }
        public IEnumerable<SectionDto> Sections { get; set; } = Enumerable.Empty<SectionDto>();




        [RegularExpression(@"^\d+$", ErrorMessage = "Select Lesson Status")]
        public int LessonStatusID { get; set; }
        public IEnumerable<LessonStatusDto>? LessonStatuses { get; set; } = Enumerable.Empty<LessonStatusDto>();
    }
}
