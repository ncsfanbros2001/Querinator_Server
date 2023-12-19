using Data;
using Models.Entity;
using Models.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWT_Demo.Application.Query
{
    public class GetOneSavedQuery
    {
        public class Query : IRequest<API_Response>
        {
            public Guid Id { get; set; }
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
                QueryToSave query = await _db.SavedQuery.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

                if (query == null)
                {
                    return API_Response.Failure("This query is not available", HttpStatusCode.NotFound);
                }
                else
                {
                    return API_Response.Success(query);
                }
            }
        }
    }
}
