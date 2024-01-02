using Application.HelperMethods;
using AutoMapper;
using Data;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entity;
using Models.Helper;
using System.Net;

namespace Application.Connection
{
    public class SetConnectionString
    {
        public class Command : IRequest<API_Response>
        {
            public SetConnectionDTO setConnectionDTO { get; set; }
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
                string conn = Statics.SqlServerCS(
                        request.setConnectionDTO.serverName,
                        request.setConnectionDTO.databaseName,
                        request.setConnectionDTO.username!,
                        request.setConnectionDTO.password!);

                using SqlConnection connection = new(conn);

                try
                {
                    connection.Open();

                    PersonalConnection personalConnectionFromDb = await _db.PersonalConnections
                        .FirstOrDefaultAsync(x => x.belongsTo.ToLower() == request.setConnectionDTO.belongsTo.ToLower());

                    if (personalConnectionFromDb == null)
                    {
                        return API_Response.Failure("Error retrieving connection", HttpStatusCode.BadRequest);
                    }

                    PersonalConnection infoToUpdate = new()
                    {
                        Id = personalConnectionFromDb.Id,
                        serverName = request.setConnectionDTO.serverName,
                        databaseName = request.setConnectionDTO.databaseName,
                        username = request.setConnectionDTO.username!,
                        password = Statics.Encrypt(request.setConnectionDTO.password!),
                        belongsTo = request.setConnectionDTO.belongsTo
                    };

                    _mapper.Map(infoToUpdate, personalConnectionFromDb);

                    var result = await _db.SaveChangesAsync();

                    if (result > 0)
                    {
                        PersonalConnection personalConnection = new()
                        {
                            serverName = request.setConnectionDTO.serverName,
                            databaseName = request.setConnectionDTO.databaseName
                        };

                        return API_Response.Success(personalConnection);
                    }
                    else
                    {
                        return API_Response.Failure("Please change something to update", HttpStatusCode.BadRequest);
                    }
                }
                catch (Exception ex)
                {
                    return API_Response.Failure(ex.Message, HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
