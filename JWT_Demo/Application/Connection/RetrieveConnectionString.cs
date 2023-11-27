using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;

namespace JWT_Demo.Application.Connection
{
    public class RetrieveConnectionString
    {
        public class Query : IRequest<API_Response>
        {

        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            public Handler()
            {
                
            }

            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var dbConnection = Environment.GetEnvironmentVariable(Statics.QueryDbConnectionName);
                return API_Response.Success(dbConnection);
            }
        }
    }
}
