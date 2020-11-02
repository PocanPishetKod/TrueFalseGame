using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Auth.Key
{
    internal interface ISecurityKeyGenerator
    {
        SecurityKey Generate(string secretKey);
    }
}
