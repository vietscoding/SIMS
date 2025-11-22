using SIMS.Models;

namespace SIMS.ViewModels
{
    public class CoursePagingViewModel
    {
        public List<Course> Courses { get; set; } = new List<Course>();

        // Pagination info
        public int Page { get; set; }
        public int TotalPages { get; set; }

        // Optional: useful for UI
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        // For easier check
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

    }
}
