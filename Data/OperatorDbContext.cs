using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entity;

namespace Data
{
    public class OperatorDbContext : IdentityDbContext<AppUser>
    {
        public OperatorDbContext(DbContextOptions<OperatorDbContext> options) : base(options)
        {

        }

        public DbSet<QueryToSave> SavedQuery { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<PersonalConnection> PersonalConnections { get; set; }
    }
}
