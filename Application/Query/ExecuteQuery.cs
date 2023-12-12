using Application.HelperMethods;
using Dapper;
using Data;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entity;
using Models.Helper;
using System.Net;

namespace Application.Query
{
    public class ExecuteQuery
    {
        public class Query : IRequest<API_Response>
        {
            public QueryHistoryDTO historyDTO { get; set; }
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
                object userList;

                try
                {
                    await using (var connection = new SqlConnection(
                        Environment.GetEnvironmentVariable(Statics.QueryDbConnectionName)))
                    {
                        if (request.historyDTO.role != Statics.AdminRole &&
                            request.historyDTO.query.ToLower().Contains("select") == false)
                        {
                            throw new Exception("You can't execute any other query other than SELECT");
                        }
                        else
                        {

                            userList = connection.Query(request.historyDTO.query);

                            History historyToSave = new()
                            {
                                Id = new Guid(),
                                Query = request.historyDTO.query,
                                ExecutedTime = DateTime.Now,
                                UserId = request.historyDTO.userId
                            };

                            if (_db.Histories.ToList().Count() > 0)
                            {
                                var oldestQuery = await _db.Histories.OrderBy(x => x.ExecutedTime)
                                    .FirstOrDefaultAsync(x => x.UserId == request.historyDTO.userId);

                                if (_db.Histories.Where(x => x.UserId == request.historyDTO.userId).ToList().Count() > 9)
                                {
                                    _db.Histories.Remove(oldestQuery);
                                }
                            }

                            _db.Histories.Add(historyToSave);
                            int result = await _db.SaveChangesAsync();

                            if (result == 0)
                            {
                                throw new Exception();
                            }
                            else
                            {
                                return API_Response.Success(userList);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    return API_Response.Failure(exception.Message, HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
