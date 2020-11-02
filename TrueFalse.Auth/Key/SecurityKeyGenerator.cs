using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Auth.Key
{
    internal class SymmetricSecurityKeyGenerator : ISecurityKeyGenerator
    {
        public SecurityKey Generate(string secretKey)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            var key = Convert.FromBase64String(secretKey);
            return new SymmetricSecurityKey(key);
        }
    }
}
