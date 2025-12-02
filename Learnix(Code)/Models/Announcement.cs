using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnix.Models
{
    public class Announcement
    {
       [Key]
       public int Id { get; set; }
       public string Title { get; set; }
       public string Message { get; set; }
       public DateTime PostedAt { get; set; }


        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public Course Course { get; set; }

    }
}
