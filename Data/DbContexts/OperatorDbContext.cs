using JWT_Demo.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entity;

namespace JWT_Demo.Data
{
    public class OperatorDbContext : IdentityDbContext<AppUser>
    {
        public OperatorDbContext(DbContextOptions<OperatorDbContext> options) : base (options)
        {
            
        }

        public DbSet<QueryToSave> SavedQuery { get; set; }
        public DbSet<History> Histories { get; set; }
    }
}
