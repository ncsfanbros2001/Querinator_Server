using JWT_Demo.Application.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace JWT_Demo.Controllers.DerivedController
{
    public class ConnectionController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SetDbConnection([FromBody]SetConnectionDTO connectionDTO)
        {
            return HandleResult(await Mediator.Send(new SetConnectionString.Query { connectionDTO = connectionDTO }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RetrieveDbConnection()
        {
            return HandleResult(await Mediator.Send(new RetrieveConnectionString.Query { }));
        }
    }
}
