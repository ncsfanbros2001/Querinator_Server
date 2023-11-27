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

            Environment.SetEnvironmentVariable(Statics.OperatorDbConnectionName, $"Server={Statics.DefaultServer()};Database=Querinator;" +
                $"Trusted_Connection=True;TrustServerCertificate=True");

            services.AddDbContext<OperatorDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable(Statics.OperatorDbConnectionName));
            });

            services.AddDbContext<QueryDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Statics.QueryDbConnectionName));
            });

            services.AddCors();

            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(ExecuteQuery.Handler).Assembly));

            services.AddAutoMapper(typeof(Mapping).Assembly);

            return services;
        }
    }
}
