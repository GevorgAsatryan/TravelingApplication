using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TravelingApplication
{
    public static class TokenManagement
    {
        public static string GenerateJwtToken(string name, string email, string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("username", name),
                new Claim("email", email),
            };

            var token = new JwtSecurityToken(
                issuer: "traveling",
                audience: "users",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string ValidateToken(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "traveling",
                ValidateAudience = true,
                ValidAudience = "users",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
                return "Token is valied";
            }
            catch(Exception ex)
            {
                return "Token is invalid";
            }
        }

        public static string DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string decodedResult = "";

            foreach (var decodedInfo in jwtToken.Payload)
            {
                decodedResult += $"{decodedInfo.Key} {decodedInfo.Value}\n";
            }

            return "Token is valied\n" + $"{decodedResult}";
        }
    }
}
