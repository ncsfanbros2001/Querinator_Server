using JWT_Demo.Data;
using Microsoft.AspNetCore.Identity;
using Models.Entity;

namespace Data
{
    public class SeedData
    {
        public static async Task Seed(OperatorDbContext db, UserManager<AppUser> userManager)
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
                }
            }
        }
    }
}
