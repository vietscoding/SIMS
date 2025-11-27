using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Program")]
    public class AcademicProgram
    {
        [Key]
        [Column("program_id")]
        public int AcademicProgramId { get; set; }

        [Column("program_name")]
        public string? AcademicProgramName { get; set; }

        [Column("major_id")]
        public int? MajorId { get; set; }

        [Column("faculty_id")]
        public int? FacultyId { get; set; }

        [Column("program_language")]
        public string? Language { get; set; }

        [Column("program_description")]
        public string? Description { get; set; }

        [Column("number_of_semester")]
        public byte? NumberOfSemester { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("total_of_required_credits")]
        public decimal? TotalOfRequiredCredits { get; set; }

        [Column("obligated_credits")]
        public decimal? ObligatedCredits { get; set; }

        [Column("elective_credits")]
        public decimal? ElectiveCredits { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        public AcademicProgram() { }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty? Faculty { get; set; } = null!;

        [ForeignKey(nameof(MajorId))]
        public virtual Major? Major { get; set; } = null!;
    }
}
