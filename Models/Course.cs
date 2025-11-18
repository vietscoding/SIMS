using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        [Column("course_id")]
        public int CourseId { get; set; }

        [Column("course_name")]
        public string? CourseName { get; set; }

        [Column("ten_hoc_phan")]
        public string? TenHocPhan { get; set; }

        [Column("course_code")]
        public string? CourseCode { get; set; }

        [Column("faculty_in_charge")]
        public int? FacultyId { get; set; }

        [Column("total_credits")]
        public decimal? TotalCredits { get; set; }

        [Column("lecture_credits")]
        public decimal? LectureCredits { get; set; }

        [Column("practical_credits")]
        public decimal? PracticalCredits { get; set; }

        [Column("internship_credits")]
        public decimal? InternshipCredits { get; set; }

        [Column("capstone_credits")]
        public decimal? CapstoneCredits { get; set; }

        [Column("course_summary")]
        public string? CourseSummary { get; set; } = string.Empty;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty Faculty { get; set; } = null!;

    }
}
