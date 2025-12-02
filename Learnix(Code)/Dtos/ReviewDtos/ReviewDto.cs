using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.ReviewDtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }



       
        public string StudentID { get; set; }
        public Student Student { get; set; }



        
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
