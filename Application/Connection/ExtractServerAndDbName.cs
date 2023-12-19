using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entity;
using Models.Helper;
using System.Net;

namespace Application.Connection
{
    public class ExtractServerAndDbName
    {
        public class Query : IRequest<API_Response>
        {
            public string userId { get; set; }    
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
                PersonalConnection personalConnectionFromDb = await _db.PersonalConnections.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.belongsTo == request.userId);

                if (personalConnectionFromDb == null)
                {
                    return API_Response.Failure("Can't find your connection string", HttpStatusCode.BadRequest);
                }

                return API_Response.Success(personalConnectionFromDb);
            }
        }
    }
}
