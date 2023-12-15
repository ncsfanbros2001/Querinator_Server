using Application.HelperMethods;
using Microsoft.AspNetCore.Identity;

namespace JWT_Demo.SeedDatas
{
    public class SeedRole
    {
        public static async Task Seed(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(Statics.AdminRole).GetAwaiter().GetResult())
            {
                await roleManager.CreateAsync(new IdentityRole(Statics.AdminRole));
                await roleManager.CreateAsync(new IdentityRole(Statics.UserRole));
            }
        }
    }
}
