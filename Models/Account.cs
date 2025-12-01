using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [Column("account_id")]
        public int AccountId { get; set; }

        
        [Column("person_id")]
        public int? PersonId { get; set; }

        
        [Column("user_name")]
        public string? UserName { get; set; }

        
        [Column("password")]
        public string? Password { get; set; }

        
        [Column("role_id")]
        public int? RoleId { get; set; }
        
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;


    }
}
