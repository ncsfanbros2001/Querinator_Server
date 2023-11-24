using Dapper;
using JWT_Demo.Data;
using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Net;

namespace JWT_Demo.Application
{
    public class ExecuteQuery
    {
        public class Query : IRequest<API_Response>
        {
            public string query { get; set; }
            public string role { get; set; }
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
                object userList;

                try
                {
                    await using (var connection = new SqlConnection(
                        _configuration.GetConnectionString("DB_To_Query_Connection")))
                    {
                        if (request.role != Statics.AdminRole && request.query.ToLower().Contains("select") == false)
                        {
                            throw new Exception("You can't execute any other query other than SELECT");
                        }
                        else
                        {
                            userList = connection.Query(request.query);
                        }
                    }
                }
                catch (Exception exception)
                {
                    return API_Response.Failure(exception.Message, HttpStatusCode.BadRequest);
                }

                return API_Response.Success(userList);
            }
        }
    }
}
