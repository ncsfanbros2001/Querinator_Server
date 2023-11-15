using Dapper;
using JWT_Demo.Data;
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
                try
                {
                    object userList;

                    await using (var connection = new SqlConnection(
                        _configuration.GetConnectionString("DB_To_Query_Connection")))
                    {
                        userList = connection.Query(request.query);
                    }

                    return API_Response.Success(userList);
                }
                catch (Exception exception)
                {
                    return API_Response.Failure(exception.Message, HttpStatusCode.BadRequest);
                }

                //try
                //{
                //    object userList;

                //    using (var connection = new SqlConnection(
                //        _configuration.GetConnectionString("DB_To_Query_Connection")))
                //    {
                //        // Create a task to execute the query asynchronously
                //        var queryTask = Task.Run(() => connection.Query(request.query), cancellationToken);

                //        // Wait for either the query task or a delay of 5 seconds
                //        var completedTask = await Task.WhenAny(queryTask, Task.Delay(500, cancellationToken));

                //        // Check if the query task completed within 5 seconds
                //        if (completedTask != queryTask)
                //        {
                //            // Throw a custom exception indicating timeout
                //            cancellationToken.ThrowIfCancellationRequested();
                //        }

                //        // Assign the query result to userList
                //        userList = queryTask.Result;
                //    }

                //    return API_Response.Success(userList);
                //}
                //catch (Exception exception)
                //{
                //    return API_Response.Failure(exception.Message, HttpStatusCode.BadRequest);
                //}
            }
        }
    }
}
