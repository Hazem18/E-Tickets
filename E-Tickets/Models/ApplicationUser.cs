using Microsoft.AspNetCore.Identity;

namespace E_Tickets.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? City { get; set; }
    }
}
