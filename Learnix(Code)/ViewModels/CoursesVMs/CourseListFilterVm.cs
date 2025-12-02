namespace Learnix.ViewModels.CoursesVMs
{
    public class CourseListFilterVm
    {
        public string? Search { get; set; }           // by course title
        public int? CategoryId { get; set; }
        public string? InstructorId { get; set; }
        public int? LevelId { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool OnlyFree { get; set; }
        // Paging
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
