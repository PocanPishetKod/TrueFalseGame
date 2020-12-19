using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public abstract class ResponseParams
    {
        internal int RequestId { get; }

        public bool Succeeded { get; set; }
    }
}
