using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Auth.Key;

namespace TrueFalse.Auth
{
    internal class AuthConfigurator
    {
        public void ConfigureAuth(IServiceCollection services, ISecurityKeyGenerator keyGenerator, JwtConfiguration jwtConfiguration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = keyGenerator.Generate(jwtConfiguration.Key),
                        ValidIssuer = jwtConfiguration.Issuer,
                        ValidAudience = jwtConfiguration.MobileAudience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false
                    };
                });
        }
    }
}
