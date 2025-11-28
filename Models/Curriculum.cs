using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Curriculum")]
    public class Curriculum
    {
        [Key]
        [Column("curriculum_id")]
        public int CurriculumId { get; set; }

        [Column("program_id")]
        public int? ProgramId { get; set; }

        [Column("course_id")]
        public int? CourseId { get; set; }

        [Column("is_elective")]
        public bool? IsElective { get; set; }

        [Column("is_before_capstone_project")]
        public bool? IsBeforeCapstoneProject { get; set; }

        [Column("is_prerequisite_capstone_project")]
        public bool? IsPrerequisiteCapstoneProject { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ProgramId))]
        public virtual AcademicProgram Program { get; set; } = null!;
        
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;

        public Curriculum() { }

    }
}
