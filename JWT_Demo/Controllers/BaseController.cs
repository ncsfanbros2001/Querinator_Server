using JWT_Demo.Models.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JWT_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= 
            HttpContext.RequestServices.GetService<IMediator>()!;

        protected ActionResult HandleResult(API_Response response)
        {
            if (response == null)
            {
                return NotFound();
            }


            if (response.IsSuccess == false)
            {
                if (response.IsSuccess == false && response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
