using JWT_Demo.Application.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace JWT_Demo.Controllers.DerivedController
{
    public class ConnectionController : BaseController
    {
        [HttpPost("setConnection")]
        [Authorize]
        public async Task<IActionResult> SetDbConnection([FromBody]SetConnectionDTO connectionDTO)
        {
            return HandleResult(await Mediator.Send(new SetConnectionString.Query { connectionDTO = connectionDTO }));
        }

        [HttpGet("servers")]
        [Authorize]
        public async Task<IActionResult> RetrieveServers()
        {
            return HandleResult(await Mediator.Send(new RetrieveServers.Query { }));
        }

        [HttpGet("databases")]
        [Authorize]
        public async Task<IActionResult> RetrieveDatabases()
        {
            return HandleResult(await Mediator.Send(new RetrieveDatabases.Query { }));
        }
    }
}
