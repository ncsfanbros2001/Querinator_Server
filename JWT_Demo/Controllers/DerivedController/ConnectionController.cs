using Application.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entity;

namespace JWT_Demo.Controllers.DerivedController
{
    public class ConnectionController : BaseController
    {
        [HttpPost("setConnection")]
        [Authorize]
        public async Task<IActionResult> SetDbConnection([FromBody]PersonalConnection personalConnection)
        {
            return HandleResult(await Mediator.Send(new SetConnectionString.Query { personalConnection = personalConnection }));
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

        [HttpGet("serverAndDb")]
        [Authorize]
        public async Task<IActionResult> CurrentServerAndDb()
        {
            return HandleResult(await Mediator.Send(new ExtractServerAndDbName.Query { }));
        }
    }
}
