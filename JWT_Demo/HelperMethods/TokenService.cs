using Microsoft.IdentityModel.Tokens;
using Models.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Demo.HelperMethods
{
    public class TokenService
    {
        private IConfiguration _configuration;
        public TokenService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public string CreateToken(AppUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim("username", user.UserName!),
                new Claim("displayName", user.DisplayName),
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor); 

            return tokenHandler.WriteToken(token);
        }
    }
}
