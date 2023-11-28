using Dapper;
using JWT_Demo.Data;
using JWT_Demo.HelperMethods;
using JWT_Demo.Models.DTOs;
using JWT_Demo.Models.Entity;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Entity;
using System.Net;

namespace JWT_Demo.Application
{
    public class SaveQuery
    {
        public class Command : IRequest<API_Response>
        {
            public SaveQueryDTO queryDTO {  get; set; } 
        }

        public class Handler : IRequestHandler<Command, API_Response>
        {
            private readonly OperatorDbContext _db;
            private readonly IConfiguration _configuration;
            private readonly UserManager<AppUser> _userManager;

            public Handler(OperatorDbContext db, IConfiguration configuration, UserManager<AppUser> userManager)
            {
                _db = db;
                _configuration = configuration;
                _userManager = userManager;
            }

            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUser = await _userManager.FindByIdAsync(request.queryDTO.UserId);
                var currentUserRole = await _userManager.IsInRoleAsync(currentUser, Statics.AdminRole);

                if (currentUserRole == false && request.queryDTO.Query.Contains("select") == false)
                {
                    return API_Response.Failure("You can't save any query other than SELECT",
                        HttpStatusCode.BadRequest);
                }

                if (request.queryDTO.Title == "")
                {
                    return API_Response.Failure("You must enter a title to save this query",
                        HttpStatusCode.BadRequest);
                }

                QueryToSave queryFromDb = await _db.SavedQuery.FirstOrDefaultAsync(
                    x => x.Query.ToLower() == request.queryDTO.Query.ToLower() &&
                    x.UserId.ToLower() == request.queryDTO.UserId.ToLower())!;

                if (queryFromDb != null)
                {
                    return API_Response.Failure("This query has been saved before", HttpStatusCode.BadRequest);
                }

                try
                {
                    await using (var connection = new SqlConnection(
                        Environment.GetEnvironmentVariable(Statics.QueryDbConnectionName)))
                    {
                        await connection.QueryAsync(request.queryDTO.Query);
                    }
                }
                catch
                {
                    return API_Response.Failure("This query is invalid", HttpStatusCode.BadRequest);
                }

                QueryToSave queryToSave = new()
                {
                    Id = new Guid(),
                    Title = request.queryDTO.Title,
                    Query = request.queryDTO.Query,
                    UserId = request.queryDTO.UserId,
                };

                await _db.SavedQuery.AddAsync(queryToSave);
                await _db.SaveChangesAsync();

                return API_Response.Success(null);
            }
        }
    }
}
