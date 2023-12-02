using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.Data.SqlClient;
using Models.DTOs;
using System.Net;

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
                string conn = Statics.ConnectionString(request.connectionDTO.serverName, request.connectionDTO.databaseName,
                    request.connectionDTO.username, request.connectionDTO.password);

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    try
                    {
                        connection.Open();
                        Environment.SetEnvironmentVariable(Statics.QueryDbConnectionName, conn);

                        return API_Response.Success(null);
                    }
                    catch (Exception ex)
                    {
                        return API_Response.Failure(ex.Message, HttpStatusCode.BadRequest);
                    }
                }
            }
        }
    }
}
