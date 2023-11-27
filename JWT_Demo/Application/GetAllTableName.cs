using Dapper;
using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWT_Demo.Application
{
    public class GetAllTableName
    {
        public class Query : IRequest<API_Response>
        {

        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            private IConfiguration _configuration;
            public Handler(IConfiguration configuration)
            {
                _configuration = configuration;
            } 

            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                object tableNames;

                string retrieveTableQuery = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " +
                    $"WHERE TABLE_NAME != '__EFMigrationsHistory'";

                try
                {
                    await using (var connection = new SqlConnection(
                        _configuration.GetConnectionString(Statics.QueryDbConnectionName)))
                    {
                        tableNames = connection.Query<string>(retrieveTableQuery);
                    }
                }
                catch (Exception exception)
                {
                    return API_Response.Failure(exception.Message, HttpStatusCode.BadRequest);
                }

                return API_Response.Success(tableNames);
            }
        }
    }
}
