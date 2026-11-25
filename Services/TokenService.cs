using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using obrazo.Models.Entities;

namespace obrazo.Services
{
    public class TokenService
    {
        public static object GerarToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(Key.keyToken);
            var tokenConfig = new SecurityTokenDescriptor
            {
              Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
              {
                  new Claim("userID", user.Id.ToString())
              }), 
              Expires = DateTime.UtcNow.AddHours(1),
              SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

    }
}