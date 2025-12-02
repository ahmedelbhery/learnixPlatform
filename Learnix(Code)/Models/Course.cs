using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningOutCome { get; set; }
        public string Duration { get; set; }
        public string Requirement { get; set; }
        public double? Price { get; set; }
        public bool IsFree { get; set; }



        [ForeignKey("Instructor")]
        public string? InstructorID { get; set; }
        public Instructor? Instructor { get; set; }



        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public Category? Category { get; set; }


        [ForeignKey("Language")]
        public int? LanguageID { get; set; }
        public CourseLanguage? Language { get; set; }


        [ForeignKey("Level")]
        public int? LevelID { get; set; }
        public CourseLevel? Level { get; set; }


        [ForeignKey("Status")]
        public int? StatusID { get; set; }
        public CourseStatus? Status { get; set; }



        public ICollection<Announcement> Announcements { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Section> Sections { get; set; }
        public ICollection<Review> Reviews { get; set; }


    }
}
