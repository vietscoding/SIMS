using Microsoft.AspNetCore.Identity;

namespace SIMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Optional: link to your Person entity if you need additional profile data
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
    }
}