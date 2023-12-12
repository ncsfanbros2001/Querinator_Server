using Data;
using Microsoft.AspNetCore.Identity;
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
        private readonly OperatorDbContext _db;

        public TokenService(IConfiguration configuration, OperatorDbContext db) 
        {
            _configuration = configuration;
            _db = db;
        }

        public string CreateToken(AppUser user)
        {
            var userRoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
            var userRole = _db.Roles.FirstOrDefault(x => x.Id == userRoleId.RoleId);

            var userClaims = new List<Claim>
            {
                new Claim("username", user.UserName!),
                new Claim("displayName", user.DisplayName),
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("role", userRole.Name)
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
