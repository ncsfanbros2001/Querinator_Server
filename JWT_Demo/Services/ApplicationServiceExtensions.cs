using JWT_Demo.Application;
using JWT_Demo.Data;
using JWT_Demo.HelperMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.Services
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<OperatorDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DB_To_Save_Connection"));
            });

            services.AddDbContext<QueryDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DB_To_Query_Connection"));
            });

            services.AddCors();

            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(ExecuteQuery.Handler).Assembly));

            services.AddAutoMapper(typeof(Mapping).Assembly);

            return services;
        }
    }
}
