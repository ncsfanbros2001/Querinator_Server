using JWT_Demo.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.Data
{
    public class OperatorDbContext : DbContext
    {
        public OperatorDbContext(DbContextOptions<OperatorDbContext> options) : base (options)
        {
            
        }
        public DbSet<QueryToSave> SavedQuery { get; set; }
    }
}
