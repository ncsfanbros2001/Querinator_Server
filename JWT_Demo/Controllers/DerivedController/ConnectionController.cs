using Application.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Entity;

namespace JWT_Demo.Controllers.DerivedController
{
    public class ConnectionController : BaseController
    {
        [HttpPost("setConnection")]
        [Authorize]
        public async Task<IActionResult> SetDbConnection([FromBody]SetConnectionDTO setConnectionDTO)
        {
            return HandleResult(await Mediator.Send(new SetConnectionString.Command { setConnectionDTO = setConnectionDTO }));
        }

        [HttpGet("servers")]
        [Authorize]
        public async Task<IActionResult> RetrieveServers()
        {
            return HandleResult(await Mediator.Send(new RetrieveServers.Query { }));
        }

        [HttpGet("databases/{server}")]
        [Authorize]
        public async Task<IActionResult> RetrieveDatabases(string server)
        {
            return HandleResult(await Mediator.Send(new RetrieveDatabases.Query { server = server }));
        }

        [HttpGet("serverAndDb/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CurrentServerAndDb(string userId)
        {
            return HandleResult(await Mediator.Send(new ExtractServerAndDbName.Query { userId = userId }));
        }
    }
}
