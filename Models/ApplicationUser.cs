using Microsoft.AspNetCore.Identity;

namespace SIMS.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int PersonId { get; set; }  // FK đến Person
        public int RoleId { get; set; }    // FK đến Role (custom)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Liên kết navigation nếu cần
        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
    }
}
