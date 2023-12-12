using Application.HelperMethods;
using MediatR;
using Models.Helper;

namespace Application.Connection
{
    public class RetrieveDatabases
    {
        public class Query : IRequest<API_Response>
        {
            
        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return API_Response.Success(Statics.DefaultDatabases());
            }
        }
    }
}
