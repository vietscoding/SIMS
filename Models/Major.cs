using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Major")]
    public class Major
    {
        [Key]
        [Column("major_id")]
        public int MajorId { get; set; }

        [Column("major_name")]
        public string MajorName { get; set; } = string.Empty;

        [Column("alternative_major_name")]
        public string AlternativeMajorName { get; set; } = string.Empty;

        [Column("major_code")]
        public string MajorCode { get; set; } = string.Empty;

        [Column("ten_nganh")]
        public string TenNganh { get; set; } = string.Empty;

        [Column("faculty_id")]
        public int FacultyId { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public Major() { }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty? Faculty { get; set; } = null!;
    }
}
