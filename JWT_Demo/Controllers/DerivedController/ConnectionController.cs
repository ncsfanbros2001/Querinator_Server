using JWT_Demo.Application.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Demo.Controllers.DerivedController
{
    public class ConnectionController : BaseController
    {
        [HttpGet("{serverName}/{databaseName}")]
        [AllowAnonymous]
        public async Task<IActionResult> SetDbConnection(string serverName, string databaseName)
        {
            return HandleResult(await Mediator.Send(new SetConnectionString.Query 
            { serverName = serverName, databaseName = databaseName }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RetrieveDbConnection()
        {
            return HandleResult(await Mediator.Send(new RetrieveConnectionString.Query { }));
        }
    }
}
