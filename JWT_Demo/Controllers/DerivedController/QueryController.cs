using Microsoft.AspNetCore.Mvc;
using JWT_Demo.Models.DTOs;
using JWT_Demo.Application;
using Microsoft.AspNetCore.Authorization;
using JWT_Demo.Models.Entity;

namespace JWT_Demo.Controllers.DerivedController
{
    public class QueryController : BaseController
    {
        [HttpGet("executeQuery/{queryString}")]
        [Authorize]
        public async Task<IActionResult> ExecuteQuery(string queryString)
        {
            return HandleResult(await Mediator.Send(new ExecuteQuery.Query { query = queryString }));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllSavedQueries()
        {
            return HandleResult(await Mediator.Send(new GetAllSavedQueries.Query {}));
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
        public async Task<ActionResult> SaveQuery([FromBody] SaveQueryDTO saveQueryDTO)
        {
            return HandleResult(await Mediator.Send(new SaveQuery.Command { queryDTO = saveQueryDTO }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSavedQuery(Guid id, [FromBody]SaveQueryDTO query)
        {
            return HandleResult(await Mediator.Send(new UpdateSavedQuery.Command { Id = id, saveQueryDTO = query }));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSavedQuery(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteSavedQuery.Command { Id = id }));
        }
    }
}
