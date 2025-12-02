using Learnix.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.EnrollementDtos
{
    public class EnrollementDto
    {
        public int Id { get; set; }
        public DateTime EnrollementDate { get; set; }



       
        public string StudentId { get; set; }
        public Student Student { get; set; }


        
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
