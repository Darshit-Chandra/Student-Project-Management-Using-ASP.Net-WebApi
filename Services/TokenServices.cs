using Microsoft.IdentityModel.Tokens;
using SPMBACKENDSELF.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SPMBACKENDSELF.Services
{
    public class TokenServices
    {
        private readonly IConfiguration _config;

        public TokenServices(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserModel user)
        {
            //the info on the ID card (email, unique token ID)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //locks the card with our secret key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //JwtSecurityToken the actual card with issuer, audience, claims, expiry
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:ExpiresInMinutes"]!)),
                signingCredentials: credentials //the key + algorithm combo used to sign (seal) the token.
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

