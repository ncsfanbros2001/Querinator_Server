using AutoMapper;
using Dapper;
using JWT_Demo.Data;
using JWT_Demo.Models.DTOs;
using JWT_Demo.Models.Entity;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWT_Demo.Application
{
    public class UpdateSavedQuery
    {
        public class Command : IRequest<API_Response>
        {
            public Guid Id { get; set; }
            public SaveQueryDTO saveQueryDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, API_Response>
        {
            private readonly OperatorDbContext _db;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;

            public Handler(OperatorDbContext db, IMapper mapper, IConfiguration configuration)
            {
                _db = db;
                _mapper = mapper;
                _configuration = configuration;

            }
            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    QueryToSave? queryToUpdate = await _db.SavedQuery.FirstOrDefaultAsync(x => x.Id == request.Id)!;

                    if (queryToUpdate == null)
                    {
                        return API_Response.Failure("This query doesn't exist", HttpStatusCode.NotFound);
                    }

                    QueryToSave? queryFromDb = await _db.SavedQuery.FirstOrDefaultAsync(x => x.Query.ToLower() ==
                        request.saveQueryDTO.Query.ToLower() && x.Query.ToLower() != queryToUpdate.Query.ToLower())!;

                    if (queryFromDb != null)
                    {
                        return API_Response.Failure("This query has been saved before", HttpStatusCode.BadRequest);
                    }

                    await using (var connection = new SqlConnection(
                            _configuration.GetConnectionString("DB_To_Query_Connection")))
                    {
                        await connection.ExecuteAsync(request.saveQueryDTO.Query);
                    }

                    QueryToSave infoToUpdate = new()
                    {
                        Id = request.Id,
                        Title = request.saveQueryDTO.Title,
                        Query = request.saveQueryDTO.Query
                    };

                    _mapper.Map(infoToUpdate, queryToUpdate);

                    var result = await _db.SaveChangesAsync();

                    if (result > 0)
                    {
                        return API_Response.Success(null);
                    }
                    else
                    {
                        return API_Response.Failure("Save changes failed", HttpStatusCode.BadRequest);
                    }
                }
                catch
                {
                    return API_Response.Failure("This query is invalid", HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
