using Microsoft.AspNetCore.Identity;

namespace Models.Entity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public bool IsLocked { get; set; } = false;
    }
}
