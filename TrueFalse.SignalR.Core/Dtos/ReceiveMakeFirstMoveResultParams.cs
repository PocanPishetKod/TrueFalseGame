using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class ReceiveMakeFirstMoveResultParams : ResponseParams
    {
        public Guid MoverId { get; set; }

        public Guid? NextMoverId { get; set; }
    }
}
