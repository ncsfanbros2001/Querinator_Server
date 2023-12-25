using Application.HelperMethods;
using Data;
using Microsoft.AspNetCore.Identity;
using Models.Entity;

namespace JWT_Demo.SeedDatas
{
    public class SeedUser
    {
        public static async Task Seed(UserManager<AppUser> userManager, OperatorDbContext db)
        {
            if (!userManager.Users.Any())
            {
                AppUser user = new()
                {
                    DisplayName = "System Admin",
                    UserName = "SYSTEM_ADMIN",
                    Email = "system@admin.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, Statics.AdminRole);

                PersonalConnection personalConnection = new()
                {
                    Id = new Guid(),
                    serverName = null,
                    databaseName = null,
                    username = null,
                    password = null,
                    belongsTo = user.Id
                };

                db.PersonalConnections.Add(personalConnection);
                await db.SaveChangesAsync();
            }
            
        }
    }
}
