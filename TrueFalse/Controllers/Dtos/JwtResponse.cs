using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Controllers.Dtos
{
    public class JwtResponse
    {
        public string Token { get; set; }

        public Guid PlayerId { get; set; }

        public string PlayerName { get; set; }
    }
}
