using Learnix.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Dtos.AnnouncementDtos
{
    public class AnnoucementDto
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime PostedAt { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
