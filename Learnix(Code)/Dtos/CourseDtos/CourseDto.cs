using Learnix.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.CourseDtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningOutCome { get; set; }
        public string Duration { get; set; }
        public string Requirement { get; set; }
        public double? Price { get; set; }
        public bool IsFree { get; set; }



        public string InstructorID { get; set; }
        public Instructor Instructor { get; set; }



        public int CategoryID { get; set; }
        public Category Category { get; set; }


        public int LanguageID { get; set; }
        public CourseLanguage Language { get; set; }


        public int LevelID { get; set; }
        public CourseLevel Level { get; set; }


        public int StatusID { get; set; }
        public CourseStatus Status { get; set; }

    }
}
