using JWT_Demo.Data;
using Models.Entity;

namespace JWT_Demo.Services
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<OperatorDbContext>();

            services.AddAuthentication();

            return services;
        }
    }
}
