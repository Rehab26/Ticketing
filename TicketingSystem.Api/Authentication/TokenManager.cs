using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace TicketingSystem.Api
{
    public class TokenManager
    {
        // Take time of token expiration from web.confid
        private static int TokenExpiration = int.Parse(ConfigurationManager.AppSettings["TokenExpiration"]);

        // Generate unique token 
        private static HMACSHA256 hmac = new HMACSHA256();
        static string Secret = Convert.ToBase64String(hmac.Key);

        // Generate token
        public static string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[] {
                      new Claim(type:ClaimTypes.Name, value:username)}),
                Expires = DateTime.UtcNow.AddMinutes(TokenExpiration),
                SigningCredentials = new SigningCredentials(securityKey,
                algorithm: SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
        // Get principal and assign parameters
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        // To make sure the original user is using the token
        public static string ValidateToken(string token) 
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(type: ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

    }
}
