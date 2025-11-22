using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Faculty")]
    public class Faculty
    {
        [Key]
        [Column("faculty_id")]
        public int FacultyId { get; set; }

        [Column("faculty_name")]
        public string FacultyName { get; set; } = string.Empty;

        [Column("ten_khoa")]
        public string TenKhoa { get; set; } = string.Empty;

        public Faculty() { }
    }
}
