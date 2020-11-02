using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrueFalse.Auth.Key;

namespace TrueFalse.Auth.Token
{
    internal class TokenGenerator
    {
        private JwtSecurityToken GenerateJwt(JwtConfiguration jwtConfiguration, ISecurityKeyGenerator keyGenerator, ICollection<Claim> claims)
        {
            return new JwtSecurityToken(issuer: jwtConfiguration.Issuer,
                audience: jwtConfiguration.MobileAudience,
                claims: claims,
                signingCredentials: new SigningCredentials(keyGenerator.Generate(jwtConfiguration.Key), jwtConfiguration.SecurityAlgorithm));
        }

        public string Generate(JwtConfiguration jwtConfiguration, ISecurityKeyGenerator keyGenerator, ICollection<Claim> claims)
        {
            var jwt = GenerateJwt(jwtConfiguration, keyGenerator, claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
