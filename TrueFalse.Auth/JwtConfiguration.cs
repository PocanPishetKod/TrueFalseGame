using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Auth
{
    internal class JwtConfiguration
    {
        public string Issuer { get; set; }

        public string MobileAudience { get; set; }

        public string Key { get; set; }

        public string SecurityAlgorithm => SecurityAlgorithms.HmacSha256;

        public static JwtConfiguration Create(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration.GetSection("Jwt").Get<JwtConfiguration>();
        }
    }
}
