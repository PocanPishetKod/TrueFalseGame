using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class JwtResponse
    {
        public string Token { get; set; }

        public Guid PlayerId { get; set; }

        public string PlayerName { get; set; }
    }
}
