using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using AliseeksFE.Configuration.Options;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using AliseeksFE.Utility.Extensions;
using Newtonsoft.Json;

namespace AliseeksFE.Authentication
{
    //On FE this class is only used for TokenValidationParameters
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

        private readonly JwtOptions jwtOptions;
        private readonly IRavenClient raven;

        public AliseeksJwtAuthentication(IOptions<JwtOptions> jwtOptions, IRavenClient raven)
        {
            this.jwtOptions = jwtOptions.Value;
            this.raven = raven;
        }

        //Should not be used on the FE, this is for backend JWT generation
        public string GenerateToken(Claim[] claims)
        {
            //Add breadcrumb for Raven error monitoring
            var crumb = new Breadcrumb("AliseeksJwtAuthentication");
            crumb.Message = "Creating jwt token";
            raven.AddTrail(crumb);

            var securityKey = System.Text.Encoding.ASCII.GetBytes(jwtOptions.SecretKey);

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
                //Blocking I/O
                raven.CaptureNetCoreEvent(e).Wait();

                //Throw for whatever service is calling it
                throw e;
            }
        }
    }

    public class AliseeksJwtCookieAuthentication : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string algorithm;
        private readonly TokenValidationParameters validationParameters;
        private readonly IRavenClient raven;

        public AliseeksJwtCookieAuthentication(TokenValidationParameters parameters, IRavenClient raven)
        {
            this.algorithm = SecurityAlgorithms.HmacSha256;
            this.validationParameters = parameters;
            this.raven = raven;
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
                //Add breadcrumb for Sentry error monitoring
                var crumb = new Breadcrumb("AliseeksJwtCookieAuthentication");
                crumb.Message = "Attempting to validate JWT token";
                crumb.Data = new Dictionary<string, string>() {
                    { "ValidationParameters", JsonConvert.SerializeObject(this.validationParameters) },
                    { "ProtectedText", protectedText }
                };
                raven.AddTrail(crumb);

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

                //Append token value to identity
                var tokenClaim = new Claim[] { new Claim("Token", protectedText) };
                principal.AddIdentity(new ClaimsIdentity(tokenClaim));

                return new AuthenticationTicket(principal, new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties()
                {
                    
                },
                    "AliseeksCookie");
            }
            catch (SecurityTokenValidationException e)
            {
                //Blocking I/O
                raven.CaptureNetCoreEvent(e).Wait();

                return null;
            }
            catch (ArgumentException e)
            {
                //Blocking I/O
                raven.CaptureNetCoreEvent(e).Wait();

                return null;
            }
            catch(Exception e)
            {
                //Blocking I/O
                raven.CaptureNetCoreEvent(e).Wait();

                return null;
            }
        }
    }
}
