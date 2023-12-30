using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Models.DTOs;
using JWT_Demo.Application.Query;
using Application.Query;

namespace JWT_Demo.Controllers.DerivedController
{
    public class QueryController : BaseController
    {
        [HttpPost("executeQuery")]
        [Authorize]
        public async Task<IActionResult> ExecuteQuery([FromBody]HistoryDTO historyDTO)
        {
            return HandleResult(await Mediator.Send(new ExecuteQuery.Query { historyDTO = historyDTO }));
        }

        [HttpGet("history/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetQueryHistory(string userId)
        {
            return HandleResult(await Mediator.Send(new GetQueryHistory.Query { UserId = userId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOneSavedQuery(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetOneSavedQuery.Query { Id = id }));
        }

        [HttpGet("getByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetQueryByUserId(string userId)
        {
            return HandleResult(await Mediator.Send(new GetQueryByUserId.Query { UserId = userId }));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SaveQuery([FromBody]SaveQueryDTO saveQueryDTO)
        {
            return HandleResult(await Mediator.Send(new SaveQuery.Command { queryDTO = saveQueryDTO }));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateSavedQuery(Guid id, [FromBody]UpdateQueryDTO updateQueryDTO)
        {
            return HandleResult(await Mediator.Send(new UpdateSavedQuery.Command { Id = id, updateQueryDTO = updateQueryDTO }));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteSavedQuery(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteSavedQuery.Command { Id = id }));
        }
    }
}
