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
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Carl Reygard",
                        UserName = "CarlReygard",
                        Email = "carl@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Grace Bishop",
                        UserName = "GraceBishop",
                        Email = "grace@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Eric Stevens",
                        UserName = "EricStevens",
                        Email = "erik@test.com"
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
        }
    }
}
