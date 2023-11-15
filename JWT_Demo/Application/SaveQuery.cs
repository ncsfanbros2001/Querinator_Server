using Dapper;
using JWT_Demo.Data;
using JWT_Demo.Models.DTOs;
using JWT_Demo.Models.Entity;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Data.SqlClient;
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

            public Handler(OperatorDbContext db, IConfiguration configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request.queryDTO.Title == "")
                    {
                        return API_Response.Failure("You must enter a title to save this query", HttpStatusCode.BadRequest);
                    }

                    QueryToSave queryFromDb = _db.SavedQuery.FirstOrDefault(x => x.Query.ToLower() == 
                        request.queryDTO.Query.ToLower())!;

                    if (queryFromDb != null)
                    {
                        return API_Response.Failure("This query has been saved before", HttpStatusCode.BadRequest);
                    }

                    await using (var connection = new SqlConnection(
                        _configuration.GetConnectionString("DB_To_Query_Connection")))
                    {
                        await connection.ExecuteAsync(request.queryDTO.Query);
                    }

                    QueryToSave queryToSave = new()
                    {
                        Id = new Guid(),
                        Title = request.queryDTO.Title,
                        Query = request.queryDTO.Query
                    };

                    // await HelperMethods.TimeLimiterForUpdate(_db.SavedQuery.Add(queryToSave));
                    await _db.SavedQuery.AddAsync(queryToSave);
                    await _db.SaveChangesAsync();

                    return API_Response.Success(null);
                }
                catch
                {
                    return API_Response.Failure("This query is invalid", HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
