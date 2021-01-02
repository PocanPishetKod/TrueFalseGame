using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public abstract class ResponseParams
    {
        public int RequestId { get; set; }

        public bool Succeeded { get; set; }
    }
}
