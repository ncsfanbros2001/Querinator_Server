using Application.HelperMethods;
using MediatR;
using Microsoft.Data.SqlClient;
using Models.DTOs;
using Models.Helper;

namespace Application.Connection
{
    public class ExtractServerAndDbName
    {
        public class Query : IRequest<API_Response>
        {

        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var connectionString = new SqlConnectionStringBuilder(
                    Environment.GetEnvironmentVariable(Statics.QueryDbConnectionName));

                SetConnectionDTO connectionInfo = new()
                {
                    serverName = connectionString.DataSource,
                    databaseName = connectionString.InitialCatalog,
                };

                return API_Response.Success(connectionInfo);
            }
        }
    }
}
