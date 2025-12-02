using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.ViewModels.CoursesVMs
{
    public class CourseDetailsVM
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningOutCome { get; set; }
        public string Duration { get; set; }
        public string Requirement { get; set; }
        public double? Price { get; set; }
        public bool IsFree { get; set; }
        public string? InstructorName { get; set; }
        public string? CategoryName { get; set; }
        public string? LanguageName { get; set; }
        public string? LevelName { get; set; }
        public string? StatusName { get; set; }

    }
}
