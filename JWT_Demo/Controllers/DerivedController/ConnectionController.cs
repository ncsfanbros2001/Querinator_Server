using JWT_Demo.Application.Connection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Demo.Controllers.DerivedController
{
    public class ConnectionController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetDbConnection()
        {
            return HandleResult(await Mediator.Send(new SetConnectionString.Query { }));
        }
    }
}
