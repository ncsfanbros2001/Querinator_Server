using Application.HelperMethods;
using Dapper;
using Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entity;
using Models.Helper;
using System.Net;

namespace JWT_Demo.Application.Query
{
    public class SaveQuery
    {
        public class Command : IRequest<API_Response>
        {
            public SaveQueryDTO queryDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, API_Response>
        {
            private readonly OperatorDbContext _db;
            private readonly UserManager<AppUser> _userManager;

            public Handler(OperatorDbContext db, UserManager<AppUser> userManager)
            {
                _db = db;
                _userManager = userManager;
            }

            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUser = await _userManager.FindByIdAsync(request.queryDTO.UserId);
                var currentUserRole = await _userManager.IsInRoleAsync(currentUser!, Statics.AdminRole);

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
                    return API_Response.Failure("This query has already been saved", HttpStatusCode.BadRequest);
                }

                PersonalConnection personalConnection = await _db.PersonalConnections.FirstOrDefaultAsync(
                    x => x.belongsTo == request.queryDTO.UserId);

                try
                {
                    await using (var connection = new SqlConnection(Statics.SqlServerCS(personalConnection.serverName, 
                        personalConnection.databaseName, personalConnection.username,
                        Statics.Decrypt(personalConnection.password))))
                    {
                        await connection.QueryAsync(request.queryDTO.Query);
                    }
                }
                catch (Exception ex)
                {
                    return API_Response.Failure(ex.Message, HttpStatusCode.BadRequest);
                }

                QueryToSave queryToSave = new()
                {
                    Id = new Guid(),
                    Title = request.queryDTO.Title,
                    Query = request.queryDTO.Query,
                    Server = personalConnection.serverName,
                    Database = personalConnection.databaseName,
                    UserId = request.queryDTO.UserId
                };

                await _db.SavedQuery.AddAsync(queryToSave);
                await _db.SaveChangesAsync();

                return API_Response.Success(null);
            }
        }
    }
}
