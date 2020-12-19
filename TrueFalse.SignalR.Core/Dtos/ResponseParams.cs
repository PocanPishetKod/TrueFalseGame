using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public abstract class ResponseParams
    {
        public int RequestId { get; set; }

        public bool Succeeded { get; set; }
    }
}
