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
            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                object tableNames;

                string retrieveTableQuery = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " +
                    $"WHERE TABLE_NAME != '__EFMigrationsHistory'";

                try
                {
                    await using (var connection = new SqlConnection(
                        Environment.GetEnvironmentVariable(Statics.QueryDbConnectionName)))
                    {
                        tableNames = await connection.QueryAsync<string>(retrieveTableQuery);
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
