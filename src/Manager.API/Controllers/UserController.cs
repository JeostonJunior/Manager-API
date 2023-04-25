using Manager.API.ViewModels;
using Manager.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    [Route("Api/{controller}")]//verificar depois
    public class UserController : ControllerBase
    {
        [HttpPost("/api/v1/users/create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserViewModel user)
        {
            try
            {
                return Ok(user);
            }
            catch (DomainExceptions ex)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error");
            }
        }
    }
}
