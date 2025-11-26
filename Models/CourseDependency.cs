using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("CourseDependency")]
    public class CourseDependency
    {
        [Key]
        [Column("course_dependency_id")]
        public int CourseDependencyId { get; set; }

        [Column("curriculum_id")]
        public int? CurriculumId { get; set; }

        [Column("previous_course")]
        public int? PreviousCourseId { get; set; }

        [Column("corequisite_course")]
        public int? CorequisiteCourseId { get; set; }

        [Column("prerequisite_course")]
        public int? PrerequisiteCourseId { get; set; }

        [Column("created_at")]
        public DateTime CreateAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdateAt { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }


        [ForeignKey(nameof(CurriculumId))]
        public virtual Curriculum Curriculum { get; set; } = null!;

        [ForeignKey(nameof(PreviousCourseId))]
        public virtual Course PreviousCourse { get; set; } = null!;

        [ForeignKey(nameof(CorequisiteCourseId))]
        public virtual Course CorequisiteCourse { get; set; } = null!;

        [ForeignKey(nameof(PrerequisiteCourseId))]
        public virtual Course PrerequisiteCourse { get; set; } = null!;

        public CourseDependency() { }

    }
}
