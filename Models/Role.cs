using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Role")]
    public class Role
    {
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("role_name")]
        public string? RoleName { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }
    }
}
