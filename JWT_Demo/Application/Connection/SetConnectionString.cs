using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Win32;

namespace JWT_Demo.Application.Connection
{
    public class SetConnectionString
    {
        public class Query : IRequest<API_Response>
        {
            public string serverName { get; set; }
            public string databaseName { get; set; }
        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Environment.SetEnvironmentVariable(Statics.QueryDbConnectionName,
                    $"Server={request.serverName};" +
                    $"Database={request.databaseName};" +
                    $"Trusted_Connection=True;" +
                    $"TrustServerCertificate=True;" +
                    $"User id=;" +
                    $"Password=");

                return API_Response.Success(null);
            }
        }
    }
}
