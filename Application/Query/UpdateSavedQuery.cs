using Application.HelperMethods;
using AutoMapper;
using Dapper;
using Data;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entity;
using Models.Helper;
using System.Net;

namespace JWT_Demo.Application.Query
{
    public class UpdateSavedQuery
    {
        public class Command : IRequest<API_Response>
        {
            public string UserId { get; set; }
            public UpdateQueryDTO updateQueryDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, API_Response>
        {
            private readonly OperatorDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OperatorDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;

            }
            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                QueryToSave queryToUpdate = await _db.SavedQuery.FirstOrDefaultAsync(x => x.Id.ToString().ToLower() 
                    == request.updateQueryDTO.QueryId.ToLower())!;

                if (queryToUpdate == null)
                {
                    return API_Response.Failure("This query doesn't exist", HttpStatusCode.NotFound);
                }

                QueryToSave queryFromDb = await _db.SavedQuery.FirstOrDefaultAsync(
                    x => x.Query.ToLower() == request.updateQueryDTO.Query.ToLower() &&
                    x.UserId.ToLower() == request.UserId.ToString().ToLower() &&
                    x.Title.ToLower() == request.updateQueryDTO.Title.ToLower())!;

                if (queryFromDb != null)
                {
                    return API_Response.Failure("This query has been saved before", HttpStatusCode.BadRequest);
                }

                PersonalConnection personalConnection = await _db.PersonalConnections.FirstOrDefaultAsync(
                    x => x.belongsTo.ToLower() == request.UserId.ToString().ToLower());

                try
                {
                    string connectionString = Statics.SqlServerCS(personalConnection.serverName,
                        personalConnection.databaseName, personalConnection.username,
                        Statics.Decrypt(personalConnection.password));

                    await using var connection = new SqlConnection(connectionString);

                    await connection.QueryAsync(request.updateQueryDTO.Query);
                }
                catch (Exception ex)
                {
                    return API_Response.Failure(ex.Message, HttpStatusCode.BadRequest);
                }

                QueryToSave infoToUpdate = new()
                {
                    Id = queryToUpdate.Id,
                    Title = request.updateQueryDTO.Title,
                    Query = request.updateQueryDTO.Query,
                    Server = personalConnection.serverName,
                    Database = personalConnection.databaseName,
                    UserId = queryToUpdate.UserId
                };

                _mapper.Map(infoToUpdate, queryToUpdate);

                var result = await _db.SaveChangesAsync();

                if (result > 0)
                {
                    return API_Response.Success(null);
                }
                else
                {
                    return API_Response.Failure("Please change something to update", HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
