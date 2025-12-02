using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Learnix.ViewModels.AdminVMs
{
    public class AdminCourseVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000.")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Instructor name is required.")]
        public string InstructorName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Level is required.")]
        public string Level { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Students count cannot be negative.")]
        public int StudentsCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Lessons count cannot be negative.")]
        public int LessonsCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Duration cannot be negative.")]
        public int Duration { get; set; }
    }
}