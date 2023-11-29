using JWT_Demo.HelperMethods;
using JWT_Demo.Models.Helper;
using MediatR;
using Models.DTOs;

namespace JWT_Demo.Application.Connection
{
    public class RetrieveServers
    {
        public class Query : IRequest<API_Response>
        {

        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return API_Response.Success(Statics.DefaultServer());
            }
        }
    }
}
