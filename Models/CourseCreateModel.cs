namespace SIMS.Models
{
    // DTO used for course creation binding
    public class CourseCreateModel
    {
        public string? CourseName { get; set; }
        public string? TenHocPhan { get; set; }
        public string? CourseCode { get; set; }
        public int? FacultyId { get; set; }
        public decimal? LectureCredits { get; set; }
        public decimal? PracticalCredits { get; set; }
        public decimal? InternshipCredits { get; set; }
        public decimal? CapstoneCredits { get; set; }
        public string? CourseSummary { get; set; }
    }
}
