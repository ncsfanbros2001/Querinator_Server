using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.Helper;
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
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized(response);
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
