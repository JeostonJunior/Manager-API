using Manager.API.Utilities;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {       
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ApiSettings _apiSettings;

        public AuthController(ITokenGenerator tokenGenerator, ApiSettings apiSettings)
        {           
            _tokenGenerator = tokenGenerator;
            _apiSettings = apiSettings;
        }

        [HttpPost("/api/v1/auth/login")]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {              
                var tokenLogin = _apiSettings.JwtSettings.Login;
                var tokenPassword = _apiSettings.JwtSettings.Password;
                var jwtExpires = _apiSettings.JwtSettings.HoursToExpire;

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
