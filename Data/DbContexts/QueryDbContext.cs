using JWT_Demo.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.Data
{
    public class QueryDbContext : DbContext
    {
        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base (options)
        {
            
        }

        public DbSet<Users> Users { get; set; }
    }
}
