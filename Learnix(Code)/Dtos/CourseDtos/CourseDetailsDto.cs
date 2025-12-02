

namespace Learnix.Dtos.CourseDtos
{
    public class CourseDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public string CourseStatus { get; set; }
        public int TotalNumberOfStudent { get; set; }
        public double? TotalEarning { get; set; }
    }
}
