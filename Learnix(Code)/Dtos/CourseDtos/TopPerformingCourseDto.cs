namespace Learnix.Dtos.CourseDtos
{
    public class TopPerformingCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int TotalNumberOfStudent {  get; set; }
        public double? TotalEarning {  get; set; }
        
    }
}
