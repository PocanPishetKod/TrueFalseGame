using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class ReceiveMakeFirstMoveResultParams
    {
        public bool Succeeded { get; set; }

        public Guid? NextMoverId { get; set; }
    }
}
