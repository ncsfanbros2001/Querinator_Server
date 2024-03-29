﻿using Application.HelperMethods;
using Application.Query;
using Data;
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
                options.UseSqlServer(Statics.SqlServerCS(Statics.OperatorServerName, Statics.OperatorDbName, "sa", "12345"));
            });

            services.AddCors();

            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(ExecuteQuery.Handler).Assembly));

            services.AddAutoMapper(typeof(Mapping).Assembly);

            return services;
        }
    }
}
