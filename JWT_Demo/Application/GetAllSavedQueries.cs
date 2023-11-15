using JWT_Demo.Data;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace JWT_Demo.Application
{
    public class GetAllSavedQueries
    {
        public class Query : IRequest<API_Response>
        {

        }

        public class Handler : IRequestHandler<Query, API_Response>
        {
            private readonly OperatorDbContext _db;

            public Handler(OperatorDbContext db)
            {
                _db = db;
            }

            public async Task<API_Response> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    // await HelperMethods.TimeLimiterForGet(_db.SavedQuery.ToListAsync());

                    return API_Response.Success(await _db.SavedQuery.ToListAsync());
                }
                catch
                {
                    // Handle the timeout error accordingly
                    return API_Response.Failure("Response time exceeded 5 seconds", HttpStatusCode.RequestTimeout);
                }
            }
        }
    }
}
