using Microsoft.AspNetCore.Mvc;
using JWT_Demo.Models.DTOs;
using JWT_Demo.Application;
using Microsoft.AspNetCore.Authorization;

namespace JWT_Demo.Controllers.DerivedController
{
    public class QueryController : BaseController
    {
        [HttpGet("executeQuery/{queryString}")]
        public async Task<IActionResult> ExecuteQuery(string queryString)
        {
            return HandleResult(await Mediator.Send(new ExecuteQuery.Query { query = queryString }));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSavedQueries()
        {
            return HandleResult(await Mediator.Send(new GetAllSavedQueries.Query {}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSavedQuery(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetOneSavedQuery.Query { Id = id }));
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
