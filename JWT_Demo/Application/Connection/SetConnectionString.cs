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

        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            private readonly IConfiguration _configuration;
            public Handler(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Environment.SetEnvironmentVariable(Statics.QueryDbConnectionName, 
                    $"Server={Statics.DefaultServer()};Database=Querinator;" +
                    $"Trusted_Connection=True;TrustServerCertificate=True");

                return API_Response.Success(null);
            }
        }
    }
}
