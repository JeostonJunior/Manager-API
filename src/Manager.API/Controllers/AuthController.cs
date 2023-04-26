using Manager.API.Utilities;
using Manager.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IConfiguration configuration, ITokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("/api/v1/auth/login")]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                var config = _configuration.GetSection("Jwt");

                var tokenLogin = config.GetValue<string>("Login");
                var tokenPassword = config.GetValue<string>("Password");
                var jwtExpires = config.GetValue<string>("HoursToExpire");

                if (loginViewModel.Login.Equals(tokenLogin) && loginViewModel.Password.Equals(tokenPassword))
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "User successfully authenticate",
                        Success = true,
                        Data = new
                        {
                            Token = _tokenGenerator.GenerateToken(),
                            TokenExpires = DateTime.UtcNow.AddHours(int.Parse(jwtExpires))
                        }
                    });
                }

                return StatusCode(401, Responses.UnauthorizedErrorMessage());
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}
