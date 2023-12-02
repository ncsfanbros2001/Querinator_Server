using JWT_Demo.Data;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JWT_Demo.Application
{
    public class GetQueryHistory
    {
        public class Query : IRequest<API_Response>
        {
            public string UserId { get; set; }
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
                return API_Response.Success(await _db.Histories.Where(x => x.UserId == request.UserId).ToListAsync());
            }
        }
    }
}
