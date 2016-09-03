using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AliseeksFE.Authentication
{
    public class AliseeksJwtAuthentication
    {
        public static TokenValidationParameters TokenValidationParameters(string securityKey)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(securityKey)),

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        }

        const string issuer = "AliseeksIssuer";
        const string audience = "AliseeksUser";

        public AliseeksJwtAuthentication()
        {

        }

        public string GenerateToken(Claim[] claims)
        {
            var securityKey = System.Text.Encoding.ASCII.GetBytes("thisismykeyanditslongandsecurehopefullywhoknows");

            var handler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = "AliseeksIssuer",
                Audience = "AliseeksUser",
                Expires = DateTime.Now.AddDays(14),
                NotBefore = now,
                IssuedAt = now,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(securityKey),
                    SecurityAlgorithms.HmacSha256)
            };

            try
            {
                var token = handler.CreateToken(tokenDescriptor);
                var tokenString = handler.WriteToken(token);
                return tokenString;
            }
            catch(Exception e)
            {

            }
            return "";
        }
    }

    public class AliseeksJwtCookieAuthentication : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string algorithm;
        private readonly TokenValidationParameters validationParameters;

        public AliseeksJwtCookieAuthentication(TokenValidationParameters parameters)
        {
            this.algorithm = SecurityAlgorithms.HmacSha256;
            this.validationParameters = parameters;
        }

        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotImplementedException();
        }

        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;

            try
            {
                principal = handler.ValidateToken(protectedText, this.validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if(validJwt == null)
                {
                    throw new ArgumentException("Invalid JWT");
                }

                if(!validJwt.Header.Alg.Equals(algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be {algorithm}");
                }

                return new AuthenticationTicket(principal, new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties()
                {
                    
                },
                    "AliseeksCookie");
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
