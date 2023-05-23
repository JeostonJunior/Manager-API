using Manager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Manager.API.Utilities
{
    public class TokenGenerator : ITokenGenerator
    {       
        private readonly ApiSettings _apiSettings;

        public TokenGenerator(ApiSettings apiSettings)
        {           
            _apiSettings = apiSettings;
        }

        public string GenerateToken()
        {            
            var jwtKey = _apiSettings.JwtSettings.Key;

            var jwtLogin = _apiSettings.JwtSettings.Login;

            var jwtExpires = _apiSettings.JwtSettings.HoursToExpire;


            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, jwtLogin),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddHours(int.Parse(jwtExpires)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
