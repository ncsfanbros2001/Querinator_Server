using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Win32;
using Models.DTOs;

namespace JWT_Demo.Application.Connection
{
    public class SetConnectionString
    {
        public class Query : IRequest<API_Response>
        {
            public SetConnectionDTO connectionDTO {  get; set; }
        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Environment.SetEnvironmentVariable(Statics.QueryDbConnectionName,
                    $"Server={request.connectionDTO.serverName};" +
                    $"Database={request.connectionDTO.databaseName};" +
                    $"Trusted_Connection=True;" +
                    $"TrustServerCertificate=True;" +
                    $"User id={request.connectionDTO.username};" +
                    $"Password={request.connectionDTO.password}");

                return API_Response.Success(null);
            }
        }
    }
}
