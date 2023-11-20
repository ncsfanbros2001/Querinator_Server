using JWT_Demo.Data;
using JWT_Demo.Models.Entity;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWT_Demo.Application
{
    public class DeleteSavedQuery
    {
        public class Command : IRequest<API_Response>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, API_Response>
        {
            private readonly OperatorDbContext _db;

            public Handler(OperatorDbContext db)
            {
                _db = db;
            }

            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                QueryToSave queryToDelete = await _db.SavedQuery.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (queryToDelete == null)
                {
                    return API_Response.Failure("This query doesn't exist", HttpStatusCode.NotFound);
                }

                _db.SavedQuery.Remove(queryToDelete);
                await _db.SaveChangesAsync();

                return API_Response.Success(null);
            }
        }
    }
}
