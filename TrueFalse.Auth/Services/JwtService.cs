using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TrueFalse.Auth.Extensions;
using TrueFalse.Auth.Key;
using TrueFalse.Auth.Token;

namespace TrueFalse.Auth.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwt(Guid userId)
        {
            var tokenGenerator = new TokenGenerator();
            return tokenGenerator.Generate(JwtConfiguration.Create(_configuration),
                new SymmetricSecurityKeyGenerator(),
                new List<Claim>() { new Claim(ClaimsPrincipalExtensions.UserIdClaims, userId.ToString()) });
        }
    }
}
