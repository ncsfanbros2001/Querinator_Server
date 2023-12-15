using Application.HelperMethods;
using Microsoft.AspNetCore.Identity;
using Models.Entity;

namespace JWT_Demo.SeedDatas
{
    public class SeedUser
    {
        public static async Task Seed(UserManager<AppUser> userManager)
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

                
            }
            
        }
    }
}
