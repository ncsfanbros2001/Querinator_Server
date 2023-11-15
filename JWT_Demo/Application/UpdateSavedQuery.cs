using AutoMapper;
using JWT_Demo.Data;
using JWT_Demo.Models.DTOs;
using JWT_Demo.Models.Entity;
using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JWT_Demo.Application
{
    public class UpdateSavedQuery
    {
        public class Command : IRequest<API_Response>
        {
            public Guid Id { get; set; }
            // public SaveQueryDTO saveQueryDTO { get; set; }

            public SaveQueryDTO saveQueryDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, API_Response>
        {
            private readonly OperatorDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OperatorDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }
            public async Task<API_Response> Handle(Command request, CancellationToken cancellationToken)
            {
                QueryToSave? queryToUpdate = await _db.SavedQuery.FirstOrDefaultAsync(x => x.Id == request.Id)!;

                if (queryToUpdate == null)
                {
                    return API_Response.Failure("This query doesn't exist", HttpStatusCode.NotFound);
                }

                QueryToSave infoToUpdate = new()
                {
                    Id = request.Id,
                    Title = request.saveQueryDTO.Title,
                    Query = request.saveQueryDTO.Query
                };

                _mapper.Map(infoToUpdate, queryToUpdate);

                var result = await _db.SaveChangesAsync();

                if (result > 0)
                {
                    return API_Response.Success(null);
                }
                else
                {
                    return API_Response.Failure("Save changes failed", HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
