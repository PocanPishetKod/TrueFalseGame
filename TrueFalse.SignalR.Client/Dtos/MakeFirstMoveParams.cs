using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class MakeFirstMoveParams
    {
        public List<int> CardIds { get; set; }

        public int Rank { get; set; }
    }
}
